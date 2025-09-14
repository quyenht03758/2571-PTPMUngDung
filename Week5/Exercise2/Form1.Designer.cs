using System.Windows.Forms;

namespace Exercise2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvProjects;
        private TextBox txtId;
        private TextBox txtName;
        private TextBox txtLocation;
        private DateTimePicker dtpStartDate;
        private TextBox txtManager;
        private Label lblId;
        private Label lblName;
        private Label lblLocation;
        private Label lblStartDate;
        private Label lblManager;
        private Button btnSave;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnShowAll;
        private BindingNavigator bindingNavigator;
        private ToolStripButton bindingNavigatorAddNewItem;
        private ToolStripLabel bindingNavigatorCountItem;
        private ToolStripButton bindingNavigatorDeleteItem;
        private ToolStripButton bindingNavigatorMoveFirstItem;
        private ToolStripButton bindingNavigatorMovePreviousItem;
        private ToolStripSeparator bindingNavigatorSeparator;
        private ToolStripTextBox bindingNavigatorPositionItem;
        private ToolStripSeparator bindingNavigatorSeparator1;
        private ToolStripButton bindingNavigatorMoveNextItem;
        private ToolStripButton bindingNavigatorMoveLastItem;
        private ToolStripSeparator bindingNavigatorSeparator2;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.dgvProjects = new DataGridView();
            this.txtId = new TextBox();
            this.txtName = new TextBox();
            this.txtLocation = new TextBox();
            this.dtpStartDate = new DateTimePicker();
            this.txtManager = new TextBox();
            this.lblId = new Label();
            this.lblName = new Label();
            this.lblLocation = new Label();
            this.lblStartDate = new Label();
            this.lblManager = new Label();
            this.btnSave = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnShowAll = new Button();
            this.bindingNavigator = new BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new ToolStripButton();
            this.bindingNavigatorCountItem = new ToolStripLabel();
            this.bindingNavigatorDeleteItem = new ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new ToolStripButton();
            this.bindingNavigatorSeparator = new ToolStripSeparator();
            this.bindingNavigatorPositionItem = new ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new ToolStripButton();
            this.bindingNavigatorMoveLastItem = new ToolStripButton();
            this.bindingNavigatorSeparator2 = new ToolStripSeparator();

            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).BeginInit();
            this.bindingNavigator.SuspendLayout();
            this.SuspendLayout();

            // BindingNavigator (UI only; nguồn sẽ gán trong Form1.cs)
            this.bindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigator.Items.AddRange(new ToolStripItem[] {
                this.bindingNavigatorMoveFirstItem,
                this.bindingNavigatorMovePreviousItem,
                this.bindingNavigatorSeparator,
                this.bindingNavigatorPositionItem,
                this.bindingNavigatorCountItem,
                this.bindingNavigatorSeparator1,
                this.bindingNavigatorMoveNextItem,
                this.bindingNavigatorMoveLastItem,
                this.bindingNavigatorSeparator2,
                this.bindingNavigatorAddNewItem,
                this.bindingNavigatorDeleteItem
            });
            this.bindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator.Name = "bindingNavigator";
            this.bindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator.Size = new System.Drawing.Size(860, 25);
            this.bindingNavigator.TabIndex = 100;

            // Toolstrip items (text/icons mặc định)
            this.bindingNavigatorAddNewItem.Text = "Add";
            this.bindingNavigatorDeleteItem.Text = "Delete";
            this.bindingNavigatorMoveFirstItem.Text = "|<";
            this.bindingNavigatorMovePreviousItem.Text = "<";
            this.bindingNavigatorMoveNextItem.Text = ">";
            this.bindingNavigatorMoveLastItem.Text = ">|";
            this.bindingNavigatorCountItem.Text = "of {0}";

            // DataGridView
            this.dgvProjects.Location = new System.Drawing.Point(20, 40);
            this.dgvProjects.Name = "dgvProjects";
            this.dgvProjects.Size = new System.Drawing.Size(820, 220);
            this.dgvProjects.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProjects.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvProjects.ReadOnly = false;
            this.dgvProjects.TabIndex = 0;

            // Labels
            this.lblId.Text = "Project ID:";
            this.lblId.Location = new System.Drawing.Point(20, 280);

            this.lblName.Text = "Project Name:";
            this.lblName.Location = new System.Drawing.Point(20, 310);

            this.lblLocation.Text = "Location:";
            this.lblLocation.Location = new System.Drawing.Point(20, 340);

            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.Location = new System.Drawing.Point(20, 370);

            this.lblManager.Text = "Manager:";
            this.lblManager.Location = new System.Drawing.Point(20, 400);

            // Inputs
            this.txtId.Location = new System.Drawing.Point(130, 276);
            this.txtId.Width = 240;

            this.txtName.Location = new System.Drawing.Point(130, 306);
            this.txtName.Width = 240;

            this.txtLocation.Location = new System.Drawing.Point(130, 336);
            this.txtLocation.Width = 240;

            this.dtpStartDate.Location = new System.Drawing.Point(130, 366);
            this.dtpStartDate.Width = 240;

            this.txtManager.Location = new System.Drawing.Point(130, 396);
            this.txtManager.Width = 240;

            // Buttons
            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(420, 276);

            this.btnUpdate.Text = "Update";
            this.btnUpdate.Location = new System.Drawing.Point(420, 316);

            this.btnDelete.Text = "Delete";
            this.btnDelete.Location = new System.Drawing.Point(420, 356);

            this.btnShowAll.Text = "Show All";
            this.btnShowAll.Location = new System.Drawing.Point(420, 396);

            // Form
            this.ClientSize = new System.Drawing.Size(860, 450);
            this.Controls.Add(this.bindingNavigator);
            this.Controls.Add(this.dgvProjects);
            this.Controls.Add(this.lblId);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.lblManager);
            this.Controls.Add(this.txtManager);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnShowAll);
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Project Manager (Data Binding)";

            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).EndInit();
            this.bindingNavigator.ResumeLayout(false);
            this.bindingNavigator.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}

