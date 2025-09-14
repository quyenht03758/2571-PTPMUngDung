using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Exercise1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BindDepartments();
        }

        private void BindDepartments()
        {
            BindingList<Department> departmentList = new BindingList<Department>()
            {
                new Department() { Id="D001", Name="Dev App", CurrentProject="BookStore", StartDate=new DateTime(2023,3,1) },
                new Department() { Id="D002", Name="Mobile App", CurrentProject="Game", StartDate=new DateTime(2023,4,15) }
            };

            BindingSource bs = new BindingSource();
            bs.DataSource = departmentList;

            cbxDepartments.DataSource = bs;
            cbxDepartments.DisplayMember = "Name";
            cbxDepartments.ValueMember = "Id";
        }
    }

    public class Department
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CurrentProject { get; set; }
        public DateTime StartDate { get; set; }
    }
}
