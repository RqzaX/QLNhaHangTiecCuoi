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
Go
--Bao Cao Khách Hàng

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
--Bao cao Ton Kho
IF OBJECT_ID('dbo.sp_BaoCaoTonKho_Final', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_BaoCaoTonKho_Final;
GO

CREATE PROCEDURE dbo.sp_BaoCaoTonKho_Final
    @ChiNhanhId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Trả về 1 result set duy nhất với tất cả thông tin cần thiết
    SELECT 
        -- Thông tin chi nhánh (dùng LEFT JOIN và ISNULL để đảm bảo luôn có giá trị)
        @ChiNhanhId AS chi_nhanh_id,
        ISNULL(cn.ten, N'Không xác định') AS ten_chi_nhanh,
        ISNULL(cn.dia_chi, N'') AS dia_chi_chi_nhanh,
        ISNULL(cn.sdt, N'') AS sdt_chi_nhanh,
        CONVERT(VARCHAR(10), GETDATE(), 103) AS ngay_bao_cao,
        CONVERT(VARCHAR(8), GETDATE(), 108) AS gio_bao_cao,
        CONVERT(VARCHAR(10), GETDATE(), 103) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108) AS thoi_gian_bao_cao,
        
        -- Chi tiết tồn kho (dùng trong Details section)
        ROW_NUMBER() OVER (ORDER BY nl.ten_nl) AS stt,
        nl.nl_id,
        nl.ma_nl,
        nl.ten_nl,
        nl.don_vi,
        ISNULL(tk.sl_ton, 0) AS sl_ton,
        ISNULL(tk.ton_toi_thieu, 0) AS ton_toi_thieu,
        CASE 
            WHEN ISNULL(tk.sl_ton, 0) = 0 THEN N'Hết hàng'
            WHEN ISNULL(tk.sl_ton, 0) <= ISNULL(tk.ton_toi_thieu, 0) THEN N'Tồn thấp'
            ELSE N'Đủ tồn'
        END AS trang_thai,
        CASE 
            WHEN ISNULL(tk.sl_ton, 0) < ISNULL(tk.ton_toi_thieu, 0) 
            THEN ISNULL(tk.ton_toi_thieu, 0) - ISNULL(tk.sl_ton, 0)
            ELSE 0
        END AS sl_can_nhap
        
    FROM dbo.nguyen_lieu nl
    LEFT JOIN dbo.ton_kho tk ON tk.nl_id = nl.nl_id AND tk.chi_nhanh_id = @ChiNhanhId
    LEFT JOIN dbo.chi_nhanh cn ON cn.chi_nhanh_id = @ChiNhanhId
    ORDER BY nl.ten_nl;
END;
GO
--Bao Cao Danh Sach Dat Tiec
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
-- In Hóa đơn cho khách
IF OBJECT_ID('dbo.sp_InHoaDonChoKhach', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_InHoaDonChoKhach;
GO
CREATE PROCEDURE sp_InHoaDonChoKhach
    @hoa_don_id INT,
    @chi_nhanh_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Lấy thông tin hóa đơn và thông tin khuyến mãi
    SELECT 
        h.hoa_don_id,
        kh.ho_ten AS ten_khach_hang,
        h.ngay_lap,
        h.trang_thai,
        h.tong_truoc_thue AS tam_tinh, -- Số tiền tạm tính
        h.vat, -- % VAT
        h.giam_gia,
        h.phi_dv,
        -- Tính số tiền thuế phải đóng
        ROUND(h.tong_truoc_thue * (h.vat / 100), 0) AS so_tien_thue, -- Thuế phải đóng
        h.tong_sau_thue AS tong_cong, -- Tổng tiền
        ISNULL(km.ten, '-') AS ten_km,
        ISNULL(km.ma_km, '-') AS ma_km,
        ISNULL(hdkm.so_tien_km, 0) AS so_tien_km,
        (h.tong_sau_thue - ISNULL(hdkm.so_tien_km, 0)) AS so_tien_phai_tra,
        (SELECT ten FROM chi_nhanh WHERE chi_nhanh_id = @chi_nhanh_id) AS ten_chi_nhanh,
        (SELECT dia_chi FROM chi_nhanh WHERE chi_nhanh_id = @chi_nhanh_id) AS dia_chi_chi_nhanh,
        (SELECT sdt FROM chi_nhanh WHERE chi_nhanh_id = @chi_nhanh_id) AS sdt_chi_nhanh,
        h.so_ban_sanh,
        CAST(h.ngay_lap AS DATE) AS ngay,  -- Ngày đặt
        CAST(h.ngay_lap AS TIME) AS gio   -- Giờ đặt
    FROM 
        dbo.hoa_don h
    LEFT JOIN 
        dbo.khach_hang kh ON h.khach_hang_id = kh.khach_hang_id
    LEFT JOIN 
        hoa_don_km hdkm ON hdkm.hoa_don_id = h.hoa_don_id
    LEFT JOIN 
        chuong_trinh_km km ON km.km_id = hdkm.km_id
    WHERE 
        h.hoa_don_id = @hoa_don_id;

    -- Lấy chi tiết hóa đơn
    SELECT 
        hd_ct.hd_ct_id,
        hd_ct.ten_hang,
        hd_ct.so_luong,
        hd_ct.don_gia,
        hd_ct.thanh_tien
    FROM 
        dbo.hoa_don_ct hd_ct
    WHERE 
        hd_ct.hoa_don_id = @hoa_don_id;
END
GO

-- ======================================================================
-- STORED PROCEDURE: In Hợp đồng đặt tiệc
-- ======================================================================
IF OBJECT_ID('dbo.sp_InHopDongDatTiec', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_InHopDongDatTiec;
GO

CREATE PROCEDURE dbo.sp_InHopDongDatTiec
    @HopDongId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Biến tạm để tính toán
    DECLARE @TongTienMon DECIMAL(18,2) = 0;
    DECLARE @TongTienDV DECIMAL(18,2) = 0;
    DECLARE @GiaBan DECIMAL(18,2) = 0;
    DECLARE @SoBan INT = 0;
    DECLARE @GoiId INT;
    DECLARE @DatSanhId INT;
    DECLARE @VAT_Percent DECIMAL(5,2) = 10.0; -- Giả sử VAT 10%
    DECLARE @Coc_Percent DECIMAL(5,2) = 20.0; -- Cọc 20%

    -- Lấy thông tin cơ bản
    SELECT 
        @DatSanhId = dat_sanh_id 
    FROM hop_dong WHERE hop_dong_id = @HopDongId;

    SELECT 
        @SoBan = so_ban_du_kien,
        @GoiId = goi_id
    FROM dat_sanh WHERE dat_sanh_id = @DatSanhId;

    -- Tính tổng tiền món
    IF EXISTS (SELECT 1 FROM hop_dong_ct_mon WHERE hop_dong_id = @HopDongId)
    BEGIN
        SELECT @TongTienMon = SUM(thanh_tien) FROM hop_dong_ct_mon WHERE hop_dong_id = @HopDongId;
    END

    -- Xác định giá bàn (@GiaBan)
    IF @GoiId IS NOT NULL
    BEGIN
        -- Nếu dùng gói, lấy giá gói
        SELECT @GiaBan = gia_co_ban FROM goi_tiec WHERE goi_id = @GoiId;
    END

    -- Nếu không dùng gói (hoặc giá gói = 0) và có chi tiết món, tính giá bàn trung bình từ tổng tiền món
    IF @GiaBan = 0 AND @SoBan > 0 AND @TongTienMon > 0
    BEGIN
        SET @GiaBan = @TongTienMon / @SoBan;
    END

    -- Tính tổng tiền dịch vụ
    SELECT @TongTienDV = ISNULL(SUM(thanh_tien), 0) 
    FROM hop_dong_ct_dv WHERE hop_dong_id = @HopDongId;

    -- Tính toán tổng cộng
    -- Thành tiền tiệc bàn = Giá bàn * Số bàn
    DECLARE @TongTienBan DECIMAL(18,2) = @GiaBan * ISNULL(@SoBan, 0);
    
    DECLARE @TamTinh DECIMAL(18,2) = @TongTienBan + @TongTienDV;
    DECLARE @TienVAT DECIMAL(18,2) = ROUND(@TamTinh * (@VAT_Percent / 100.0), 0);
    DECLARE @TongThanhToan DECIMAL(18,2) = @TamTinh + @TienVAT;
    DECLARE @TienCocYeuCau DECIMAL(18,2) = ROUND(@TongThanhToan * (@Coc_Percent / 100.0), 0);
    
    DECLARE @DaDatCoc DECIMAL(18,2) = 0;
    SELECT @DaDatCoc = ISNULL(SUM(so_tien), 0) FROM hop_dong_coc WHERE hop_dong_id = @HopDongId;

    DECLARE @ConLai DECIMAL(18,2) = @TongThanhToan - @DaDatCoc;

    -- 1. Header & General Info
    SELECT
        hd.hop_dong_id,
        hd.so_hop_dong,
		hd.dieu_khoan,
        FORMAT(hd.ngay_ky, 'dd/MM/yyyy') AS ngay_ky,
        FORMAT(ds.ngay_to_chuc, 'dd/MM/yyyy') AS ngay_to_chuc,
        c.ten_ca,
        ds.gio_to_chuc,
        s.suc_chua,
		s.ten_sanh AS ten_sanh_da_dat,
        -- Party A
        kh.ho_ten AS ten_khach_hang,
        FORMAT(kh.ngay_sinh, 'dd/MM/yyyy') AS ngay_sinh,
        '' AS cccd, -- Placeholder
        '' AS ngay_cap, -- Placeholder
        '' AS noi_cap, -- Placeholder
        kh.sdt,
        kh.email,
        N'' AS dia_chi_kh, -- Placeholder
        ds.ghi_chu AS ghi_chu_dac_biet,
        -- Party B (Branch info)
        cn.ten AS ten_nha_hang,
        cn.dia_chi AS dia_chi_nh,
        (SELECT dia_chi FROM chi_nhanh WHERE chi_nhanh_id = 1) AS dia_chi_tru_so,
        cn.sdt AS sdt_nh,
        N'0123456789' AS mst_nh, -- Placeholder
        N'Trần Thị Bình' AS dai_dien_nh, -- Placeholder
        N'1234567890 - VCB' AS tai_khoan_nh, -- Placeholder
        -- Package Info
        ISNULL(gt.ten_goi, N'Tự chọn') AS ten_goi,
        @GiaBan AS gia_ban,
        -- Financials
        ds.so_ban_du_kien AS so_ban,
        @TamTinh AS tam_tinh,
        @VAT_Percent AS phan_tram_vat,
        @TienVAT AS tien_vat,
        @TongThanhToan AS tong_thanh_toan,
        @Coc_Percent AS phan_tram_coc,
        @TienCocYeuCau AS tien_coc_yeu_cau,
        @DaDatCoc AS da_dat_coc,
        @ConLai AS con_lai
    FROM hop_dong hd
    JOIN dat_sanh ds ON hd.dat_sanh_id = ds.dat_sanh_id
    JOIN chi_nhanh cn ON ds.chi_nhanh_id = cn.chi_nhanh_id
    JOIN sanh s ON ds.sanh_id = s.sanh_id
    JOIN ca c ON ds.ca_id = c.ca_id
    JOIN khach_hang kh ON ds.khach_hang_id = kh.khach_hang_id
    LEFT JOIN goi_tiec gt ON ds.goi_id = gt.goi_id
    WHERE hd.hop_dong_id = @HopDongId;

    -- 2. Details (Services & Table)
    -- Rows for Services
    SELECT
        2 AS stt,
        dv.ten_dv AS hang_muc,
        ct.so_luong,
        ct.don_gia,
        ct.thanh_tien
    FROM hop_dong_ct_dv ct
    JOIN dich_vu dv ON ct.dv_id = dv.dv_id
    WHERE ct.hop_dong_id = @HopDongId;

    -- 3. Menu List
    -- If hop_dong_ct_mon has data, use it
    IF EXISTS (SELECT 1 FROM hop_dong_ct_mon WHERE hop_dong_id = @HopDongId)
    BEGIN
        SELECT
            m.ten_mon,
            ct.so_luong,
            m.don_vi_tinh
        FROM hop_dong_ct_mon ct
        JOIN mon_an m ON ct.mon_id = m.mon_id
        WHERE ct.hop_dong_id = @HopDongId;
    END
    ELSE
    BEGIN
        -- Else use package menu
        SELECT
            m.ten_mon,
            gtm.so_luong,
            m.don_vi_tinh
        FROM hop_dong hd
        JOIN dat_sanh ds ON hd.dat_sanh_id = ds.dat_sanh_id
        JOIN goi_tiec_mon gtm ON ds.goi_id = gtm.goi_id
        JOIN mon_an m ON gtm.mon_id = m.mon_id
        WHERE hd.hop_dong_id = @HopDongId;
    END
END