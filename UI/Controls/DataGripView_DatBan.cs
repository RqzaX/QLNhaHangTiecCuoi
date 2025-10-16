using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Forms;

namespace UI.Controls
{
    [SupportedOSPlatform("windows")]
    public class DataGripView_DatBan : UserControl
    {
        // core controls
        private Panel topBar;
        private TextBox txtSearch;
        private ComboBox cboStatusFilter;
        private Button btnExportCsv;
        private Button btnPrint;
        private CheckBox chkVirtual;
        private DataGridView dgv;
        private ContextMenuStrip rowContext;

        // data storage
        private List<Reservation> _fullData = new List<Reservation>();
        private List<Reservation> _filteredData = new List<Reservation>();
        private Dictionary<string, Image> _iconCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        // sorting state
        private string _sortColumn = null;
        private bool _sortAsc = true;

        // properties
        [Category("Behavior"), Description("Nếu true thì dùng VirtualMode (phù hợp nhiều hàng)")]
        public bool VirtualModeEnabled
        {
            get => chkVirtual.Checked;
            set => chkVirtual.Checked = value;
        }

        [Category("Behavior"), Description("Thư mục chứa các icon PNG (tên file tương ứng 'view.png','confirm.png',...')")]
        public string IconFolder { get; set; } = "";

        // events
        public event EventHandler<ReservationEventArgs> ViewClicked;
        public event EventHandler<ReservationEventArgs> ConfirmClicked;
        public event EventHandler<ReservationEventArgs> ArrivedClicked;
        public event EventHandler<ReservationEventArgs> EditClicked;
        public event EventHandler<ReservationEventArgs> CancelClicked;

        // columns indices (same as before)
        private const int COL_ICON = 0;
        private const int COL_CODE = 1;
        private const int COL_CUSTOMER = 2;
        private const int COL_DATETIME = 3;
        private const int COL_TABLE = 4;
        private const int COL_GUESTS = 5;
        private const int COL_STATUS = 6;
        private const int COL_DEPOSIT = 7;
        private const int COL_VIEW = 8;
        private const int COL_CONFIRM = 9;
        private const int COL_ARRIVED = 10;
        private const int COL_EDIT = 11;

        // printing
        private PrintDocument _printDoc;
        private int _printRowIndex = 0;

        public DataGripView_DatBan()
        {
            InitializeComponent();
            BuildLayout();
            BuildGrid();
            BuildContextMenu();
            HookEvents();

            // sample data for design-time preview
            if (!DesignMode)
            {
                SetData(CreateSampleData());
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "ReservationGridExtended";
            this.Size = new Size(980, 520);
            this.ResumeLayout(false);
        }

        private void BuildLayout()
        {
            topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(8),
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(topBar);

            txtSearch = new TextBox
            {
                PlaceholderText = "Tìm mã, tên, số điện thoại...",
                Width = 320,
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };
            topBar.Controls.Add(txtSearch);

            cboStatusFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 160,
                Left = txtSearch.Right + 8,
                Top = 6
            };
            topBar.Controls.Add(cboStatusFilter);

            btnExportCsv = new Button
            {
                Text = "Xuất CSV",
                AutoSize = true,
                Left = cboStatusFilter.Right + 8,
                Top = 4
            };
            topBar.Controls.Add(btnExportCsv);

            btnPrint = new Button
            {
                Text = "In",
                AutoSize = true,
                Left = btnExportCsv.Right + 8,
                Top = 4
            };
            topBar.Controls.Add(btnPrint);

            chkVirtual = new CheckBox
            {
                Text = "VirtualMode",
                AutoSize = true,
                Left = btnPrint.Right + 12,
                Top = 8
            };
            topBar.Controls.Add(chkVirtual);
        }

        private void BuildGrid()
        {
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 44,
                EnableHeadersVisualStyles = false,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9F),
                    Padding = new Padding(8, 6, 8, 6),
                    WrapMode = DataGridViewTriState.True,
                    SelectionBackColor = Color.FromArgb(235, 244, 255),
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(250, 250, 251),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 45, 45),
                    Padding = new Padding(8, 0, 8, 0),
                }
            };

            // create columns
            dgv.Columns.Add(new DataGridViewImageColumn { Name = "Icon", HeaderText = "", ImageLayout = DataGridViewImageCellLayout.Normal, Width = 40 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Mã đặt bàn", Width = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Customer", HeaderText = "Khách hàng", Width = 220 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "DateTime", HeaderText = "Ngày & Giờ", Width = 160 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Table", HeaderText = "Bàn", Width = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Guests", HeaderText = "Số khách", Width = 80, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", Width = 120 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Deposit", HeaderText = "Tiền cọc", Width = 120, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgv.Columns.Add(new DataGridViewImageColumn { Name = "View", HeaderText = " ", Width = 32, ImageLayout = DataGridViewImageCellLayout.Normal });
            dgv.Columns.Add(new DataGridViewImageColumn { Name = "Confirm", HeaderText = " ", Width = 32, ImageLayout = DataGridViewImageCellLayout.Normal });
            dgv.Columns.Add(new DataGridViewImageColumn { Name = "Arrived", HeaderText = " ", Width = 32, ImageLayout = DataGridViewImageCellLayout.Normal });
            dgv.Columns.Add(new DataGridViewImageColumn { Name = "Edit", HeaderText = " ", Width = 32, ImageLayout = DataGridViewImageCellLayout.Normal });

            dgv.RowTemplate.Height = 72;
            dgv.CellPainting += Dgv_CellPainting;
            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.CellContentClick += Dgv_CellContentClick;
            dgv.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;

            this.Controls.Add(dgv);
            dgv.BringToFront();

            // print document
            _printDoc = new PrintDocument();
            _printDoc.PrintPage += PrintDoc_PrintPage;
        }

        private void BuildContextMenu()
        {
            rowContext = new ContextMenuStrip();
            rowContext.Items.Add("Xem chi tiết", null, (s, e) => InvokeRowAction(ActionType.View));
            rowContext.Items.Add("Xác nhận", null, (s, e) => InvokeRowAction(ActionType.Confirm));
            rowContext.Items.Add("Đã đến", null, (s, e) => InvokeRowAction(ActionType.Arrived));
            rowContext.Items.Add("Chỉnh sửa", null, (s, e) => InvokeRowAction(ActionType.Edit));
            rowContext.Items.Add(new ToolStripSeparator());
            rowContext.Items.Add("Hủy", null, (s, e) => InvokeRowAction(ActionType.Cancel));
        }

        private void HookEvents()
        {
            txtSearch.TextChanged += (s, e) => ApplyFilterAndSearch();
            cboStatusFilter.SelectedIndexChanged += (s, e) => ApplyFilterAndSearch();
            btnExportCsv.Click += (s, e) => ExportCsvDialog();
            btnPrint.Click += (s, e) => PrintDialogAndStart();
            chkVirtual.CheckedChanged += (s, e) => ToggleVirtualMode();
            dgv.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var hit = dgv.HitTest(e.X, e.Y);
                    if (hit.RowIndex >= 0)
                    {
                        dgv.ClearSelection();
                        dgv.Rows[hit.RowIndex].Selected = true;
                        rowContext.Show(dgv, new Point(e.X, e.Y));
                    }
                }
            };
        }

        // ---------------- public API ----------------
        public void SetData(List<Reservation> list)
        {
            _fullData = list ?? new List<Reservation>();
            BuildStatusFilter();
            ApplyFilterAndSearch();
        }

        public void Add(Reservation r)
        {
            _fullData.Add(r);
            ApplyFilterAndSearch();
        }

        public void ClearData()
        {
            _fullData.Clear();
            ApplyFilterAndSearch();
        }

        // -------------- filter / search / sort -------------
        private void BuildStatusFilter()
        {
            var statuses = _fullData.Select(x => x.Status).Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x).ToList();
            cboStatusFilter.Items.Clear();
            cboStatusFilter.Items.Add("Tất cả");
            foreach (var s in statuses) cboStatusFilter.Items.Add(s);
            cboStatusFilter.SelectedIndex = 0;
        }

        private void ApplyFilterAndSearch()
        {
            var query = txtSearch.Text?.Trim().ToLowerInvariant();
            var selectedStatus = cboStatusFilter.SelectedItem as string;
            var q = _fullData.AsEnumerable();

            if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "Tất cả")
            {
                q = q.Where(x => string.Equals(x.Status, selectedStatus, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(query))
            {
                q = q.Where(x =>
                    (x.Code ?? "").ToLowerInvariant().Contains(query)
                    || (x.CustomerName ?? "").ToLowerInvariant().Contains(query)
                    || (x.Phone ?? "").ToLowerInvariant().Contains(query)
                );
            }

            // apply sort if set
            if (!string.IsNullOrEmpty(_sortColumn))
            {
                q = _sortAsc ? q.OrderBy(x => SortKey(x, _sortColumn)) : q.OrderByDescending(x => SortKey(x, _sortColumn));
            }

            _filteredData = q.ToList();

            if (VirtualModeEnabled)
            {
                dgv.VirtualMode = true;
                dgv.RowCount = _filteredData.Count;
            }
            else
            {
                dgv.VirtualMode = false;
                dgv.Rows.Clear();
                foreach (var r in _filteredData)
                {
                    var icon = GetIcon("calendar") ?? DrawSmallCalendarIcon(Color.FromArgb(32, 123, 255), 20);
                    dgv.Rows.Add(
                        icon,
                        r.Code,
                        r.CustomerName + Environment.NewLine + r.Phone,
                        r.Date.ToString("dd/MM/yyyy") + Environment.NewLine + r.Date.ToString("HH:mm"),
                        r.TableName + (string.IsNullOrEmpty(r.Area) ? "" : Environment.NewLine + r.Area),
                        r.Guests.ToString(),
                        r.Status,
                        r.Deposit > 0 ? r.Deposit.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ" : "Chưa cọc",
                        GetIcon("view") ?? DrawEyeIcon(18),
                        GetIcon("confirm") ?? DrawCheckIcon(18),
                        GetIcon("arrived") ?? DrawPersonCheckIcon(18),
                        GetIcon("edit") ?? DrawEditIcon(18)
                    );
                    dgv.Rows[dgv.Rows.Count - 1].Tag = r;
                }
            }
            dgv.Refresh();
        }

        private object SortKey(Reservation r, string column)
        {
            switch (column)
            {
                case "Code": return r.Code;
                case "Customer": return r.CustomerName;
                case "DateTime": return r.Date;
                case "Table": return r.TableName;
                case "Guests": return r.Guests;
                case "Status": return r.Status;
                case "Deposit": return r.Deposit;
                default: return r.Code;
            }
        }

        private void Dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // allow sort on clickable columns
            var col = dgv.Columns[e.ColumnIndex].Name;
            if (col == "Icon" || col == "View" || col == "Confirm" || col == "Arrived" || col == "Edit") return;
            // map to our sort names
            _sortColumn = col;
            _sortAsc = (_sortColumn != null && !_sortAsc) ? true : !_sortAsc; // toggle
            ApplyFilterAndSearch();
        }

        // --------------- Virtual mode support ---------------
        // handle CellValueNeeded to supply cell values when virtual
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dgv.CellValueNeeded += Dgv_CellValueNeeded;
            dgv.CellValuePushed += Dgv_CellValuePushed;
        }

        private void ToggleVirtualMode()
        {
            ApplyFilterAndSearch();
        }

        private void Dgv_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (!VirtualModeEnabled) return;
            if (e.RowIndex < 0 || e.RowIndex >= _filteredData.Count) return;
            var r = _filteredData[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case COL_ICON:
                    e.Value = GetIcon("calendar") ?? DrawSmallCalendarIcon(Color.FromArgb(32, 123, 255), 20);
                    break;
                case COL_CODE:
                    e.Value = r.Code;
                    break;
                case COL_CUSTOMER:
                    e.Value = r.CustomerName + Environment.NewLine + r.Phone;
                    break;
                case COL_DATETIME:
                    e.Value = r.Date.ToString("dd/MM/yyyy") + Environment.NewLine + r.Date.ToString("HH:mm");
                    break;
                case COL_TABLE:
                    e.Value = r.TableName + (string.IsNullOrEmpty(r.Area) ? "" : Environment.NewLine + r.Area);
                    break;
                case COL_GUESTS:
                    e.Value = r.Guests.ToString();
                    break;
                case COL_STATUS:
                    e.Value = r.Status;
                    break;
                case COL_DEPOSIT:
                    e.Value = r.Deposit > 0 ? r.Deposit.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ" : "Chưa cọc";
                    break;
                case COL_VIEW:
                    e.Value = GetIcon("view") ?? DrawEyeIcon(18);
                    break;
                case COL_CONFIRM:
                    e.Value = GetIcon("confirm") ?? DrawCheckIcon(18);
                    break;
                case COL_ARRIVED:
                    e.Value = GetIcon("arrived") ?? DrawPersonCheckIcon(18);
                    break;
                case COL_EDIT:
                    e.Value = GetIcon("edit") ?? DrawEditIcon(18);
                    break;
            }
        }

        private void Dgv_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            // not used, but ready for editing if needed
        }

        // --------------- painting & formatting ----------------
        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // deposit color
            if (e.ColumnIndex == COL_DEPOSIT && e.Value != null)
            {
                var s = e.Value.ToString();
                if (s != "Chưa cọc")
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.FromArgb(0, 150, 0);
                }
                else
                {
                    dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Gray;
                }
            }
        }

        private void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Status pill and multi-line custom draw
            if (e.RowIndex >= 0 && e.ColumnIndex == COL_STATUS)
            {
                e.Handled = true;
                e.PaintBackground(e.ClipBounds, true);
                string status = (e.FormattedValue ?? "").ToString();
                var rc = e.CellBounds;
                rc.Inflate(-8, -18);
                if (rc.Height < 22) rc.Height = 22;
                Color fill = Color.LightGray;
                Color text = Color.FromArgb(40, 40, 40);

                switch (status)
                {
                    case "Đã xác nhận":
                        fill = Color.FromArgb(226, 240, 255);
                        text = Color.FromArgb(24, 103, 255);
                        break;
                    case "Chờ xác nhận":
                        fill = Color.FromArgb(255, 247, 205);
                        text = Color.FromArgb(150, 110, 0);
                        break;
                    case "Đã đến":
                        fill = Color.FromArgb(219, 255, 235);
                        text = Color.FromArgb(3, 136, 88);
                        break;
                    case "Đã hủy":
                        fill = Color.FromArgb(255, 230, 230);
                        text = Color.FromArgb(190, 30, 45);
                        break;
                    case "Hoàn thành":
                        fill = Color.FromArgb(243, 244, 246);
                        text = Color.FromArgb(90, 92, 95);
                        break;
                    default:
                        fill = Color.FromArgb(243, 244, 246);
                        text = Color.FromArgb(90, 92, 95);
                        break;
                }

                using (var gp = RoundedRect(rc, rc.Height / 2))
                using (var br = new SolidBrush(fill))
                using (var pen = new Pen(Color.FromArgb(230, 230, 230)))
                {
                    e.Graphics.FillPath(br, gp);
                    e.Graphics.DrawPath(pen, gp);
                }
                var textRect = new Rectangle(rc.Left + 8, rc.Top, rc.Width - 16, rc.Height);
                TextRenderer.DrawText(e.Graphics, status, new Font("Segoe UI", 9F), textRect, text, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                return;
            }

            if (e.RowIndex >= 0 && (e.ColumnIndex == COL_CUSTOMER || e.ColumnIndex == COL_DATETIME || e.ColumnIndex == COL_TABLE))
            {
                e.Handled = true;
                e.PaintBackground(e.ClipBounds, true);
                var rc = e.CellBounds;
                var val = e.FormattedValue?.ToString() ?? "";

                var lines = val.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                if (lines.Length >= 2)
                {
                    var titleRect = new Rectangle(rc.Left + 6, rc.Top + 6, rc.Width - 12, (rc.Height / 2) - 6);
                    TextRenderer.DrawText(e.Graphics, lines[0], new Font("Segoe UI", 9F, FontStyle.Regular), titleRect, Color.FromArgb(35, 35, 35), TextFormatFlags.Left | TextFormatFlags.Top);

                    var y2 = rc.Top + rc.Height / 2 - 2;
                    Bitmap iconBmp = null;
                    if (e.ColumnIndex == COL_CUSTOMER) iconBmp = DrawPhoneIcon(14);
                    else if (e.ColumnIndex == COL_DATETIME) iconBmp = DrawClockIcon(14);
                    else if (e.ColumnIndex == COL_TABLE) iconBmp = DrawMapPinIcon(14);

                    if (iconBmp != null)
                    {
                        e.Graphics.DrawImage(iconBmp, new Rectangle(rc.Left + 6, y2, 14, 14));
                    }
                    var secondRect = new Rectangle(rc.Left + 26, y2 - 2, rc.Width - 32, 18);
                    TextRenderer.DrawText(e.Graphics, lines[1], new Font("Segoe UI", 8F, FontStyle.Regular), secondRect, Color.Gray, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                }
                else
                {
                    var r = new Rectangle(rc.Left + 6, rc.Top + 10, rc.Width - 12, rc.Height - 12);
                    TextRenderer.DrawText(e.Graphics, val, dgv.Font, r, Color.FromArgb(35, 35, 35), TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                }
                return;
            }
        }

        // --------------- handle clicks on action icons ----------------
        private void Dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            Reservation r = null;
            if (VirtualModeEnabled)
            {
                if (e.RowIndex >= 0 && e.RowIndex < _filteredData.Count) r = _filteredData[e.RowIndex];
            }
            else
            {
                r = dgv.Rows[e.RowIndex].Tag as Reservation;
            }
            if (r == null) return;

            if (e.ColumnIndex == COL_VIEW) ViewClicked?.Invoke(this, new ReservationEventArgs(r));
            else if (e.ColumnIndex == COL_CONFIRM) ConfirmClicked?.Invoke(this, new ReservationEventArgs(r));
            else if (e.ColumnIndex == COL_ARRIVED) ArrivedClicked?.Invoke(this, new ReservationEventArgs(r));
            else if (e.ColumnIndex == COL_EDIT) EditClicked?.Invoke(this, new ReservationEventArgs(r));
        }

        private enum ActionType { View, Confirm, Arrived, Edit, Cancel }
        private void InvokeRowAction(ActionType act)
        {
            if (dgv.SelectedRows.Count == 0) return;
            var row = dgv.SelectedRows[0];
            Reservation r = row.Tag as Reservation;
            if (VirtualModeEnabled)
            {
                if (row.Index >= 0 && row.Index < _filteredData.Count) r = _filteredData[row.Index];
            }
            if (r == null) return;
            switch (act)
            {
                case ActionType.View: ViewClicked?.Invoke(this, new ReservationEventArgs(r)); break;
                case ActionType.Confirm: ConfirmClicked?.Invoke(this, new ReservationEventArgs(r)); break;
                case ActionType.Arrived: ArrivedClicked?.Invoke(this, new ReservationEventArgs(r)); break;
                case ActionType.Edit: EditClicked?.Invoke(this, new ReservationEventArgs(r)); break;
                case ActionType.Cancel: CancelClicked?.Invoke(this, new ReservationEventArgs(r)); break;
            }
        }

        // ---------------- icons loader ----------------
        private Image GetIcon(string name)
        {
            if (!string.IsNullOrEmpty(IconFolder) && Directory.Exists(IconFolder))
            {
                var key = name.ToLowerInvariant();
                if (_iconCache.ContainsKey(key)) return _iconCache[key];
                var path = Path.Combine(IconFolder, key + ".png");
                if (File.Exists(path))
                {
                    try
                    {
                        var img = Image.FromFile(path);
                        _iconCache[key] = img;
                        return img;
                    }
                    catch { return null; }
                }
            }
            return null;
        }

        // ---------------- Export CSV ----------------
        private void ExportCsvDialog()
        {
            using var sfd = new SaveFileDialog { Filter = "CSV file|*.csv", FileName = "reservations.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportToCsv(sfd.FileName);
                    MessageBox.Show("Đã xuất CSV.", "Xuất thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportToCsv(string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Mã đặt bàn,Khách hàng,SĐT,Ngày giờ,Bàn,Khu,Số khách,Trạng thái,Tiền cọc");
            foreach (var r in _filteredData)
            {
                var line = string.Join(",",
                    EscapeCsv(r.Code),
                    EscapeCsv(r.CustomerName),
                    EscapeCsv(r.Phone),
                    EscapeCsv(r.Date.ToString("dd/MM/yyyy HH:mm")),
                    EscapeCsv(r.TableName),
                    EscapeCsv(r.Area),
                    r.Guests.ToString(),
                    EscapeCsv(r.Status),
                    r.Deposit.ToString("F0")
                );
                sb.AppendLine(line);
            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private static string EscapeCsv(string s)
        {
            if (s == null) return "";
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
                return $"\"{s.Replace("\"", "\"\"")}\"";
            return s;
        }

        // ---------------- Printing ----------------
        private void PrintDialogAndStart()
        {
            using var pd = new PrintDialog { Document = _printDoc };
            if (pd.ShowDialog() == DialogResult.OK)
            {
                _printRowIndex = 0;
                _printDoc.DocumentName = "Danh sách đặt bàn";
                _printDoc.DefaultPageSettings.Landscape = true;
                _printDoc.Print();
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            int left = e.MarginBounds.Left;
            int top = e.MarginBounds.Top;
            int lineHeight = 24;
            var font = new Font("Segoe UI", 9F);

            // header
            e.Graphics.DrawString("Danh sách đặt bàn", new Font("Segoe UI", 12F, FontStyle.Bold), Brushes.Black, left, top);
            top += 32;

            // columns
            string[] headers = { "Mã", "Khách hàng", "Ngày giờ", "Bàn", "Khách", "Trạng thái", "Tiền cọc" };
            int[] widths = { 80, 220, 140, 140, 60, 140, 100 };
            int x = left;
            for (int i = 0; i < headers.Length; i++)
            {
                e.Graphics.DrawString(headers[i], font, Brushes.Black, x, top);
                x += widths[i];
            }
            top += lineHeight;

            // rows
            while (_printRowIndex < _filteredData.Count)
            {
                x = left;
                var r = _filteredData[_printRowIndex];
                e.Graphics.DrawString(r.Code, font, Brushes.Black, x, top); x += widths[0];
                e.Graphics.DrawString(r.CustomerName + " / " + r.Phone, font, Brushes.Black, x, top); x += widths[1];
                e.Graphics.DrawString(r.Date.ToString("dd/MM/yyyy HH:mm"), font, Brushes.Black, x, top); x += widths[2];
                e.Graphics.DrawString(r.TableName + (string.IsNullOrEmpty(r.Area) ? "" : " - " + r.Area), font, Brushes.Black, x, top); x += widths[3];
                e.Graphics.DrawString(r.Guests.ToString(), font, Brushes.Black, x, top); x += widths[4];
                e.Graphics.DrawString(r.Status, font, Brushes.Black, x, top); x += widths[5];
                e.Graphics.DrawString((r.Deposit > 0 ? r.Deposit.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " đ" : "Chưa cọc"), font, Brushes.Black, x, top); x += widths[6];

                top += lineHeight;
                _printRowIndex++;

                if (top + lineHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
            e.HasMorePages = false;
        }

        // -------------- helpers & icon drawings --------------
        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var gp = new GraphicsPath();
            int d = radius * 2;
            gp.AddArc(r.Left, r.Top, d, d, 180, 90);
            gp.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            gp.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            gp.CloseFigure();
            return gp;
        }
        private static Bitmap DrawSmallCalendarIcon(Color c, int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(c, 1.8f))
            using (var br = new SolidBrush(c))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawRectangle(pen, 1, 3, size - 4, size - 6);
                g.DrawLine(pen, 3, 1, 3, 5);
                g.DrawLine(pen, size - 4, 1, size - 4, 5);
                g.FillRectangle(br, 3, 3, 3, 2);
                g.FillRectangle(br, size - 6, 3, 3, 2);
            }
            return bmp;
        }
        private static Bitmap DrawPhoneIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.Gray, 1.5f))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawArc(pen, 1, 1, size - 3, size - 3, 180, 180);
                g.DrawRectangle(pen, size - 6, size - 5, 3, 3);
            }
            return bmp;
        }
        private static Bitmap DrawClockIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.Gray, 1.5f))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawEllipse(pen, 1, 1, size - 3, size - 3);
                g.DrawLine(pen, size / 2, size / 2, size / 2, 3);
                g.DrawLine(pen, size / 2, size / 2, size - 3, size / 2);
            }
            return bmp;
        }
        private static Bitmap DrawMapPinIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.Gray, 1.4f))
            using (var br = new SolidBrush(Color.Gray))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(2, 2, size - 5, size - 7);
                g.FillEllipse(br, rect);
                g.FillEllipse(Brushes.White, new Rectangle(size / 3, size / 3, size / 3, size / 3));
            }
            return bmp;
        }
        private static Bitmap DrawEyeIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.FromArgb(90, 90, 90), 1.4f))
            using (var br = new SolidBrush(Color.FromArgb(90, 90, 90)))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawEllipse(pen, 1, size / 3, size - 3, size / 3);
                g.FillEllipse(br, new Rectangle(size / 3, size / 3, size / 4, size / 4));
            }
            return bmp;
        }
        private static Bitmap DrawCheckIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.FromArgb(24, 103, 255), 2f))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLines(pen, new Point[] { new Point(2, size / 2), new Point(size / 3, size - 3), new Point(size - 2, 3) });
            }
            return bmp;
        }
        private static Bitmap DrawPersonCheckIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.FromArgb(3, 136, 88), 1.7f))
            using (var br = new SolidBrush(Color.FromArgb(3, 136, 88)))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(br, new Rectangle(2, 2, size / 3, size / 3));
                g.DrawArc(pen, 1, size / 3, size - 4, size - 2, 0, 180);
                g.DrawLines(pen, new[] { new Point(size - 8, size / 2), new Point(size - 5, size - 3), new Point(size - 1, size - 8) });
            }
            return bmp;
        }
        private static Bitmap DrawEditIcon(int size)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.FromArgb(80, 80, 80), 1.6f))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(pen, 3, size - 4, size - 4, 3);
                g.DrawLine(pen, size - 6, 3, size - 2, 7);
            }
            return bmp;
        }

        // ---------------- sample data ----------------
        public class Reservation
        {
            public string Code { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public DateTime Date { get; set; }
            public string TableName { get; set; }
            public string Area { get; set; }
            public int Guests { get; set; }
            public string Status { get; set; }
            public decimal Deposit { get; set; }
        }

        public class ReservationEventArgs : EventArgs
        {
            public Reservation Reservation { get; }
            public ReservationEventArgs(Reservation r) { Reservation = r; }
        }

        private List<Reservation> CreateSampleData()
        {
            return new List<Reservation>
            {
                new Reservation{ Code="RES001", CustomerName="Nguyễn Văn A", Phone="0901234567", Date=new DateTime(2025,10,20,12,0,0), TableName="Bàn A05", Area="Khu A", Guests=4, Status="Đã xác nhận", Deposit=200000 },
                new Reservation{ Code="RES002", CustomerName="Trần Thị B", Phone="0912345678", Date=new DateTime(2025,10,20,18,30,0), TableName="Bàn B03", Area="Khu B", Guests=6, Status="Chờ xác nhận", Deposit=0 },
                new Reservation{ Code="RES003", CustomerName="Lê Văn C", Phone="0923456789", Date=new DateTime(2025,10,20,11,0,0), TableName="Bàn A01", Area="Khu A", Guests=2, Status="Đã xác nhận", Deposit=500000 },
                new Reservation{ Code="RES004", CustomerName="Phạm Thị D", Phone="0934567890", Date=new DateTime(2025,10,19,19,0,0), TableName="Bàn VIP01", Area="VIP", Guests=8, Status="Đã đến", Deposit=1000000 },
                new Reservation{ Code="RES005", CustomerName="Hoàng Văn E", Phone="0945678901", Date=new DateTime(2025,10,18,13,0,0), TableName="Bàn A04", Area="Khu A", Guests=4, Status="Đã hủy", Deposit=0 },
                new Reservation{ Code="RES006", CustomerName="Võ Minh F", Phone="0956789012", Date=new DateTime(2025,10,21,20,0,0), TableName="Bàn VIP02", Area="VIP", Guests=10, Status="Đã xác nhận", Deposit=2000000 },
                new Reservation{ Code="RES007", CustomerName="Đỗ Thị G", Phone="0967890123", Date=new DateTime(2025,10,19,12,30,0), TableName="Bàn B02", Area="Khu B", Guests=5, Status="Hoàn thành", Deposit=300000 },
            };
        }
    }
}
