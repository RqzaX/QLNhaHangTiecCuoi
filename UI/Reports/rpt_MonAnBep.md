# Thiết Kế Mẫu In Danh Sách Món Ăn Cho Bếp
## Hệ Thống Quản Lý Nhà Hàng Tiệc Cưới

---

## 1. TỔNG QUAN

Mẫu in này được sử dụng để in danh sách món ăn từ phiếu KOT (Kitchen Order Ticket) cho bếp, bao gồm thông tin chi tiết về từng món và ghi chú đặc biệt.

---

## 2. THÔNG TIN ĐẦU PHIẾU

### 2.1. Header Section
```
┌─────────────────────────────────────────────────────────┐
│  NHÀ HÀNG TIỆC CƯỚI [TÊN NHÀ HÀNG]                      │
│  PHIẾU ĐƠN BẾP - KOT                                     │
│                                                          │
│  Mã KOT: [KOT_CODE]          Thời gian: [HH:mm dd/MM]   │
│  Bàn/Tiệc: [TABLE_NAME]      Trạng thái: [STATUS]      │
│                                                          │
│  ─────────────────────────────────────────────────────  │
```

**Chi tiết:**
- **Tên nhà hàng**: Lấy từ thông tin chi nhánh
- **Mã KOT**: Mã phiếu đơn bếp (ví dụ: KOT024)
- **Thời gian**: Thời gian đặt món (HH:mm dd/MM/yyyy)
- **Bàn/Tiệc**: 
  - Nếu `so_ban = "TIỆC"` → hiển thị "Tiệc cưới"
  - Ngược lại → hiển thị "Bàn [so_ban]" (ví dụ: "Bàn T01")
- **Trạng thái**: 
  - "ĐANG PHỤC VỤ" → "Chờ làm"
  - "CHỜ THANH TOÁN" → "Đang làm"
  - "ĐÃ ĐÓNG" → "Sẵn sàng"

### 2.2. Ghi Chú Đơn Hàng (Nếu có)
```
│  📝 GHI CHÚ ĐƠN HÀNG:                                    │
│  [NOTES]                                                 │
│                                                          │
│  ─────────────────────────────────────────────────────  │
```

**Hiển thị khi:** `kot.Notes` không rỗng

---

## 3. DANH SÁCH MÓN ĂN

### 3.1. Format Mỗi Món Ăn
```
│  ┌───────────────────────────────────────────────────┐  │
│  │ [SỐ THỨ TỰ]. [TÊN MÓN]                            │  │
│  │     Số lượng: [QUANTITY]                          │  │
│  │     📌 Ghi chú bếp: [GHI_CHU_BEP]                 │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
```

**Chi tiết:**
- **Số thứ tự**: 1, 2, 3, ...
- **Tên món**: `ten_mon` từ database
- **Số lượng**: `so_luong` (ví dụ: 2x, 3x)
- **Ghi chú bếp**: `ghi_chu_bep` (chỉ hiển thị nếu có)

### 3.2. Ví Dụ Danh Sách Món
```
│  ┌───────────────────────────────────────────────────┐  │
│  │ 1. Nước suối                                       │  │
│  │     Số lượng: 1                                    │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │ 2. Lẩu thái hải sản                                │  │
│  │     Số lượng: 1                                    │  │
│  │     📌 Ghi chú bếp: Ít cay, không hành tây        │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │ 3. Salad rau trộn                                  │  │
│  │     Số lượng: 2                                    │  │
│  │     📌 Ghi chú bếp: Không dầu giấm                 │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │ 4. Súp bí đỏ                                       │  │
│  │     Số lượng: 2                                    │  │
│  └───────────────────────────────────────────────────┘  │
```

---

## 4. FOOTER SECTION

### 4.1. Thông Tin Cuối Phiếu
```
│  ─────────────────────────────────────────────────────  │
│                                                          │
│  Tổng số món: [TOTAL_ITEMS]                             │
│  Tổng số lượng: [TOTAL_QUANTITY]                         │
│                                                          │
│  [PRIORITY_BADGE]                                       │
│                                                          │
│  ─────────────────────────────────────────────────────  │
│  In lúc: [PRINT_TIME]                                    │
│  Người in: [USER_NAME]                                   │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

**Chi tiết:**
- **Tổng số món**: Số lượng món khác nhau trong đơn
- **Tổng số lượng**: Tổng số lượng tất cả món
- **Priority Badge**: 
  - Nếu `IsPriority = true` → hiển thị "⚠️ ĐƠN ƯU TIÊN"
  - Nếu không → không hiển thị
- **In lúc**: Thời gian in phiếu (HH:mm:ss dd/MM/yyyy)
- **Người in**: Tên người dùng đang đăng nhập

---

## 5. THIẾT KẾ ĐỊNH DẠNG

### 5.1. Kích Thước Giấy
- **Khổ giấy**: A5 (148mm x 210mm) hoặc 80mm (receipt printer)
- **Hướng**: Dọc (Portrait)
- **Margin**: 
  - Top: 10mm
  - Bottom: 10mm
  - Left: 10mm
  - Right: 10mm

### 5.2. Font Chữ
- **Header (Tên nhà hàng)**: 
  - Font: Arial/Bold
  - Size: 14pt
  - Alignment: Center
  
- **Tiêu đề section**:
  - Font: Arial/Bold
  - Size: 11pt
  - Alignment: Left
  
- **Nội dung thông tin**:
  - Font: Arial/Regular
  - Size: 10pt
  - Alignment: Left
  
- **Tên món**:
  - Font: Arial/Bold
  - Size: 11pt
  - Alignment: Left
  
- **Ghi chú**:
  - Font: Arial/Italic
  - Size: 9pt
  - Color: #666666
  - Alignment: Left

### 5.3. Màu Sắc
- **Text chính**: #000000 (Đen)
- **Text phụ**: #666666 (Xám đậm)
- **Border**: #CCCCCC (Xám nhạt)
- **Priority badge**: #FF0000 (Đỏ) - Bold
- **Background**: #FFFFFF (Trắng)

### 5.4. Khoảng Cách
- **Line spacing**: 1.2
- **Spacing giữa các món**: 8mm
- **Padding trong box món**: 5mm

---

## 6. LOGIC XỬ LÝ

### 6.1. Điều Kiện Hiển Thị
1. **Ghi chú đơn hàng**: Chỉ hiển thị khi `kot.Notes` không rỗng
2. **Ghi chú bếp từng món**: Chỉ hiển thị khi `item.Notes` (ghi_chu_bep) không rỗng
3. **Priority badge**: Chỉ hiển thị khi `kot.IsPriority = true`

### 6.2. Sắp Xếp Món Ăn
- Sắp xếp theo thứ tự trong database (theo `mon_id` hoặc thứ tự thêm vào)
- Đánh số thứ tự từ 1

### 6.3. Tính Toán
- **Tổng số món**: `kot.Items.Count`
- **Tổng số lượng**: `kot.Items.Sum(item => item.Quantity)`

---

## 7. VÍ DỤ PHIẾU HOÀN CHỈNH

```
┌─────────────────────────────────────────────────────────┐
│  NHÀ HÀNG TIỆC CƯỚI HOA SEN                              │
│  PHIẾU ĐƠN BẾP - KOT                                     │
│                                                          │
│  Mã KOT: KOT024          Thời gian: 08:01 15/12/2024    │
│  Bàn/Tiệc: Bàn T01       Trạng thái: Chờ làm            │
│                                                          │
│  ─────────────────────────────────────────────────────  │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │ 1. Nước suối                                       │  │
│  │     Số lượng: 1                                    │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │ 2. Lẩu thái hải sản                                │  │
│  │     Số lượng: 1                                    │  │
│  │     📌 Ghi chú bếp: Ít cay, không hành tây        │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │ 3. Salad rau trộn                                  │  │
│  │     Số lượng: 2                                    │  │
│  │     📌 Ghi chú bếp: Không dầu giấm                 │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  ┌───────────────────────────────────────────────────┐  │
│  │ 4. Súp bí đỏ                                       │  │
│  │     Số lượng: 2                                    │  │
│  └───────────────────────────────────────────────────┘  │
│                                                          │
│  ─────────────────────────────────────────────────────  │
│                                                          │
│  Tổng số món: 4                                          │
│  Tổng số lượng: 6                                        │
│                                                          │
│  ─────────────────────────────────────────────────────  │
│  In lúc: 08:05:30 15/12/2024                             │
│  Người in: Nguyễn Văn A                                  │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

---

## 8. TRƯỜNG HỢP ĐẶC BIỆT

### 8.1. Đơn Ưu Tiên
```
│  ⚠️ ĐƠN ƯU TIÊN - XỬ LÝ NGAY                            │
```
Hiển thị ngay sau header, màu đỏ, in đậm.

### 8.2. Đơn Tiệc Cưới
- **Bàn/Tiệc**: Hiển thị "Tiệc cưới" thay vì "Bàn [số]"
- Có thể thêm thông tin: "Sảnh: [Tên sảnh]" nếu có

### 8.3. Món Không Có Ghi Chú
- Bỏ qua dòng "📌 Ghi chú bếp" nếu `ghi_chu_bep` rỗng

### 8.4. Đơn Dài (Nhiều Món)
- Nếu số món > 10, tự động chia trang
- Mỗi trang có header và footer riêng
- Đánh số trang: "Trang 1/2"

---

## 9. DỮ LIỆU NGUỒN

### 9.1. Bảng Dữ Liệu
- **KOT**: `phieu_order` (kot_id, ma_kot, so_ban, thoi_gian_dat, trang_thai, uu_tien, ghi_chu)
- **Chi tiết KOT**: `phieu_order_ct` (phieu_order_id, mon_id, so_luong, ghi_chu_bep)
- **Món ăn**: `mon_an` (mon_id, ten_mon)
- **Chi nhánh**: `chi_nhanh` (ten_chi_nhanh)

### 9.2. Mapping Dữ Liệu
```csharp
KOTTicket {
    KOTId → kot_id
    TicketCode → ma_kot
    TableName → so_ban (format: "Bàn {so_ban}" hoặc "Tiệc cưới")
    OrderTime → thoi_gian_dat
    Status → trang_thai (map: "ĐANG PHỤC VỤ" → "Chờ làm", ...)
    IsPriority → uu_tien
    Notes → ghi_chu
    Items → List<KOTItem>
}

KOTItem {
    ItemId → mon_id
    Name → ten_mon (join với bảng mon_an)
    Quantity → so_luong
    Notes → ghi_chu_bep
}
```

---

## 10. YÊU CẦU KỸ THUẬT

### 10.1. Export Format
- **PDF**: Cho in từ máy tính
- **Receipt Printer**: Cho máy in nhiệt (80mm)
- **Image**: PNG/JPG (cho preview)

### 10.2. Performance
- Render nhanh (< 1 giây cho đơn < 20 món)
- Hỗ trợ in trực tiếp từ giao diện
- Hỗ trợ lưu file PDF

### 10.3. Tích Hợp
- Tích hợp với method `PrintKOT()` trong `FrmBep_Bar.cs`
- Có thể gọi từ button "In món" trên KOT Card
- Hỗ trợ preview trước khi in

---

## 11. PHÁT TRIỂN TƯƠNG LAI

### 11.1. Tính Năng Mở Rộng
- [ ] In nhiều đơn cùng lúc
- [ ] Template tùy chỉnh theo loại món (món nóng, món lạnh, đồ uống)
- [ ] Mã QR code để tracking
- [ ] Barcode cho KOT code
- [ ] Thời gian dự kiến hoàn thành
- [ ] Phân loại món theo khu vực bếp (bếp chính, bếp nướng, bar)

### 11.2. Tối Ưu
- [ ] Cache template
- [ ] Batch printing
- [ ] Auto-print khi có đơn mới (tùy chọn)

---

**Phiên bản**: 1.0  
**Ngày tạo**: 15/12/2024  
**Người thiết kế**: Hệ thống QLNhaHangTiecCuoi

