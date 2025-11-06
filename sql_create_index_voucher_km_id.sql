-- Script tạo index để tối ưu hiệu suất query GetAll() trong ChuongTrinhKMDAL
-- Index này sẽ giúp tăng tốc độ query khi tính tổng số voucher đã dùng và tổng số lần cho mỗi chương trình khuyến mãi

USE QL_NhaHangTiecCuoi_V3;
GO

-- Kiểm tra xem index đã tồn tại chưa
IF NOT EXISTS (
    SELECT 1 
    FROM sys.indexes 
    WHERE name = 'IX_voucher_km_id' 
    AND object_id = OBJECT_ID('dbo.voucher')
)
BEGIN
    -- Tạo index trên km_id, bao gồm cả da_dung và so_lan để query có thể lấy dữ liệu trực tiếp từ index
    CREATE NONCLUSTERED INDEX IX_voucher_km_id 
    ON dbo.voucher(km_id) 
    INCLUDE (da_dung, so_lan)
    WITH (FILLFACTOR = 90);
    
    PRINT 'Index IX_voucher_km_id đã được tạo thành công!';
END
ELSE
BEGIN
    PRINT 'Index IX_voucher_km_id đã tồn tại.';
END
GO

-- Kiểm tra statistics của index
UPDATE STATISTICS dbo.voucher WITH FULLSCAN;
GO

PRINT 'Đã cập nhật statistics cho bảng voucher.';
GO

