using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using SocialInsuranceService;
using SocialInsuranceService.Models;

namespace ClientApplication
{
    /// <summary>
    /// Main form for Social Insurance Management client application.
    /// </summary>
    public partial class FrmSocialInsurance : Form
    {
        private ISocialInsuranceService _serviceProxy;
        private ChannelFactory<ISocialInsuranceService> _channelFactory;
        private List<Person> _persons;

        public FrmSocialInsurance()
        {
            InitializeComponent();
            InitializeWcfConnection();
            LoadPersons();
        }

        /// <summary>
        /// Initializes the WCF service connection.
        /// </summary>
        private void InitializeWcfConnection()
        {
            try
            {
                var binding = new BasicHttpBinding
                {
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    ReceiveTimeout = TimeSpan.FromMinutes(10),
                    SendTimeout = TimeSpan.FromMinutes(10)
                };

                binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                binding.ReaderQuotas.MaxArrayLength = int.MaxValue;

                var endpoint = new EndpointAddress("http://localhost:8080/SocialInsuranceService");

                _channelFactory = new ChannelFactory<ISocialInsuranceService>(binding, endpoint);
                _serviceProxy = _channelFactory.CreateChannel();

                lblStatus.Text = "Status: Connected to WCF Service";
                lblStatus.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Status: Connection Failed";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show(
                    $"Failed to connect to WCF service.\n\nError: {ex.Message}\n\nPlease ensure the Host application is running.",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads all persons into the combo box.
        /// </summary>
        private void LoadPersons()
        {
            try
            {
                if (_serviceProxy == null)
                {
                    MessageBox.Show("Not connected to service.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _persons = _serviceProxy.GetAllPersons();

                cboSocialNumber.Items.Clear();
                cboSocialNumber.Items.Add("-- Select Social Number --");

                foreach (var person in _persons)
                {
                    cboSocialNumber.Items.Add($"{person.SocialNumber} - {person.FullName}");
                }

                cboSocialNumber.SelectedIndex = 0;
                DisplayPersonInfo(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load persons.\n\nError: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets all insurance processes for selected social number.
        /// </summary>
        private void GetAllProcessesBySocialNumber()
        {
            try
            {
                if (cboSocialNumber.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select a social number.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string socialNumber = GetSelectedSocialNumber();
                var processes = _serviceProxy.GetAllSocialInsuranceProcessBySocialNumber(socialNumber);

                DisplayProcesses(processes);
                lblRecordCount.Text = $"Total Records: {processes.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to retrieve insurance processes.\n\nError: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets insurance processes by social number and date range.
        /// </summary>
        private void GetProcessesByDateRange()
        {
            try
            {
                if (cboSocialNumber.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select a social number.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string socialNumber = GetSelectedSocialNumber();
                DateTime beginDate = dtpBeginDate.Value.Date;
                DateTime endDate = dtpEndDate.Value.Date;

                if (beginDate > endDate)
                {
                    MessageBox.Show("Begin date must be before end date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var processes = _serviceProxy.GetSocialInsuranceProcessBySocialNumberByDate(socialNumber, beginDate, endDate);

                DisplayProcesses(processes);
                lblRecordCount.Text = $"Total Records: {processes.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to retrieve insurance processes by date.\n\nError: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Gets all insurance processes by company name.
        /// </summary>
        private void GetProcessesByCompany()
        {
            try
            {
                string company = txtCompany.Text.Trim();

                if (string.IsNullOrWhiteSpace(company))
                {
                    MessageBox.Show("Please enter a company name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var processes = _serviceProxy.GetAllSocialInsuranceProcessByCompanyName(company);

                DisplayProcesses(processes);
                lblRecordCount.Text = $"Total Records: {processes.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to retrieve insurance processes by company.\n\nError: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Displays insurance processes in the DataGridView.
        /// </summary>
        private void DisplayProcesses(List<SocialInsuranceProcess> processes)
        {
            dgvProcesses.DataSource = null;
            dgvProcesses.DataSource = processes;

            if (dgvProcesses.Columns.Count > 0)
            {
                dgvProcesses.Columns["Id"].HeaderText = "ID";
                dgvProcesses.Columns["Id"].Width = 50;

                dgvProcesses.Columns["BeginDate"].HeaderText = "Begin Date";
                dgvProcesses.Columns["BeginDate"].Width = 100;
                dgvProcesses.Columns["BeginDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvProcesses.Columns["EndDate"].HeaderText = "End Date";
                dgvProcesses.Columns["EndDate"].Width = 100;
                dgvProcesses.Columns["EndDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

                dgvProcesses.Columns["Company"].HeaderText = "Company";
                dgvProcesses.Columns["Company"].Width = 150;

                dgvProcesses.Columns["Address"].HeaderText = "Address";
                dgvProcesses.Columns["Address"].Width = 200;

                dgvProcesses.Columns["Position"].HeaderText = "Position";
                dgvProcesses.Columns["Position"].Width = 120;

                dgvProcesses.Columns["SalaryByMonth"].HeaderText = "Monthly Salary";
                dgvProcesses.Columns["SalaryByMonth"].Width = 120;
                dgvProcesses.Columns["SalaryByMonth"].DefaultCellStyle.Format = "N0";

                dgvProcesses.Columns["Unit"].HeaderText = "Unit";
                dgvProcesses.Columns["Unit"].Width = 150;

                dgvProcesses.Columns["SalaryPay"].HeaderText = "Insurance Payment";
                dgvProcesses.Columns["SalaryPay"].Width = 120;
                dgvProcesses.Columns["SalaryPay"].DefaultCellStyle.Format = "N0";

                dgvProcesses.Columns["SocialNumber"].HeaderText = "Social Number";
                dgvProcesses.Columns["SocialNumber"].Width = 100;
            }
        }

        /// <summary>
        /// Displays person information.
        /// </summary>
        private void DisplayPersonInfo(Person person)
        {
            if (person == null)
            {
                txtFullName.Clear();
                txtAddress.Clear();
                txtExpDate.Clear();
                txtTreatmentLocation.Clear();
                txtDescription.Clear();
            }
            else
            {
                txtFullName.Text = person.FullName;
                txtAddress.Text = person.Address;
                txtExpDate.Text = person.ExpDate.ToString("dd/MM/yyyy");
                txtTreatmentLocation.Text = person.RegisterTreatmentLocation;
                txtDescription.Text = person.Description;
            }
        }

        /// <summary>
        /// Gets the selected social number from combo box.
        /// </summary>
        private string GetSelectedSocialNumber()
        {
            string selected = cboSocialNumber.SelectedItem.ToString();
            return selected.Split('-')[0].Trim();
        }

        // Event Handlers
        private void cboSocialNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSocialNumber.SelectedIndex > 0)
            {
                string socialNumber = GetSelectedSocialNumber();
                var person = _persons.FirstOrDefault(p => p.SocialNumber == socialNumber);
                DisplayPersonInfo(person);
            }
            else
            {
                DisplayPersonInfo(null);
            }
        }

        private void btnGetAll_Click(object sender, EventArgs e)
        {
            GetAllProcessesBySocialNumber();
        }

        private void btnGetByDate_Click(object sender, EventArgs e)
        {
            GetProcessesByDateRange();
        }

        private void btnGetByCompany_Click(object sender, EventArgs e)
        {
            GetProcessesByCompany();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cboSocialNumber.SelectedIndex = 0;
            txtCompany.Clear();
            dtpBeginDate.Value = DateTime.Now.AddYears(-5);
            dtpEndDate.Value = DateTime.Now;
            dgvProcesses.DataSource = null;
            lblRecordCount.Text = "Total Records: 0";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (_channelFactory != null && _channelFactory.State == CommunicationState.Opened)
            {
                try
                {
                    _channelFactory.Close();
                }
                catch
                {
                    _channelFactory.Abort();
                }
            }
        }
    }
}
