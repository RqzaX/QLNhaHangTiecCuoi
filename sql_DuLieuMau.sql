/* ============================================================
   DỮ LIỆU MẪU HỆ THỐNG NHÀ HÀNG – CHẠY SAU KHI TẠO SCHEMA
   Idempotent: có điều kiện NOT EXISTS / MERGE để chạy nhiều lần an toàn
   Phạm vi: Tất cả bảng trong sql_QL-NhaHang-V3.sql
   ============================================================ */

USE QL_NhaHangTiecCuoi_V3;
GO

/* ========================
   1) CHI NHÁNH, CA
   ======================== */
IF NOT EXISTS (SELECT 1 FROM dbo.chi_nhanh WHERE ten = N'CN Trung tâm')
  INSERT dbo.chi_nhanh(ten, dia_chi, sdt, trang_thai)
  VALUES (N'CN Trung tâm', N'12 Nguyễn Huệ, Q.1, TP.HCM', N'028-12345678', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.chi_nhanh WHERE ten = N'CN Quận 7')
  INSERT dbo.chi_nhanh(ten, dia_chi, sdt, trang_thai)
  VALUES (N'CN Quận 7', N'88 Nguyễn Văn Linh, Q.7, TP.HCM', N'028-87654321', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.ca WHERE ten_ca = N'Sáng')
  INSERT dbo.ca(ten_ca, gio_bd, gio_kt) VALUES (N'Sáng', '07:00', '11:00');
IF NOT EXISTS (SELECT 1 FROM dbo.ca WHERE ten_ca = N'Trưa')
  INSERT dbo.ca(ten_ca, gio_bd, gio_kt) VALUES (N'Trưa', '11:00', '16:00');
IF NOT EXISTS (SELECT 1 FROM dbo.ca WHERE ten_ca = N'Tối')
  INSERT dbo.ca(ten_ca, gio_bd, gio_kt) VALUES (N'Tối', '16:00', '22:00');
GO

DECLARE @cn_tt INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Trung tâm');
DECLARE @cn_q7 INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Quận 7');

/* ========================
   2) KHU VỰC, BÀN, SẢNH
   ======================== */
IF NOT EXISTS (SELECT 1 FROM dbo.khu_vuc WHERE ten_khu_vuc=N'Khu A' AND chi_nhanh_id=@cn_tt)
  INSERT dbo.khu_vuc(chi_nhanh_id, ten_khu_vuc) VALUES(@cn_tt, N'Khu A');
IF NOT EXISTS (SELECT 1 FROM dbo.khu_vuc WHERE ten_khu_vuc=N'Khu B' AND chi_nhanh_id=@cn_tt)
  INSERT dbo.khu_vuc(chi_nhanh_id, ten_khu_vuc) VALUES(@cn_tt, N'Khu B');

IF NOT EXISTS (SELECT 1 FROM dbo.sanh WHERE ten_sanh=N'Sảnh Ruby 1' AND chi_nhanh_id=@cn_tt)
  INSERT dbo.sanh(chi_nhanh_id, ten_sanh, suc_chua, phi_thue_cb)
  VALUES(@cn_tt, N'Sảnh Ruby 1', 200, 5000000);
IF NOT EXISTS (SELECT 1 FROM dbo.sanh WHERE ten_sanh=N'Sảnh Ruby 2' AND chi_nhanh_id=@cn_tt)
  INSERT dbo.sanh(chi_nhanh_id, ten_sanh, suc_chua, phi_thue_cb)
  VALUES(@cn_tt, N'Sảnh Ruby 2', 250, 7000000);
IF NOT EXISTS (SELECT 1 FROM dbo.sanh WHERE ten_sanh=N'Sảnh Ruby 3' AND chi_nhanh_id=@cn_tt)
  INSERT dbo.sanh(chi_nhanh_id, ten_sanh, suc_chua, phi_thue_cb)
  VALUES(@cn_tt, N'Sảnh Ruby 3', 300, 9000000);

DECLARE @kv_a INT = (SELECT khu_vuc_id FROM dbo.khu_vuc WHERE ten_khu_vuc=N'Khu A' AND chi_nhanh_id=@cn_tt);
DECLARE @kv_b INT = (SELECT khu_vuc_id FROM dbo.khu_vuc WHERE ten_khu_vuc=N'Khu B' AND chi_nhanh_id=@cn_tt);

;WITH t AS (
  SELECT @cn_tt AS chi_nhanh_id, @kv_a AS kv, v AS so_ban, 4 AS suc
  FROM (VALUES(N'T01'),(N'T02'),(N'T03'),(N'T04'),(N'T05')) x(v)
  UNION ALL
  SELECT @cn_tt, @kv_b, v, 6 FROM (VALUES(N'T06'),(N'T07'),(N'T08'),(N'T09'),(N'T10')) x(v)
)
INSERT dbo.ban(chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT t.chi_nhanh_id, t.kv, t.so_ban, t.suc, N'TRỐNG'
FROM t
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.ban b
  WHERE b.chi_nhanh_id=t.chi_nhanh_id AND b.so_ban=t.so_ban
);
GO

/* ========================
   3) KHÁCH HÀNG + HẠNG
   ======================== */
-- Bảng dm_hang_kh đã có ở schema script; đảm bảo tồn tại.

;WITH kh(ma, ten, ngay_sinh_str, sdt) AS (
  SELECT N'KH001', N'Nguyễn Trúc Vy', N'1995-03-12', N'0965000001' UNION ALL
  SELECT N'KH002', N'Phạm Anh Tuấn',  N'1992-07-21', N'0965000002' UNION ALL
  SELECT N'KH003', N'Võ Hà My',       N'1998-11-02', N'0965000003' UNION ALL
  SELECT N'KH004', N'Lê Minh Khang',  N'1990-05-19', N'0965000004' UNION ALL
  SELECT N'KH005', N'Đỗ Gia Huy',     N'1988-01-10', N'0965000005' UNION ALL
  SELECT N'KH006', N'Ngô Phúc An',    N'1996-09-09', N'0965000006' UNION ALL
  SELECT N'KH007', N'Bùi Trúc Ly',    N'1993-12-30', N'0965000007' UNION ALL
  SELECT N'KH008', N'Trần Quang Huy', N'1991-06-14', N'0965000008' UNION ALL
  SELECT N'KH009', N'Nguyễn Nhật Nam',N'1999-04-27', N'0965000009' UNION ALL
  SELECT N'KH010', N'Đặng Bảo Ngọc',  N'1997-02-05', N'0965000010'
)
INSERT dbo.khach_hang(ho_ten, sdt, email, ghi_chu, ngay_sinh, hang_code, tong_chi_tieu, so_lan_den, diem, lan_cuoi_den)
SELECT ten, sdt, NULL, NULL,
       ISNULL(TRY_CONVERT(date, ngay_sinh_str), CONVERT(date,'1990-01-01',23)) AS ngay_sinh,
       CASE WHEN rn <= 6 THEN N'MEM'
            WHEN rn <= 8 THEN N'BAC'
            WHEN rn = 9 THEN N'VANG'
            ELSE N'VIP' END AS hang_code,
       rn * 1500000 AS tong_chi_tieu,
       rn AS so_lan_den,
       rn * 10 AS diem,
       DATEADD(day, -rn, CAST(GETDATE() AS date)) AS lan_cuoi_den
FROM (
  SELECT *, ROW_NUMBER() OVER (ORDER BY ma) AS rn FROM kh
) s
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.khach_hang k WHERE k.ho_ten = s.ten AND k.sdt = s.sdt
);
GO

/* ========================
   4) THỰC ĐƠN, DỊCH VỤ, GÓI TIỆC
   ======================== */
MERGE dbo.mon_an AS T
USING (VALUES
  (N'MA-01', N'Salad rau trộn', N'Khai vị', N'đĩa', 45000, 1),
  (N'MA-02', N'Súp bí đỏ', N'Khai vị', N'bát', 35000, 1),
  (N'MA-03', N'Gà quay mật ong', N'Món chính', N'phần', 120000, 1),
  (N'MA-04', N'Bò lúc lắc', N'Món chính', N'phần', 140000, 1),
  (N'MA-05', N'Cá hồi áp chảo', N'Món chính', N'phần', 160000, 1),
  (N'MA-06', N'Rau củ xào thập cẩm', N'Món chính', N'đĩa', 60000, 1),
  (N'MA-07', N'Lẩu thái hải sản', N'Lẩu', N'nồi', 220000, 1),
  (N'MA-08', N'Cơm chiên trứng', N'Ăn kèm', N'đĩa', 35000, 1),
  (N'MA-09', N'Tráng miệng trái cây', N'Tráng miệng', N'đĩa', 50000, 1),
  (N'MA-10', N'Nước suối', N'Đồ uống', N'chai', 15000, 1)
) AS S(ma,ten,nhom,dvt,gia,ban)
ON T.ma_mon=S.ma
WHEN NOT MATCHED THEN
  INSERT(ma_mon,ten_mon,nhom,don_vi_tinh,don_gia,dang_ban)
  VALUES(S.ma,S.ten,S.nhom,S.dvt,S.gia,S.ban)
WHEN MATCHED THEN UPDATE SET ten_mon=S.ten, nhom=S.nhom, don_vi_tinh=S.dvt, don_gia=S.gia, dang_ban=S.ban;

MERGE dbo.dich_vu AS T
USING (VALUES
  (N'DV-01', N'Trang trí tiêu chuẩn', N'gói', 800000, 1),
  (N'DV-02', N'Âm thanh ánh sáng', N'gói', 1500000, 1),
  (N'DV-03', N'Ban nhạc acoustic', N'suất', 2000000, 1),
  (N'DV-04', N'Karaoke', N'giờ', 300000, 1),
  (N'DV-05', N'Phục vụ MC', N'suất', 2500000, 1),
  (N'DV-06', N'Chụp ảnh cưới', N'suất', 3000000, 1),
  (N'DV-07', N'Quay phim cưới', N'suất', 4000000, 1),
  (N'DV-08', N'Hoa cưới cô dâu', N'bộ', 1500000, 1),
  (N'DV-09', N'Hoa cưới chú rể', N'bộ', 500000, 1),
  (N'DV-10', N'Bánh cưới', N'cái', 2000000, 1),
  (N'DV-11', N'Xe hoa', N'chuyến', 2500000, 1),
  (N'DV-12', N'Makeup cô dâu', N'suất', 2000000, 1),
  (N'DV-13', N'Trang trí hoa tươi cao cấp', N'gói', 3500000, 1),
  (N'DV-14', N'Phục vụ bàn chuyên nghiệp', N'người', 800000, 1),
  (N'DV-15', N'Thợ chụp ảnh chuyên nghiệp', N'suất', 5000000, 1),
  (N'DV-16', N'DJ chuyên nghiệp', N'suất', 3000000, 1),
  (N'DV-17', N'Trang trí backdrop', N'bộ', 1200000, 1),
  (N'DV-18', N'Pháo hoa', N'gói', 2000000, 1),
  (N'DV-19', N'Bàn tiệc buffet', N'bộ', 500000, 1),
  (N'DV-20', N'Thiệp mời', N'bộ', 500000, 1)
) AS S(ma,ten,dvt,gia,ban)
ON T.ma_dv=S.ma
WHEN NOT MATCHED THEN
  INSERT(ma_dv,ten_dv,don_vi_tinh,don_gia,dang_ban)
  VALUES(S.ma,S.ten,S.dvt,S.gia,S.ban)
WHEN MATCHED THEN UPDATE SET ten_dv=S.ten, don_vi_tinh=S.dvt, don_gia=S.gia, dang_ban=S.ban;

MERGE dbo.goi_tiec AS T
USING (VALUES
  (N'GT-CB01', N'Gói Tiệc Cưới Cơ Bản', 5000000.00),
  (N'GT-TC02', N'Gói Tiệc Sinh Nhật Thịnh Vượng', 8500000.00),
  (N'GT-CC03', N'Gói Tiệc Cưới Cao Cấp', 12000000.00),
  (N'GT-TT04', N'Gói Tiệc Cưới Tiêu Chuẩn', 6500000.00),
  (N'GT-PS05', N'Gói Tiệc Cưới Phong Cách', 9500000.00),
  (N'GT-VIP06', N'Gói Tiệc Cưới VIP', 15000000.00)
) AS S(ma,ten,gia)
ON T.ma_goi=S.ma
WHEN NOT MATCHED THEN INSERT(ma_goi,ten_goi,gia_co_ban) VALUES(S.ma,S.ten,S.gia)
WHEN MATCHED THEN UPDATE SET ten_goi=S.ten, gia_co_ban=S.gia;
GO

-- Map món vào gói
DECLARE @goi_cb INT = (SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi=N'GT-CB01');
DECLARE @goi_tc INT = (SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi=N'GT-TC02');
DECLARE @goi_cc INT = (SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi=N'GT-CC03');
DECLARE @goi_tt INT = (SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi=N'GT-TT04');
DECLARE @goi_ps INT = (SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi=N'GT-PS05');
DECLARE @goi_vip INT = (SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi=N'GT-VIP06');

;WITH items AS (
  SELECT @goi_cb AS goi, ma FROM (VALUES(N'MA-01'),(N'MA-03'),(N'MA-06'),(N'MA-09')) x(ma)
  UNION ALL SELECT @goi_tc, ma FROM (VALUES(N'MA-01'),(N'MA-04'),(N'MA-07'),(N'MA-09')) x(ma)
  UNION ALL SELECT @goi_cc, ma FROM (VALUES(N'MA-02'),(N'MA-05'),(N'MA-07'),(N'MA-09')) x(ma)
  UNION ALL SELECT @goi_tt, ma FROM (VALUES(N'MA-01'),(N'MA-02'),(N'MA-04'),(N'MA-08'),(N'MA-09')) x(ma)
  UNION ALL SELECT @goi_ps, ma FROM (VALUES(N'MA-02'),(N'MA-04'),(N'MA-05'),(N'MA-07'),(N'MA-09')) x(ma)
  UNION ALL SELECT @goi_vip, ma FROM (VALUES(N'MA-02'),(N'MA-05'),(N'MA-07'),(N'MA-08'),(N'MA-09'),(N'MA-10')) x(ma)
)
INSERT dbo.goi_tiec_mon(goi_id, mon_id, so_luong)
SELECT i.goi, m.mon_id, 1
FROM items i
JOIN dbo.mon_an m ON m.ma_mon=i.ma
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.goi_tiec_mon g WHERE g.goi_id=i.goi AND g.mon_id=m.mon_id
);

;WITH dvs AS (
  -- Gói Cơ Bản: chỉ trang trí tiêu chuẩn
  SELECT @goi_cb AS goi, ma FROM (VALUES(N'DV-01')) x(ma)
  
  -- Gói Sinh Nhật: trang trí + âm thanh ánh sáng
  UNION ALL SELECT @goi_tc, ma FROM (VALUES(N'DV-01'),(N'DV-02')) x(ma)
  
  -- Gói Tiêu Chuẩn: trang trí + âm thanh + hoa cưới + thiệp mời
  UNION ALL SELECT @goi_tt, ma FROM (VALUES(N'DV-01'),(N'DV-02'),(N'DV-08'),(N'DV-09'),(N'DV-20')) x(ma)
  
  -- Gói Phong Cách: trang trí + âm thanh + chụp ảnh + makeup + backdrop + karaoke
  UNION ALL SELECT @goi_ps, ma FROM (VALUES(N'DV-01'),(N'DV-02'),(N'DV-04'),(N'DV-06'),(N'DV-12'),(N'DV-17')) x(ma)
  
  -- Gói Cao Cấp: trang trí + âm thanh + ban nhạc + quay phim + DJ + hoa tươi cao cấp + bánh cưới
  UNION ALL SELECT @goi_cc, ma FROM (VALUES(N'DV-01'),(N'DV-02'),(N'DV-03'),(N'DV-07'),(N'DV-10'),(N'DV-13'),(N'DV-16')) x(ma)
  
  -- Gói VIP: đầy đủ dịch vụ cao cấp nhất
  UNION ALL SELECT @goi_vip, ma FROM (VALUES(N'DV-01'),(N'DV-02'),(N'DV-03'),(N'DV-05'),(N'DV-07'),(N'DV-10'),(N'DV-11'),(N'DV-13'),(N'DV-15'),(N'DV-16'),(N'DV-17'),(N'DV-18'),(N'DV-19')) x(ma)
)
INSERT dbo.goi_tiec_dv(goi_id, dv_id)
SELECT d.goi, v.dv_id
FROM dvs d
JOIN dbo.dich_vu v ON v.ma_dv=d.ma
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.goi_tiec_dv g WHERE g.goi_id=d.goi AND g.dv_id=v.dv_id
);
GO

/* ========================
   5) KHUYẾN MÃI & VOUCHER
   ======================== */
MERGE dbo.chuong_trinh_km AS T
USING (VALUES
  (N'KM-10', N'Giảm 10% hóa đơn', N'PERCENT', 10, CAST(GETDATE() AS datetime2(0)), DATEADD(day, 60, CAST(GETDATE() AS datetime2(0))), N'ALL')
) AS S(ma,ten,ht,gt,bd,kt,ap)
ON T.ma_km=S.ma
WHEN NOT MATCHED THEN INSERT(ma_km,ten,hinh_thuc,gia_tri,tg_bat_dau,tg_ket_thuc,ap_dung_loai)
VALUES(S.ma,S.ten,S.ht,S.gt,S.bd,S.kt,S.ap)
WHEN MATCHED THEN UPDATE SET ten=S.ten, hinh_thuc=S.ht, gia_tri=S.gt, tg_bat_dau=S.bd, tg_ket_thuc=S.kt, ap_dung_loai=S.ap;

IF NOT EXISTS (SELECT 1 FROM dbo.voucher WHERE code=N'KM10-001')
  INSERT dbo.voucher(km_id, code, so_lan, da_dung, han_dung)
  SELECT km_id, N'KM10-001', 5, 0, DATEADD(day, 60, CAST(GETDATE() AS date)) FROM dbo.chuong_trinh_km WHERE ma_km=N'KM-10';
IF NOT EXISTS (SELECT 1 FROM dbo.voucher WHERE code=N'KM10-002')
  INSERT dbo.voucher(km_id, code, so_lan, da_dung, han_dung)
  SELECT km_id, N'KM10-002', 3, 0, DATEADD(day, 60, CAST(GETDATE() AS date)) FROM dbo.chuong_trinh_km WHERE ma_km=N'KM-10';
GO

/* ========================
   6) NGUYÊN LIỆU & TỒN KHO (TỐI THIỂU)
   ======================== */
DECLARE @cn_tt INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Trung tâm');
MERGE dbo.nguyen_lieu AS T
USING (VALUES
  (N'NL-GAO', N'Gạo', N'kg'),
  (N'NL-THIT-BO', N'Thịt bò', N'kg'),
  (N'NL-THIT-GA', N'Thịt gà', N'kg'),
  (N'NL-RAU', N'Rau củ', N'kg'),
  (N'NL-TRUNG', N'Trứng', N'quả')
) AS S(ma,ten,donvi)
ON T.ma_nl=S.ma
WHEN NOT MATCHED THEN INSERT(ma_nl,ten_nl,don_vi) VALUES(S.ma,S.ten,S.donvi)
WHEN MATCHED THEN UPDATE SET ten_nl=S.ten, don_vi=S.donvi;

INSERT dbo.ton_kho(chi_nhanh_id, nl_id, sl_ton)
SELECT @cn_tt, nl_id,
       CASE ma_nl WHEN N'NL-GAO' THEN 100 WHEN N'NL-THIT-BO' THEN 40 WHEN N'NL-THIT-GA' THEN 60 WHEN N'NL-RAU' THEN 80 ELSE 200 END
FROM dbo.nguyen_lieu nl
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.ton_kho tk WHERE tk.chi_nhanh_id=@cn_tt AND tk.nl_id=nl.nl_id
);
GO

/* ========================
   7) VAI TRÒ & NGƯỜI DÙNG
   ======================== */
DECLARE @cn_tt INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Trung tâm');
MERGE dbo.vai_tro AS T
USING (VALUES
  (N'ADMIN',      N'Quản trị'),
  (N'QLCN',       N'Quản lý chi nhánh'),
  (N'LETAN_THUNGAN', N'Lễ tân/Thu ngân'),
  (N'QLBEP',      N'Quản lý bếp'),
  (N'QLKHO',      N'Quản lý kho')
) AS S(ma,ten)
ON T.ma=S.ma
WHEN NOT MATCHED THEN INSERT(ma,ten) VALUES(S.ma,S.ten)
WHEN MATCHED THEN UPDATE SET ten=S.ten;

-- Thêm người dùng mẫu
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'admin')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'admin', N'123456', N'Quản trị hệ thống', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'qlcn01')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'qlcn01', N'123456', N'Nguyễn Văn Quản Lý Chi Nhánh', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'qlcn02')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'qlcn02', N'123456', N'Trần Thị Quản Lý Chi Nhánh', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'letan01')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'letan01', N'123456', N'Bùi Thị Lễ Tân', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'letan02')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'letan02', N'123456', N'Ngô Văn Lễ Tân', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'thungan01')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'thungan01', N'123456', N'Phạm Thị Thu Ngân', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'thungan02')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'thungan02', N'123456', N'Lê Văn Thu Ngân', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'qlbep01')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'qlbep01', N'123456', N'Võ Văn Quản Lý Bếp', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'qlbep02')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'qlbep02', N'123456', N'Đặng Thị Quản Lý Bếp', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'qlkho01')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'qlkho01', N'123456', N'Hoàng Văn Quản Lý Kho', 1);
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan=N'qlkho02')
  INSERT dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES(N'qlkho02', N'123456', N'Đỗ Thị Quản Lý Kho', 1);

-- Map vai trò
INSERT dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
SELECT nd.nguoi_dung_id, vt.vai_tro_id
FROM (SELECT tai_khoan, vai = N'ADMIN' FROM dbo.nguoi_dung WHERE tai_khoan=N'admin'
      UNION ALL SELECT N'qlcn01', N'QLCN'
      UNION ALL SELECT N'qlcn02', N'QLCN'
      UNION ALL SELECT N'letan01', N'LETAN_THUNGAN'
      UNION ALL SELECT N'letan02', N'LETAN_THUNGAN'
      UNION ALL SELECT N'thungan01', N'LETAN_THUNGAN'
      UNION ALL SELECT N'thungan02', N'LETAN_THUNGAN'
      UNION ALL SELECT N'qlbep01', N'QLBEP'
      UNION ALL SELECT N'qlbep02', N'QLBEP'
      UNION ALL SELECT N'qlkho01', N'QLKHO'
      UNION ALL SELECT N'qlkho02', N'QLKHO') m
JOIN dbo.nguoi_dung nd ON nd.tai_khoan=m.tai_khoan
JOIN dbo.vai_tro vt ON vt.ma=m.vai
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_vai_tro x WHERE x.nguoi_dung_id=nd.nguoi_dung_id AND x.vai_tro_id=vt.vai_tro_id
);

-- Gán chi nhánh cho người dùng
-- Admin được gán cho TẤT CẢ chi nhánh
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, cn.chi_nhanh_id
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.chi_nhanh cn
WHERE nd.tai_khoan = N'admin'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m 
  WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=cn.chi_nhanh_id
);

-- Gán chi nhánh cho người dùng
DECLARE @cn_q7 INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Quận 7');

-- Quản lý chi nhánh 01: CN Trung tâm
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_tt
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'qlcn01'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_tt
);

-- Quản lý chi nhánh 02: CN Quận 7
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_q7
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'qlcn02'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_q7
);

-- Lễ tân 01: CN Trung tâm
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_tt
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'letan01'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_tt
);

-- Lễ tân 02: CN Quận 7
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_q7
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'letan02'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_q7
);

-- Thu ngân 01: CN Trung tâm
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_tt
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'thungan01'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_tt
);

-- Thu ngân 02: CN Quận 7
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_q7
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'thungan02'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_q7
);

-- Quản lý bếp 01: CN Trung tâm
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_tt
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'qlbep01'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_tt
);

-- Quản lý bếp 02: CN Quận 7
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_q7
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'qlbep02'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_q7
);

-- Quản lý kho 01: CN Trung tâm
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_tt
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'qlkho01'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_tt
);

-- Quản lý kho 02: CN Quận 7
INSERT dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, @cn_q7
FROM dbo.nguoi_dung nd
WHERE nd.tai_khoan = N'qlkho02'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh m WHERE m.nguoi_dung_id=nd.nguoi_dung_id AND m.chi_nhanh_id=@cn_q7
);
GO

/* ========================
   7) PHÂN CA CHO NHÂN VIÊN (nguoi_dung_ca)
   ======================== */
DECLARE @cn_tt INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Trung tâm');
DECLARE @cn_q7 INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Quận 7');
DECLARE @ca_sang INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca=N'Sáng');
DECLARE @ca_trua INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca=N'Trưa');
DECLARE @ca_toi INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca=N'Tối');

-- Admin: Tất cả ca ở tất cả chi nhánh (quản lý toàn hệ thống)
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, cn.chi_nhanh_id, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.chi_nhanh cn
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'admin'
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=cn.chi_nhanh_id 
    AND ndc.ca_id=c.ca_id
);

-- Quản lý chi nhánh 01 (CN Trung tâm): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_tt, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'qlcn01'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_tt 
    AND ndc.ca_id=c.ca_id
);

-- Quản lý chi nhánh 02 (CN Quận 7): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_q7, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'qlcn02'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_q7 
    AND ndc.ca_id=c.ca_id
);

-- Lễ tân 01 (CN Trung tâm): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_tt, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'letan01'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_tt 
    AND ndc.ca_id=c.ca_id
);

-- Lễ tân 02 (CN Quận 7): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_q7, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'letan02'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_q7 
    AND ndc.ca_id=c.ca_id
);

-- Thu ngân 01 (CN Trung tâm): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_tt, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'thungan01'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_tt 
    AND ndc.ca_id=c.ca_id
);

-- Thu ngân 02 (CN Quận 7): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_q7, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'thungan02'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_q7 
    AND ndc.ca_id=c.ca_id
);

-- Quản lý bếp 01 (CN Trung tâm): Làm ca Trưa và Tối
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_tt, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'qlbep01'
AND c.ten_ca IN (N'Trưa', N'Tối')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_tt 
    AND ndc.ca_id=c.ca_id
);

-- Quản lý bếp 02 (CN Quận 7): Làm ca Trưa và Tối
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_q7, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'qlbep02'
AND c.ten_ca IN (N'Trưa', N'Tối')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_q7 
    AND ndc.ca_id=c.ca_id
);

-- Quản lý kho 01 (CN Trung tâm): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_tt, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'qlkho01'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_tt 
    AND ndc.ca_id=c.ca_id
);

-- Quản lý kho 02 (CN Quận 7): Làm ca Sáng và Trưa
INSERT dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
SELECT nd.nguoi_dung_id, @cn_q7, c.ca_id, 1
FROM dbo.nguoi_dung nd
CROSS JOIN dbo.ca c
WHERE nd.tai_khoan = N'qlkho02'
AND c.ten_ca IN (N'Sáng', N'Trưa')
AND NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_ca ndc 
  WHERE ndc.nguoi_dung_id=nd.nguoi_dung_id 
    AND ndc.chi_nhanh_id=@cn_q7 
    AND ndc.ca_id=c.ca_id
);
GO

/* ========================
   8) ĐẶT BÀN, ORDER, HÓA ĐƠN
   ======================== */
DECLARE @cn_tt INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Trung tâm');
DECLARE @ban_t01 INT = (SELECT TOP 1 ban_id FROM dbo.ban WHERE chi_nhanh_id=@cn_tt AND so_ban=N'T01');
DECLARE @ban_t02 INT = (SELECT TOP 1 ban_id FROM dbo.ban WHERE chi_nhanh_id=@cn_tt AND so_ban=N'T02');
DECLARE @ban_t03 INT = (SELECT TOP 1 ban_id FROM dbo.ban WHERE chi_nhanh_id=@cn_tt AND so_ban=N'T03');
DECLARE @kh_1 INT = (SELECT MIN(khach_hang_id) FROM dbo.khach_hang);
DECLARE @kh_2 INT = (
  SELECT MIN(khach_hang_id) FROM dbo.khach_hang WHERE khach_hang_id > ISNULL(@kh_1, 0)
);

IF NOT EXISTS (SELECT 1 FROM dbo.dat_ban WHERE ban_id=@ban_t01 AND CAST(ngay_gio AS date)=CAST(GETDATE() AS date))
  INSERT dbo.dat_ban(chi_nhanh_id, ban_id, khach_hang_id, ngay_gio, so_khach, trang_thai, ghi_chu)
  VALUES(@cn_tt, @ban_t01, @kh_1, DATEADD(hour, 12, CAST(GETDATE() AS datetime2(0))), 4, N'ĐÃ XÁC NHẬN', NULL);
IF NOT EXISTS (SELECT 1 FROM dbo.dat_ban WHERE ban_id=@ban_t02 AND CAST(ngay_gio AS date)=CAST(GETDATE() AS date))
  INSERT dbo.dat_ban(chi_nhanh_id, ban_id, khach_hang_id, ngay_gio, so_khach, trang_thai, ghi_chu)
  VALUES(@cn_tt, @ban_t02, @kh_2, DATEADD(hour, 19, CAST(GETDATE() AS datetime2(0))), 6, N'ĐÃ XÁC NHẬN', NULL);

-- ORDER cho bàn T01
DECLARE @po1 INT;
IF NOT EXISTS (SELECT 1 FROM dbo.phieu_order WHERE ban_id=@ban_t01 AND CAST(ngay_gio AS date)=CAST(GETDATE() AS date))
BEGIN
  INSERT dbo.phieu_order(chi_nhanh_id, ban_id, ngay_gio, nhan_vien, trang_thai)
  VALUES(@cn_tt, @ban_t01, DATEADD(hour, 12, CAST(GETDATE() AS datetime2(0))), N'thu_ngan', N'CHỜ THANH TOÁN');
  SET @po1 = SCOPE_IDENTITY();
  INSERT dbo.phieu_order_ct(phieu_order_id, mon_id, so_luong, don_gia)
  SELECT @po1, m.mon_id, x.sl, m.don_gia
  FROM (VALUES(N'MA-03',2),(N'MA-06',1),(N'MA-08',3),(N'MA-10',4)) x(ma,sl)
  JOIN dbo.mon_an m ON m.ma_mon=x.ma;
END
ELSE SET @po1 = (SELECT TOP 1 phieu_order_id FROM dbo.phieu_order WHERE ban_id=@ban_t01 ORDER BY phieu_order_id DESC);

-- ORDER cho bàn T02
DECLARE @po2 INT;
IF NOT EXISTS (SELECT 1 FROM dbo.phieu_order WHERE ban_id=@ban_t02 AND CAST(ngay_gio AS date)=CAST(GETDATE() AS date))
BEGIN
  INSERT dbo.phieu_order(chi_nhanh_id, ban_id, ngay_gio, nhan_vien, trang_thai)
  VALUES(@cn_tt, @ban_t02, DATEADD(hour, 19, CAST(GETDATE() AS datetime2(0))), N'thu_ngan', N'CHỜ THANH TOÁN');
  SET @po2 = SCOPE_IDENTITY();
  INSERT dbo.phieu_order_ct(phieu_order_id, mon_id, so_luong, don_gia)
  SELECT @po2, m.mon_id, x.sl, m.don_gia
  FROM (VALUES(N'MA-04',2),(N'MA-07',1),(N'MA-08',2),(N'MA-10',6)) x(ma,sl)
  JOIN dbo.mon_an m ON m.ma_mon=x.ma;
END
ELSE SET @po2 = (SELECT TOP 1 phieu_order_id FROM dbo.phieu_order WHERE ban_id=@ban_t02 ORDER BY phieu_order_id DESC);

-- HÓA ĐƠN cho các order
DECLARE @tong1 DECIMAL(18,2) = (SELECT SUM(thanh_tien) FROM dbo.phieu_order_ct WHERE phieu_order_id=@po1);
DECLARE @tong2 DECIMAL(18,2) = (SELECT SUM(thanh_tien) FROM dbo.phieu_order_ct WHERE phieu_order_id=@po2);

DECLARE @hd1 INT;
IF NOT EXISTS (SELECT 1 FROM dbo.hoa_don WHERE tham_chieu_id=@po1 AND loai=N'NHAHANG')
BEGIN
  INSERT dbo.hoa_don(chi_nhanh_id, loai, tham_chieu_id, ngay_lap, vat, phi_dv, giam_gia, tong_truoc_thue, tong_sau_thue, trang_thai)
  VALUES(@cn_tt, N'NHAHANG', @po1, SYSUTCDATETIME(), 8, 0, 0, @tong1, @tong1*1.08, N'ĐÃ THANH TOÁN');
  SET @hd1 = SCOPE_IDENTITY();
  INSERT dbo.hoa_don_ct(hoa_don_id, loai_hang, ref_id, ten_hang, so_luong, don_gia)
  SELECT @hd1, N'MÓN', m.mon_id, m.ten_mon, ct.so_luong, ct.don_gia
  FROM dbo.phieu_order_ct ct
  JOIN dbo.mon_an m ON m.mon_id=ct.mon_id
  WHERE ct.phieu_order_id=@po1;
  INSERT dbo.thanh_toan(hoa_don_id, so_tien, ngay_tt, hinh_thuc, ma_tham_chieu)
  VALUES(@hd1, @tong1*1.08, SYSUTCDATETIME(), N'Tiền mặt', NULL);
END

DECLARE @hd2 INT;
IF NOT EXISTS (SELECT 1 FROM dbo.hoa_don WHERE tham_chieu_id=@po2 AND loai=N'NHAHANG')
BEGIN
  INSERT dbo.hoa_don(chi_nhanh_id, loai, tham_chieu_id, ngay_lap, vat, phi_dv, giam_gia, tong_truoc_thue, tong_sau_thue, trang_thai)
  VALUES(@cn_tt, N'NHAHANG', @po2, SYSUTCDATETIME(), 8, 0, 0, @tong2, @tong2*1.08, N'ĐÃ THANH TOÁN');
  SET @hd2 = SCOPE_IDENTITY();
  INSERT dbo.hoa_don_ct(hoa_don_id, loai_hang, ref_id, ten_hang, so_luong, don_gia)
  SELECT @hd2, N'MÓN', m.mon_id, m.ten_mon, ct.so_luong, ct.don_gia
  FROM dbo.phieu_order_ct ct
  JOIN dbo.mon_an m ON m.mon_id=ct.mon_id
  WHERE ct.phieu_order_id=@po2;
  -- Áp mã KM
  INSERT dbo.hoa_don_km(hoa_don_id, km_id, voucher_id, so_tien_km)
  SELECT @hd2, km.km_id, v.voucher_id, CAST(@tong2*0.10 AS DECIMAL(18,2))
  FROM dbo.chuong_trinh_km km
  JOIN dbo.voucher v ON v.km_id=km.km_id AND v.code=N'KM10-001';
  INSERT dbo.thanh_toan(hoa_don_id, so_tien, ngay_tt, hinh_thuc, ma_tham_chieu)
  VALUES(@hd2, @tong2*1.08*0.9, SYSUTCDATETIME(), N'Chuyển khoản', N'TT'+CONVERT(nvarchar(10), @hd2));
END
GO

/* ========================
   9) TIỆC CƯỚI (ĐẶT SẢNH, HỢP ĐỒNG, CỌC, THANH TOÁN) – MẪU TỐI GIẢN
   ======================== */
DECLARE @cn_tt INT = (SELECT chi_nhanh_id FROM dbo.chi_nhanh WHERE ten=N'CN Trung tâm');
DECLARE @goi_cc INT = (SELECT goi_id FROM dbo.goi_tiec WHERE ma_goi=N'GT-CC03');
DECLARE @sanh1 INT = (SELECT sanh_id FROM dbo.sanh WHERE ten_sanh=N'Sảnh Ruby 1' AND chi_nhanh_id=@cn_tt);
DECLARE @ca_toi INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca=N'Tối');
DECLARE @kh_vip INT = (SELECT TOP 1 khach_hang_id FROM dbo.khach_hang WHERE hang_code=N'VIP');

-- Kiểm tra và sửa CHECK constraint nếu cần
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.dat_sanh') AND name LIKE 'CK__dat_sanh__trang%')
BEGIN
  DECLARE @constraint_name NVARCHAR(200) = (SELECT TOP 1 name FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID('dbo.dat_sanh') AND name LIKE 'CK__dat_sanh__trang%');
  DECLARE @sql NVARCHAR(MAX) = N'ALTER TABLE dbo.dat_sanh DROP CONSTRAINT ' + QUOTENAME(@constraint_name);
  EXEC sp_executesql @sql;
  ALTER TABLE dbo.dat_sanh ADD CONSTRAINT CK_dat_sanh_trang_thai CHECK (trang_thai IN (N'CHỜ XÁC NHẬN',N'ĐÃ CỌC',N'ĐÃ HỦY',N'ĐÃ THANH TOÁN',N'HOÀN TẤT'));
END

IF NOT EXISTS (SELECT 1 FROM dbo.dat_sanh WHERE sanh_id=@sanh1 AND ngay_to_chuc = DATEADD(day, 14, CAST(GETDATE() AS date)))
BEGIN
  -- Kiểm tra xem ca_id có tồn tại không
  IF @ca_toi IS NULL
  BEGIN
    PRINT N'Lỗi: Không tìm thấy ca "Tối"';
    RETURN;
  END
  
  INSERT dbo.dat_sanh(chi_nhanh_id, sanh_id, ca_id, gio_to_chuc, ngay_to_chuc, khach_hang_id, so_ban_du_kien, goi_id, trang_thai, ghi_chu)
  VALUES(@cn_tt, @sanh1, @ca_toi, CAST('17:30:00' AS TIME(0)), DATEADD(day, 14, CAST(GETDATE() AS date)), @kh_vip, 30, @goi_cc, N'ĐÃ CỌC', NULL);
  DECLARE @ds_id INT = SCOPE_IDENTITY();
  INSERT dbo.hop_dong(so_hop_dong, dat_sanh_id, ngay_ky, tong_du_kien, dieu_khoan, file_url)
  VALUES(N'HD-TC-0001', @ds_id, CAST(GETDATE() AS date), 12000000, N'Thanh toán 2 đợt.', NULL);
  DECLARE @hd_tc INT = SCOPE_IDENTITY();
  -- Cọc
  INSERT dbo.hop_dong_coc(hop_dong_id, so_tien, ngay_nop, hinh_thuc, ghi_chu)
  VALUES(@hd_tc, 3000000, SYSUTCDATETIME(), N'Chuyển khoản', NULL);
  -- Thanh toán
  INSERT dbo.hop_dong_tt(hop_dong_id, so_tien, ngay_tt, hinh_thuc, noi_dung)
  VALUES(@hd_tc, 9000000, DATEADD(day, 10, SYSUTCDATETIME()), N'Chuyển khoản', N'Thanh toán phần còn lại');
END
GO

/* KẾT THÚC – DỮ LIỆU MẪU */

