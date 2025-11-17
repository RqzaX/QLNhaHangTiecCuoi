/* ============================================================
   GÁN CHI NHÁNH CHO NGƯỜI DÙNG
   Script để gán chi nhánh cho tất cả người dùng trong hệ thống
   Idempotent: có điều kiện NOT EXISTS để chạy nhiều lần an toàn
   ============================================================ */

USE QL_NhaHangTiecCuoi_V3;
GO

/* ========================
   GÁN CHI NHÁNH CHO TẤT CẢ NGƯỜI DÙNG
   ======================== */

-- Lấy ID các chi nhánh
DECLARE @cn_tt INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'CN Trung tâm' ORDER BY chi_nhanh_id);
DECLARE @cn_q7 INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'CN Quận 7' ORDER BY chi_nhanh_id);

-- Nếu chưa có chi nhánh, tạo mặc định
IF @cn_tt IS NULL
BEGIN
  INSERT INTO dbo.chi_nhanh(ten, dia_chi, sdt, trang_thai)
  VALUES (N'CN Trung tâm', N'12 Nguyễn Huệ, Q.1, TP.HCM', N'028-12345678', 1);
  SET @cn_tt = SCOPE_IDENTITY();
END

IF @cn_q7 IS NULL
BEGIN
  INSERT INTO dbo.chi_nhanh(ten, dia_chi, sdt, trang_thai)
  VALUES (N'CN Quận 7', N'88 Nguyễn Văn Linh, Q.7, TP.HCM', N'028-87654321', 1);
  SET @cn_q7 = SCOPE_IDENTITY();
END

-- Gán chi nhánh cho ADMIN (gán tất cả chi nhánh)
IF @cn_tt IS NOT NULL
BEGIN
  DECLARE @nd_admin INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'admin');
  
  IF @nd_admin IS NOT NULL
  BEGIN
    -- Gán CN Trung tâm cho admin
    IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_admin AND chi_nhanh_id = @cn_tt)
      INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
      VALUES (@nd_admin, @cn_tt);
    
    -- Gán CN Quận 7 cho admin (nếu có)
    IF @cn_q7 IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_admin AND chi_nhanh_id = @cn_q7)
      INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
      VALUES (@nd_admin, @cn_q7);
  END
END

-- Gán chi nhánh cho các nhân viên
-- Thu ngân
DECLARE @nd_thungan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
IF @cn_tt IS NOT NULL AND @nd_thungan01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_thungan01 AND chi_nhanh_id = @cn_tt)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_thungan01, @cn_tt);

-- Phục vụ 01
DECLARE @nd_phucvu01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
IF @cn_tt IS NOT NULL AND @nd_phucvu01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_phucvu01 AND chi_nhanh_id = @cn_tt)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_phucvu01, @cn_tt);

-- Phục vụ 02
DECLARE @nd_phucvu02 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
IF @cn_tt IS NOT NULL AND @nd_phucvu02 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_phucvu02 AND chi_nhanh_id = @cn_tt)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_phucvu02, @cn_tt);

-- Đầu bếp
DECLARE @nd_daubep01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
IF @cn_tt IS NOT NULL AND @nd_daubep01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_daubep01 AND chi_nhanh_id = @cn_tt)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_daubep01, @cn_tt);

-- Lễ tân
DECLARE @nd_letan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');
IF @cn_tt IS NOT NULL AND @nd_letan01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_letan01 AND chi_nhanh_id = @cn_tt)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_letan01, @cn_tt);
GO

/* ========================
   GÁN CHI NHÁNH CHO TẤT CẢ NGƯỜI DÙNG CHƯA CÓ CHI NHÁNH
   (Script tự động gán chi nhánh mặc định cho người dùng chưa có)
   ======================== */

-- Gán CN Trung tâm cho tất cả người dùng chưa có chi nhánh nào
DECLARE @cn_tt_auto INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'CN Trung tâm' ORDER BY chi_nhanh_id);

IF @cn_tt_auto IS NOT NULL
BEGIN
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  SELECT nd.nguoi_dung_id, @cn_tt_auto
  FROM dbo.nguoi_dung nd
  WHERE nd.hoat_dong = 1
    AND NOT EXISTS (
      SELECT 1 
      FROM dbo.nguoi_dung_chi_nhanh ndcn 
      WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
    );
END
GO

/* ========================
   KIỂM TRA KẾT QUẢ
   ======================== */
PRINT N'=== DANH SÁCH NGƯỜI DÙNG VÀ CHI NHÁNH ===';
SELECT 
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  cn.ten AS ten_chi_nhanh,
  cn.dia_chi,
  CASE WHEN cn.trang_thai = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai_chi_nhanh
FROM dbo.nguoi_dung nd
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
ORDER BY nd.tai_khoan, cn.ten;

PRINT N'=== NGƯỜI DÙNG CHƯA CÓ CHI NHÁNH ===';
SELECT 
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  N'Chưa có chi nhánh' AS ghi_chu
FROM dbo.nguoi_dung nd
WHERE nd.hoat_dong = 1
  AND NOT EXISTS (
    SELECT 1 
    FROM dbo.nguoi_dung_chi_nhanh ndcn 
    WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
  )
ORDER BY nd.tai_khoan;
GO

PRINT N'Hoàn tất gán chi nhánh cho người dùng!';
GO

