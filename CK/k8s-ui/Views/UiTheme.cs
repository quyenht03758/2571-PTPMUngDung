namespace K8sManager.Views
{
    public static class UiTheme
    {
        private static readonly Color DarkBg = Color.FromArgb(26, 28, 34);
        private static readonly Color DarkPanel = Color.FromArgb(35, 38, 45);
        private static readonly Color DarkText = Color.FromArgb(240, 240, 240);
        private static readonly Color LightBg = Color.White;
        private static readonly Color LightPanel = Color.WhiteSmoke;
        private static readonly Color LightText = Color.FromArgb(33, 33, 33);

        public static bool IsDark(Form f) => f.BackColor == DarkBg;

        public static void ApplyDark(Form root)
        {
            root.BackColor = DarkBg;
            root.ForeColor = DarkText;
            ApplyRecursive(root, isDark: true);
        }

        public static void ApplyLight(Form root)
        {
            root.BackColor = LightBg;
            root.ForeColor = LightText;
            ApplyRecursive(root, isDark: false);
        }

        private static void ApplyRecursive(Control c, bool isDark)
        {
            if (c is StatusStrip ss)
            {
                ss.BackColor = isDark ? DarkPanel : LightPanel;
                ss.ForeColor = isDark ? DarkText : LightText;
                foreach (ToolStripItem item in ss.Items)
                {
                    item.ForeColor = isDark ? DarkText : LightText;
                    item.BackColor = isDark ? DarkPanel : LightPanel;
                }
            }
            else if (c is ToolStrip ts)
            {
                ts.BackColor = isDark ? DarkPanel : LightPanel;
                ts.ForeColor = isDark ? DarkText : LightText;
                foreach (ToolStripItem item in ts.Items)
                {
                    item.ForeColor = isDark ? DarkText : LightText;
                    item.BackColor = isDark ? DarkPanel : LightPanel;
                }
            }
            else if (c is TabControl tc)
            {
                tc.BackColor = isDark ? DarkPanel : LightBg;
                tc.ForeColor = isDark ? DarkText : LightText;
                foreach (TabPage tab in tc.TabPages)
                {
                    tab.BackColor = isDark ? DarkBg : LightBg;
                    tab.ForeColor = isDark ? DarkText : LightText;
                }
            }
            else if (c is DataGridView dgv)
            {
                dgv.BackgroundColor = isDark ? DarkPanel : LightBg;
                dgv.DefaultCellStyle.BackColor = isDark ? DarkPanel : LightBg;
                dgv.DefaultCellStyle.ForeColor = isDark ? DarkText : LightText;
                dgv.DefaultCellStyle.SelectionBackColor = isDark ? Color.FromArgb(60, 110, 220) : Color.FromArgb(51, 103, 214);
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = isDark ? DarkPanel : LightPanel;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = isDark ? DarkText : LightText;
                dgv.GridColor = isDark ? Color.FromArgb(60, 60, 70) : Color.FromArgb(230, 230, 230);
                dgv.EnableHeadersVisualStyles = false;
            }
            else if (c is ListView lv)
            {
                lv.BackColor = isDark ? DarkPanel : LightBg;
                lv.ForeColor = isDark ? DarkText : LightText;
            }
            else if (c is TextBox tb)
            {
                tb.BackColor = isDark ? DarkPanel : LightBg;
                tb.ForeColor = isDark ? DarkText : LightText;
                if (tb.BorderStyle == BorderStyle.None)
                {
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }
            }
            else if (c is RichTextBox rtb)
            {
                rtb.BackColor = isDark ? DarkPanel : LightBg;
                rtb.ForeColor = isDark ? DarkText : LightText;
            }
            else if (c is ComboBox cb)
            {
                cb.BackColor = isDark ? DarkPanel : LightBg;
                cb.ForeColor = isDark ? DarkText : LightText;
            }
            else if (c is Button btn)
            {
                // Skip MaterialSkin buttons - they have their own styling
                if (!c.GetType().FullName?.Contains("MaterialSkin") == true)
                {
                    btn.BackColor = isDark ? DarkPanel : LightPanel;
                    btn.ForeColor = isDark ? DarkText : LightText;
                }
            }
            else if (c is Label lbl)
            {
                lbl.BackColor = isDark ? DarkBg : LightBg;
                lbl.ForeColor = isDark ? DarkText : LightText;
            }
            else if (c is Panel pnl)
            {
                pnl.BackColor = isDark ? DarkBg : LightBg;
                pnl.ForeColor = isDark ? DarkText : LightText;
            }
            else
            {
                // Default for all other controls
                if (!c.GetType().FullName?.Contains("MaterialSkin") == true)
                {
                    c.BackColor = isDark ? DarkBg : LightBg;
                    c.ForeColor = isDark ? DarkText : LightText;
                }
            }

            foreach (Control child in c.Controls)
            {
                ApplyRecursive(child, isDark);
            }
        }
    }
}
