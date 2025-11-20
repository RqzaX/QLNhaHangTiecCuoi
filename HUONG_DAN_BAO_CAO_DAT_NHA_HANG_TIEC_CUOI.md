# HƯỚNG DẪN BÁO CÁO ĐẶT NHÀ HÀNG VÀ TIỆC CƯỚI

## MỤC ĐÍCH
Báo cáo số lượng đặt nhà hàng/tiệc cưới, tổng số tiền, tổng doanh thu dựa trên các hóa đơn đã thanh toán.

---

## STORED PROCEDURES

### 1. `sp_BaoCaoDatNhaHangTiecCuoi` (Chi tiết)
- **1 result set** với đầy đủ thông tin
- Nhóm theo ngày, loại, chi nhánh
- Phù hợp cho report chi tiết

### 2. `sp_BaoCaoTongHopDatNhaHangTiecCuoi` (Tổng hợp)
- **3 result sets**:
  - Result Set 1: Header
  - Result Set 2: Chi tiết theo ngày
  - Result Set 3: Tổng hợp theo loại
- Phù hợp cho report tổng hợp

---

## CÁC THAM SỐ

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| `@ChiNhanhId` | INT | NULL = tất cả chi nhánh, số = lọc theo chi nhánh |
| `@TuNgay` | DATE | NULL = không lọc, DATE = ngày bắt đầu |
| `@DenNgay` | DATE | NULL = không lọc, DATE = ngày kết thúc |
| `@Loai` | NVARCHAR(15) | NULL = tất cả, 'NHAHANG' = nhà hàng, 'TIECCUOI' = tiệc cưới |

---

## CÁC TRƯỜNG TRẢ VỀ

### Header Fields (dùng cho Report Header):
- `ngay_bao_cao` - Ngày báo cáo (dd/MM/yyyy)
- `thoi_gian_bao_cao` - Thời gian báo cáo đầy đủ
- `ten_chi_nhanh` - Tên chi nhánh
- `tieu_de_bao_cao` - Tiêu đề báo cáo

### Chi tiết Fields (dùng cho Details):
- `ngay` - Ngày (DATE)
- `ngay_hien_thi` - Ngày hiển thị (dd/MM/yyyy)
- `loai` - Loại (NHAHANG/TIECCUOI)
- `ten_loai` - Tên loại (Nhà hàng/Tiệc cưới)
- `so_luong_dat` - Số lượng đặt (COUNT)
- `tong_so_tien` - Tổng số tiền (số)
- `tong_so_tien_hien_thi` - Tổng số tiền (đã format)
- `tong_doanh_thu` - Tổng doanh thu (số)
- `tong_doanh_thu_hien_thi` - Tổng doanh thu (đã format)
- `tong_truoc_thue` - Tổng trước thuế
- `tong_vat` - Tổng VAT
- `tong_giam_gia` - Tổng giảm giá
- `tong_phi_dv` - Tổng phí dịch vụ
- `ban_sanh` - Tên bàn/sảnh
- `ten_khach_hang` - Tên khách hàng
- `sdt_khach_hang` - SĐT khách hàng

---

## CÁCH SỬ DỤNG

### Test trong SSMS:

```sql
-- 1. Tất cả đơn đã thanh toán (tất cả chi nhánh, tất cả loại)
EXEC dbo.sp_BaoCaoDatNhaHangTiecCuoi NULL, NULL, NULL, NULL;

-- 2. Chỉ nhà hàng
EXEC dbo.sp_BaoCaoDatNhaHangTiecCuoi NULL, NULL, NULL, 'NHAHANG';

-- 3. Chỉ tiệc cưới
EXEC dbo.sp_BaoCaoDatNhaHangTiecCuoi NULL, NULL, NULL, 'TIECCUOI';

-- 4. Theo chi nhánh cụ thể
EXEC dbo.sp_BaoCaoDatNhaHangTiecCuoi 1, NULL, NULL, NULL;

-- 5. Theo khoảng thời gian
EXEC dbo.sp_BaoCaoDatNhaHangTiecCuoi NULL, '2024-01-01', '2024-12-31', NULL;

-- 6. Kết hợp tất cả điều kiện
EXEC dbo.sp_BaoCaoDatNhaHangTiecCuoi 1, '2024-01-01', '2024-12-31', 'NHAHANG';
```

### Test stored procedure tổng hợp:

```sql
-- Tổng hợp tất cả
EXEC dbo.sp_BaoCaoTongHopDatNhaHangTiecCuoi NULL, NULL, NULL, NULL;
```

---

## LOGIC BÁO CÁO

### 1. Chỉ lấy hóa đơn đã thanh toán
- `trang_thai = N'ĐÃ THANH TOÁN'`

### 2. Ngày báo cáo
- Ưu tiên: `ngay_tt` từ bảng `thanh_toan` (ngày thanh toán thực tế)
- Nếu không có: dùng `ngay_lap` từ `hoa_don` (ngày lập hóa đơn)

### 3. Tổng doanh thu
- Ưu tiên: SUM từ bảng `thanh_toan.so_tien` (số tiền đã thanh toán thực tế)
- Nếu không có: dùng `tong_sau_thue` từ `hoa_don`

### 4. Nhóm dữ liệu
- Nhóm theo: Ngày, Chi nhánh, Loại (Nhà hàng/Tiệc cưới)

---

## VÍ DỤ KẾT QUẢ

```
Ngày        | Loại        | Số lượng | Tổng số tiền | Tổng doanh thu
------------|-------------|----------|--------------|---------------
15/12/2024  | Nhà hàng    | 5        | 2,500,000    | 2,500,000
15/12/2024  | Tiệc cưới   | 2        | 50,000,000   | 50,000,000
14/12/2024  | Nhà hàng    | 8        | 4,200,000    | 4,200,000
14/12/2024  | Tiệc cưới   | 1        | 30,000,000   | 30,000,000
```

---

## LƯU Ý

1. **Chỉ lấy hóa đơn đã thanh toán**: Báo cáo chỉ hiển thị các hóa đơn có `trang_thai = N'ĐÃ THANH TOÁN'`

2. **Ngày báo cáo**: Sử dụng ngày thanh toán thực tế (nếu có), nếu không thì dùng ngày lập hóa đơn

3. **Tổng doanh thu**: Tính từ số tiền đã thanh toán thực tế (nếu có), nếu không thì dùng tổng sau thuế

4. **Nhóm theo ngày**: Mỗi dòng trong result set là một nhóm theo ngày + loại + chi nhánh

5. **Số lượng đặt**: Là COUNT(*) số hóa đơn trong nhóm đó

---

## TROUBLESHOOTING

### Không có dữ liệu trả về
1. Kiểm tra có hóa đơn đã thanh toán không:
   ```sql
   SELECT COUNT(*) FROM dbo.hoa_don WHERE trang_thai = N'ĐÃ THANH TOÁN';
   ```

2. Kiểm tra có thanh toán không:
   ```sql
   SELECT COUNT(*) FROM dbo.thanh_toan;
   ```

3. Kiểm tra điều kiện lọc:
   - Chi nhánh có đúng không?
   - Khoảng ngày có đúng không?
   - Loại có đúng không?

### Tổng doanh thu = 0
- Kiểm tra bảng `thanh_toan` có dữ liệu không
- Nếu không có, stored procedure sẽ dùng `tong_sau_thue` từ `hoa_don`

### Ngày không đúng
- Stored procedure ưu tiên `ngay_tt` từ `thanh_toan`
- Nếu không có, dùng `ngay_lap` từ `hoa_don`

---

## TẠO REPORT TRONG CRYSTAL REPORTS

### Bước 1: Tạo Report mới
1. Mở Crystal Reports Designer
2. **File** → **New** → **Blank Report**

### Bước 2: Kết nối Database
1. **Database** → **Database Expert**
2. Chọn stored procedure: `sp_BaoCaoDatNhaHangTiecCuoi`
3. Click **OK**

### Bước 3: Thiết kế Report

#### Report Header:
- Tiêu đề: "BÁO CÁO ĐẶT NHÀ HÀNG VÀ TIỆC CƯỚI"
- `ngay_bao_cao`
- `ten_chi_nhanh`
- `tieu_de_bao_cao`

#### Page Header:
- STT | Ngày | Loại | Số lượng | Tổng số tiền | Tổng doanh thu

#### Details:
- `ngay_hien_thi`
- `ten_loai`
- `so_luong_dat`
- `tong_so_tien_hien_thi`
- `tong_doanh_thu_hien_thi`

#### Report Footer:
- Tổng số lượng: SUM(`so_luong_dat`)
- Tổng doanh thu: SUM(`tong_doanh_thu`)

---

## CHECKLIST

- [ ] Stored procedure đã được tạo chưa?
- [ ] Test stored procedure trong SSMS có trả về dữ liệu không?
- [ ] Có hóa đơn đã thanh toán trong database không?
- [ ] Crystal Reports đã kết nối đến stored procedure chưa?
- [ ] Report đã hiển thị đúng dữ liệu chưa?

