-- Script thêm dữ liệu mẫu cho khu vực và bàn theo chi nhánh đã có
-- Chạy script này sau khi đã có dữ liệu chi nhánh trong database

USE QL_NhaHangTiecCuoi_V3;
GO

-- Kiểm tra chi nhánh hiện có
PRINT '=== KIỂM TRA CHI NHÁNH HIỆN CÓ ===';
SELECT chi_nhanh_id, ten, dia_chi, sdt, trang_thai 
FROM chi_nhanh 
ORDER BY chi_nhanh_id;

-- Tạo dữ liệu chi nhánh mẫu nếu chưa có
PRINT '=== TẠO DỮ LIỆU CHI NHÁNH MẪU ===';
INSERT INTO chi_nhanh (ten, dia_chi, sdt, trang_thai)
SELECT N'Nhà hàng truyền thống', N'123 Đường ABC, Quận 1, TP.HCM', N'0123456789', 1
WHERE NOT EXISTS (SELECT 1 FROM chi_nhanh WHERE chi_nhanh_id = 1);

INSERT INTO chi_nhanh (ten, dia_chi, sdt, trang_thai)
SELECT N'Nhà hàng cao cấp', N'456 Đường XYZ, Quận 2, TP.HCM', N'0987654321', 1
WHERE NOT EXISTS (SELECT 1 FROM chi_nhanh WHERE chi_nhanh_id = 2);

INSERT INTO chi_nhanh (ten, dia_chi, sdt, trang_thai)
SELECT N'Nhà hàng gia đình', N'789 Đường DEF, Quận 3, TP.HCM', N'0369852147', 1
WHERE NOT EXISTS (SELECT 1 FROM chi_nhanh WHERE chi_nhanh_id = 3);

-- 1. THÊM DỮ LIỆU MẪU CHO KHU VỰC (Mỗi chi nhánh có khu vực khác nhau)
PRINT '=== THÊM DỮ LIỆU MẪU CHO KHU VỰC ===';

-- Chi nhánh 1: Nhà hàng truyền thống
INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Tầng 1'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Tầng 1'
);

INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Tầng 2'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Tầng 2'
);

INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Sân vườn'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Sân vườn'
);

-- Chi nhánh 2: Nhà hàng cao cấp
INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Khu VIP'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Khu VIP'
);

INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Khu thường'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Khu thường'
);

INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Khu tiệc cưới'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Khu tiệc cưới'
);

-- Chi nhánh 3: Nhà hàng gia đình (nếu có)
INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Khu gia đình'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 3
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Khu gia đình'
);

INSERT INTO khu_vuc (chi_nhanh_id, ten_khu_vuc)
SELECT 
    c.chi_nhanh_id,
    N'Khu ngoài trời'
FROM chi_nhanh c
WHERE c.chi_nhanh_id = 3
AND NOT EXISTS (
    SELECT 1 FROM khu_vuc k 
    WHERE k.chi_nhanh_id = c.chi_nhanh_id 
    AND k.ten_khu_vuc = N'Khu ngoài trời'
);

-- 2. THÊM DỮ LIỆU MẪU CHO BÀN (Mỗi chi nhánh có bàn khác nhau)
PRINT '=== THÊM DỮ LIỆU MẪU CHO BÀN ===';

-- CHI NHÁNH 1: Nhà hàng truyền thống
-- Tầng 1 - Bàn nhỏ cho 2-4 người
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'T1-01',
    2,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Tầng 1'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'T1-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'T1-02',
    4,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Tầng 1'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'T1-02'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'T1-03',
    4,
    N'PHỤC VỤ'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Tầng 1'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'T1-03'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'T1-04',
    6,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Tầng 1'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'T1-04'
);

-- Tầng 2 - Bàn lớn cho 6-8 người
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'T2-01',
    6,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Tầng 2'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'T2-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'T2-02',
    8,
    N'ĐÃ ĐẶT'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Tầng 2'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'T2-02'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'T2-03',
    8,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Tầng 2'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'T2-03'
);

-- Sân vườn - Bàn ngoài trời
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'SV-01',
    4,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Sân vườn'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'SV-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'SV-02',
    6,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Sân vườn'
WHERE c.chi_nhanh_id = 1
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'SV-02'
);

-- CHI NHÁNH 2: Nhà hàng cao cấp
-- Khu VIP - Bàn cao cấp
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'VIP-01',
    4,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu VIP'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'VIP-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'VIP-02',
    6,
    N'ĐÃ ĐẶT'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu VIP'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'VIP-02'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'VIP-03',
    8,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu VIP'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'VIP-03'
);

-- Khu thường - Bàn tiêu chuẩn
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'TH-01',
    4,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu thường'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'TH-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'TH-02',
    6,
    N'PHỤC VỤ'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu thường'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'TH-02'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'TH-03',
    8,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu thường'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'TH-03'
);

-- Khu tiệc cưới - Bàn lớn cho tiệc
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'TC-01',
    12,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu tiệc cưới'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'TC-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'TC-02',
    16,
    N'ĐÃ ĐẶT'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu tiệc cưới'
WHERE c.chi_nhanh_id = 2
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'TC-02'
);

-- CHI NHÁNH 3: Nhà hàng gia đình (nếu có)
-- Khu gia đình - Bàn cho gia đình
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'GD-01',
    6,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu gia đình'
WHERE c.chi_nhanh_id = 3
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'GD-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'GD-02',
    8,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu gia đình'
WHERE c.chi_nhanh_id = 3
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'GD-02'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'GD-03',
    10,
    N'PHỤC VỤ'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu gia đình'
WHERE c.chi_nhanh_id = 3
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'GD-03'
);

-- Khu ngoài trời - Bàn ngoài trời
INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'NT-01',
    4,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu ngoài trời'
WHERE c.chi_nhanh_id = 3
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'NT-01'
);

INSERT INTO ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    c.chi_nhanh_id,
    k.khu_vuc_id,
    N'NT-02',
    6,
    N'TRỐNG'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id AND k.ten_khu_vuc = N'Khu ngoài trời'
WHERE c.chi_nhanh_id = 3
AND NOT EXISTS (
    SELECT 1 FROM ban b 
    WHERE b.chi_nhanh_id = c.chi_nhanh_id 
    AND b.so_ban = N'NT-02'
);

-- 3. KIỂM TRA KẾT QUẢ
PRINT '=== KIỂM TRA KẾT QUẢ SAU KHI THÊM ===';

-- Hiển thị thống kê khu vực theo chi nhánh
PRINT '--- THỐNG KÊ KHU VỰC THEO CHI NHÁNH ---';
SELECT 
    c.ten as 'Chi nhánh',
    COUNT(k.khu_vuc_id) as 'Số khu vực',
    STRING_AGG(k.ten_khu_vuc, ', ') as 'Danh sách khu vực'
FROM chi_nhanh c
LEFT JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id
GROUP BY c.chi_nhanh_id, c.ten
ORDER BY c.chi_nhanh_id;

-- Hiển thị thống kê bàn theo chi nhánh và khu vực
PRINT '--- THỐNG KÊ BÀN THEO CHI NHÁNH VÀ KHU VỰC ---';
SELECT 
    c.ten as 'Chi nhánh',
    k.ten_khu_vuc as 'Khu vực',
    COUNT(b.ban_id) as 'Số bàn',
    SUM(CASE WHEN b.trang_thai = N'TRỐNG' THEN 1 ELSE 0 END) as 'Trống',
    SUM(CASE WHEN b.trang_thai = N'PHỤC VỤ' THEN 1 ELSE 0 END) as 'Đang phục vụ',
    SUM(CASE WHEN b.trang_thai = N'ĐÃ ĐẶT' THEN 1 ELSE 0 END) as 'Đã đặt',
    SUM(CASE WHEN b.trang_thai = N'VỆ SINH' THEN 1 ELSE 0 END) as 'Vệ sinh'
FROM chi_nhanh c
LEFT JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id
LEFT JOIN ban b ON b.chi_nhanh_id = c.chi_nhanh_id AND b.khu_vuc_id = k.khu_vuc_id
GROUP BY c.chi_nhanh_id, c.ten, k.khu_vuc_id, k.ten_khu_vuc
ORDER BY c.chi_nhanh_id, k.ten_khu_vuc;

-- Hiển thị chi tiết bàn
PRINT '--- CHI TIẾT BÀN ---';
SELECT 
    c.ten as 'Chi nhánh',
    k.ten_khu_vuc as 'Khu vực',
    b.so_ban as 'Số bàn',
    b.suc_chua as 'Sức chứa',
    b.trang_thai as 'Trạng thái'
FROM chi_nhanh c
INNER JOIN khu_vuc k ON k.chi_nhanh_id = c.chi_nhanh_id
INNER JOIN ban b ON b.chi_nhanh_id = c.chi_nhanh_id AND b.khu_vuc_id = k.khu_vuc_id
ORDER BY c.chi_nhanh_id, k.ten_khu_vuc, b.so_ban;

PRINT '=== HOÀN THÀNH THÊM DỮ LIỆU MẪU ===';
