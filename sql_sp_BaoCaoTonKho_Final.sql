-- ======================================================================
-- STORED PROCEDURE: BÁO CÁO TỒN KHO (PHIÊN BẢN ĐƠN GIẢN - KHÔNG LỖI)
-- Mục đích: Trả về dữ liệu tồn kho đơn giản, dễ sử dụng trong Crystal Reports
-- Chỉ có 1 result set duy nhất, không có JOIN phức tạp
-- ======================================================================

USE QL_NhaHangTiecCuoi_V3;
GO

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

-- ======================================================================
-- TEST STORED PROCEDURE
-- ======================================================================
-- Chạy lệnh sau để test:
-- EXEC sp_BaoCaoTonKho_Final @ChiNhanhId = 1
-- ======================================================================

