using k8s;
using k8s.Models;
using K8sManager.Services;
using System.ComponentModel;

namespace K8sManager.Views
{
    [DesignerCategory("Form")]
    public partial class JobDetailsDialog : Form
    {
        private readonly IKubernetes _client;
        private readonly string _namespace;
        private readonly string _jobName;
        private V1Job? _job;

        public JobDetailsDialog(IKubernetes client, string namespaceName, string jobName)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _namespace = namespaceName ?? throw new ArgumentNullException(nameof(namespaceName));
            _jobName = jobName ?? throw new ArgumentNullException(nameof(jobName));

            InitializeComponent();
            LoadJobDetailsAsync();
        }

        private async void LoadJobDetailsAsync()
        {
            try
            {
                _job = await _client.BatchV1.ReadNamespacedJobAsync(_jobName, _namespace);
                PopulateOverview();
                await LoadPodsAsync();
                LoadYaml();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateOverview()
        {
            if (_job == null) return;

            _detailsPanel.Controls.Clear();
            _detailsPanel.RowStyles.Clear();
            _detailsPanel.RowCount = 0;

            AddDetailRow("Name:", _job.Metadata?.Name ?? "");
            AddDetailRow("Namespace:", _job.Metadata?.NamespaceProperty ?? "");
            AddDetailRow("Status:", GetJobStatus());
            AddDetailRow("Completions:", $"{_job.Status?.Succeeded ?? 0}/{_job.Spec?.Completions ?? 1}");
            AddDetailRow("Parallelism:", _job.Spec?.Parallelism?.ToString() ?? "1");
            AddDetailRow("Active:", _job.Status?.Active?.ToString() ?? "0");
            AddDetailRow("Succeeded:", _job.Status?.Succeeded?.ToString() ?? "0");
            AddDetailRow("Failed:", _job.Status?.Failed?.ToString() ?? "0");
            AddDetailRow("Start Time:", _job.Status?.StartTime?.ToString() ?? "N/A");
            AddDetailRow("Completion Time:", _job.Status?.CompletionTime?.ToString() ?? "N/A");
            
            var duration = GetDuration();
            if (!string.IsNullOrEmpty(duration))
                AddDetailRow("Duration:", duration);

            AddDetailRow("Backoff Limit:", _job.Spec?.BackoffLimit?.ToString() ?? "6");
            AddDetailRow("Active Deadline:", _job.Spec?.ActiveDeadlineSeconds?.ToString() ?? "None");

            // Labels
            if (_job.Metadata?.Labels != null && _job.Metadata.Labels.Count > 0)
            {
                var labels = string.Join(", ", _job.Metadata.Labels.Select(kv => $"{kv.Key}={kv.Value}"));
                AddDetailRow("Labels:", labels);
            }

            // Selector
            if (_job.Spec?.Selector?.MatchLabels != null)
            {
                var selector = string.Join(", ", _job.Spec.Selector.MatchLabels.Select(kv => $"{kv.Key}={kv.Value}"));
                AddDetailRow("Selector:", selector);
            }
        }

        private string GetJobStatus()
        {
            if (_job?.Status == null) return "Unknown";
            
            var succeeded = _job.Status.Succeeded ?? 0;
            var completions = _job.Spec?.Completions ?? 1;
            
            if (succeeded >= completions)
                return "✅ Complete";
            
            if ((_job.Status.Active ?? 0) > 0)
                return "🔄 Running";
            
            if ((_job.Status.Failed ?? 0) > 0)
                return "❌ Failed";
            
            return "⏸️ Pending";
        }

        private string GetDuration()
        {
            if (_job?.Status?.StartTime == null) return "";
            
            var endTime = _job.Status.CompletionTime ?? DateTime.UtcNow;
            var duration = endTime - _job.Status.StartTime.Value;
            
            if (duration.TotalDays >= 1)
                return $"{(int)duration.TotalDays}d {duration.Hours}h";
            if (duration.TotalHours >= 1)
                return $"{(int)duration.TotalHours}h {duration.Minutes}m";
            if (duration.TotalMinutes >= 1)
                return $"{(int)duration.TotalMinutes}m {duration.Seconds}s";
            
            return $"{(int)duration.TotalSeconds}s";
        }

        private void AddDetailRow(string label, string value)
        {
            var lblKey = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9.75F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 5, 10, 5)
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9.75F),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 5, 10, 5),
                AutoSize = false
            };

            _detailsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            _detailsPanel.Controls.Add(lblKey, 0, _detailsPanel.RowCount);
            _detailsPanel.Controls.Add(lblValue, 1, _detailsPanel.RowCount);
            _detailsPanel.RowCount++;
        }

        private async Task LoadPodsAsync()
        {
            try
            {
                var labelSelector = _job?.Spec?.Selector?.MatchLabels != null
                    ? string.Join(",", _job.Spec.Selector.MatchLabels.Select(kv => $"{kv.Key}={kv.Value}"))
                    : $"job-name={_jobName}";

                var pods = await _client.CoreV1.ListNamespacedPodAsync(
                    _namespace,
                    labelSelector: labelSelector);

                var podData = pods.Items.Select(p => new
                {
                    Name = p.Metadata?.Name ?? "",
                    Status = p.Status?.Phase ?? "",
                    Ready = $"{p.Status?.ContainerStatuses?.Count(c => c.Ready) ?? 0}/{p.Status?.ContainerStatuses?.Count ?? 0}",
                    Restarts = p.Status?.ContainerStatuses?.Sum(c => c.RestartCount) ?? 0,
                    Age = CalculateAge(p.Metadata?.CreationTimestamp)
                }).ToList();

                _gridPods.DataSource = podData;

                if (_gridPods.Columns.Count > 0)
                {
                    _gridPods.Columns["Name"].Width = 300;
                    _gridPods.Columns["Status"].Width = 100;
                    _gridPods.Columns["Ready"].Width = 80;
                    _gridPods.Columns["Restarts"].Width = 80;
                    _gridPods.Columns["Age"].Width = 100;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading pods: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadYaml()
        {
            if (_job == null) return;

            try
            {
                var yaml = KubernetesYaml.Serialize(_job);
                _txtYaml.Text = yaml;
            }
            catch (Exception ex)
            {
                _txtYaml.Text = $"Error generating YAML: {ex.Message}";
            }
        }

        private static string CalculateAge(DateTime? creationTimestamp)
        {
            if (!creationTimestamp.HasValue) return "N/A";

            var age = DateTime.UtcNow - creationTimestamp.Value;

            if (age.TotalDays >= 1)
                return $"{(int)age.TotalDays}d";
            if (age.TotalHours >= 1)
                return $"{(int)age.TotalHours}h";
            if (age.TotalMinutes >= 1)
                return $"{(int)age.TotalMinutes}m";

            return $"{(int)age.TotalSeconds}s";
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadJobDetailsAsync();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
