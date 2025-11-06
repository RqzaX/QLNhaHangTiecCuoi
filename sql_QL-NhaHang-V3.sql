IF DB_ID(N'QL_NhaHangTiecCuoi_V3') IS NULL
BEGIN
  EXEC('CREATE DATABASE QL_NhaHangTiecCuoi_V3 COLLATE Vietnamese_100_CI_AS_SC;');
END
GO
USE QL_NhaHangTiecCuoi_V3;
GO
/* ======================================================================
   1) DANH MỤC CỐT LÕI (Áp dụng cho cả Nhà hàng & Tiệc cưới)
   ====================================================================== */
IF OBJECT_ID('dbo.chi_nhanh','U') IS NULL
CREATE TABLE dbo.chi_nhanh(
  chi_nhanh_id INT IDENTITY(1,1) PRIMARY KEY,
  ten          NVARCHAR(150) NOT NULL,
  dia_chi      NVARCHAR(250) NULL,
  sdt          NVARCHAR(30)  NULL,
  trang_thai   TINYINT NOT NULL DEFAULT 1
);
select * from chi_nhanh
IF OBJECT_ID('dbo.ca','U') IS NULL
CREATE TABLE dbo.ca(
  ca_id  INT IDENTITY(1,1) PRIMARY KEY,
  ten_ca NVARCHAR(50) NOT NULL,
  gio_bd TIME(0) NOT NULL,
  gio_kt TIME(0) NOT NULL
);

IF OBJECT_ID('dbo.khu_vuc','U') IS NULL
CREATE TABLE dbo.khu_vuc(
  khu_vuc_id   INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id INT NOT NULL,
  ten_khu_vuc  NVARCHAR(100) NOT NULL,
  mo_ta        NVARCHAR(300) NULL,
  FOREIGN KEY (chi_nhanh_id) REFERENCES dbo.chi_nhanh(chi_nhanh_id)
);

IF OBJECT_ID('dbo.ban','U') IS NULL
CREATE TABLE dbo.ban(
  ban_id       INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id INT NOT NULL,
  khu_vuc_id   INT NULL,
  so_ban       NVARCHAR(20) NOT NULL,
  suc_chua     INT NOT NULL CHECK (suc_chua > 0),
  trang_thai   NVARCHAR(20) NOT NULL DEFAULT N'TRỐNG'
              CHECK (trang_thai IN (N'TRỐNG',N'ĐÃ ĐẶT',N'PHỤC VỤ',N'VỆ SINH')),
  UNIQUE (chi_nhanh_id, so_ban),
  FOREIGN KEY (chi_nhanh_id) REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (khu_vuc_id)   REFERENCES dbo.khu_vuc(khu_vuc_id)
);

IF OBJECT_ID('dbo.sanh','U') IS NULL
CREATE TABLE dbo.sanh(
  sanh_id      INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id INT NOT NULL,
  ten_sanh     NVARCHAR(100) NOT NULL,
  suc_chua     INT NOT NULL CHECK (suc_chua > 0),
  phi_thue_cb  DECIMAL(18,2) NOT NULL DEFAULT 0,
  UNIQUE (chi_nhanh_id, ten_sanh),
  FOREIGN KEY (chi_nhanh_id) REFERENCES dbo.chi_nhanh(chi_nhanh_id)
);

IF OBJECT_ID('dbo.khach_hang','U') IS NULL
CREATE TABLE dbo.khach_hang(
  khach_hang_id INT IDENTITY(1,1) PRIMARY KEY,
  ho_ten        NVARCHAR(150) NOT NULL,
  sdt           NVARCHAR(30)  NULL,
  email         NVARCHAR(150) NULL,
  ghi_chu       NVARCHAR(300) NULL
);

-- Thực đơn & Dịch vụ (đơn giản)
IF OBJECT_ID('dbo.mon_an','U') IS NULL
CREATE TABLE dbo.mon_an(
  mon_id      INT IDENTITY(1,1) PRIMARY KEY,
  ma_mon      NVARCHAR(30)  NOT NULL UNIQUE,
  ten_mon     NVARCHAR(200) NOT NULL,
  nhom        NVARCHAR(100) NULL,
  don_vi_tinh NVARCHAR(30)  NOT NULL,
  don_gia     DECIMAL(18,2) NOT NULL CHECK (don_gia >= 0),
  dang_ban    TINYINT NOT NULL DEFAULT 1
);


IF OBJECT_ID('dbo.dich_vu','U') IS NULL
CREATE TABLE dbo.dich_vu(
  dv_id       INT IDENTITY(1,1) PRIMARY KEY,
  ma_dv       NVARCHAR(30)  NOT NULL UNIQUE,
  ten_dv      NVARCHAR(200) NOT NULL,
  don_vi_tinh NVARCHAR(30)  NOT NULL,
  don_gia     DECIMAL(18,2) NOT NULL CHECK (don_gia >= 0),
  dang_ban    TINYINT NOT NULL DEFAULT 1
);

-- Gói tiệc cưới (tùy chọn)
IF OBJECT_ID('dbo.goi_tiec','U') IS NULL
CREATE TABLE dbo.goi_tiec(
  goi_id     INT IDENTITY(1,1) PRIMARY KEY,
  ma_goi     NVARCHAR(30)  NOT NULL UNIQUE,
  ten_goi    NVARCHAR(150) NOT NULL,
  gia_co_ban DECIMAL(18,2) NOT NULL DEFAULT 0
);

IF OBJECT_ID('dbo.goi_tiec_mon','U') IS NULL
CREATE TABLE dbo.goi_tiec_mon(
  goi_id   INT NOT NULL,
  mon_id   INT NOT NULL,
  so_luong DECIMAL(18,3) NOT NULL CHECK (so_luong > 0),
  PRIMARY KEY (goi_id, mon_id),
  FOREIGN KEY (goi_id) REFERENCES dbo.goi_tiec(goi_id),
  FOREIGN KEY (mon_id) REFERENCES dbo.mon_an(mon_id)
);

IF OBJECT_ID('dbo.goi_tiec_dv','U') IS NULL
CREATE TABLE dbo.goi_tiec_dv(
  goi_id   INT NOT NULL,
  dv_id    INT NOT NULL,
  so_luong DECIMAL(18,3) NOT NULL CHECK (so_luong > 0),
  PRIMARY KEY (goi_id, dv_id),
  FOREIGN KEY (goi_id) REFERENCES dbo.goi_tiec(goi_id),
  FOREIGN KEY (dv_id)  REFERENCES dbo.dich_vu(dv_id)
);	

GO
/* ======================================================================
   2) LUỒNG NHÀ HÀNG (BÀN) — ĐẶT CHỖ, ORDER, HÓA ĐƠN
   ====================================================================== */
IF OBJECT_ID('dbo.dat_ban','U') IS NULL
CREATE TABLE dbo.dat_ban(
  dat_ban_id    INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id  INT NOT NULL,
  ban_id        INT NOT NULL,
  khach_hang_id INT NOT NULL,
  ngay_gio      DATETIME2(0) NOT NULL,
  so_khach      INT NOT NULL CHECK (so_khach > 0),
  trang_thai    NVARCHAR(20) NOT NULL DEFAULT N'CHỜ XÁC NHẬN'
               CHECK (trang_thai IN (N'CHỜ XÁC NHẬN',N'ĐÃ XÁC NHẬN',N'ĐÃ HỦY',N'ĐÃ PHỤC VỤ')),
  ghi_chu       NVARCHAR(300) NULL,
  FOREIGN KEY (chi_nhanh_id)  REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (ban_id)        REFERENCES dbo.ban(ban_id),
  FOREIGN KEY (khach_hang_id) REFERENCES dbo.khach_hang(khach_hang_id)
);

IF OBJECT_ID('dbo.phieu_order','U') IS NULL
CREATE TABLE dbo.phieu_order(
  phieu_order_id INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id   INT NOT NULL,
  ban_id         INT NULL,
  dat_sanh_id    INT NULL, -- dùng chung, nếu order cho tiệc
  ngay_gio       DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  nhan_vien      NVARCHAR(100) NULL,
  trang_thai     NVARCHAR(20) NOT NULL DEFAULT N'ĐANG PHỤC VỤ'
               CHECK (trang_thai IN (N'ĐANG PHỤC VỤ',N'CHỜ THANH TOÁN',N'ĐÃ ĐÓNG')),
  FOREIGN KEY (chi_nhanh_id) REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (ban_id)       REFERENCES dbo.ban(ban_id)
  -- dat_sanh_id: FK thêm sau khi tạo dat_sanh
);

IF OBJECT_ID('dbo.phieu_order_ct','U') IS NULL
CREATE TABLE dbo.phieu_order_ct(
  order_ct_id    INT IDENTITY(1,1) PRIMARY KEY,
  phieu_order_id INT NOT NULL,
  mon_id         INT NOT NULL,
  so_luong       DECIMAL(18,3) NOT NULL CHECK (so_luong > 0),
  don_gia        DECIMAL(18,2) NOT NULL CHECK (don_gia >= 0),
  thanh_tien     AS (ROUND(so_luong * don_gia, 0)) PERSISTED,
  ghi_chu_bep    NVARCHAR(200) NULL,
  FOREIGN KEY (phieu_order_id) REFERENCES dbo.phieu_order(phieu_order_id),
  FOREIGN KEY (mon_id)         REFERENCES dbo.mon_an(mon_id)
);

IF OBJECT_ID('dbo.hoa_don','U') IS NULL
CREATE TABLE dbo.hoa_don(
  hoa_don_id    INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id  INT NOT NULL,
  loai          NVARCHAR(15) NOT NULL CHECK (loai IN (N'NHAHANG',N'TIECCUOI')),
  tham_chieu_id INT NULL, -- dat_ban_id hoặc hop_dong_id
  ngay_lap      DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  vat           DECIMAL(5,2) NOT NULL DEFAULT 0,
  phi_dv        DECIMAL(18,2) NOT NULL DEFAULT 0,
  giam_gia      DECIMAL(18,2) NOT NULL DEFAULT 0,
  tong_truoc_thue DECIMAL(18,2) NOT NULL DEFAULT 0,
  tong_sau_thue   DECIMAL(18,2) NOT NULL DEFAULT 0,
  trang_thai    NVARCHAR(20) NOT NULL DEFAULT N'NHÁP'
               CHECK (trang_thai IN (N'NHÁP',N'CHỜ TT',N'ĐÃ THANH TOÁN')),
  FOREIGN KEY (chi_nhanh_id) REFERENCES dbo.chi_nhanh(chi_nhanh_id)
);

IF OBJECT_ID('dbo.hoa_don_ct','U') IS NULL
CREATE TABLE dbo.hoa_don_ct(
  hd_ct_id   INT IDENTITY(1,1) PRIMARY KEY,
  hoa_don_id INT NOT NULL,
  loai_hang  NVARCHAR(10) NOT NULL CHECK (loai_hang IN (N'MÓN',N'DV')),
  ref_id     INT NOT NULL,  -- mon_id hoặc dv_id
  ten_hang   NVARCHAR(200) NOT NULL,
  so_luong   DECIMAL(18,3) NOT NULL CHECK (so_luong > 0),
  don_gia    DECIMAL(18,2) NOT NULL CHECK (don_gia >= 0),
  thanh_tien AS (ROUND(so_luong * don_gia, 0)) PERSISTED,
  FOREIGN KEY (hoa_don_id) REFERENCES dbo.hoa_don(hoa_don_id)
);

IF OBJECT_ID('dbo.thanh_toan','U') IS NULL
CREATE TABLE dbo.thanh_toan(
  tt_id      INT IDENTITY(1,1) PRIMARY KEY,
  hoa_don_id INT NOT NULL,
  so_tien    DECIMAL(18,2) NOT NULL CHECK (so_tien > 0),
  ngay_tt    DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  hinh_thuc  NVARCHAR(30)  NULL,
  ma_tham_chieu NVARCHAR(50) NULL,
  FOREIGN KEY (hoa_don_id) REFERENCES dbo.hoa_don(hoa_don_id)
);

GO
/* ======================================================================
   3) LUỒNG TIỆC CƯỚI — ĐẶT SẢNH, HỢP ĐỒNG, CỌC/THANH TOÁN
   ====================================================================== */
IF OBJECT_ID('dbo.dat_sanh','U') IS NULL
CREATE TABLE dbo.dat_sanh(
  dat_sanh_id    INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id   INT NOT NULL,
  sanh_id        INT NOT NULL,
  ca_id          INT NOT NULL,
  ngay_to_chuc   DATE NOT NULL,
  khach_hang_id  INT NOT NULL,
  so_ban_du_kien INT NULL,
  goi_id         INT NULL,
  trang_thai     NVARCHAR(20) NOT NULL DEFAULT N'CHỜ XÁC NHẬN'
                CHECK (trang_thai IN (N'CHỜ XÁC NHẬN',N'ĐÃ XÁC NHẬN',N'ĐÃ HỦY',N'HOÀN TẤT')),
  ghi_chu        NVARCHAR(300) NULL,
  UNIQUE (sanh_id, ca_id, ngay_to_chuc),
  FOREIGN KEY (chi_nhanh_id)  REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (sanh_id)       REFERENCES dbo.sanh(sanh_id),
  FOREIGN KEY (ca_id)         REFERENCES dbo.ca(ca_id),
  FOREIGN KEY (khach_hang_id) REFERENCES dbo.khach_hang(khach_hang_id),
  FOREIGN KEY (goi_id)        REFERENCES dbo.goi_tiec(goi_id)
);

-- Bổ sung FK còn thiếu cho phieu_order (order phục vụ trong tiệc)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_po_ds')
  ALTER TABLE dbo.phieu_order ADD CONSTRAINT FK_po_ds FOREIGN KEY (dat_sanh_id) REFERENCES dbo.dat_sanh(dat_sanh_id);

IF OBJECT_ID('dbo.hop_dong','U') IS NULL
CREATE TABLE dbo.hop_dong(
  hop_dong_id  INT IDENTITY(1,1) PRIMARY KEY,
  so_hop_dong  NVARCHAR(30) NOT NULL UNIQUE,
  dat_sanh_id  INT NOT NULL,
  ngay_ky      DATE NOT NULL,
  tong_du_kien DECIMAL(18,2) NOT NULL DEFAULT 0,
  dieu_khoan   NVARCHAR(MAX) NULL,
  file_url     NVARCHAR(400) NULL,
  FOREIGN KEY (dat_sanh_id) REFERENCES dbo.dat_sanh(dat_sanh_id)
);

IF OBJECT_ID('dbo.hop_dong_ct_mon','U') IS NULL
CREATE TABLE dbo.hop_dong_ct_mon(
  ct_mon_id   INT IDENTITY(1,1) PRIMARY KEY,
  hop_dong_id INT NOT NULL,
  mon_id      INT NOT NULL,
  so_luong    DECIMAL(18,3) NOT NULL CHECK (so_luong > 0),
  don_gia     DECIMAL(18,2) NOT NULL CHECK (don_gia >= 0),
  thanh_tien  AS (ROUND(so_luong * don_gia, 0)) PERSISTED,
  FOREIGN KEY (hop_dong_id) REFERENCES dbo.hop_dong(hop_dong_id),
  FOREIGN KEY (mon_id)      REFERENCES dbo.mon_an(mon_id)
);

IF OBJECT_ID('dbo.hop_dong_ct_dv','U') IS NULL
CREATE TABLE dbo.hop_dong_ct_dv(
  ct_dv_id    INT IDENTITY(1,1) PRIMARY KEY,
  hop_dong_id INT NOT NULL,
  dv_id       INT NOT NULL,
  so_luong    DECIMAL(18,3) NOT NULL CHECK (so_luong > 0),
  don_gia     DECIMAL(18,2) NOT NULL CHECK (don_gia >= 0),
  thanh_tien  AS (ROUND(so_luong * don_gia, 0)) PERSISTED,
  FOREIGN KEY (hop_dong_id) REFERENCES dbo.hop_dong(hop_dong_id),
  FOREIGN KEY (dv_id)       REFERENCES dbo.dich_vu(dv_id)
);

IF OBJECT_ID('dbo.hop_dong_coc','U') IS NULL
CREATE TABLE dbo.hop_dong_coc(
  coc_id      INT IDENTITY(1,1) PRIMARY KEY,
  hop_dong_id INT NOT NULL,
  so_tien     DECIMAL(18,2) NOT NULL CHECK (so_tien > 0),
  ngay_nop    DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  hinh_thuc   NVARCHAR(30)  NULL,
  ghi_chu     NVARCHAR(300) NULL,
  FOREIGN KEY (hop_dong_id) REFERENCES dbo.hop_dong(hop_dong_id)
);

IF OBJECT_ID('dbo.hop_dong_tt','U') IS NULL
CREATE TABLE dbo.hop_dong_tt(
  tt_id       INT IDENTITY(1,1) PRIMARY KEY,
  hop_dong_id INT NOT NULL,
  so_tien     DECIMAL(18,2) NOT NULL CHECK (so_tien > 0),
  ngay_tt     DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  hinh_thuc   NVARCHAR(30)  NULL,
  noi_dung    NVARCHAR(200) NULL,
  FOREIGN KEY (hop_dong_id) REFERENCES dbo.hop_dong(hop_dong_id)
);

GO
/* ======================================================================
   4) MODULE TỐI THIỂU — KHO, KHUYẾN MÃI, NGƯỜI DÙNG (MVP)
   ====================================================================== */

-- Kho: master + tồn theo chi nhánh (không chi tiết phiếu)
IF OBJECT_ID('dbo.nguyen_lieu','U') IS NULL
CREATE TABLE dbo.nguyen_lieu(
  nl_id   INT IDENTITY(1,1) PRIMARY KEY,
  ma_nl   NVARCHAR(50) NOT NULL UNIQUE,
  ten_nl  NVARCHAR(200) NOT NULL,
  don_vi  NVARCHAR(30)  NOT NULL
);


IF OBJECT_ID('dbo.ton_kho','U') IS NULL
CREATE TABLE dbo.ton_kho(
  chi_nhanh_id INT NOT NULL,
  nl_id        INT NOT NULL,
  sl_ton       DECIMAL(18,3) NOT NULL DEFAULT 0,
  PRIMARY KEY (chi_nhanh_id, nl_id),
  FOREIGN KEY (chi_nhanh_id) REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (nl_id)        REFERENCES dbo.nguyen_lieu(nl_id)
);

-- Khuyến mãi: CTKM giảm theo hóa đơn + voucher (đơn giản)
IF OBJECT_ID('dbo.chuong_trinh_km','U') IS NULL
CREATE TABLE dbo.chuong_trinh_km(
  km_id       INT IDENTITY(1,1) PRIMARY KEY,
  ma_km       NVARCHAR(40) NOT NULL UNIQUE,
  ten         NVARCHAR(200) NOT NULL,
  hinh_thuc   NVARCHAR(10) NOT NULL CHECK (hinh_thuc IN (N'PERCENT',N'AMOUNT')),
  gia_tri     DECIMAL(18,2) NOT NULL CHECK (gia_tri>=0),
  tg_bat_dau  DATETIME2(0) NOT NULL,
  tg_ket_thuc DATETIME2(0) NOT NULL,
  ap_dung_loai NVARCHAR(20) NOT NULL DEFAULT N'ALL' CHECK (ap_dung_loai IN (N'ALL',N'NHAHANG',N'TIECCUOI'))
);

IF OBJECT_ID('dbo.voucher','U') IS NULL
CREATE TABLE dbo.voucher(
  voucher_id INT IDENTITY(1,1) PRIMARY KEY,
  km_id      INT NOT NULL,
  code       NVARCHAR(50) NOT NULL UNIQUE,
  so_lan     INT NOT NULL DEFAULT 1,
  da_dung    INT NOT NULL DEFAULT 0,
  han_dung   DATE NULL,
  FOREIGN KEY (km_id) REFERENCES dbo.chuong_trinh_km(km_id)
);

IF OBJECT_ID('dbo.hoa_don_km','U') IS NULL
CREATE TABLE dbo.hoa_don_km(
  hd_km_id   INT IDENTITY(1,1) PRIMARY KEY,
  hoa_don_id INT NOT NULL,
  km_id      INT NULL,
  voucher_id INT NULL,
  so_tien_km DECIMAL(18,2) NOT NULL,
  FOREIGN KEY (hoa_don_id) REFERENCES dbo.hoa_don(hoa_don_id),
  FOREIGN KEY (km_id)      REFERENCES dbo.chuong_trinh_km(km_id),
  FOREIGN KEY (voucher_id) REFERENCES dbo.voucher(voucher_id)
);

-- Người dùng tối thiểu
IF OBJECT_ID('dbo.vai_tro','U') IS NULL
CREATE TABLE dbo.vai_tro(
  vai_tro_id INT IDENTITY(1,1) PRIMARY KEY,
  ma         NVARCHAR(40) NOT NULL UNIQUE,
  ten        NVARCHAR(100) NOT NULL
);

IF OBJECT_ID('dbo.nguoi_dung','U') IS NULL
CREATE TABLE dbo.nguoi_dung(
  nguoi_dung_id INT IDENTITY(1,1) PRIMARY KEY,
  tai_khoan     NVARCHAR(80) NOT NULL UNIQUE,
  mat_khau		NVARCHAR(80) NOT NULL,
  ho_ten        NVARCHAR(150) NOT NULL,
  hoat_dong     BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('dbo.nguoi_dung_vai_tro','U') IS NULL
CREATE TABLE dbo.nguoi_dung_vai_tro(
  nguoi_dung_id INT NOT NULL,
  vai_tro_id    INT NOT NULL,
  PRIMARY KEY (nguoi_dung_id, vai_tro_id),
  FOREIGN KEY (nguoi_dung_id) REFERENCES dbo.nguoi_dung(nguoi_dung_id),
  FOREIGN KEY (vai_tro_id)    REFERENCES dbo.vai_tro(vai_tro_id)
);

IF OBJECT_ID('dbo.nguoi_dung_chi_nhanh','U') IS NULL
CREATE TABLE dbo.nguoi_dung_chi_nhanh(
    nguoi_dung_id INT NOT NULL,
    chi_nhanh_id  INT NOT NULL,
    PRIMARY KEY (nguoi_dung_id, chi_nhanh_id),
    FOREIGN KEY (nguoi_dung_id) REFERENCES dbo.nguoi_dung(nguoi_dung_id),
    FOREIGN KEY (chi_nhanh_id)  REFERENCES dbo.chi_nhanh(chi_nhanh_id)
);

GO
/* ======================================================================
   5) INDEXES THIẾT THỰC
   ====================================================================== */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_dat_sanh_unique' AND object_id=OBJECT_ID('dbo.dat_sanh'))
  CREATE UNIQUE INDEX IX_dat_sanh_unique ON dbo.dat_sanh(sanh_id, ca_id, ngay_to_chuc);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_order_head' AND object_id=OBJECT_ID('dbo.phieu_order'))
  CREATE INDEX IX_order_head ON dbo.phieu_order(chi_nhanh_id, trang_thai, ngay_gio);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_hd_lap' AND object_id=OBJECT_ID('dbo.hoa_don'))
  CREATE INDEX IX_hd_lap ON dbo.hoa_don(chi_nhanh_id, loai, ngay_lap);

GO
/*=======Thêm Dữ liệu vào Nguyên liệu==========*/
IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-GAO')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-GAO',      N'Gạo',          N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-THIT-BO')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-THIT-BO',  N'Thịt bò',      N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-THIT-GA')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-THIT-GA',  N'Thịt gà',      N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-TOM')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-TOM',      N'Tôm',          N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-CA-HOI')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-CA-HOI',   N'Cá hồi',       N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-TRUNG')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-TRUNG',    N'Trứng',        N'quả');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-BOT-MI')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-BOT-MI',   N'Bột mì',       N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-DUONG')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-DUONG',    N'Đường',        N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-MUOI')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-MUOI',     N'Muối',         N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-TIEU')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-TIEU',     N'Tiêu',         N'g');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-DAU-AN')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-DAU-AN',   N'Dầu ăn',       N'lít');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-NUOC-MAM')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-NUOC-MAM', N'Nước mắm',     N'chai');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-TOI')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-TOI',      N'Tỏi',          N'kg');

IF NOT EXISTS (SELECT 1 FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-HANH-LA')
INSERT INTO dbo.nguyen_lieu (ma_nl, ten_nl, don_vi) VALUES (N'NL-HANH-LA',  N'Hành lá',      N'bó');

/* 1) Xác định chi nhánh để seed
        - Ưu tiên tên 'Chi nhánh Hồ Chí Minh'
        - Nếu không có, lấy chi nhánh có ID nhỏ nhất
  */
  DECLARE @cn_id INT =
      (SELECT TOP 1 chi_nhanh_id
       FROM dbo.chi_nhanh
       WHERE ten = N'Chi nhánh Hồ Chí Minh'
       ORDER BY chi_nhanh_id);

  IF @cn_id IS NULL
      SELECT @cn_id = MIN(chi_nhanh_id) FROM dbo.chi_nhanh;
	  /* 2) Chèn tồn kho cho TẤT CẢ nguyên liệu chưa có trong tồn kho của chi nhánh @cn_id,
        gán số lượng khởi tạo theo ma_nl (có thể chỉnh sửa các giá trị bên dưới) */
  INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton)
  SELECT
      @cn_id,
      nl.nl_id,
      CAST(CASE nl.ma_nl
            WHEN N'NL-GAO'       THEN 100
            WHEN N'NL-THIT-BO'   THEN 40
            WHEN N'NL-THIT-GA'   THEN 60
            WHEN N'NL-TOM'       THEN 30
            WHEN N'NL-CA-HOI'    THEN 20
            WHEN N'NL-TRUNG'     THEN 200
            WHEN N'NL-BOT-MI'    THEN 100
            WHEN N'NL-DUONG'     THEN 80
            WHEN N'NL-MUOI'      THEN 50
            WHEN N'NL-TIEU'      THEN 10
            WHEN N'NL-DAU-AN'    THEN 30
            WHEN N'NL-NUOC-MAM'  THEN 25
            WHEN N'NL-TOI'       THEN 40
            WHEN N'NL-HANH-LA'   THEN 50
            ELSE 0
          END AS DECIMAL(18,3)) AS sl_ton
  FROM dbo.nguyen_lieu AS nl
  WHERE NOT EXISTS (
      SELECT 1
      FROM dbo.ton_kho tk
      WHERE tk.chi_nhanh_id = @cn_id
        AND tk.nl_id = nl.nl_id
  );
  select * from mon_an
  
/*----Nếu muốn tạo đủ dòng ton_kho cho mọi chi nhánh trước, để sau này mới cập nhật số lượng:
INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton)
SELECT cn.chi_nhanh_id, nl.nl_id, 0
FROM dbo.chi_nhanh cn
CROSS JOIN dbo.nguyen_lieu nl
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.ton_kho tk
  WHERE tk.chi_nhanh_id = cn.chi_nhanh_id
    AND tk.nl_id = nl.nl_id
);
---*/
-- Lấy nl_id nhanh theo mã NL (ví dụ: 'NL-BOT-MI')
DECLARE @nl_id INT =
(
    SELECT nl_id FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-BOT-MI'
);
-- CHÈN các dòng ton_kho = 0 cho mọi chi nhánh còn thiếu của nguyên liệu này
INSERT INTO dbo.ton_kho (chi_nhanh_id, nl_id, sl_ton)
SELECT cn.chi_nhanh_id, @nl_id, 0
FROM dbo.chi_nhanh AS cn
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.ton_kho AS tk
    WHERE tk.chi_nhanh_id = cn.chi_nhanh_id
      AND tk.nl_id        = @nl_id
);
--kiem tra
SELECT tk.chi_nhanh_id, cn.ten AS ten_chi_nhanh, tk.sl_ton
FROM dbo.ton_kho tk
LEFT JOIN dbo.chi_nhanh cn ON cn.chi_nhanh_id = tk.chi_nhanh_id
WHERE tk.nl_id = (SELECT nl_id FROM dbo.nguyen_lieu WHERE ma_nl = N'NL-BOT-MI')
ORDER BY cn.ten;


	select * from ton_kho
--- Thêm Dữ Liệu Gói Tiệc
IF OBJECT_ID('dbo.goi_tiec','U') IS NOT NULL
BEGIN
    -- Lệnh INSERT INTO để thêm dữ liệu
    INSERT INTO dbo.goi_tiec (ma_goi, ten_goi, gia_co_ban)
    VALUES
    -- Gói tiệc CƠ BẢN
    ('GT-CB01', N'Gói Tiệc Cưới Cơ Bản', 5000000.00),
    
    -- Gói tiệc TRUNG CẤP
    ('GT-TC02', N'Gói Tiệc Sinh Nhật Thịnh Vượng', 8500000.00),
    
    -- Gói tiệc CAO CẤP
    ('GT-CC03', N'Gói Tiệc Cưới Cao Cấp', 12000000.00),
    
    -- Gói tiệc KẾT HỢP
    ('GT-KH04', N'Gói Tiệc  Trọn Gói', 7250000.00);
END

select * from goi_tiec

DECLARE @map TABLE(
  tai_khoan NVARCHAR(80),
  ten_cn    NVARCHAR(150)
);

INSERT INTO @map(tai_khoan, ten_cn) VALUES
 (N'admin',     N'Chi nhánh Hồ Chí Minh'),
 (N'admin',     N'Chi nhánh Hà Nội'),
 (N'ql_hcm',    N'Chi nhánh Hồ Chí Minh'),
 (N'ql_hn',     N'Chi nhánh Hà Nội'),
 (N'nhanvien',  N'Chi nhánh Hồ Chí Minh');

INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
SELECT nd.nguoi_dung_id, cn.chi_nhanh_id
FROM @map m
JOIN dbo.nguoi_dung nd ON nd.tai_khoan = m.tai_khoan
JOIN dbo.chi_nhanh  cn ON cn.ten       = m.ten_cn
WHERE NOT EXISTS (
  SELECT 1 FROM dbo.nguoi_dung_chi_nhanh x
  WHERE x.nguoi_dung_id = nd.nguoi_dung_id
    AND x.chi_nhanh_id  = cn.chi_nhanh_id
);

/* Kiểm tra nhanh */
SELECT nd.tai_khoan, nd.ho_ten, cn.ten AS ten_chi_nhanh
FROM dbo.nguoi_dung nd
JOIN dbo.nguoi_dung_chi_nhanh ncn ON ncn.nguoi_dung_id = nd.nguoi_dung_id
JOIN dbo.chi_nhanh cn ON cn.chi_nhanh_id = ncn.chi_nhanh_id
ORDER BY nd.tai_khoan, cn.ten;

--Gói tiệc món 
select * from goi_tiec_mon


 INSERT INTO dbo.goi_tiec_mon (goi_id, mon_id, so_luong)
  SELECT g.goi_id, m.mon_id, v.so_luong
  FROM dbo.goi_tiec g
  JOIN (VALUES
      (N'MA001', 10.0), -- Phở bò
      (N'MA002', 10.0), -- Cơm gà
      (N'MA003', 10.0), -- Bún chả
      (N'MA004', 10.0), -- Bánh mì (phụ)
      (N'MA005', 30.0)  -- Trà đá
  ) v(ma_mon, so_luong) ON 1=1
  JOIN dbo.mon_an m ON m.ma_mon = v.ma_mon
  WHERE g.ma_goi = N'GT-CB01'
    AND NOT EXISTS (
      SELECT 1 FROM dbo.goi_tiec_mon x
      WHERE x.goi_id = g.goi_id AND x.mon_id = m.mon_id
    );

	INSERT INTO dbo.goi_tiec_mon (goi_id, mon_id, so_luong)
  SELECT g.goi_id, m.mon_id, v.so_luong
  FROM dbo.goi_tiec g
  JOIN (VALUES
      (N'MA001', 8),
      (N'MA002', 8),
      (N'MA003', 8),
      (N'MA004', 12),
      (N'MA005', 40)
  ) v(ma_mon, so_luong) ON 1=1
  JOIN dbo.mon_an m ON m.ma_mon = v.ma_mon
  WHERE g.ma_goi = N'GT-TC02'
    AND NOT EXISTS (
      SELECT 1 FROM dbo.goi_tiec_mon x
      WHERE x.goi_id = g.goi_id AND x.mon_id = m.mon_id
    );
	
	 INSERT INTO dbo.goi_tiec_mon (goi_id, mon_id, so_luong)
  SELECT g.goi_id, m.mon_id, v.so_luong
  FROM dbo.goi_tiec g
  JOIN (VALUES
      (N'MA001', 12),
      (N'MA002', 12),
      (N'MA003', 12),
      (N'MA004', 12),
      (N'MA005', 60),
      (N'MA006', 4)
  ) v(ma_mon, so_luong) ON 1=1
  JOIN dbo.mon_an m ON m.ma_mon = v.ma_mon
  WHERE g.ma_goi = N'GT003'
    AND NOT EXISTS (
      SELECT 1 FROM dbo.goi_tiec_mon x
      WHERE x.goi_id = g.goi_id AND x.mon_id = m.mon_id
    );
		 INSERT INTO dbo.goi_tiec_mon (goi_id, mon_id, so_luong)
  SELECT g.goi_id, m.mon_id, v.so_luong
  FROM dbo.goi_tiec g
  JOIN (VALUES
      (N'MA001', 12),
      (N'MA002', 12),
      (N'MA003', 12),
      (N'MA004', 12),
      (N'MA005', 12),
      (N'MA006', 12)
  ) v(ma_mon, so_luong) ON 1=1
  JOIN dbo.mon_an m ON m.ma_mon = v.ma_mon
  WHERE g.ma_goi = N'GT001'
    AND NOT EXISTS (
      SELECT 1 FROM dbo.goi_tiec_mon x
      WHERE x.goi_id = g.goi_id AND x.mon_id = m.mon_id
    );

	  SELECT g.ma_goi, m.ma_mon, m.ten_mon, gtm.so_luong
  FROM dbo.goi_tiec_mon gtm
  JOIN dbo.goi_tiec g ON g.goi_id = gtm.goi_id
  JOIN dbo.mon_an m ON m.mon_id = gtm.mon_id
  ORDER BY g.ma_goi, m.ma_mon;	
-- Thêm dữ liệu mới vào dbo.goi_tiec (idempotent)
INSERT INTO dbo.goi_tiec (ma_goi, ten_goi, gia_co_ban)
VALUES (N'GT001', N'Gói tiệc cưới Ngon', 35000000.00);

--Thêm dữ liệu vào sảnh
INSERT INTO dbo.sanh (chi_nhanh_id, ten_sanh, suc_chua, phi_thue_cb)
SELECT v.chi_nhanh_id, v.ten_sanh, v.suc_chua, v.phi_thue_cb
FROM (VALUES
  (1, N'Sảnh Ruby',           300, 15000000.00),
  (1, N'Sảnh Sapphire',       100, 25000000.00),
  (1, N'Sảnh Emerald',        200,  8000000.00),

  (2, N'Sảnh Diamond',        350, 30000000.00),
  (2, N'Sảnh Pearl',          250, 12000000.00),
  (2, N'Sảnh Topaz',          400, 18000000.00)

) AS v(chi_nhanh_id, ten_sanh, suc_chua, phi_thue_cb)
WHERE NOT EXISTS (
  SELECT 1
  FROM dbo.sanh s
  WHERE s.chi_nhanh_id = v.chi_nhanh_id
    AND s.ten_sanh     = v.ten_sanh
);
-- Kiểm tra sảnh
SELECT sanh_id, chi_nhanh_id, ten_sanh, suc_chua, phi_thue_cb
FROM dbo.sanh
ORDER BY chi_nhanh_id, ten_sanh;
-----------------------------------------
-- ===============================
-- SEED: CHUONG TRINH KHUYEN MAI
-- ===============================
BEGIN TRAN;

-- 1) KM toàn hệ thống: Black Friday 2025, giảm 20%
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMALL_BLACKFRIDAY_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMALL_BLACKFRIDAY_2025', N'Black Friday 2025 - Giảm 20%', N'PERCENT', 20.00,
        '2025-11-25T00:00:00', '2025-11-30T23:59:59', N'ALL');

-- 2) KM nhà hàng: Trưa ngày thường Q4/2025, giảm 10%
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMNH_LUNCH_Q4_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMNH_LUNCH_Q4_2025', N'Lunch Weekday Q4/2025 - Giảm 10%', N'PERCENT', 10.00,
        '2025-10-01T00:00:00', '2025-12-31T23:59:59', N'NHAHANG');

-- 3) KM tiệc cưới: Tháng 12/2025 giảm thẳng 500,000đ
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMTIEC_T12_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMTIEC_T12_2025', N'Tiệc cưới 12/2025 - Giảm 500K', N'AMOUNT', 500000.00,
        '2025-12-01T00:00:00', '2025-12-31T23:59:59', N'TIECCUOI');

-- 4) KM nhà hàng: Happy Hour Q4/2025 giảm thẳng 100,000đ
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMNH_HAPPYHOUR_Q4_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMNH_HAPPYHOUR_Q4_2025', N'Happy Hour Q4/2025 - Giảm 100K', N'AMOUNT', 100000.00,
        '2025-10-01T00:00:00', '2025-12-31T23:59:59', N'NHAHANG');

COMMIT;
GO

BEGIN TRAN;

-- 1) KM toàn hệ thống: Black Friday 2025, giảm 20%
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMALL_BLACKFRIDAY_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMALL_BLACKFRIDAY_2025', N'Black Friday 2025 - Giảm 20%', N'PERCENT', 20.00,
        '2025-11-25T00:00:00', '2025-11-30T23:59:59', N'ALL');

-- 2) KM nhà hàng: Trưa ngày thường Q4/2025, giảm 10%
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMNH_LUNCH_Q4_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMNH_LUNCH_Q4_2025', N'Lunch Weekday Q4/2025 - Giảm 10%', N'PERCENT', 10.00,
        '2025-10-01T00:00:00', '2025-12-31T23:59:59', N'NHAHANG');

-- 3) KM tiệc cưới: Tháng 12/2025 giảm thẳng 500,000đ
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMTIEC_T12_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMTIEC_T12_2025', N'Tiệc cưới 12/2025 - Giảm 500K', N'AMOUNT', 500000.00,
        '2025-12-01T00:00:00', '2025-12-31T23:59:59', N'TIECCUOI');

-- 4) KM nhà hàng: Happy Hour Q4/2025 giảm thẳng 100,000đ
IF NOT EXISTS (SELECT 1 FROM dbo.chuong_trinh_km WHERE ma_km = N'KMNH_HAPPYHOUR_Q4_2025')
INSERT INTO dbo.chuong_trinh_km (ma_km, ten, hinh_thuc, gia_tri, tg_bat_dau, tg_ket_thuc, ap_dung_loai)
VALUES (N'KMNH_HAPPYHOUR_Q4_2025', N'Happy Hour Q4/2025 - Giảm 100K', N'AMOUNT', 100000.00,
        '2025-10-01T00:00:00', '2025-12-31T23:59:59', N'NHAHANG');


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
---cập nhật
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

SELECT 
    ma_goi AS [Mã gói],
    ten_goi AS [Tên gói],
    gia_co_ban AS [Giá cơ bản],
    suc_chua AS [Sức chứa]
FROM dbo.goi_tiec
ORDER BY ma_goi;
GO
 select * from chuong_trinh_km
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

-- TẠO 5 VOUCHER / MỖI CHƯƠNG TRÌNH KHUYẾN MÃI
;WITH nums(n) AS (
    SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5
)
INSERT INTO dbo.voucher(km_id, code, so_lan, da_dung, han_dung)
SELECT
    km.km_id,
    CONCAT(km.ma_km, '-', FORMAT(SYSDATETIME(), 'yyyyMMdd'), '-', RIGHT('000' + CAST(nums.n AS varchar(3)), 3)) AS code,
    1 AS so_lan,               -- cơ bản: 1 lần dùng / mã
    0 AS da_dung,              -- chưa sử dụng
    CAST(km.tg_ket_thuc AS date) AS han_dung
FROM dbo.chuong_trinh_km AS km
CROSS JOIN nums;
-- Nếu bạn chỉ muốn tạo cho các CTKM còn hiệu lực, thêm:
-- WHERE km.tg_ket_thuc >= SYSDATETIME();

-- KIỂM TRA NHANH
SELECT * FROM dbo.voucher ORDER BY voucher_id;



drop table Voucher

select* from chuong_trinh_km

GO
/* ======================================================================
   SEED: KHU VỰC VÀ BÀN
   Mỗi chi nhánh: 3 khu vực, mỗi khu vực: 10 bàn
   ====================================================================== */

-- Thêm cột mo_ta vào bảng khu_vuc nếu chưa có
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'dbo.khu_vuc') 
    AND name = N'mo_ta'
)
BEGIN
    ALTER TABLE dbo.khu_vuc
    ADD mo_ta NVARCHAR(300) NULL;
    PRINT N'Đã thêm cột mo_ta vào bảng khu_vuc';
END
GO

-- Cập nhật mô tả cho các khu vực đã tồn tại
UPDATE dbo.khu_vuc
SET mo_ta = CASE 
    WHEN ten_khu_vuc = N'Tầng 1' THEN N'Khu vực chính, gần cửa sổ'
    WHEN ten_khu_vuc = N'Tầng 2' THEN N'Khu vực VIP, yên tĩnh'
    WHEN ten_khu_vuc = N'Ngoài trời' THEN N'Không gian thoáng mát, view đẹp'
    ELSE N'Khu vực chính'
END
WHERE mo_ta IS NULL;

-- OPTION: Nếu muốn xóa dữ liệu cũ trước khi thêm mới, bỏ comment dòng sau:
-- DELETE FROM dbo.ban;
-- DELETE FROM dbo.khu_vuc;

-- 1) Thêm khu vực cho mỗi chi nhánh
INSERT INTO dbo.khu_vuc (chi_nhanh_id, ten_khu_vuc, mo_ta)
SELECT 
    cn.chi_nhanh_id,
    kv.ten_khu_vuc,
    kv.mo_ta
FROM dbo.chi_nhanh cn
CROSS JOIN (
    VALUES 
        (N'Tầng 1', N'Khu vực chính, gần cửa sổ'),
        (N'Tầng 2', N'Khu vực VIP, yên tĩnh'),
        (N'Ngoài trời', N'Không gian thoáng mát, view đẹp')
) AS kv(ten_khu_vuc, mo_ta)
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.khu_vuc kv_existing
    WHERE kv_existing.chi_nhanh_id = cn.chi_nhanh_id
      AND kv_existing.ten_khu_vuc = kv.ten_khu_vuc
);

-- 2) Thêm bàn cho mỗi khu vực (10 bàn/khu vực)
-- Số bàn unique trong mỗi chi nhánh: tính từ số bàn hiện có + 1
-- Sức chứa: 2-6 người (phân bổ đều)
-- Trạng thái: mặc định TRỐNG
WITH kv_with_seq AS (
    -- Đánh số thứ tự khu vực trong mỗi chi nhánh
    SELECT 
        kv.khu_vuc_id,
        kv.chi_nhanh_id,
        kv.ten_khu_vuc,
        ROW_NUMBER() OVER (PARTITION BY kv.chi_nhanh_id ORDER BY kv.khu_vuc_id) AS kv_seq
    FROM dbo.khu_vuc kv
),
kv_ban_count AS (
    -- Đếm số bàn hiện có của mỗi khu vực
    SELECT 
        kv.khu_vuc_id,
        kv.chi_nhanh_id,
        kv.kv_seq,
        COUNT(b.ban_id) AS so_ban_hien_co
    FROM kv_with_seq kv
    LEFT JOIN dbo.ban b ON b.khu_vuc_id = kv.khu_vuc_id
    GROUP BY kv.khu_vuc_id, kv.chi_nhanh_id, kv.kv_seq
),
kv_can_them_ban AS (
    -- Chỉ lấy những khu vực chưa đủ 10 bàn
    SELECT 
        kv.khu_vuc_id,
        kv.chi_nhanh_id,
        kv.kv_seq,
        kv.so_ban_hien_co,
        (kv.kv_seq - 1) * 10 AS ban_start_num
    FROM kv_ban_count kv
    WHERE kv.so_ban_hien_co < 10
),
ban_numbers AS (
    SELECT 1 AS num UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL SELECT 5
    UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL SELECT 9 UNION ALL SELECT 10
)
INSERT INTO dbo.ban (chi_nhanh_id, khu_vuc_id, so_ban, suc_chua, trang_thai)
SELECT 
    kv.chi_nhanh_id,
    kv.khu_vuc_id,
    N'B' + RIGHT('00' + CAST(kv.ban_start_num + bn.num AS NVARCHAR(2)), 2) AS so_ban,
    CASE 
        WHEN bn.num % 3 = 0 THEN 2  -- Bàn nhỏ: 2 người
        WHEN bn.num % 3 = 1 THEN 4  -- Bàn trung: 4 người
        ELSE 6                       -- Bàn lớn: 6 người
    END AS suc_chua,
    N'TRỐNG' AS trang_thai
FROM kv_can_them_ban kv
CROSS JOIN ban_numbers bn
WHERE NOT EXISTS (
    SELECT 1 
    FROM dbo.ban b_existing
    WHERE b_existing.chi_nhanh_id = kv.chi_nhanh_id
      AND b_existing.so_ban = N'B' + RIGHT('00' + CAST(kv.ban_start_num + bn.num AS NVARCHAR(2)), 2)
)
  AND bn.num > kv.so_ban_hien_co;  -- Chỉ thêm những bàn còn thiếu

-- Kiểm tra kết quả
SELECT 
    cn.ten AS [Chi nhánh],
    kv.ten_khu_vuc AS [Khu vực],
    kv.mo_ta AS [Mô tả],
    COUNT(b.ban_id) AS [Số bàn],
    STRING_AGG(b.so_ban + N'(' + CAST(b.suc_chua AS NVARCHAR) + N' người)', N', ') WITHIN GROUP (ORDER BY b.so_ban) AS [Danh sách bàn]
FROM dbo.chi_nhanh cn
LEFT JOIN dbo.khu_vuc kv ON kv.chi_nhanh_id = cn.chi_nhanh_id
LEFT JOIN dbo.ban b ON b.khu_vuc_id = kv.khu_vuc_id
GROUP BY cn.chi_nhanh_id, cn.ten, kv.khu_vuc_id, kv.ten_khu_vuc, kv.mo_ta
ORDER BY cn.ten, kv.ten_khu_vuc;

PRINT N'Đã thêm dữ liệu khu vực và bàn cho tất cả chi nhánh';

