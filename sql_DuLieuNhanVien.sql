/* ============================================================
   DỮ LIỆU MẪU QUẢN LÝ NHÂN VIÊN
   Chạy sau khi tạo các bảng: vai_tro, nguoi_dung, nguoi_dung_vai_tro, nguoi_dung_chi_nhanh
   Idempotent: có điều kiện NOT EXISTS để chạy nhiều lần an toàn
   ============================================================ */

USE QL_NhaHangTiecCuoi_V3;
GO

/* ========================
   1) THÊM VAI TRÒ (ROLES)
   ======================== */
IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'ADMIN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'ADMIN', N'Quản trị viên');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'QLCN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'QLCN', N'Quản lý chi nhánh');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'QLNV')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'QLNV', N'Quản lý nhân viên');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'THUNGAN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'THUNGAN', N'Thu ngân');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'PHUCVU')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'PHUCVU', N'Nhân viên phục vụ');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'DAUBEP')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'DAUBEP', N'Đầu bếp');


IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'LETAN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'LETAN', N'Lễ tân');
GO

/* ========================
   2) THÊM NGƯỜI DÙNG (NHÂN VIÊN)
   ======================== */
-- Lấy ID chi nhánh (giả sử đã có dữ liệu chi nhánh)
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

-- Thêm người dùng (chỉ thêm nhân viên, admin đã có sẵn)
-- 1 Thu ngân
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'thungan01', N'123456', N'Phạm Thị Thu Ngân', 1);

-- 2 Phục vụ
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'phucvu01', N'123456', N'Nguyễn Thị Phục Vụ', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'phucvu02', N'123456', N'Trần Văn Phục Vụ', 1);

-- 1 Đầu bếp
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'daubep01', N'123456', N'Võ Văn Đầu Bếp', 1);

-- 1 Lễ tân
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'letan01', N'123456', N'Bùi Thị Lễ Tân', 1);
GO

/* ========================
   3) GÁN VAI TRÒ CHO NGƯỜI DÙNG
   ======================== */
DECLARE @vai_tro_admin INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'ADMIN');
DECLARE @vai_tro_qlcn INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'QLCN');
DECLARE @vai_tro_thungan INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'THUNGAN');
DECLARE @vai_tro_phucvu INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'PHUCVU');
DECLARE @vai_tro_daubep INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'DAUBEP');
DECLARE @vai_tro_letan INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'LETAN');

DECLARE @nd_thungan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
DECLARE @nd_phucvu01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
DECLARE @nd_phucvu02 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
DECLARE @nd_daubep01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
DECLARE @nd_letan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');

-- Thu ngân
IF @nd_thungan01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_thungan01 AND vai_tro_id = @vai_tro_thungan)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_thungan01, @vai_tro_thungan);

-- Phục vụ
IF @nd_phucvu01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_phucvu01 AND vai_tro_id = @vai_tro_phucvu)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_phucvu01, @vai_tro_phucvu);

IF @nd_phucvu02 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_phucvu02 AND vai_tro_id = @vai_tro_phucvu)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_phucvu02, @vai_tro_phucvu);

-- Đầu bếp
IF @nd_daubep01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_daubep01 AND vai_tro_id = @vai_tro_daubep)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_daubep01, @vai_tro_daubep);

-- Lễ tân
IF @nd_letan01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_letan01 AND vai_tro_id = @vai_tro_letan)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_letan01, @vai_tro_letan);
GO

/* ========================
   4) GÁN CHI NHÁNH CHO NGƯỜI DÙNG
   ======================== */
DECLARE @cn_tt2 INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'CN Trung tâm' ORDER BY chi_nhanh_id);

DECLARE @nd_thungan01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
DECLARE @nd_phucvu01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
DECLARE @nd_phucvu02_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
DECLARE @nd_daubep01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
DECLARE @nd_letan01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');

-- Thu ngân làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_thungan01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_thungan01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_thungan01_2, @cn_tt2);

-- Phục vụ 01, 02 làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_phucvu01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_phucvu01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_phucvu01_2, @cn_tt2);

IF @cn_tt2 IS NOT NULL AND @nd_phucvu02_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_phucvu02_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_phucvu02_2, @cn_tt2);

-- Đầu bếp làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_daubep01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_daubep01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_daubep01_2, @cn_tt2);

-- Lễ tân làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_letan01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_letan01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_letan01_2, @cn_tt2);
GO

/* ========================
   5) KIỂM TRA DỮ LIỆU ĐÃ THÊM
   ======================== */
PRINT N'=== DANH SÁCH VAI TRÒ ===';
SELECT vai_tro_id, ma, ten FROM dbo.vai_tro ORDER BY vai_tro_id;

PRINT N'=== DANH SÁCH NGƯỜI DÙNG ===';
SELECT nguoi_dung_id, tai_khoan, ho_ten, 
       CASE WHEN hoat_dong = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai
FROM dbo.nguoi_dung 
ORDER BY nguoi_dung_id;

PRINT N'=== NGƯỜI DÙNG VÀ VAI TRÒ ===';
SELECT nd.tai_khoan, nd.ho_ten, vt.ma AS ma_vai_tro, vt.ten AS ten_vai_tro
FROM dbo.nguoi_dung nd
INNER JOIN dbo.nguoi_dung_vai_tro ndvt ON nd.nguoi_dung_id = ndvt.nguoi_dung_id
INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
ORDER BY nd.tai_khoan, vt.ma;

PRINT N'=== NGƯỜI DÙNG VÀ CHI NHÁNH ===';
SELECT nd.tai_khoan, nd.ho_ten, cn.ten AS ten_chi_nhanh, cn.dia_chi
FROM dbo.nguoi_dung nd
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
ORDER BY nd.tai_khoan, cn.ten;
GO

PRINT N'Hoàn tất thêm dữ liệu mẫu cho quản lý nhân viên!';
