using k8s.Models;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views;

/// <summary>
/// Dialog for viewing pods belonging to a workload
/// </summary>
[DesignerCategory("Form")]

public partial class WorkloadPodsDialog : MaterialForm
{
    private ListView _listPods = null!;
    private MaterialButton _btnRefresh = null!;
    private MaterialButton _btnLogs = null!;
    private MaterialButton _btnShell = null!;
    private MaterialButton _btnClose = null!;
    private Panel _bottomPanel = null!;
    
    private readonly Func<Task<IList<V1Pod>>> _loadPodsFunc;
    private readonly Action<V1Pod> _onViewLogs;
    private readonly Action<V1Pod> _onShell;

    public WorkloadPodsDialog(
        string workloadName,
        string kind,
        Func<Task<IList<V1Pod>>> loadPodsFunc,
        Action<V1Pod> onViewLogs,
        Action<V1Pod> onShell)
    {
        InitializeComponent();
        
        _loadPodsFunc = loadPodsFunc;
        _onViewLogs = onViewLogs;
        _onShell = onShell;

        Text = $"Pods - {kind}/{workloadName}";

        _btnRefresh.Click += async (s, e) => await LoadPodsAsync().ConfigureAwait(false);
        _btnLogs.Click += BtnLogs_Click;
        _btnShell.Click += BtnShell_Click;
        _btnClose.Click += (s, e) => Close();
        _listPods.SelectedIndexChanged += ListPods_SelectedIndexChanged;

        Load += async (s, e) => await LoadPodsAsync().ConfigureAwait(false);
    }

    private void BtnLogs_Click(object? sender, EventArgs e)
    {
        if (_listPods?.SelectedItems?.Count > 0)
        {
            var pod = _listPods.SelectedItems[0].Tag as V1Pod;
            if (pod != null)
                _onViewLogs?.Invoke(pod);
        }
    }

    private void BtnShell_Click(object? sender, EventArgs e)
    {
        if (_listPods?.SelectedItems?.Count > 0)
        {
            var pod = _listPods.SelectedItems[0].Tag as V1Pod;
            if (pod != null)
                _onShell?.Invoke(pod);
        }
    }

    private void ListPods_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var hasSelection = _listPods.SelectedItems.Count > 0;
        _btnLogs.Enabled = hasSelection;
        _btnShell.Enabled = hasSelection;
    }

    private async Task LoadPodsAsync()
    {
        try
        {
            _btnRefresh.Enabled = false;
            _listPods.Items.Clear();

            var pods = await _loadPodsFunc().ConfigureAwait(false);

            foreach (var pod in pods)
            {
                var status = pod.Status?.Phase ?? "Unknown";
                var ready = $"{pod.Status?.ContainerStatuses?.Count(cs => cs.Ready) ?? 0}/{pod.Status?.ContainerStatuses?.Count ?? 0}";
                var restarts = pod.Status?.ContainerStatuses?.Sum(cs => cs.RestartCount) ?? 0;
                var age = GetAge(pod.Metadata?.CreationTimestamp);
                var ip = pod.Status?.PodIP ?? "-";

                var item = new ListViewItem(new[]
                {
                    pod.Metadata?.Name ?? "Unknown",
                    status,
                    ready,
                    restarts.ToString(),
                    age,
                    ip
                });
                item.Tag = pod;

                // Color by status
                if (status == "Running")
                    item.ForeColor = Color.Green;
                else if (status == "Pending")
                    item.ForeColor = Color.Orange;
                else if (status == "Failed" || status == "CrashLoopBackOff")
                    item.ForeColor = Color.Red;

                _listPods.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading pods: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _btnRefresh.Enabled = true;
        }
    }

    private string GetAge(DateTime? creationTime)
    {
        if (!creationTime.HasValue) return "Unknown";

        var age = DateTime.UtcNow - creationTime.Value;

        if (age.TotalDays >= 1)
            return $"{(int)age.TotalDays}d";
        if (age.TotalHours >= 1)
            return $"{(int)age.TotalHours}h";
        if (age.TotalMinutes >= 1)
            return $"{(int)age.TotalMinutes}m";
        return $"{(int)age.TotalSeconds}s";
    }
}
