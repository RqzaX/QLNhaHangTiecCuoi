-- ======================================================================
-- STORED PROCEDURE: BÁO CÁO KHÁCH HÀNG
-- Mục đích: Lấy dữ liệu khách hàng để in báo cáo
-- Trả về 1 result set duy nhất với tất cả thông tin cần thiết
-- ======================================================================

USE QL_NhaHangTiecCuoi_V3;
GO

IF OBJECT_ID('dbo.sp_BaoCaoKhachHang', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_BaoCaoKhachHang;
GO

CREATE PROCEDURE dbo.sp_BaoCaoKhachHang
    @HangCode NVARCHAR(10) = NULL,  -- NULL = tất cả hạng, 'MEM'/'BAC'/'VANG'/'VIP' = lọc theo hạng
    @TuNgay DATE = NULL,              -- NULL = không lọc theo ngày
    @DenNgay DATE = NULL              -- NULL = không lọc theo ngày
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Trả về 1 result set duy nhất với tất cả thông tin cần thiết
    SELECT 
        -- Thông tin header báo cáo (dùng cho mọi dòng)
        CONVERT(VARCHAR(10), GETDATE(), 103) AS ngay_bao_cao,
        CONVERT(VARCHAR(8), GETDATE(), 108) AS gio_bao_cao,
        CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) AS thoi_gian_bao_cao,
        CASE 
            WHEN @HangCode IS NULL THEN N'TẤT CẢ HẠNG KHÁCH HÀNG'
            ELSE (SELECT ten_hang FROM dbo.dm_hang_kh WHERE hang_code = @HangCode)
        END AS tieu_de_bao_cao,
        CASE 
            WHEN @HangCode IS NULL THEN NULL
            ELSE @HangCode
        END AS hang_code_loc,
        
        -- Chi tiết khách hàng (dùng trong Details section)
        ROW_NUMBER() OVER (ORDER BY kh.ho_ten) AS stt,
        kh.khach_hang_id,
        kh.ho_ten,
        ISNULL(kh.sdt, N'') AS sdt,
        ISNULL(kh.email, N'') AS email,
        CASE 
            WHEN kh.ngay_sinh IS NOT NULL THEN FORMAT(kh.ngay_sinh, 'dd/MM/yyyy')
            ELSE N''
        END AS ngay_sinh,
        CASE 
            WHEN kh.ngay_sinh IS NOT NULL THEN DATEDIFF(YEAR, kh.ngay_sinh, GETDATE())
            ELSE NULL
        END AS tuoi,
        ISNULL(kh.hang_code, N'MEM') AS hang_code,
        ISNULL(dm.ten_hang, N'Thành viên') AS ten_hang,
        ISNULL(dm.min_tich_luy, 0) AS min_tich_luy,
        ISNULL(kh.tong_chi_tieu, 0) AS tong_chi_tieu,
        FORMAT(ISNULL(kh.tong_chi_tieu, 0), 'N0') AS tong_chi_tieu_hien_thi,
        ISNULL(kh.so_lan_den, 0) AS so_lan_den,
        ISNULL(kh.diem, 0) AS diem,
        CASE 
            WHEN kh.lan_cuoi_den IS NOT NULL THEN FORMAT(kh.lan_cuoi_den, 'dd/MM/yyyy')
            ELSE N'Chưa có'
        END AS lan_cuoi_den,
        CASE 
            WHEN kh.lan_cuoi_den IS NOT NULL THEN DATEDIFF(DAY, kh.lan_cuoi_den, GETDATE())
            ELSE NULL
        END AS so_ngay_chua_den,
        ISNULL(kh.ghi_chu, N'') AS ghi_chu,
        
        -- Thông tin phân tích
        CASE 
            WHEN ISNULL(kh.tong_chi_tieu, 0) >= ISNULL(dm.min_tich_luy, 0) THEN N'Đạt hạng'
            ELSE N'Chưa đạt'
        END AS trang_thai_hang,
        CASE 
            WHEN ISNULL(kh.tong_chi_tieu, 0) >= ISNULL(dm.min_tich_luy, 0) THEN 1
            ELSE 0
        END AS ma_trang_thai_hang,
        -- Tính số tiền còn thiếu để lên hạng tiếp theo
        CASE 
            WHEN dm.thu_tu < (SELECT MAX(thu_tu) FROM dbo.dm_hang_kh) THEN
                (SELECT MIN(min_tich_luy) FROM dbo.dm_hang_kh WHERE thu_tu > dm.thu_tu) - ISNULL(kh.tong_chi_tieu, 0)
            ELSE 0
        END AS so_tien_con_thieu,
        -- Hạng tiếp theo
        CASE 
            WHEN dm.thu_tu < (SELECT MAX(thu_tu) FROM dbo.dm_hang_kh) THEN
                (SELECT ten_hang FROM dbo.dm_hang_kh WHERE thu_tu = (SELECT MIN(thu_tu) FROM dbo.dm_hang_kh WHERE thu_tu > dm.thu_tu))
            ELSE N'Đã đạt hạng cao nhất'
        END AS hang_tiep_theo
        
    FROM dbo.khach_hang kh
    LEFT JOIN dbo.dm_hang_kh dm ON kh.hang_code = dm.hang_code
    WHERE 
        -- Lọc theo hạng
        (@HangCode IS NULL OR kh.hang_code = @HangCode)
        -- Lọc theo ngày (chỉ áp dụng nếu cả 2 parameters đều có giá trị)
        AND (
            @TuNgay IS NULL OR @DenNgay IS NULL  -- Nếu một trong hai NULL → không lọc theo ngày
            OR (kh.lan_cuoi_den IS NOT NULL AND kh.lan_cuoi_den BETWEEN @TuNgay AND @DenNgay)  -- Có ngày và khách hàng có lan_cuoi_den trong khoảng
            OR (kh.lan_cuoi_den IS NULL)  -- Nếu khách hàng chưa có lan_cuoi_den, vẫn hiển thị (nếu không lọc theo ngày)
        )
    ORDER BY 
        CASE WHEN @HangCode IS NULL THEN dm.thu_tu END,
        kh.ho_ten;
END;
GO

-- ======================================================================
-- STORED PROCEDURE: BÁO CÁO KHÁCH HÀNG (PHIÊN BẢN ĐẦY ĐỦ - NHIỀU RESULT SET)
-- Mục đích: Trả về nhiều result set để có thể tạo report phức tạp hơn
-- ======================================================================

IF OBJECT_ID('dbo.sp_BaoCaoKhachHang_Full', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_BaoCaoKhachHang_Full;
GO

CREATE PROCEDURE dbo.sp_BaoCaoKhachHang_Full
    @HangCode NVARCHAR(10) = NULL,
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @TuNgay IS NULL
        SET @TuNgay = CAST(GETDATE() AS DATE);
    IF @DenNgay IS NULL
        SET @DenNgay = CAST(GETDATE() AS DATE);
    
    -- Result Set 1: Thông tin header báo cáo
    SELECT 
        CONVERT(VARCHAR(10), GETDATE(), 103) AS ngay_bao_cao,
        CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) AS thoi_gian_in,
        CASE 
            WHEN @HangCode IS NULL THEN N'TẤT CẢ HẠNG KHÁCH HÀNG'
            ELSE (SELECT ten_hang FROM dbo.dm_hang_kh WHERE hang_code = @HangCode)
        END AS tieu_de_bao_cao,
        CASE 
            WHEN @HangCode IS NULL THEN NULL
            ELSE @HangCode
        END AS hang_code_loc,
        (SELECT COUNT(*) FROM dbo.khach_hang 
         WHERE (@HangCode IS NULL OR hang_code = @HangCode)) AS tong_so_khach_hang,
        (SELECT SUM(ISNULL(tong_chi_tieu, 0)) FROM dbo.khach_hang 
         WHERE (@HangCode IS NULL OR hang_code = @HangCode)) AS tong_doanh_thu;
    
    -- Result Set 2: Chi tiết khách hàng
    SELECT 
        ROW_NUMBER() OVER (ORDER BY kh.ho_ten) AS stt,
        kh.khach_hang_id,
        kh.ho_ten,
        ISNULL(kh.sdt, N'') AS sdt,
        ISNULL(kh.email, N'') AS email,
        CASE 
            WHEN kh.ngay_sinh IS NOT NULL THEN FORMAT(kh.ngay_sinh, 'dd/MM/yyyy')
            ELSE N''
        END AS ngay_sinh,
        CASE 
            WHEN kh.ngay_sinh IS NOT NULL THEN DATEDIFF(YEAR, kh.ngay_sinh, GETDATE())
            ELSE NULL
        END AS tuoi,
        ISNULL(kh.hang_code, N'MEM') AS hang_code,
        ISNULL(dm.ten_hang, N'Thành viên') AS ten_hang,
        ISNULL(kh.tong_chi_tieu, 0) AS tong_chi_tieu,
        FORMAT(ISNULL(kh.tong_chi_tieu, 0), 'N0') AS tong_chi_tieu_hien_thi,
        ISNULL(kh.so_lan_den, 0) AS so_lan_den,
        ISNULL(kh.diem, 0) AS diem,
        CASE 
            WHEN kh.lan_cuoi_den IS NOT NULL THEN FORMAT(kh.lan_cuoi_den, 'dd/MM/yyyy')
            ELSE N'Chưa có'
        END AS lan_cuoi_den,
        ISNULL(kh.ghi_chu, N'') AS ghi_chu
    FROM dbo.khach_hang kh
    LEFT JOIN dbo.dm_hang_kh dm ON kh.hang_code = dm.hang_code
    WHERE 
        (@HangCode IS NULL OR kh.hang_code = @HangCode)
        AND (@TuNgay IS NULL OR @DenNgay IS NULL OR 
             (kh.lan_cuoi_den IS NOT NULL AND kh.lan_cuoi_den BETWEEN @TuNgay AND @DenNgay)
             OR (kh.lan_cuoi_den IS NULL AND @TuNgay IS NULL AND @DenNgay IS NULL))
    ORDER BY 
        CASE WHEN @HangCode IS NULL THEN dm.thu_tu END,
        kh.ho_ten;
    
    -- Result Set 3: Tổng hợp thống kê theo hạng
    SELECT 
        dm.hang_code,
        dm.ten_hang,
        COUNT(kh.khach_hang_id) AS so_luong_khach_hang,
        SUM(ISNULL(kh.tong_chi_tieu, 0)) AS tong_chi_tieu,
        AVG(ISNULL(kh.tong_chi_tieu, 0)) AS trung_binh_chi_tieu,
        SUM(ISNULL(kh.so_lan_den, 0)) AS tong_so_lan_den,
        AVG(ISNULL(kh.so_lan_den, 0)) AS trung_binh_lan_den
    FROM dbo.dm_hang_kh dm
    LEFT JOIN dbo.khach_hang kh ON dm.hang_code = kh.hang_code
        AND (@HangCode IS NULL OR kh.hang_code = @HangCode)
    GROUP BY dm.hang_code, dm.ten_hang, dm.thu_tu
    ORDER BY dm.thu_tu;
END;
GO

