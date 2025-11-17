/* ============================================================
   PHÂN CA CHO NHÂN VIÊN
   Script để tạo bảng và phân ca làm việc cho nhân viên
   ============================================================ */

USE QL_NhaHangTiecCuoi_V3;
GO

/* ========================
   1) TẠO BẢNG PHÂN CA CHO NHÂN VIÊN
   ======================== */
IF OBJECT_ID('dbo.nguoi_dung_ca','U') IS NULL
CREATE TABLE dbo.nguoi_dung_ca(
  nguoi_dung_ca_id INT IDENTITY(1,1) PRIMARY KEY,
  nguoi_dung_id    INT NOT NULL,
  chi_nhanh_id     INT NOT NULL,
  ca_id            INT NOT NULL,
  trang_thai       TINYINT NOT NULL DEFAULT 1, -- 1=Hoạt động, 0=Không hoạt động
  FOREIGN KEY (nguoi_dung_id) REFERENCES dbo.nguoi_dung(nguoi_dung_id),
  FOREIGN KEY (chi_nhanh_id)  REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (ca_id)         REFERENCES dbo.ca(ca_id),
  -- Một nhân viên có thể có nhiều ca trong 1 chi nhánh, nhưng không được trùng lặp cùng một ca
  UNIQUE (nguoi_dung_id, chi_nhanh_id, ca_id)
);

-- Tạo index để tăng tốc truy vấn
CREATE NONCLUSTERED INDEX IX_nguoi_dung_ca_nguoi_dung 
ON dbo.nguoi_dung_ca(nguoi_dung_id);

CREATE NONCLUSTERED INDEX IX_nguoi_dung_ca_chi_nhanh 
ON dbo.nguoi_dung_ca(chi_nhanh_id);

CREATE NONCLUSTERED INDEX IX_nguoi_dung_ca_ca 
ON dbo.nguoi_dung_ca(ca_id);

PRINT N'Đã tạo bảng nguoi_dung_ca thành công!';
GO

/* ========================
   2) THÊM DỮ LIỆU CA (NẾU CHƯA CÓ)
   ======================== */
IF NOT EXISTS (SELECT 1 FROM dbo.ca WHERE ten_ca = N'Sáng')
  INSERT INTO dbo.ca(ten_ca, gio_bd, gio_kt)
  VALUES (N'Sáng', '07:00', '11:00');

IF NOT EXISTS (SELECT 1 FROM dbo.ca WHERE ten_ca = N'Trưa')
  INSERT INTO dbo.ca(ten_ca, gio_bd, gio_kt)
  VALUES (N'Trưa', '11:00', '16:00');

IF NOT EXISTS (SELECT 1 FROM dbo.ca WHERE ten_ca = N'Tối')
  INSERT INTO dbo.ca(ten_ca, gio_bd, gio_kt)
  VALUES (N'Tối', '16:00', '22:00');
GO

/* ========================
   3) PHÂN CA CHO NHÂN VIÊN
   ======================== */

-- Lấy ID các ca
DECLARE @ca_sang INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Sáng');
DECLARE @ca_trua INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Trưa');
DECLARE @ca_toi INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Tối');

-- Lấy ID chi nhánh
DECLARE @cn_tt INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'CN Trung tâm' ORDER BY chi_nhanh_id);

-- Lấy ID người dùng
DECLARE @nd_thungan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
DECLARE @nd_phucvu01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
DECLARE @nd_phucvu02 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
DECLARE @nd_daubep01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
DECLARE @nd_letan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');

-- Phân ca cho Thu ngân: Ca sáng và ca trưa
IF @cn_tt IS NOT NULL AND @nd_thungan01 IS NOT NULL AND @ca_sang IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_thungan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_sang)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_thungan01, @cn_tt, @ca_sang, 1);

IF @cn_tt IS NOT NULL AND @nd_thungan01 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_thungan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_thungan01, @cn_tt, @ca_trua, 1);

-- Phân ca cho Phục vụ 01: Ca sáng
IF @cn_tt IS NOT NULL AND @nd_phucvu01 IS NOT NULL AND @ca_sang IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_sang)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu01, @cn_tt, @ca_sang, 1);

-- Phân ca cho Phục vụ 02: Ca trưa và ca tối
IF @cn_tt IS NOT NULL AND @nd_phucvu02 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu02 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu02, @cn_tt, @ca_trua, 1);

IF @cn_tt IS NOT NULL AND @nd_phucvu02 IS NOT NULL AND @ca_toi IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu02 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_toi)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu02, @cn_tt, @ca_toi, 1);

-- Phân ca cho Đầu bếp: Ca trưa và ca tối
IF @cn_tt IS NOT NULL AND @nd_daubep01 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_daubep01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_daubep01, @cn_tt, @ca_trua, 1);

IF @cn_tt IS NOT NULL AND @nd_daubep01 IS NOT NULL AND @ca_toi IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_daubep01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_toi)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_daubep01, @cn_tt, @ca_toi, 1);

-- Phân ca cho Lễ tân: Ca sáng và ca trưa
IF @cn_tt IS NOT NULL AND @nd_letan01 IS NOT NULL AND @ca_sang IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_letan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_sang)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_letan01, @cn_tt, @ca_sang, 1);

IF @cn_tt IS NOT NULL AND @nd_letan01 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_letan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_letan01, @cn_tt, @ca_trua, 1);
GO

/* ========================
   3.1) PHÂN CA CHO NHÂN VIÊN - CHI NHÁNH ID = 2
   ======================== */

-- Lấy ID các ca (khai báo lại vì đã có GO statement)
DECLARE @ca_sang2 INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Sáng');
DECLARE @ca_trua2 INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Trưa');
DECLARE @ca_toi2 INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Tối');

-- Lấy ID chi nhánh ID = 2
DECLARE @cn_id2 INT = 2;

-- Lấy ID người dùng (khai báo lại vì đã có GO statement)
DECLARE @nd_thungan01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
DECLARE @nd_phucvu01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
DECLARE @nd_phucvu02_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
DECLARE @nd_daubep01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
DECLARE @nd_letan01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');

-- Phân ca cho Thu ngân: Ca sáng và ca tối
IF @cn_id2 IS NOT NULL AND @nd_thungan01_2 IS NOT NULL AND @ca_sang2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_thungan01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_sang2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_thungan01_2, @cn_id2, @ca_sang2, 1);

IF @cn_id2 IS NOT NULL AND @nd_thungan01_2 IS NOT NULL AND @ca_toi2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_thungan01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_toi2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_thungan01_2, @cn_id2, @ca_toi2, 1);

-- Phân ca cho Phục vụ 01: Ca trưa và ca tối
IF @cn_id2 IS NOT NULL AND @nd_phucvu01_2 IS NOT NULL AND @ca_trua2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_trua2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu01_2, @cn_id2, @ca_trua2, 1);

IF @cn_id2 IS NOT NULL AND @nd_phucvu01_2 IS NOT NULL AND @ca_toi2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_toi2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu01_2, @cn_id2, @ca_toi2, 1);

-- Phân ca cho Phục vụ 02: Ca sáng và ca trưa
IF @cn_id2 IS NOT NULL AND @nd_phucvu02_2 IS NOT NULL AND @ca_sang2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu02_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_sang2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu02_2, @cn_id2, @ca_sang2, 1);

IF @cn_id2 IS NOT NULL AND @nd_phucvu02_2 IS NOT NULL AND @ca_trua2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu02_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_trua2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu02_2, @cn_id2, @ca_trua2, 1);

-- Phân ca cho Đầu bếp: Ca sáng và ca trưa
IF @cn_id2 IS NOT NULL AND @nd_daubep01_2 IS NOT NULL AND @ca_sang2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_daubep01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_sang2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_daubep01_2, @cn_id2, @ca_sang2, 1);

IF @cn_id2 IS NOT NULL AND @nd_daubep01_2 IS NOT NULL AND @ca_trua2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_daubep01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_trua2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_daubep01_2, @cn_id2, @ca_trua2, 1);

-- Phân ca cho Lễ tân: Ca trưa và ca tối
IF @cn_id2 IS NOT NULL AND @nd_letan01_2 IS NOT NULL AND @ca_trua2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_letan01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_trua2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_letan01_2, @cn_id2, @ca_trua2, 1);

IF @cn_id2 IS NOT NULL AND @nd_letan01_2 IS NOT NULL AND @ca_toi2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_letan01_2 AND chi_nhanh_id = @cn_id2 AND ca_id = @ca_toi2)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_letan01_2, @cn_id2, @ca_toi2, 1);
GO

/* ========================
   4) XEM DANH SÁCH PHÂN CA
   ======================== */
PRINT N'=== DANH SÁCH PHÂN CA CHO NHÂN VIÊN ===';
SELECT 
  nd.tai_khoan,
  nd.ho_ten,
  cn.ten AS ten_chi_nhanh,
  c.ten_ca,
  c.gio_bd,
  c.gio_kt,
  CASE WHEN ndc.trang_thai = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai
FROM dbo.nguoi_dung_ca ndc
INNER JOIN dbo.nguoi_dung nd ON ndc.nguoi_dung_id = nd.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON ndc.chi_nhanh_id = cn.chi_nhanh_id
INNER JOIN dbo.ca c ON ndc.ca_id = c.ca_id
ORDER BY nd.ho_ten, c.ten_ca;
GO

/* ========================
   5) PHÂN CA THEO CHI NHÁNH
   ======================== */
PRINT N'=== PHÂN CA THEO CHI NHÁNH ===';
SELECT 
  cn.chi_nhanh_id,
  cn.ten AS ten_chi_nhanh,
  c.ten_ca,
  c.gio_bd,
  c.gio_kt,
  COUNT(ndc.nguoi_dung_id) AS so_luong_nhan_vien,
  STRING_AGG(nd.ho_ten, N', ') WITHIN GROUP (ORDER BY nd.ho_ten) AS danh_sach_nhan_vien
FROM dbo.chi_nhanh cn
INNER JOIN dbo.nguoi_dung_ca ndc ON cn.chi_nhanh_id = ndc.chi_nhanh_id
INNER JOIN dbo.ca c ON ndc.ca_id = c.ca_id
INNER JOIN dbo.nguoi_dung nd ON ndc.nguoi_dung_id = nd.nguoi_dung_id
WHERE ndc.trang_thai = 1
GROUP BY cn.chi_nhanh_id, cn.ten, c.ca_id, c.ten_ca, c.gio_bd, c.gio_kt
ORDER BY cn.chi_nhanh_id, c.ten_ca;
GO

/* ========================
   6) NHÂN VIÊN CHƯA ĐƯỢC PHÂN CA
   ======================== */
PRINT N'=== NHÂN VIÊN CHƯA ĐƯỢC PHÂN CA ===';
SELECT 
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  cn.ten AS ten_chi_nhanh
FROM dbo.nguoi_dung nd
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
WHERE nd.hoat_dong = 1
  AND NOT EXISTS (
    SELECT 1 
    FROM dbo.nguoi_dung_ca ndc 
    WHERE ndc.nguoi_dung_id = nd.nguoi_dung_id 
      AND ndc.chi_nhanh_id = cn.chi_nhanh_id
      AND ndc.trang_thai = 1
  )
ORDER BY nd.ho_ten, cn.ten;
GO

/* ========================
   7) STORED PROCEDURE: LẤY CA CỦA NHÂN VIÊN THEO NGÀY
   ======================== */
IF OBJECT_ID('dbo.sp_LayCaCuaNhanVienTheoNgay', 'P') IS NOT NULL
  DROP PROCEDURE dbo.sp_LayCaCuaNhanVienTheoNgay;
GO

CREATE PROCEDURE dbo.sp_LayCaCuaNhanVienTheoNgay
  @nguoi_dung_id INT,
  @ngay DATE,
  @chi_nhanh_id INT = NULL
AS
BEGIN
  SET NOCOUNT ON;
  
  SELECT 
    ndc.nguoi_dung_ca_id,
    nd.tai_khoan,
    nd.ho_ten,
    cn.ten AS ten_chi_nhanh,
    c.ca_id,
    c.ten_ca,
    c.gio_bd,
    c.gio_kt
  FROM dbo.nguoi_dung_ca ndc
  INNER JOIN dbo.nguoi_dung nd ON ndc.nguoi_dung_id = nd.nguoi_dung_id
  INNER JOIN dbo.chi_nhanh cn ON ndc.chi_nhanh_id = cn.chi_nhanh_id
  INNER JOIN dbo.ca c ON ndc.ca_id = c.ca_id
  WHERE ndc.nguoi_dung_id = @nguoi_dung_id
    AND ndc.trang_thai = 1
    AND (@chi_nhanh_id IS NULL OR ndc.chi_nhanh_id = @chi_nhanh_id)
  ORDER BY c.gio_bd;
END;
GO

PRINT N'Đã tạo stored procedure sp_LayCaCuaNhanVienTheoNgay!';
GO

/* ========================
   VÍ DỤ SỬ DỤNG:
   ========================
   
-- Lấy ca của nhân viên theo ngày
EXEC dbo.sp_LayCaCuaNhanVienTheoNgay 
  @nguoi_dung_id = 1, 
  @ngay = '2024-01-15',
  @chi_nhanh_id = 1;

-- Phân ca cho nhân viên
INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
VALUES (1, 1, 1, 1);

*/
GO

PRINT N'Hoàn tất script phân ca cho nhân viên!';
GO

