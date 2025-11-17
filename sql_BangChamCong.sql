/* ============================================================
   BẢNG CHẤM CÔNG NHÂN VIÊN
   Tạo bảng để quản lý chấm công cho nhân viên
   ============================================================ */

USE QL_NhaHangTiecCuoi_V3;
GO

IF OBJECT_ID('dbo.cham_cong','U') IS NULL
CREATE TABLE dbo.cham_cong(
  cham_cong_id    INT IDENTITY(1,1) PRIMARY KEY,
  nguoi_dung_id   INT NOT NULL,
  chi_nhanh_id    INT NOT NULL,
  ca_id           INT NOT NULL,
  ngay_cham_cong  DATE NOT NULL,
  gio_vao         DATETIME2(0) NOT NULL,
  gio_ra          DATETIME2(0) NULL,
  so_gio_lam      AS (
    CASE 
      WHEN gio_ra IS NOT NULL 
      THEN CAST(DATEDIFF(MINUTE, gio_vao, gio_ra) AS DECIMAL(10,2)) / 60.0
      ELSE NULL
    END
  ) PERSISTED,
  trang_thai      NVARCHAR(20) NOT NULL DEFAULT N'ĐÚNG GIỜ'
              CHECK (trang_thai IN (N'ĐÚNG GIỜ', N'ĐI MUỘN', N'VỀ SỚM', N'VẮNG MẶT', N'CÓ PHÉP', N'KHÔNG PHÉP')),
  ghi_chu         NVARCHAR(500) NULL,
  ngay_tao        DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  FOREIGN KEY (nguoi_dung_id) REFERENCES dbo.nguoi_dung(nguoi_dung_id),
  FOREIGN KEY (chi_nhanh_id)  REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (ca_id)         REFERENCES dbo.ca(ca_id),
  -- Một nhân viên chỉ có 1 bản ghi chấm công cho 1 ca trong 1 ngày
  UNIQUE (nguoi_dung_id, chi_nhanh_id, ca_id, ngay_cham_cong)
);

-- Tạo index để tăng tốc truy vấn
CREATE NONCLUSTERED INDEX IX_cham_cong_nguoi_dung_ngay 
ON dbo.cham_cong(nguoi_dung_id, ngay_cham_cong);

CREATE NONCLUSTERED INDEX IX_cham_cong_chi_nhanh_ngay 
ON dbo.cham_cong(chi_nhanh_id, ngay_cham_cong);

CREATE NONCLUSTERED INDEX IX_cham_cong_ngay 
ON dbo.cham_cong(ngay_cham_cong);
GO

PRINT N'Đã tạo bảng cham_cong thành công!';
GO

/* ============================================================
   TRIGGER TỰ ĐỘNG CẬP NHẬT TRẠNG THÁI CHẤM CÔNG
   So sánh gio_vao với gio_bd và gio_ra với gio_kt từ bảng ca
   ============================================================ */

-- Trigger cập nhật trạng thái khi INSERT hoặc UPDATE
IF OBJECT_ID('dbo.TR_cham_cong_tu_dong_trang_thai', 'TR') IS NOT NULL
  DROP TRIGGER dbo.TR_cham_cong_tu_dong_trang_thai;
GO

CREATE TRIGGER dbo.TR_cham_cong_tu_dong_trang_thai
ON dbo.cham_cong
AFTER INSERT, UPDATE
AS
BEGIN
  SET NOCOUNT ON;
  
  UPDATE cc
  SET trang_thai = CASE
    -- Nếu chưa có giờ ra, chỉ kiểm tra đi muộn
    WHEN cc.gio_ra IS NULL THEN
      CASE 
        WHEN CAST(cc.gio_vao AS TIME) > c.gio_bd THEN N'ĐI MUỘN'
        ELSE N'ĐÚNG GIỜ'
      END
    -- Nếu có cả giờ vào và giờ ra
    WHEN cc.gio_ra IS NOT NULL THEN
      CASE
        -- Nếu vừa đi muộn vừa về sớm, ưu tiên ghi "ĐI MUỘN" (có thể thay bằng "VỀ SỚM" nếu muốn)
        WHEN CAST(cc.gio_vao AS TIME) > c.gio_bd AND CAST(cc.gio_ra AS TIME) < c.gio_kt THEN N'ĐI MUỘN'
        WHEN CAST(cc.gio_vao AS TIME) > c.gio_bd THEN N'ĐI MUỘN'
        WHEN CAST(cc.gio_ra AS TIME) < c.gio_kt THEN N'VỀ SỚM'
        ELSE N'ĐÚNG GIỜ'
      END
    ELSE cc.trang_thai
  END
  FROM dbo.cham_cong cc
  INNER JOIN dbo.ca c ON cc.ca_id = c.ca_id
  INNER JOIN inserted i ON cc.cham_cong_id = i.cham_cong_id
  WHERE cc.trang_thai NOT IN (N'VẮNG MẶT', N'CÓ PHÉP', N'KHÔNG PHÉP');
  -- Chỉ tự động cập nhật nếu trạng thái không phải là vắng mặt, có phép, không phép
END;
GO

PRINT N'Đã tạo trigger tự động cập nhật trạng thái chấm công!';
GO

/* ============================================================
   VÍ DỤ SỬ DỤNG:
   ============================================================

-- 1. Chấm công vào ca (nhân viên vào ca)
-- Trạng thái sẽ tự động được cập nhật bởi trigger dựa trên so sánh gio_vao với gio_bd
INSERT INTO dbo.cham_cong(nguoi_dung_id, chi_nhanh_id, ca_id, ngay_cham_cong, gio_vao)
VALUES (1, 1, 1, CAST(GETDATE() AS DATE), GETDATE());

-- 2. Chấm công ra ca (cập nhật giờ ra)
-- Trạng thái sẽ tự động được cập nhật bởi trigger dựa trên so sánh gio_ra với gio_kt
UPDATE dbo.cham_cong
SET gio_ra = GETDATE()
WHERE cham_cong_id = 1;

-- 3. Ví dụ: Chấm công với giờ vào muộn (sau gio_bd)
-- Trigger sẽ tự động đặt trạng thái là "ĐI MUỘN"
INSERT INTO dbo.cham_cong(nguoi_dung_id, chi_nhanh_id, ca_id, ngay_cham_cong, gio_vao)
SELECT 1, 1, 1, CAST(GETDATE() AS DATE), 
       CAST(CAST(GETDATE() AS DATE) AS DATETIME) + CAST(DATEADD(MINUTE, 30, (SELECT gio_bd FROM dbo.ca WHERE ca_id = 1)) AS DATETIME);

-- 4. Xem báo cáo chấm công theo nhân viên (có so sánh với giờ ca)
SELECT 
  nd.ho_ten,
  nd.tai_khoan,
  cn.ten AS ten_chi_nhanh,
  c.ten_ca,
  c.gio_bd AS gio_bat_dau_ca,
  c.gio_kt AS gio_ket_thuc_ca,
  cc.ngay_cham_cong,
  CAST(cc.gio_vao AS TIME) AS gio_vao_thuc_te,
  CAST(cc.gio_ra AS TIME) AS gio_ra_thuc_te,
  DATEDIFF(MINUTE, c.gio_bd, CAST(cc.gio_vao AS TIME)) AS phut_di_muon,
  CASE WHEN cc.gio_ra IS NOT NULL 
       THEN DATEDIFF(MINUTE, CAST(cc.gio_ra AS TIME), c.gio_kt) 
       ELSE NULL END AS phut_ve_som,
  cc.so_gio_lam,
  cc.trang_thai,
  cc.ghi_chu
FROM dbo.cham_cong cc
INNER JOIN dbo.nguoi_dung nd ON cc.nguoi_dung_id = nd.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON cc.chi_nhanh_id = cn.chi_nhanh_id
INNER JOIN dbo.ca c ON cc.ca_id = c.ca_id
WHERE cc.ngay_cham_cong >= DATEADD(DAY, -30, GETDATE())
ORDER BY cc.ngay_cham_cong DESC, nd.ho_ten;

-- 5. Thống kê chấm công theo tháng
SELECT 
  nd.ho_ten,
  COUNT(*) AS so_ngay_cham_cong,
  SUM(cc.so_gio_lam) AS tong_gio_lam,
  SUM(CASE WHEN cc.trang_thai = N'ĐI MUỘN' THEN 1 ELSE 0 END) AS so_lan_di_muon,
  SUM(CASE WHEN cc.trang_thai = N'VẮNG MẶT' THEN 1 ELSE 0 END) AS so_ngay_vang
FROM dbo.cham_cong cc
INNER JOIN dbo.nguoi_dung nd ON cc.nguoi_dung_id = nd.nguoi_dung_id
WHERE YEAR(cc.ngay_cham_cong) = YEAR(GETDATE())
  AND MONTH(cc.ngay_cham_cong) = MONTH(GETDATE())
GROUP BY nd.nguoi_dung_id, nd.ho_ten
ORDER BY nd.ho_ten;

*/
