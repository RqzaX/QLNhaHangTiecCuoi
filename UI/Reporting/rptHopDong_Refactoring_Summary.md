# Contract Report Design Refactoring

## Overview
The contract report (`rptHopDong`) has been significantly refactored to improve the organization and presentation of itemized data, specifically services and menu items. The report now utilizes `DetailReport` bands to handle these distinct data sets, which are retrieved from a stored procedure (`sp_InHopDongDatTiec`) that returns multiple result sets.

## Key Changes

### 1. DetailReport Bands Implementation
Two new `DetailReportBand` components have been added to the report:
-   **DetailReportServices**: Handles the display of service details.
    -   **DataSource**: `sqlDataSource1`
    -   **DataMember**: `sp_InHopDongDatTiec.Result2` (Services Result Set)
    -   **Structure**:
        -   `ReportHeaderServices`: Contains the section title ("CHI TIẾT DỊCH VỤ") and the header table (`tblChiTietHeader`) with column names (Item, Quantity, Unit Price, Total).
        -   `DetailServices`: Contains the data table (`tblChiTietData`) bound to the service fields (`hang_muc`, `so_luong`, `don_gia`, `thanh_tien`).
-   **DetailReportMenu**: Handles the display of menu items.
    -   **DataSource**: `sqlDataSource1`
    -   **DataMember**: `sp_InHopDongDatTiec.Result3` (Menu Result Set)
    -   **Structure**:
        -   `ReportHeaderMenu`: Contains the section title ("THỰC ĐƠN") and the menu package details (`panelGoiDichVu`).
        -   `DetailMenu`: Contains the data table (`tblThucDon`) bound to the menu item fields (`TenMon`, `so_luong`, `don_vi_tinh`).

### 2. Report Structure Updates
-   **Main Report DataMember**: Updated to `sp_InHopDongDatTiec.Result1` to bind the main report to the contract header information.
-   **Bands Collection**: The `Bands` collection of the report now includes `DetailReportServices` and `DetailReportMenu` in addition to the standard bands (`TopMargin`, `BottomMargin`, `Detail`, `ReportHeader`, `ReportFooter`).
-   **Control Relocation**: Controls previously in the main `ReportHeader` or `Detail` bands (e.g., `lblTieuDeChiTiet`, `tblChiTiet`, `lblTieuDeThucDon`, `tblThucDon`) have been moved to their respective `DetailReport` bands.

### 3. Cleanup
-   Removed unused controls: `xrLabel1` and `xxxxxxxxxxxxxxxxx`.
-   Removed unused variables: `tblChiTiet`, `panelChiTiet`, `panelThucDon`.

## Current State
The report is now structured to correctly display:
1.  **Contract Header**: Customer info, venue info, event details (from Result1).
2.  **Services Section**: A list of services with a header row (from Result2).
3.  **Menu Section**: A list of menu items with package info (from Result3).
4.  **Financial Summary**: Total costs, deposit, and remaining balance (calculated/displayed in ReportFooter).
5.  **Footer**: Terms, signatures, and notes.

## Next Steps
-   **Visual Verification**: Run the application and generate the report to visually verify the layout and data binding.
-   **Fine-tuning**: Adjust padding, margins, fonts, and borders as needed to perfectly match the Figma mockup.
