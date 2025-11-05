-- Script thêm cột suc_chua vào bảng goi_tiec và cập nhật sức chứa cho các gói tiệc
USE QL_NhaHangTiecCuoi_V3;
GO

-- Kiểm tra và thêm cột suc_chua nếu chưa tồn tại
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'dbo.goi_tiec') 
    AND name = 'suc_chua'
)
BEGIN
    ALTER TABLE dbo.goi_tiec
    ADD suc_chua INT NULL;
    
    PRINT 'Đã thêm cột suc_chua vào bảng goi_tiec';
END
ELSE
BEGIN
    PRINT 'Cột suc_chua đã tồn tại trong bảng goi_tiec';
END
GO

-- Cập nhật sức chứa cho các gói tiệc từ sảnh
-- Nếu gói tiệc đã được sử dụng trong dat_sanh, lấy sức chứa từ sảnh được sử dụng nhiều nhất
-- Nếu chưa có, lấy sức chứa từ sảnh phù hợp nhất (sảnh nhỏ nhất cho gói cơ bản, lớn nhất cho gói cao cấp)

-- Bước 1: Cập nhật cho các gói tiệc đã có trong dat_sanh (lấy từ sảnh được sử dụng nhiều nhất)
UPDATE gt
SET gt.suc_chua = sub.suc_chua
FROM dbo.goi_tiec gt
INNER JOIN (
    SELECT 
        ds.goi_id,
        s.suc_chua,
        ROW_NUMBER() OVER (PARTITION BY ds.goi_id ORDER BY COUNT(*) DESC, s.suc_chua DESC) AS rn
    FROM dbo.dat_sanh ds
    INNER JOIN dbo.sanh s ON s.sanh_id = ds.sanh_id
    WHERE ds.goi_id IS NOT NULL
    GROUP BY ds.goi_id, s.suc_chua
) sub ON sub.goi_id = gt.goi_id AND sub.rn = 1
WHERE gt.suc_chua IS NULL;

-- Bước 2: Cập nhật cho các gói tiệc chưa có trong dat_sanh
-- Dựa trên giá trị gói tiệc để chọn sảnh phù hợp
UPDATE gt
SET gt.suc_chua = CASE
    -- Gói cơ bản (giá thấp): lấy sức chứa từ sảnh nhỏ nhất
    WHEN gt.gia_co_ban <= 7000000 THEN (
        SELECT MIN(s.suc_chua) 
        FROM dbo.sanh s
    )
    -- Gói trung cấp (giá trung bình): lấy sức chứa trung bình
    WHEN gt.gia_co_ban <= 15000000 THEN (
        SELECT AVG(s.suc_chua) 
        FROM dbo.sanh s
    )
    -- Gói cao cấp (giá cao): lấy sức chứa từ sảnh lớn nhất
    ELSE (
        SELECT MAX(s.suc_chua) 
        FROM dbo.sanh s
    )
END
FROM dbo.goi_tiec gt
WHERE gt.suc_chua IS NULL;

PRINT 'Đã cập nhật sức chứa cho các gói tiệc';

-- Kiểm tra kết quả
SELECT 
    ma_goi AS [Mã gói],
    ten_goi AS [Tên gói],
    gia_co_ban AS [Giá cơ bản],
    suc_chua AS [Sức chứa]
FROM dbo.goi_tiec
ORDER BY ma_goi;
GO

