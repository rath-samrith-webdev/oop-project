using System.Drawing;

namespace dasboardApplications.Core
{
    public static class UITheme
    {
        // Premium Dark Mode Palette (Slate & Indigo)
        public static Color PrimaryBackground = Color.FromArgb(15, 23, 42);      // Slate 950
        public static Color SecondaryBackground = Color.FromArgb(30, 41, 59);    // Slate 800
        public static Color ContentBackground = Color.FromArgb(51, 65, 85);      // Slate 700 (for modals)
        public static Color SidebarBackground = Color.FromArgb(15, 23, 42);      // Dark Sidebar
        public static Color SidebarButtonActive = Color.FromArgb(30, 41, 59);    // Highlight
        public static Color SidebarButtonHover = Color.FromArgb(30, 41, 59);     // Hover
        public static Color CardBackground = Color.FromArgb(30, 41, 59);         // Slate 800
        public static Color HeaderBackground = Color.FromArgb(15, 23, 42);       // Dark

        // Accents
        public static Color AccentColor = Color.FromArgb(99, 102, 241);          // Indigo 500
        public static Color HoverColor = Color.FromArgb(129, 140, 248);          // Indigo 400
        public static Color PressedColor = Color.FromArgb(79, 70, 229);          // Indigo 600

        // Functional Colors
        public static Color DangerColor = Color.FromArgb(239, 68, 68);           // Red 500
        public static Color SuccessColor = Color.FromArgb(34, 197, 94);          // Green 500
        public static Color WarningColor = Color.FromArgb(245, 158, 11);         // Amber 500

        // Text Colors (light on dark)
        public static Color TextPrimary = Color.FromArgb(248, 250, 252);         // Slate 50
        public static Color TextSecondary = Color.FromArgb(148, 163, 184);       // Slate 400
        public static Color TextMuted = Color.FromArgb(100, 116, 139);          // Slate 500
        public static Color TextInverse = Color.FromArgb(15, 23, 42);            // Slate 950 (for dark text on light accents)

        // Fonts
        public static Font HeaderFont = new Font("Segoe UI Variable Display", 24f, FontStyle.Bold);
        public static Font SubHeaderFont = new Font("Segoe UI Variable Text", 16f, FontStyle.Bold);
        public static Font TitleFont = new Font("Segoe UI Variable Text", 12f, FontStyle.Bold);
        public static Font ButtonFont = new Font("Segoe UI Variable Text", 10f, FontStyle.Bold);
        public static Font BodyFont = new Font("Segoe UI Variable Text", 10f, FontStyle.Regular);
        public static Font SmallFont = new Font("Segoe UI Variable Text", 8f, FontStyle.Regular);

        // Standard Spacing
        public static int LargePadding = 32;
        public static int DefaultPadding = 24;
        public static int SmallPadding = 12;

        // Styling Config
        public static int BorderRadius = 16;
        public static int CardRadius = 24;
        public static Color BorderColor = Color.FromArgb(51, 65, 85);           // Slate 700

        // Spacing System (8px Grid)
        public static int GridUnit = 8;
        public static int FormMargin = 32;       // Standard outer margin
        public static int SectionMargin = 40;    // Margin between major sections
        public static int ControlGutter = 24;    // Vertical space between controls
        public static int LabelSpacing = 8;      // Space between label and its input
        public static int InputHeight = 36;      // Standard height for inputs

        public static void StyleButton(Button btn, bool isPrimary = true, bool isDanger = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = ButtonFont;

            if (isDanger)
            {
                btn.BackColor = DangerColor;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(185, 28, 28);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(153, 27, 27);
            }
            else if (isPrimary)
            {
                btn.BackColor = AccentColor;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.MouseOverBackColor = HoverColor;
                btn.FlatAppearance.MouseDownBackColor = PressedColor;
            }
            else
            {
                btn.BackColor = Color.FromArgb(229, 231, 235); // Gray 200
                btn.ForeColor = TextPrimary;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = BorderColor;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(209, 213, 219); // Gray 300
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(156, 163, 175); // Gray 400
            }
        }

        public enum LabelLevel { Header, SubHeader, Title, Body, Small }

        public static void StyleLabel(Label lbl, LabelLevel level = LabelLevel.Body)
        {
            lbl.BackColor = Color.Transparent;
            switch (level)
            {
                case LabelLevel.Header:
                    lbl.ForeColor = TextPrimary;
                    lbl.Font = HeaderFont;
                    break;
                case LabelLevel.SubHeader:
                    lbl.ForeColor = TextPrimary;
                    lbl.Font = SubHeaderFont;
                    break;
                case LabelLevel.Title:
                    lbl.ForeColor = AccentColor;
                    lbl.Font = TitleFont;
                    break;
                case LabelLevel.Small:
                    lbl.ForeColor = TextMuted;
                    lbl.Font = SmallFont;
                    break;
                default:
                    lbl.ForeColor = TextSecondary;
                    lbl.Font = BodyFont;
                    break;
            }
        }

        public static void StyleTextBox(TextBox txt)
        {
            txt.BackColor = SecondaryBackground;
            txt.ForeColor = TextPrimary;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = BodyFont;
        }

        public static void StyleDataGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = PrimaryBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = BorderColor;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SecondaryBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.Font = ButtonFont;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 12, 8, 12);
            dgv.ColumnHeadersHeight = 48; // Significantly increased for premium look
            dgv.AllowUserToResizeColumns = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.DefaultCellStyle.BackColor = PrimaryBackground;
            dgv.DefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.Font = BodyFont;
            dgv.DefaultCellStyle.Padding = new Padding(8, 6, 8, 6);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, AccentColor);
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = SecondaryBackground;
            dgv.RowHeadersVisible = false;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowTemplate.Height = 40;
        }

        public static void StyleTabControl(TabControl tab)
        {
            tab.DrawMode = TabDrawMode.OwnerDrawFixed;
            tab.SizeMode = TabSizeMode.Fixed;
            tab.ItemSize = new Size(240, 50);
            tab.Padding = new Point(0, 0);
        }

        public static void PaintTabControlBackground(TabControl tab, PaintEventArgs e)
        {
            // Fill the entire control area with the background color
            using (var brush = new SolidBrush(PrimaryBackground))
            {
                e.Graphics.FillRectangle(brush, tab.ClientRectangle);
            }
        }

        public static void DrawTab(TabControl tab, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= tab.TabCount) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Use e.Bounds which is the correct area for the current tab being drawn
            Rectangle tabRect = e.Bounds;
            bool isSelected = tab.SelectedIndex == e.Index;

            // Background
            using (var brush = new SolidBrush(isSelected ? SecondaryBackground : PrimaryBackground))
            {
                g.FillRectangle(brush, tabRect);
            }

            // Text with perfect centering
            string text = tab.TabPages[e.Index].Text;
            using (var brush = new SolidBrush(isSelected ? TextPrimary : TextSecondary))
            {
                // Consistent font size for both states
                var font = BodyFont;
                using (var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.DrawString(text, font, brush, tabRect, sf);
                }
            }

            // Selection Indicator (Indigo Underline)
            if (isSelected)
            {
                using (var pen = new Pen(AccentColor, 3))
                {
                    int padding = 30; // Control underline width
                    g.DrawLine(pen, tabRect.Left + padding, tabRect.Bottom - 4, tabRect.Right - padding, tabRect.Bottom - 4);
                }
            }
        }

        public static void StyleModernCard(Panel card)
        {
            card.BackColor = CardBackground;
            card.Padding = new Padding(24);
            card.Margin = new Padding(0, 0, 24, 24);
        }

        public static void DrawModernCard(Graphics g, Rectangle rect, bool isHovered = false)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw shadow/glow if hovered
            if (isHovered)
            {
                using (var brush = new SolidBrush(Color.FromArgb(20, AccentColor)))
                {
                    var shadowRect = rect;
                    shadowRect.Inflate(4, 4);
                    DrawRoundedRect(g, brush, shadowRect, CardRadius + 2);
                }
            }

            // Draw Card Background
            using (var brush = new SolidBrush(CardBackground))
            {
                DrawRoundedRect(g, brush, rect, CardRadius);
            }

            // Draw Border
            using (var pen = new Pen(isHovered ? AccentColor : BorderColor, 1))
            {
                DrawRoundedRect(g, pen, rect, CardRadius);
            }
        }

        private static void DrawRoundedRect(Graphics g, Brush brush, Rectangle rect, int radius)
        {
            using (var path = GetRoundedPath(rect, radius))
            {
                g.FillPath(brush, path);
            }
        }

        private static void DrawRoundedRect(Graphics g, Pen pen, Rectangle rect, int radius)
        {
            using (var path = GetRoundedPath(rect, radius))
            {
                g.DrawPath(pen, path);
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            float d = radius * 2;
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
