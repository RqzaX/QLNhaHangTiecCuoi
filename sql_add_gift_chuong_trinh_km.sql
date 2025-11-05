-- Script để thêm hỗ trợ "GIFT" (Tặng Quà) vào bảng chuong_trinh_km
USE QL_NhaHangTiecCuoi_V3;
GO

-- Xóa CHECK constraint cũ
IF EXISTS (
    SELECT * FROM sys.check_constraints 
    WHERE name = 'CK_chuong_trinh_km_hinh_thuc' 
    OR name LIKE '%hinh_thuc%'
)
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SELECT @sql = 'ALTER TABLE dbo.chuong_trinh_km DROP CONSTRAINT ' + name
    FROM sys.check_constraints 
    WHERE parent_object_id = OBJECT_ID('dbo.chuong_trinh_km')
    AND definition LIKE '%hinh_thuc%';
    
    IF @sql IS NOT NULL
        EXEC sp_executesql @sql;
END
GO

-- Thêm lại CHECK constraint với GIFT
ALTER TABLE dbo.chuong_trinh_km
ADD CONSTRAINT CK_chuong_trinh_km_hinh_thuc 
CHECK (hinh_thuc IN (N'PERCENT', N'AMOUNT', N'GIFT'));
GO

PRINT 'Đã cập nhật hỗ trợ loại khuyến mãi GIFT (Tặng Quà)';
GO

