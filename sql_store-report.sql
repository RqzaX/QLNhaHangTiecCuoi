-- ======================================================================
-- STORED PROCEDURE: Lấy dữ liệu in phiếu đơn bếp (KOT)
-- Mục đích: Lấy thông tin chi tiết KOT để in danh sách món ăn cho bếp
-- ======================================================================

USE QL_NhaHangTiecCuoi_V3;
GO

-- Stored Procedure: Lấy thông tin KOT để in
IF OBJECT_ID('dbo.sp_LayThongTinKOTIn', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_LayThongTinKOTIn;
GO

CREATE PROCEDURE dbo.sp_LayThongTinKOTIn
    @KOTId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Lấy thông tin header của KOT
    SELECT 
        po.phieu_order_id AS kot_id,
        'KOT' + RIGHT('000' + CAST(po.phieu_order_id AS VARCHAR), 3) AS ma_kot,
        po.chi_nhanh_id,
        cn.ten AS ten_chi_nhanh,
        po.ban_id,
        CASE 
            WHEN po.ban_id IS NOT NULL THEN ISNULL(b.so_ban, N'TIỆC')
            WHEN po.dat_sanh_id IS NOT NULL THEN N'TIỆC'
            ELSE N'TIỆC'
        END AS so_ban,
        CASE 
            WHEN po.ban_id IS NOT NULL AND b.so_ban != N'TIỆC' THEN N'Bàn ' + b.so_ban
            WHEN po.dat_sanh_id IS NOT NULL THEN N'Tiệc cưới'
            ELSE N'Tiệc cưới'
        END AS table_name,
        po.dat_sanh_id,
        ISNULL(s.ten_sanh, N'') AS ten_sanh,
        po.ngay_gio AS thoi_gian_dat,
        po.trang_thai,
        CASE 
            WHEN po.trang_thai = N'ĐANG PHỤC VỤ' THEN N'Chờ làm'
            WHEN po.trang_thai = N'CHỜ THANH TOÁN' THEN N'Đang làm'
            WHEN po.trang_thai = N'ĐÃ ĐÓNG' THEN N'Sẵn sàng'
            ELSE po.trang_thai
        END AS trang_thai_hien_thi,
        CASE 
            WHEN po.ban_id IS NOT NULL THEN N'BẾP'
            ELSE N'BAR'
        END AS loai_kot,
        ISNULL(ds.ghi_chu, N'') AS ghi_chu_don,
        0 AS uu_tien, -- Mặc định, có thể thêm cột sau
        po.nhan_vien
    FROM phieu_order po
    INNER JOIN chi_nhanh cn ON po.chi_nhanh_id = cn.chi_nhanh_id
    LEFT JOIN ban b ON po.ban_id = b.ban_id
    LEFT JOIN dat_sanh ds ON po.dat_sanh_id = ds.dat_sanh_id
    LEFT JOIN sanh s ON ds.sanh_id = s.sanh_id
    WHERE po.phieu_order_id = @KOTId;
    
    -- Lấy danh sách món ăn trong KOT
    SELECT 
        poc.order_ct_id AS stt,
        poc.mon_id,
        m.ma_mon,
        m.ten_mon,
        poc.so_luong,
        CAST(poc.so_luong AS INT) AS so_luong_int, -- Để hiển thị số nguyên
        poc.don_gia,
        poc.thanh_tien,
        ISNULL(poc.ghi_chu_bep, N'') AS ghi_chu_bep,
        m.nhom AS nhom_mon,
        m.don_vi_tinh
    FROM phieu_order_ct poc
    INNER JOIN mon_an m ON poc.mon_id = m.mon_id
    WHERE poc.phieu_order_id = @KOTId
    ORDER BY poc.order_ct_id;
    
    -- Lấy tổng hợp thống kê
    SELECT 
        COUNT(*) AS tong_so_mon,
        SUM(CAST(poc.so_luong AS INT)) AS tong_so_luong,
        SUM(poc.thanh_tien) AS tong_tien
    FROM phieu_order_ct poc
    WHERE poc.phieu_order_id = @KOTId;
END;
GO

-- ======================================================================
-- STORED PROCEDURE: Lấy danh sách KOT theo điều kiện (cho màn hình bếp)
-- ======================================================================

IF OBJECT_ID('dbo.sp_LayDanhSachKOTBep', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_LayDanhSachKOTBep;
GO

CREATE PROCEDURE dbo.sp_LayDanhSachKOTBep
    @ChiNhanhId INT,
    @TrangThai NVARCHAR(20) = NULL,
    @LoaiKOT NVARCHAR(10) = NULL -- 'BẾP' hoặc 'BAR'
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        po.phieu_order_id AS kot_id,
        'KOT' + RIGHT('000' + CAST(po.phieu_order_id AS VARCHAR), 3) AS ma_kot,
        po.ban_id,
        CASE 
            WHEN po.ban_id IS NOT NULL THEN ISNULL(b.so_ban, N'TIỆC')
            WHEN po.dat_sanh_id IS NOT NULL THEN N'TIỆC'
            ELSE N'TIỆC'
        END AS so_ban,
        CASE 
            WHEN po.ban_id IS NOT NULL AND b.so_ban != N'TIỆC' THEN N'Bàn ' + b.so_ban
            WHEN po.dat_sanh_id IS NOT NULL THEN N'Tiệc cưới'
            ELSE N'Tiệc cưới'
        END AS table_name,
        po.ngay_gio AS thoi_gian_dat,
        po.trang_thai,
        CASE 
            WHEN po.ban_id IS NOT NULL THEN N'BẾP'
            ELSE N'BAR'
        END AS loai_kot,
        ISNULL(ds.ghi_chu, N'') AS ghi_chu,
        0 AS uu_tien,
        -- Thống kê số món
        (SELECT COUNT(*) FROM phieu_order_ct WHERE phieu_order_id = po.phieu_order_id) AS so_mon
    FROM phieu_order po
    LEFT JOIN ban b ON po.ban_id = b.ban_id
    LEFT JOIN dat_sanh ds ON po.dat_sanh_id = ds.dat_sanh_id
    WHERE po.chi_nhanh_id = @ChiNhanhId
        AND po.trang_thai != N'ĐÃ ĐÓNG' -- Không lấy đơn đã đóng
        AND (@TrangThai IS NULL OR po.trang_thai = @TrangThai)
        AND (
            @LoaiKOT IS NULL 
            OR (@LoaiKOT = N'BẾP' AND po.ban_id IS NOT NULL)
            OR (@LoaiKOT = N'BAR' AND po.ban_id IS NULL)
        )
    ORDER BY po.ngay_gio DESC;
END;
GO

-- ======================================================================
-- STORED PROCEDURE: Lấy chi tiết món ăn của KOT (cho in)
-- ======================================================================

IF OBJECT_ID('dbo.sp_LayChiTietMonAnKOT', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_LayChiTietMonAnKOT;
GO

CREATE PROCEDURE dbo.sp_LayChiTietMonAnKOT
    @KOTId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ROW_NUMBER() OVER (ORDER BY poc.order_ct_id) AS stt,
        poc.order_ct_id,
        poc.mon_id,
        m.ma_mon,
        m.ten_mon,
        poc.so_luong,
        CAST(poc.so_luong AS INT) AS so_luong_int,
        poc.don_gia,
        poc.thanh_tien,
        ISNULL(poc.ghi_chu_bep, N'') AS ghi_chu_bep,
        m.nhom AS nhom_mon,
        m.don_vi_tinh,
        CASE 
            WHEN ISNULL(poc.ghi_chu_bep, N'') != N'' THEN 1
            ELSE 0
        END AS co_ghi_chu
    FROM phieu_order_ct poc
    INNER JOIN mon_an m ON poc.mon_id = m.mon_id
    WHERE poc.phieu_order_id = @KOTId
    ORDER BY poc.order_ct_id;
END;
GO

-- ======================================================================
-- STORED PROCEDURE: Lấy thông tin đầy đủ để in phiếu bếp (tất cả trong 1)
-- Phiên bản tối ưu: Trả về tất cả thông tin cần thiết trong các result set
-- ======================================================================

IF OBJECT_ID('dbo.sp_InPhieuDonBep', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_InPhieuDonBep;
GO

CREATE PROCEDURE dbo.sp_InPhieuDonBep
    @KOTId INT,
    @NguoiIn NVARCHAR(100) = NULL -- Người đang thực hiện in phiếu
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Result Set 1: Thông tin header KOT
    SELECT 
        po.phieu_order_id AS kot_id,
        'KOT' + RIGHT('000' + CAST(po.phieu_order_id AS VARCHAR), 3) AS ma_kot,
        cn.ten AS ten_nha_hang,
        CASE 
            WHEN po.ban_id IS NOT NULL AND b.so_ban != N'TIỆC' THEN N'Bàn ' + b.so_ban
            WHEN po.dat_sanh_id IS NOT NULL THEN N'Tiệc cưới'
            ELSE N'Tiệc cưới'
        END AS table_name,
        po.ngay_gio AS thoi_gian_dat,
        FORMAT(po.ngay_gio, 'HH:mm dd/MM/yyyy') AS thoi_gian_hien_thi,
        po.trang_thai,
        CASE 
            WHEN po.trang_thai = N'ĐANG PHỤC VỤ' THEN N'Chờ làm'
            WHEN po.trang_thai = N'CHỜ THANH TOÁN' THEN N'Đang làm'
            WHEN po.trang_thai = N'ĐÃ ĐÓNG' THEN N'Sẵn sàng'
            ELSE po.trang_thai
        END AS trang_thai_hien_thi,
        ISNULL(ds.ghi_chu, N'') AS ghi_chu_don,
        CASE 
            WHEN po.ban_id IS NOT NULL THEN N'BẾP'
            ELSE N'BAR'
        END AS loai_kot,
        0 AS uu_tien,
        ISNULL(s.ten_sanh, N'') AS ten_sanh,
        -- Tổng hợp số món và số lượng
        (SELECT COUNT(*) FROM phieu_order_ct WHERE phieu_order_id = po.phieu_order_id) AS tong_so_mon,
        (SELECT SUM(CAST(so_luong AS INT)) FROM phieu_order_ct WHERE phieu_order_id = po.phieu_order_id) AS tong_so_luong,
        -- Thông tin người in
        ISNULL(@NguoiIn, ISNULL(po.nhan_vien, N'')) AS nguoi_in,
        FORMAT(GETDATE(), 'HH:mm:ss dd/MM/yyyy') AS thoi_gian_in
    FROM phieu_order po
    INNER JOIN chi_nhanh cn ON po.chi_nhanh_id = cn.chi_nhanh_id
    LEFT JOIN ban b ON po.ban_id = b.ban_id
    LEFT JOIN dat_sanh ds ON po.dat_sanh_id = ds.dat_sanh_id
    LEFT JOIN sanh s ON ds.sanh_id = s.sanh_id
    WHERE po.phieu_order_id = @KOTId;
    
    -- Result Set 2: Danh sách món ăn
    SELECT 
        ROW_NUMBER() OVER (ORDER BY poc.order_ct_id) AS stt,
        poc.mon_id,
        m.ten_mon,
        CAST(poc.so_luong AS INT) AS so_luong,
        ISNULL(poc.ghi_chu_bep, N'') AS ghi_chu_bep,
        CASE 
            WHEN ISNULL(poc.ghi_chu_bep, N'') != N'' THEN 1
            ELSE 0
        END AS co_ghi_chu
    FROM phieu_order_ct poc
    INNER JOIN mon_an m ON poc.mon_id = m.mon_id
    WHERE poc.phieu_order_id = @KOTId
    ORDER BY poc.order_ct_id;
    
    -- Result Set 3: Tổng hợp
    SELECT 
        COUNT(*) AS tong_so_mon,
        SUM(CAST(poc.so_luong AS INT)) AS tong_so_luong
    FROM phieu_order_ct poc
    WHERE poc.phieu_order_id = @KOTId;
END;
GO


