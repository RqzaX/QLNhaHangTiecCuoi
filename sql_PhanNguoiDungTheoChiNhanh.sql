/* ============================================================
   PHÂN NGƯỜI DÙNG THEO CHI NHÁNH
   Script để xem danh sách người dùng được phân theo từng chi nhánh
   ============================================================ */

USE QL_NhaHangTiecCuoi_V3;
GO

/* ========================
   1) DANH SÁCH NGƯỜI DÙNG THEO TỪNG CHI NHÁNH
   ======================== */
PRINT N'=== DANH SÁCH NGƯỜI DÙNG THEO CHI NHÁNH ===';
SELECT 
  cn.chi_nhanh_id,
  cn.ten AS ten_chi_nhanh,
  cn.dia_chi,
  cn.sdt,
  COUNT(DISTINCT ndcn.nguoi_dung_id) AS so_luong_nguoi_dung,
  STRING_AGG(nd.ho_ten, N', ') WITHIN GROUP (ORDER BY nd.ho_ten) AS danh_sach_nguoi_dung
FROM dbo.chi_nhanh cn
LEFT JOIN dbo.nguoi_dung_chi_nhanh ndcn ON cn.chi_nhanh_id = ndcn.chi_nhanh_id
LEFT JOIN dbo.nguoi_dung nd ON ndcn.nguoi_dung_id = nd.nguoi_dung_id
WHERE cn.trang_thai = 1
GROUP BY cn.chi_nhanh_id, cn.ten, cn.dia_chi, cn.sdt
ORDER BY cn.chi_nhanh_id;
GO

/* ========================
   2) CHI TIẾT NGƯỜI DÙNG THEO TỪNG CHI NHÁNH
   ======================== */
PRINT N'=== CHI TIẾT NGƯỜI DÙNG THEO CHI NHÁNH ===';
SELECT 
  cn.chi_nhanh_id,
  cn.ten AS ten_chi_nhanh,
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  ISNULL((
    SELECT TOP 1 vt.ten 
    FROM dbo.nguoi_dung_vai_tro ndvt 
    INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
    WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
    ORDER BY vt.vai_tro_id
  ), N'Chưa phân quyền') AS chuc_vu,
  CASE WHEN nd.hoat_dong = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai
FROM dbo.chi_nhanh cn
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON cn.chi_nhanh_id = ndcn.chi_nhanh_id
INNER JOIN dbo.nguoi_dung nd ON ndcn.nguoi_dung_id = nd.nguoi_dung_id
WHERE cn.trang_thai = 1
ORDER BY cn.chi_nhanh_id, nd.ho_ten;
GO

/* ========================
   3) PHÂN NGƯỜI DÙNG THEO CHI NHÁNH ID (DETAILED)
   ======================== */
PRINT N'=== PHÂN NGƯỜI DÙNG THEO CHI NHÁNH ID ===';

-- Chi nhánh ID = 1
PRINT N'--- CHI NHÁNH ID = 1 ---';
SELECT 
  cn.chi_nhanh_id,
  cn.ten AS ten_chi_nhanh,
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  ISNULL((
    SELECT TOP 1 vt.ten 
    FROM dbo.nguoi_dung_vai_tro ndvt 
    INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
    WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
    ORDER BY vt.vai_tro_id
  ), N'Chưa phân quyền') AS chuc_vu
FROM dbo.chi_nhanh cn
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON cn.chi_nhanh_id = ndcn.chi_nhanh_id
INNER JOIN dbo.nguoi_dung nd ON ndcn.nguoi_dung_id = nd.nguoi_dung_id
WHERE cn.chi_nhanh_id = 1 AND cn.trang_thai = 1
ORDER BY nd.ho_ten;

-- Chi nhánh ID = 2
PRINT N'--- CHI NHÁNH ID = 2 ---';
SELECT 
  cn.chi_nhanh_id,
  cn.ten AS ten_chi_nhanh,
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  ISNULL((
    SELECT TOP 1 vt.ten 
    FROM dbo.nguoi_dung_vai_tro ndvt 
    INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
    WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
    ORDER BY vt.vai_tro_id
  ), N'Chưa phân quyền') AS chuc_vu
FROM dbo.chi_nhanh cn
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON cn.chi_nhanh_id = ndcn.chi_nhanh_id
INNER JOIN dbo.nguoi_dung nd ON ndcn.nguoi_dung_id = nd.nguoi_dung_id
WHERE cn.chi_nhanh_id = 2 AND cn.trang_thai = 1
ORDER BY nd.ho_ten;
GO

/* ========================
   4) TỔNG HỢP SỐ LƯỢNG NGƯỜI DÙNG THEO CHI NHÁNH
   ======================== */
PRINT N'=== TỔNG HỢP SỐ LƯỢNG NGƯỜI DÙNG THEO CHI NHÁNH ===';
SELECT 
  cn.chi_nhanh_id,
  cn.ten AS ten_chi_nhanh,
  COUNT(DISTINCT ndcn.nguoi_dung_id) AS tong_so_nguoi_dung,
  COUNT(DISTINCT CASE WHEN nd.hoat_dong = 1 THEN ndcn.nguoi_dung_id END) AS so_nguoi_dung_hoat_dong,
  COUNT(DISTINCT CASE WHEN nd.hoat_dong = 0 THEN ndcn.nguoi_dung_id END) AS so_nguoi_dung_khong_hoat_dong
FROM dbo.chi_nhanh cn
LEFT JOIN dbo.nguoi_dung_chi_nhanh ndcn ON cn.chi_nhanh_id = ndcn.chi_nhanh_id
LEFT JOIN dbo.nguoi_dung nd ON ndcn.nguoi_dung_id = nd.nguoi_dung_id
WHERE cn.trang_thai = 1
GROUP BY cn.chi_nhanh_id, cn.ten
ORDER BY cn.chi_nhanh_id;
GO

/* ========================
   5) NGƯỜI DÙNG CÓ NHIỀU CHI NHÁNH
   ======================== */
PRINT N'=== NGƯỜI DÙNG CÓ NHIỀU CHI NHÁNH ===';
SELECT 
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  COUNT(ndcn.chi_nhanh_id) AS so_chi_nhanh,
  STRING_AGG(cn.ten, N', ') WITHIN GROUP (ORDER BY cn.ten) AS danh_sach_chi_nhanh
FROM dbo.nguoi_dung nd
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
WHERE cn.trang_thai = 1
GROUP BY nd.nguoi_dung_id, nd.tai_khoan, nd.ho_ten
HAVING COUNT(ndcn.chi_nhanh_id) > 1
ORDER BY so_chi_nhanh DESC, nd.ho_ten;
GO

/* ========================
   6) NGƯỜI DÙNG CHƯA CÓ CHI NHÁNH
   ======================== */
PRINT N'=== NGƯỜI DÙNG CHƯA CÓ CHI NHÁNH ===';
SELECT 
  nd.nguoi_dung_id,
  nd.tai_khoan,
  nd.ho_ten,
  CASE WHEN nd.hoat_dong = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai
FROM dbo.nguoi_dung nd
WHERE nd.hoat_dong = 1
  AND NOT EXISTS (
    SELECT 1 
    FROM dbo.nguoi_dung_chi_nhanh ndcn 
    WHERE ndcn.nguoi_dung_id = nd.nguoi_dung_id
  )
ORDER BY nd.ho_ten;
GO

/* ========================
   7) STORED PROCEDURE: LẤY NGƯỜI DÙNG THEO CHI NHÁNH ID
   ======================== */
IF OBJECT_ID('dbo.sp_LayNguoiDungTheoChiNhanh', 'P') IS NOT NULL
  DROP PROCEDURE dbo.sp_LayNguoiDungTheoChiNhanh;
GO

CREATE PROCEDURE dbo.sp_LayNguoiDungTheoChiNhanh
  @chi_nhanh_id INT
AS
BEGIN
  SET NOCOUNT ON;
  
  SELECT 
    nd.nguoi_dung_id,
    nd.tai_khoan,
    nd.ho_ten,
    ISNULL((
      SELECT TOP 1 vt.ten 
      FROM dbo.nguoi_dung_vai_tro ndvt 
      INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
      WHERE ndvt.nguoi_dung_id = nd.nguoi_dung_id
      ORDER BY vt.vai_tro_id
    ), N'Chưa phân quyền') AS chuc_vu,
    CASE WHEN nd.hoat_dong = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai
  FROM dbo.nguoi_dung nd
  INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
  WHERE ndcn.chi_nhanh_id = @chi_nhanh_id
    AND nd.hoat_dong = 1
  ORDER BY nd.ho_ten;
END;
GO

PRINT N'Đã tạo stored procedure sp_LayNguoiDungTheoChiNhanh!';
GO

/* ========================
   VÍ DỤ SỬ DỤNG STORED PROCEDURE:
   ========================
   
-- Lấy danh sách người dùng của chi nhánh ID = 1
EXEC dbo.sp_LayNguoiDungTheoChiNhanh @chi_nhanh_id = 1;

-- Lấy danh sách người dùng của chi nhánh ID = 2
EXEC dbo.sp_LayNguoiDungTheoChiNhanh @chi_nhanh_id = 2;

*/
GO

PRINT N'Hoàn tất script phân người dùng theo chi nhánh!';
GO

