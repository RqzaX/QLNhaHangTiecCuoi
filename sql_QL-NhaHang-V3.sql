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

--Phân ca cho nhân viên
IF OBJECT_ID('dbo.nguoi_dung_ca','U') IS NULL
CREATE TABLE dbo.nguoi_dung_ca(
  nguoi_dung_ca_id INT IDENTITY(1,1) PRIMARY KEY,
  nguoi_dung_id    INT NOT NULL,
  chi_nhanh_id     INT NOT NULL,
  ca_id            INT NOT NULL,
  trang_thai       TINYINT NOT NULL DEFAULT 1, -- 1=Hoạt động, 0=Không hoạt động
  FOREIGN KEY (nguoi_dung_id) REFERENCES dbo.nguoi_dung(nguoi_dung_id),
  FOREIGN KEY (chi_nhanh_id)  REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (ca_id)         REFERENCES dbo.ca(ca_id),
  -- Một nhân viên có thể có nhiều ca trong 1 chi nhánh, nhưng không được trùng lặp cùng một ca
  UNIQUE (nguoi_dung_id, chi_nhanh_id, ca_id)
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
  ghi_chu       NVARCHAR(max) NULL
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
  ghi_chu       NVARCHAR(max) NULL,
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
  ghi_chu_bep    NVARCHAR(max) NULL,
  FOREIGN KEY (phieu_order_id) REFERENCES dbo.phieu_order(phieu_order_id),
  FOREIGN KEY (mon_id)         REFERENCES dbo.mon_an(mon_id)
);

IF OBJECT_ID('dbo.hoa_don','U') IS NULL
CREATE TABLE dbo.hoa_don(
  hoa_don_id    INT IDENTITY(1,1) PRIMARY KEY,
  chi_nhanh_id  INT NOT NULL,
  khach_hang_id INT NULL,
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
  FOREIGN KEY (chi_nhanh_id) REFERENCES dbo.chi_nhanh(chi_nhanh_id),
  FOREIGN KEY (khach_hang_id) REFERENCES dbo.khach_hang(khach_hang_id)
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
  gio_to_chuc    TIME NOT NULL,
  ngay_to_chuc   DATE NOT NULL,
  khach_hang_id  INT NOT NULL,
  so_ban_du_kien INT NULL,
  goi_id         INT NULL,
  trang_thai     NVARCHAR(20) NOT NULL DEFAULT N'CHỜ XÁC NHẬN'
                CHECK (trang_thai IN (N'CHỜ XÁC NHẬN',N'ĐÃ CỌC',N'ĐÃ HỦY',N'ĐÃ THANH TOÁN', N'HOÀN TẤT')),
  ghi_chu        NVARCHAR(max) NULL,
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

-- Thêm cột khach_hang_id vào bảng hoa_don nếu bảng đã tồn tại
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'hoa_don')
  AND NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.hoa_don') AND name = 'khach_hang_id')
BEGIN
  ALTER TABLE dbo.hoa_don ADD khach_hang_id INT NULL;
  ALTER TABLE dbo.hoa_don ADD CONSTRAINT FK_hoa_don_khach_hang FOREIGN KEY (khach_hang_id) REFERENCES dbo.khach_hang(khach_hang_id);
END
GO

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
  ghi_chu     NVARCHAR(max) NULL,
  FOREIGN KEY (hop_dong_id) REFERENCES dbo.hop_dong(hop_dong_id)
);

IF OBJECT_ID('dbo.hop_dong_tt','U') IS NULL
CREATE TABLE dbo.hop_dong_tt(
  tt_id       INT IDENTITY(1,1) PRIMARY KEY,
  hop_dong_id INT NOT NULL,
  so_tien     DECIMAL(18,2) NOT NULL CHECK (so_tien > 0),
  ngay_tt     DATETIME2(0) NOT NULL DEFAULT SYSUTCDATETIME(),
  hinh_thuc   NVARCHAR(30)  NULL,
  noi_dung    NVARCHAR(max) NULL,
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
-- Bảng hạng khách hàng
IF OBJECT_ID('dbo.dm_hang_kh','U') IS NULL
CREATE TABLE dbo.dm_hang_kh(
  hang_code     NVARCHAR(10)  NOT NULL PRIMARY KEY,   -- MEM/BAC/VANG/VIP
  ten_hang      NVARCHAR(50)  NOT NULL,
  thu_tu        INT           NOT NULL,               -- xếp thứ tự tăng dần theo hạng
  min_tich_luy  DECIMAL(18,0) NOT NULL                -- ngưỡng chi tiêu để đạt hạng
);

-- Seed dữ liệu hạng khách hàng
MERGE dbo.dm_hang_kh AS T
USING (VALUES
  (N'MEM',  N'Thành viên', 1, 0),
  (N'BAC',  N'Bạc',         2, 15000000),
  (N'VANG', N'Vàng',        3, 30000000),
  (N'VIP',  N'VIP',         4, 40000000)
) AS S(hang_code,ten_hang,thu_tu,min_tich_luy)
ON T.hang_code = S.hang_code
WHEN NOT MATCHED THEN
  INSERT(hang_code,ten_hang,thu_tu,min_tich_luy)
  VALUES(S.hang_code,S.ten_hang,S.thu_tu,S.min_tich_luy)
WHEN MATCHED THEN
  UPDATE SET ten_hang=S.ten_hang, thu_tu=S.thu_tu, min_tich_luy=S.min_tich_luy;
GO

-- Bổ sung thuộc tính khách hàng liên quan Hạng/Điểm/tích lũy
-- Ngày sinh
IF COL_LENGTH('dbo.khach_hang','ngay_sinh') IS NULL
  ALTER TABLE dbo.khach_hang ADD ngay_sinh DATE NULL;

-- Mã hạng (FK tới dm_hang_kh), mặc định Thành viên
IF COL_LENGTH('dbo.khach_hang','hang_code') IS NULL
  ALTER TABLE dbo.khach_hang ADD hang_code NVARCHAR(10) NOT NULL CONSTRAINT DF_kh_hang DEFAULT N'MEM';

-- Tổng chi tiêu tích lũy
IF COL_LENGTH('dbo.khach_hang','tong_chi_tieu') IS NULL
  ALTER TABLE dbo.khach_hang ADD tong_chi_tieu DECIMAL(18,0) NOT NULL CONSTRAINT DF_kh_chitieu DEFAULT 0 
  CHECK (tong_chi_tieu >= 0);

-- Số lần đến
IF COL_LENGTH('dbo.khach_hang','so_lan_den') IS NULL
  ALTER TABLE dbo.khach_hang ADD so_lan_den INT NOT NULL CONSTRAINT DF_kh_landen DEFAULT 0 
  CHECK (so_lan_den >= 0);

-- Điểm
IF COL_LENGTH('dbo.khach_hang','diem') IS NULL
  ALTER TABLE dbo.khach_hang ADD diem INT NOT NULL CONSTRAINT DF_kh_diem DEFAULT 0
  CHECK (diem >= 0);

-- Lần cuối đến
IF COL_LENGTH('dbo.khach_hang','lan_cuoi_den') IS NULL
  ALTER TABLE dbo.khach_hang ADD lan_cuoi_den DATE NULL;

-- Ràng buộc khóa ngoại hạng
IF NOT EXISTS (
  SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_khach_hang_dm_hang'
)

ALTER TABLE dbo.khach_hang
ADD CONSTRAINT FK_khach_hang_dm_hang
FOREIGN KEY (hang_code) REFERENCES dbo.dm_hang_kh(hang_code);
GO
/* ======================================================================
   5) INDEXES THIẾT THỰC
   ====================================================================== */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_dat_sanh_unique' AND object_id=OBJECT_ID('dbo.dat_sanh'))
  CREATE UNIQUE INDEX IX_dat_sanh_unique ON dbo.dat_sanh(sanh_id, ca_id, ngay_to_chuc);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_order_head' AND object_id=OBJECT_ID('dbo.phieu_order'))
  CREATE INDEX IX_order_head ON dbo.phieu_order(chi_nhan h_id, trang_thai, ngay_gio);
    
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_hd_lap' AND object_id=OBJECT_ID('dbo.hoa_don'))
  CREATE INDEX IX_hd_lap ON dbo.hoa_don(chi_nhanh_id, loai, ngay_lap);

--Thêm vai trò
IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'ADMIN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'ADMIN', N'Quản trị viên');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'QLCN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'QLCN', N'Quản lý chi nhánh');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'QLNV')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'QLNV', N'Quản lý nhân viên');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'THUNGAN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'THUNGAN', N'Thu ngân');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'PHUCVU')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'PHUCVU', N'Nhân viên phục vụ');

IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'DAUBEP')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'DAUBEP', N'Đầu bếp');


IF NOT EXISTS (SELECT 1 FROM dbo.vai_tro WHERE ma = N'LETAN')
  INSERT INTO dbo.vai_tro(ma, ten)
  VALUES (N'LETAN', N'Lễ tân');

--Thêm Người dùng
--lấy id chi nhánh (đã có dữ liệu chi nhánh )
DECLARE @cn_tt INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'Chi nhánh Hồ Chí Minh' ORDER BY chi_nhanh_id);
DECLARE @cn_q7 INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'Chi nhánh Hà Nội' ORDER BY chi_nhanh_id);
-- Thêm người dùng (chỉ thêm nhân viên, admin đã có sẵn)
-- 1 Thu ngân
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'thungan01', N'123456', N'Phạm Thị Thu Ngân', 1);

-- 2 Phục vụ
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'phucvu01', N'123456', N'Nguyễn Thị Phục Vụ', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'phucvu02', N'123456', N'Trần Văn Phục Vụ', 1);

-- 1 Đầu bếp
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'daubep01', N'123456', N'Võ Văn Đầu Bếp', 1);

-- 1 Lễ tân
IF NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01')
  INSERT INTO dbo.nguoi_dung(tai_khoan, mat_khau, ho_ten, hoat_dong)
  VALUES (N'letan01', N'123456', N'Bùi Thị Lễ Tân', 1);

  /* ========================
   3) GÁN VAI TRÒ CHO NGƯỜI DÙNG
   ======================== */
DECLARE @vai_tro_admin INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'ADMIN');
DECLARE @vai_tro_qlcn INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'QLCN');
DECLARE @vai_tro_thungan INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'THUNGAN');
DECLARE @vai_tro_phucvu INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'PHUCVU');
DECLARE @vai_tro_daubep INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'DAUBEP');
DECLARE @vai_tro_letan INT = (SELECT vai_tro_id FROM dbo.vai_tro WHERE ma = N'LETAN');

DECLARE @nd_thungan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
DECLARE @nd_phucvu01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
DECLARE @nd_phucvu02 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
DECLARE @nd_daubep01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
DECLARE @nd_letan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');

-- Thu ngân
IF @nd_thungan01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_thungan01 AND vai_tro_id = @vai_tro_thungan)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_thungan01, @vai_tro_thungan);

-- Phục vụ
IF @nd_phucvu01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_phucvu01 AND vai_tro_id = @vai_tro_phucvu)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_phucvu01, @vai_tro_phucvu);

IF @nd_phucvu02 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_phucvu02 AND vai_tro_id = @vai_tro_phucvu)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_phucvu02, @vai_tro_phucvu);

-- Đầu bếp
IF @nd_daubep01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_daubep01 AND vai_tro_id = @vai_tro_daubep)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_daubep01, @vai_tro_daubep);

-- Lễ tân
IF @nd_letan01 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_vai_tro WHERE nguoi_dung_id = @nd_letan01 AND vai_tro_id = @vai_tro_letan)
  INSERT INTO dbo.nguoi_dung_vai_tro(nguoi_dung_id, vai_tro_id)
  VALUES (@nd_letan01, @vai_tro_letan);

  /* ========================
   4) GÁN CHI NHÁNH CHO NGƯỜI DÙNG
   ======================== */
DECLARE @cn_tt2 INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'CN Trung tâm' ORDER BY chi_nhanh_id);

DECLARE @nd_thungan01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
DECLARE @nd_phucvu01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
DECLARE @nd_phucvu02_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
DECLARE @nd_daubep01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
DECLARE @nd_letan01_2 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');

-- Thu ngân làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_thungan01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_thungan01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_thungan01_2, @cn_tt2);

-- Phục vụ 01, 02 làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_phucvu01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_phucvu01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_phucvu01_2, @cn_tt2);

IF @cn_tt2 IS NOT NULL AND @nd_phucvu02_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_phucvu02_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_phucvu02_2, @cn_tt2);

-- Đầu bếp làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_daubep01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_daubep01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_daubep01_2, @cn_tt2);

-- Lễ tân làm việc tại CN Trung tâm
IF @cn_tt2 IS NOT NULL AND @nd_letan01_2 IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_chi_nhanh WHERE nguoi_dung_id = @nd_letan01_2 AND chi_nhanh_id = @cn_tt2)
  INSERT INTO dbo.nguoi_dung_chi_nhanh(nguoi_dung_id, chi_nhanh_id)
  VALUES (@nd_letan01_2, @cn_tt2);

  PRINT N'=== DANH SÁCH VAI TRÒ ===';
SELECT vai_tro_id, ma, ten FROM dbo.vai_tro ORDER BY vai_tro_id;

PRINT N'=== DANH SÁCH NGƯỜI DÙNG ===';
SELECT nguoi_dung_id, tai_khoan, ho_ten, 
       CASE WHEN hoat_dong = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai
FROM dbo.nguoi_dung 
ORDER BY nguoi_dung_id;

PRINT N'=== NGƯỜI DÙNG VÀ VAI TRÒ ===';
SELECT nd.tai_khoan, nd.ho_ten, vt.ma AS ma_vai_tro, vt.ten AS ten_vai_tro
FROM dbo.nguoi_dung nd
INNER JOIN dbo.nguoi_dung_vai_tro ndvt ON nd.nguoi_dung_id = ndvt.nguoi_dung_id
INNER JOIN dbo.vai_tro vt ON ndvt.vai_tro_id = vt.vai_tro_id
ORDER BY nd.tai_khoan, vt.ma;

PRINT N'=== NGƯỜI DÙNG VÀ CHI NHÁNH ===';
SELECT nd.tai_khoan, nd.ho_ten, cn.ten AS ten_chi_nhanh, cn.dia_chi
FROM dbo.nguoi_dung nd
INNER JOIN dbo.nguoi_dung_chi_nhanh ndcn ON nd.nguoi_dung_id = ndcn.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON ndcn.chi_nhanh_id = cn.chi_nhanh_id
ORDER BY nd.tai_khoan, cn.ten;

/* ========================
   3) PHÂN CA CHO NHÂN VIÊN
   ======================== */
select * from ca
-- Lấy ID các ca
DECLARE @ca_sang INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Ca tiệc cưới sáng');
DECLARE @ca_trua INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Ca tiệc cưới chiều');
DECLARE @ca_toi INT = (SELECT ca_id FROM dbo.ca WHERE ten_ca = N'Ca tiệc cưới tối');

-- Lấy ID chi nhánh
DECLARE @cn_tt INT = (SELECT TOP 1 chi_nhanh_id FROM dbo.chi_nhanh WHERE ten = N'Chi nhánh Hồ Chí Minh' ORDER BY chi_nhanh_id);

-- Lấy ID người dùng
DECLARE @nd_thungan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'thungan01');
DECLARE @nd_phucvu01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu01');
DECLARE @nd_phucvu02 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'phucvu02');
DECLARE @nd_daubep01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'daubep01');
DECLARE @nd_letan01 INT = (SELECT nguoi_dung_id FROM dbo.nguoi_dung WHERE tai_khoan = N'letan01');

-- Phân ca cho Thu ngân: Ca sáng và ca trưa
IF @cn_tt IS NOT NULL AND @nd_thungan01 IS NOT NULL AND @ca_sang IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_thungan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_sang)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_thungan01, @cn_tt, @ca_sang, 1);

IF @cn_tt IS NOT NULL AND @nd_thungan01 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_thungan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_thungan01, @cn_tt, @ca_trua, 1);

-- Phân ca cho Phục vụ 01: Ca sáng
IF @cn_tt IS NOT NULL AND @nd_phucvu01 IS NOT NULL AND @ca_sang IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_sang)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu01, @cn_tt, @ca_sang, 1);

-- Phân ca cho Phục vụ 02: Ca trưa và ca tối
IF @cn_tt IS NOT NULL AND @nd_phucvu02 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu02 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu02, @cn_tt, @ca_trua, 1);

IF @cn_tt IS NOT NULL AND @nd_phucvu02 IS NOT NULL AND @ca_toi IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_phucvu02 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_toi)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_phucvu02, @cn_tt, @ca_toi, 1);

-- Phân ca cho Đầu bếp: Ca trưa và ca tối
IF @cn_tt IS NOT NULL AND @nd_daubep01 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_daubep01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_daubep01, @cn_tt, @ca_trua, 1);

IF @cn_tt IS NOT NULL AND @nd_daubep01 IS NOT NULL AND @ca_toi IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_daubep01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_toi)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_daubep01, @cn_tt, @ca_toi, 1);

-- Phân ca cho Lễ tân: Ca sáng và ca trưa
IF @cn_tt IS NOT NULL AND @nd_letan01 IS NOT NULL AND @ca_sang IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_letan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_sang)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_letan01, @cn_tt, @ca_sang, 1);

IF @cn_tt IS NOT NULL AND @nd_letan01 IS NOT NULL AND @ca_trua IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.nguoi_dung_ca WHERE nguoi_dung_id = @nd_letan01 AND chi_nhanh_id = @cn_tt AND ca_id = @ca_trua)
  INSERT INTO dbo.nguoi_dung_ca(nguoi_dung_id, chi_nhanh_id, ca_id, trang_thai)
  VALUES (@nd_letan01, @cn_tt, @ca_trua, 1);

SELECT 
  nd.tai_khoan,
  nd.ho_ten,
  cn.ten AS ten_chi_nhanh,
  c.ten_ca,
  c.gio_bd,
  c.gio_kt,
  CASE WHEN ndc.trang_thai = 1 THEN N'Hoạt động' ELSE N'Không hoạt động' END AS trang_thai
FROM dbo.nguoi_dung_ca ndc
INNER JOIN dbo.nguoi_dung nd ON ndc.nguoi_dung_id = nd.nguoi_dung_id
INNER JOIN dbo.chi_nhanh cn ON ndc.chi_nhanh_id = cn.chi_nhanh_id
INNER JOIN dbo.ca c ON ndc.ca_id = c.ca_id
ORDER BY nd.ho_ten, c.ten_ca;
select * from nguoi_dung_ca