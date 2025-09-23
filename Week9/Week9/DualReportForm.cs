using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms; 

namespace Week9
{
    public partial class DualReportForm : Form
    {
        private TabControl tabs;
        private TabPage tabFlat, tabCategoryTotal;
        private ReportViewer rvFlat, rvCategoryTotal;

        public DualReportForm()
        {
            Text = "Northwind – Dual Reports";
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            // --- UI ---
            tabs = new TabControl { Dock = DockStyle.Fill };
            tabFlat = new TabPage("Sales (Flat)");
            tabCategoryTotal = new TabPage("Sales by Category (Totals)");

            rvFlat = new ReportViewer { Dock = DockStyle.Fill };
            rvCategoryTotal = new ReportViewer { Dock = DockStyle.Fill };

            tabFlat.Controls.Add(rvFlat);
            tabCategoryTotal.Controls.Add(rvCategoryTotal);
            tabs.TabPages.Add(tabFlat);
            tabs.TabPages.Add(tabCategoryTotal);
            Controls.Add(tabs);

            Load += DualReportForm_Load;
        }

        private void DualReportForm_Load(object sender, EventArgs e)
        {
            // 1) Fetch data once (typed TableAdapter generated from NorthwindDataSet.xsd)
            var ta = new NorthwindDataSetTableAdapters.Sales_by_CategoryTableAdapter();
            DataTable dt = ta.GetData();

            // 2) Resolve RDLC paths (root or Reports\ folder)
            string rdlcFlat = Path.Combine(Application.StartupPath, "SalesReport.rdlc");
            string rdlcCategoryTotal = Path.Combine(Application.StartupPath, "SalesReport_CategoryTotal.rdlc");
            if (!File.Exists(rdlcFlat))
                rdlcFlat = Path.Combine(Application.StartupPath, "Reports", "SalesReport.rdlc");
            if (!File.Exists(rdlcCategoryTotal))
                rdlcCategoryTotal = Path.Combine(Application.StartupPath, "Reports", "SalesReport_CategoryTotal.rdlc");

            // 3) Bind Tab 1 – flat report
            rvFlat.ProcessingMode = ProcessingMode.Local;
            rvFlat.LocalReport.DataSources.Clear();
            rvFlat.LocalReport.ReportPath = rdlcFlat;
            rvFlat.LocalReport.DataSources.Add(new ReportDataSource(
                "Sales_by_Category", // <-- must match Dataset name inside the RDLC
                dt
            ));
            rvFlat.SetDisplayMode(DisplayMode.PrintLayout);
            rvFlat.ZoomMode = ZoomMode.PageWidth;

            // 4) Bind Tab 2 – category totals (Category | Total)
            rvCategoryTotal.ProcessingMode = ProcessingMode.Local;
            rvCategoryTotal.LocalReport.DataSources.Clear();
            rvCategoryTotal.LocalReport.ReportPath = rdlcCategoryTotal;
            rvCategoryTotal.LocalReport.DataSources.Add(new ReportDataSource(
                "SalesByCategoryTotal", // <-- Dataset name inside SalesReport_CategoryTotal.rdlc
                BuildCategoryTotals(dt) // we can reuse dt to compute totals locally if needed
            ));
            rvCategoryTotal.SetDisplayMode(DisplayMode.PrintLayout);
            rvCategoryTotal.ZoomMode = ZoomMode.PageWidth;

            // 5) Render both
            rvFlat.RefreshReport();
            rvCategoryTotal.RefreshReport();
        }

     
        private static DataTable BuildCategoryTotals(DataTable dtSalesByCategory)
        {
            // dtSalesByCategory columns expected: CategoryName (string), ProductName (string), ProductSales (decimal)
            var result = new DataTable();
            result.TableName = "SalesByCategoryTotal";
            result.Columns.Add("CategoryName", typeof(string));
            result.Columns.Add("CategoryTotal", typeof(decimal));

            // group & sum with in-memory DataTable (works fine for demo size datasets)
            foreach (DataRow grp in dtSalesByCategory.DefaultView.ToTable(true, "CategoryName").Rows)
            {
                string cat = grp.Field<string>("CategoryName");
                decimal total = 0m;
                foreach (DataRow r in dtSalesByCategory.Select($"CategoryName = '{cat.Replace("'", "''")}'"))
                    total += r.Field<decimal>("ProductSales");
                result.Rows.Add(cat, total);
            }
            return result;
        }
    }
}
