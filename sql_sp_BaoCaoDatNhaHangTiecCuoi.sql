-- ======================================================================
-- STORED PROCEDURE: BÁO CÁO ĐẶT NHÀ HÀNG VÀ TIỆC CƯỚI
-- Mục đích: Báo cáo số lượng đặt nhà hàng/tiệc cưới, tổng số tiền, tổng doanh thu
-- Dựa trên các hóa đơn đã thanh toán
-- ======================================================================

USE QL_NhaHangTiecCuoi_V3;
GO

IF OBJECT_ID('dbo.sp_BaoCaoDatNhaHangTiecCuoi', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_BaoCaoDatNhaHangTiecCuoi;
GO

CREATE PROCEDURE dbo.sp_BaoCaoDatNhaHangTiecCuoi
    @ChiNhanhId INT = NULL,          -- NULL = tất cả chi nhánh
    @TuNgay DATE = NULL,              -- NULL = không lọc theo ngày
    @DenNgay DATE = NULL,             -- NULL = không lọc theo ngày
    @Loai NVARCHAR(15) = NULL         -- NULL = tất cả, 'NHAHANG' hoặc 'TIECCUOI'
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Dùng CTE để tính trước ngày và tổng doanh thu
    WITH HoaDonWithInfo AS (
        SELECT 
            hd.hoa_don_id,
            hd.chi_nhanh_id,
            hd.khach_hang_id,
            hd.loai,
            hd.tham_chieu_id,
            hd.ngay_lap,
            hd.tong_truoc_thue,
            hd.tong_sau_thue,
            hd.giam_gia,
            hd.phi_dv,
            -- Tính ngày (ưu tiên ngày thanh toán)
            CONVERT(DATE, COALESCE(
                (SELECT TOP 1 ngay_tt FROM dbo.thanh_toan WHERE hoa_don_id = hd.hoa_don_id ORDER BY ngay_tt DESC),
                hd.ngay_lap
            )) AS ngay,
            -- Tổng doanh thu = tong_sau_thue (vì chỉ lấy hóa đơn đã thanh toán)
            ISNULL(hd.tong_sau_thue, 0) AS doanh_thu
        FROM dbo.hoa_don hd
        WHERE 
            hd.trang_thai = N'ĐÃ THANH TOÁN'
            AND (@ChiNhanhId IS NULL OR hd.chi_nhanh_id = @ChiNhanhId)
            AND (@Loai IS NULL OR hd.loai = @Loai)
            AND (
                @TuNgay IS NULL OR @DenNgay IS NULL
                OR CONVERT(DATE, COALESCE(
                    (SELECT TOP 1 ngay_tt FROM dbo.thanh_toan WHERE hoa_don_id = hd.hoa_don_id ORDER BY ngay_tt DESC),
                    hd.ngay_lap
                )) BETWEEN @TuNgay AND @DenNgay
            )
    )
    -- Trả về 1 result set với tất cả thông tin cần thiết
    SELECT 
        -- Thông tin header báo cáo (dùng cho mọi dòng)
        CONVERT(VARCHAR(10), GETDATE(), 103) AS ngay_bao_cao,
        CONVERT(VARCHAR(8), GETDATE(), 108) AS gio_bao_cao,
        CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) AS thoi_gian_bao_cao,
        CASE 
            WHEN @ChiNhanhId IS NULL THEN N'TẤT CẢ CHI NHÁNH'
            ELSE (SELECT ten FROM dbo.chi_nhanh WHERE chi_nhanh_id = @ChiNhanhId)
        END AS ten_chi_nhanh,
        CASE 
            WHEN @ChiNhanhId IS NULL THEN NULL
            ELSE @ChiNhanhId
        END AS chi_nhanh_id,
        CASE 
            WHEN @Loai IS NULL THEN N'TẤT CẢ LOẠI'
            WHEN @Loai = N'NHAHANG' THEN N'NHÀ HÀNG'
            WHEN @Loai = N'TIECCUOI' THEN N'TIỆC CƯỚI'
            ELSE @Loai
        END AS tieu_de_bao_cao,
        
        -- Chi tiết theo ngày (dùng trong Details section)
        hdwi.ngay,
        FORMAT(hdwi.ngay, 'dd/MM/yyyy') AS ngay_hien_thi,
        
        -- Thông tin chi nhánh
        hdwi.chi_nhanh_id,
        ISNULL(cn.ten, N'Không xác định') AS ten_cn,
        
        -- Loại (Nhà hàng hoặc Tiệc cưới)
        hdwi.loai,
        CASE 
            WHEN hdwi.loai = N'NHAHANG' THEN N'Nhà hàng'
            WHEN hdwi.loai = N'TIECCUOI' THEN N'Tiệc cưới'
            ELSE hdwi.loai
        END AS ten_loai,
        
        -- Số lượng đặt (COUNT)
        COUNT(*) AS so_luong_dat,
        
        -- Tổng số tiền (SUM tong_sau_thue)
        SUM(ISNULL(hdwi.tong_sau_thue, 0)) AS tong_so_tien,
        FORMAT(SUM(ISNULL(hdwi.tong_sau_thue, 0)), 'N0') AS tong_so_tien_hien_thi,
        
        -- Tổng doanh thu = Tổng số tiền (vì chỉ lấy hóa đơn đã thanh toán)
        SUM(ISNULL(hdwi.tong_sau_thue, 0)) AS tong_doanh_thu,
        FORMAT(SUM(ISNULL(hdwi.tong_sau_thue, 0)), 'N0') AS tong_doanh_thu_hien_thi,
        
        -- Tổng trước thuế
        SUM(ISNULL(hdwi.tong_truoc_thue, 0)) AS tong_truoc_thue,
        FORMAT(SUM(ISNULL(hdwi.tong_truoc_thue, 0)), 'N0') AS tong_truoc_thue_hien_thi,
        
        -- Tổng VAT
        SUM(ISNULL(hdwi.tong_sau_thue, 0) - ISNULL(hdwi.tong_truoc_thue, 0)) AS tong_vat,
        FORMAT(SUM(ISNULL(hdwi.tong_sau_thue, 0) - ISNULL(hdwi.tong_truoc_thue, 0)), 'N0') AS tong_vat_hien_thi,
        
        -- Tổng giảm giá
        SUM(ISNULL(hdwi.giam_gia, 0)) AS tong_giam_gia,
        FORMAT(SUM(ISNULL(hdwi.giam_gia, 0)), 'N0') AS tong_giam_gia_hien_thi,
        
        -- Tổng phí dịch vụ
        SUM(ISNULL(hdwi.phi_dv, 0)) AS tong_phi_dv,
        FORMAT(SUM(ISNULL(hdwi.phi_dv, 0)), 'N0') AS tong_phi_dv_hien_thi,
        
        -- Thông tin tham chiếu (bàn/sảnh) - BỎ so_ban_sanh
        CASE 
            WHEN hdwi.loai = N'NHAHANG' AND db.dat_ban_id IS NOT NULL AND b.so_ban IS NOT NULL THEN 
                CASE 
                    WHEN kv.ten_khu_vuc IS NOT NULL THEN kv.ten_khu_vuc + N' - ' + b.so_ban
                    ELSE b.so_ban
                END
            WHEN hdwi.loai = N'TIECCUOI' AND ds.dat_sanh_id IS NOT NULL AND s.ten_sanh IS NOT NULL THEN s.ten_sanh
            ELSE N'-'
        END AS ban_sanh,
        
        -- Thông tin khách hàng
        ISNULL(kh.ho_ten, N'Khách lẻ') AS ten_khach_hang,
        ISNULL(kh.sdt, N'') AS sdt_khach_hang
        
    FROM HoaDonWithInfo hdwi
    LEFT JOIN dbo.chi_nhanh cn ON hdwi.chi_nhanh_id = cn.chi_nhanh_id
    LEFT JOIN dbo.khach_hang kh ON hdwi.khach_hang_id = kh.khach_hang_id
    -- JOIN cho nhà hàng
    LEFT JOIN dbo.dat_ban db ON db.dat_ban_id = hdwi.tham_chieu_id AND hdwi.loai = N'NHAHANG'
    LEFT JOIN dbo.ban b ON b.ban_id = db.ban_id
    LEFT JOIN dbo.khu_vuc kv ON kv.khu_vuc_id = b.khu_vuc_id
    -- JOIN cho tiệc cưới
    LEFT JOIN dbo.dat_sanh ds ON ds.dat_sanh_id = hdwi.tham_chieu_id AND hdwi.loai = N'TIECCUOI'
    LEFT JOIN dbo.sanh s ON s.sanh_id = ds.sanh_id
    GROUP BY
        -- Nhóm theo ngày
        hdwi.ngay,
        -- Nhóm theo chi nhánh
        hdwi.chi_nhanh_id,
        cn.ten,
        -- Nhóm theo loại
        hdwi.loai,
        -- Thông tin tham chiếu
        db.dat_ban_id,
        b.so_ban,
        kv.ten_khu_vuc,
        ds.dat_sanh_id,
        s.ten_sanh,
        -- Thông tin khách hàng
        kh.ho_ten,
        kh.sdt
    ORDER BY
        -- Sắp xếp theo ngày (mới nhất trước)
        hdwi.ngay DESC,
        hdwi.loai,
        hdwi.chi_nhanh_id;
END;
GO

-- ======================================================================
-- STORED PROCEDURE: BÁO CÁO TỔNG HỢP ĐẶT NHÀ HÀNG VÀ TIỆC CƯỚI (THEO NGÀY)
-- Mục đích: Tổng hợp theo ngày, không chi tiết từng đơn
-- ======================================================================

IF OBJECT_ID('dbo.sp_BaoCaoTongHopDatNhaHangTiecCuoi', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_BaoCaoTongHopDatNhaHangTiecCuoi;
GO

CREATE PROCEDURE dbo.sp_BaoCaoTongHopDatNhaHangTiecCuoi
    @ChiNhanhId INT = NULL,
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL,
    @Loai NVARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Dùng CTE để tính trước ngày và tổng doanh thu
    WITH HoaDonWithInfo AS (
        SELECT 
            hd.hoa_don_id,
            hd.chi_nhanh_id,
            hd.loai,
            hd.tong_sau_thue,
            -- Tính ngày (ưu tiên ngày thanh toán)
            CONVERT(DATE, COALESCE(
                (SELECT TOP 1 ngay_tt FROM dbo.thanh_toan WHERE hoa_don_id = hd.hoa_don_id ORDER BY ngay_tt DESC),
                hd.ngay_lap
            )) AS ngay,
            -- Tổng doanh thu = tong_sau_thue (vì chỉ lấy hóa đơn đã thanh toán)
            ISNULL(hd.tong_sau_thue, 0) AS doanh_thu
        FROM dbo.hoa_don hd
        WHERE 
            hd.trang_thai = N'ĐÃ THANH TOÁN'
            AND (@ChiNhanhId IS NULL OR hd.chi_nhanh_id = @ChiNhanhId)
            AND (@Loai IS NULL OR hd.loai = @Loai)
            AND (
                @TuNgay IS NULL OR @DenNgay IS NULL
                OR CONVERT(DATE, COALESCE(
                    (SELECT TOP 1 ngay_tt FROM dbo.thanh_toan WHERE hoa_don_id = hd.hoa_don_id ORDER BY ngay_tt DESC),
                    hd.ngay_lap
                )) BETWEEN @TuNgay AND @DenNgay
            )
    )
    -- Result Set 1: Header báo cáo
    SELECT 
        CONVERT(VARCHAR(10), GETDATE(), 103) AS ngay_bao_cao,
        CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) AS thoi_gian_in,
        CASE 
            WHEN @ChiNhanhId IS NULL THEN N'TẤT CẢ CHI NHÁNH'
            ELSE (SELECT ten FROM dbo.chi_nhanh WHERE chi_nhanh_id = @ChiNhanhId)
        END AS ten_chi_nhanh,
        CASE 
            WHEN @Loai IS NULL THEN N'TẤT CẢ LOẠI'
            WHEN @Loai = N'NHAHANG' THEN N'NHÀ HÀNG'
            WHEN @Loai = N'TIECCUOI' THEN N'TIỆC CƯỚI'
            ELSE @Loai
        END AS tieu_de_bao_cao,
        (SELECT COUNT(*) FROM HoaDonWithInfo) AS tong_so_don;
    
    -- Result Set 2: Chi tiết theo ngày
    SELECT 
        hdwi.ngay,
        FORMAT(hdwi.ngay, 'dd/MM/yyyy') AS ngay_hien_thi,
        hdwi.loai,
        CASE 
            WHEN hdwi.loai = N'NHAHANG' THEN N'Nhà hàng'
            WHEN hdwi.loai = N'TIECCUOI' THEN N'Tiệc cưới'
            ELSE hdwi.loai
        END AS ten_loai,
        COUNT(*) AS so_luong_dat,
        SUM(ISNULL(hdwi.tong_sau_thue, 0)) AS tong_so_tien,
        FORMAT(SUM(ISNULL(hdwi.tong_sau_thue, 0)), 'N0') AS tong_so_tien_hien_thi,
        -- Tổng doanh thu = Tổng số tiền (vì chỉ lấy hóa đơn đã thanh toán)
        SUM(ISNULL(hdwi.tong_sau_thue, 0)) AS tong_doanh_thu,
        FORMAT(SUM(ISNULL(hdwi.tong_sau_thue, 0)), 'N0') AS tong_doanh_thu_hien_thi
    FROM HoaDonWithInfo hdwi
    GROUP BY
        hdwi.ngay,
        hdwi.loai
    ORDER BY
        hdwi.ngay DESC,
        hdwi.loai;
    
    -- Result Set 3: Tổng hợp theo loại
    SELECT 
        hdwi.loai,
        CASE 
            WHEN hdwi.loai = N'NHAHANG' THEN N'Nhà hàng'
            WHEN hdwi.loai = N'TIECCUOI' THEN N'Tiệc cưới'
            ELSE hdwi.loai
        END AS ten_loai,
        COUNT(*) AS so_luong_dat,
        SUM(ISNULL(hdwi.tong_sau_thue, 0)) AS tong_so_tien,
        -- Tổng doanh thu = Tổng số tiền (vì chỉ lấy hóa đơn đã thanh toán)
        SUM(ISNULL(hdwi.tong_sau_thue, 0)) AS tong_doanh_thu,
        AVG(ISNULL(hdwi.tong_sau_thue, 0)) AS trung_binh_don_gia
    FROM HoaDonWithInfo hdwi
    GROUP BY hdwi.loai
    ORDER BY hdwi.loai;
END;
GO

