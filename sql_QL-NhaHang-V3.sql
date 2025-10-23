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
select * from mon_an