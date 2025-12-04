using k8s.Models;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views;

/// <summary>
/// Detailed Pod information dialog (like Lens Desktop)
/// </summary>
[DesignerCategory("Form")]
public partial class PodDetailsDialog : MaterialForm
{
    private readonly V1Pod _pod;
    private readonly TabControl _tabs;
    private readonly RichTextBox _txtOverview;
    private readonly DataGridView _gridContainers;
    private readonly DataGridView _gridConditions;
    private readonly DataGridView _gridVolumes;
    private readonly RichTextBox _txtMetadata;
    private readonly MaterialButton _btnClose;
    private readonly MaterialButton _btnLogs;
    private readonly MaterialButton _btnShell;
    private readonly Action<V1Pod>? _onViewLogs;
    private readonly Action<V1Pod>? _onShell;

    public PodDetailsDialog(V1Pod pod, Action<V1Pod>? onViewLogs = null, Action<V1Pod>? onShell = null)
    {
        _pod = pod;
        _onViewLogs = onViewLogs;
        _onShell = onShell;

        Text = $"Pod Details - {pod.Metadata?.Name}";
        Size = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = true;
        MinimumSize = new Size(800, 600);

        // Tabs
        _tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f)
        };

        var tabOverview = new TabPage("Overview");
        var tabContainers = new TabPage("Containers");
        var tabConditions = new TabPage("Conditions");
        var tabVolumes = new TabPage("Volumes");
        var tabMetadata = new TabPage("Metadata");

        // Overview
        _txtOverview = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Font = new Font("Consolas", 12f),
            BackColor = Color.White
        };
        tabOverview.Controls.Add(_txtOverview);

        // Containers Grid
        _gridContainers = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None
        };
        _gridContainers.Columns.AddRange(new DataGridViewColumn[]
        {
            new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", DataPropertyName = "Name", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "Image", HeaderText = "Image", DataPropertyName = "Image", Width = 300 },
            new DataGridViewTextBoxColumn { Name = "Ready", HeaderText = "Ready", DataPropertyName = "Ready", Width = 80 },
            new DataGridViewTextBoxColumn { Name = "Restarts", HeaderText = "Restarts", DataPropertyName = "Restarts", Width = 100 },
            new DataGridViewTextBoxColumn { Name = "State", HeaderText = "State", DataPropertyName = "State", Width = 150 },
            new DataGridViewTextBoxColumn { Name = "Started", HeaderText = "Started", DataPropertyName = "Started", Width = 150 }
        });
        tabContainers.Controls.Add(_gridContainers);

        // Conditions Grid
        _gridConditions = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None
        };
        _gridConditions.Columns.AddRange(new DataGridViewColumn[]
        {
            new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Type", DataPropertyName = "Type", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", DataPropertyName = "Status", Width = 100 },
            new DataGridViewTextBoxColumn { Name = "LastTransition", HeaderText = "Last Transition", DataPropertyName = "LastTransition", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "Reason", HeaderText = "Reason", DataPropertyName = "Reason", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "Message", HeaderText = "Message", DataPropertyName = "Message", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
        });
        tabConditions.Controls.Add(_gridConditions);

        // Volumes Grid
        _gridVolumes = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            AllowUserToAddRows = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None
        };
        _gridVolumes.Columns.AddRange(new DataGridViewColumn[]
        {
            new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", DataPropertyName = "Name", Width = 250 },
            new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Type", DataPropertyName = "Type", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "Source", HeaderText = "Source", DataPropertyName = "Source", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
        });
        tabVolumes.Controls.Add(_gridVolumes);

        // Metadata
        _txtMetadata = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Font = new Font("Consolas", 12f),
            BackColor = Color.White
        };
        tabMetadata.Controls.Add(_txtMetadata);

        _tabs.TabPages.AddRange(new[] { tabOverview, tabContainers, tabConditions, tabVolumes, tabMetadata });

        // Bottom panel
        var bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            Padding = new Padding(10)
        };

        _btnLogs = new MaterialButton
        {
            Text = "View Logs",
            Location = new Point(10, 12),
            Size = new Size(120, 36),
            UseAccentColor = false,
            Enabled = _onViewLogs != null
        };
        _btnLogs.Click += (s, e) => _onViewLogs?.Invoke(_pod);

        _btnShell = new MaterialButton
        {
            Text = "Shell",
            Location = new Point(140, 12),
            Size = new Size(100, 36),
            UseAccentColor = false,
            Enabled = _onShell != null
        };
        _btnShell.Click += (s, e) => _onShell?.Invoke(_pod);

        _btnClose = new MaterialButton
        {
            Text = "Close",
            Location = new Point(250, 12),
            Size = new Size(100, 36),
            UseAccentColor = false
        };
        _btnClose.Click += (s, e) => Close();

        bottomPanel.Controls.AddRange(new Control[] { _btnLogs, _btnShell, _btnClose });

        Controls.Add(_tabs);
        Controls.Add(bottomPanel);

        Load += (s, e) => LoadPodDetails();
    }

    private void LoadPodDetails()
    {
        // Overview
        var overview = $@"Pod: {_pod.Metadata?.Name}
Namespace: {_pod.Metadata?.NamespaceProperty}
Status: {_pod.Status?.Phase}
IP: {_pod.Status?.PodIP ?? "N/A"}
Node: {_pod.Spec?.NodeName ?? "N/A"}
QoS Class: {_pod.Status?.QosClass ?? "N/A"}
Priority Class: {_pod.Spec?.PriorityClassName ?? "None"}
Service Account: {_pod.Spec?.ServiceAccountName ?? "default"}
Restart Policy: {_pod.Spec?.RestartPolicy ?? "N/A"}
DNS Policy: {_pod.Spec?.DnsPolicy ?? "N/A"}

Created: {_pod.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss UTC")}
UID: {_pod.Metadata?.Uid}

Labels:
{string.Join("\n", _pod.Metadata?.Labels?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}

Annotations:
{string.Join("\n", _pod.Metadata?.Annotations?.Take(10).Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}
";
        _txtOverview.Text = overview;

        // Containers
        var containers = _pod.Status?.ContainerStatuses?.Select(cs => new
        {
            Name = cs.Name,
            Image = cs.Image,
            Ready = cs.Ready ? "Yes" : "No",
            Restarts = cs.RestartCount,
            State = GetContainerState(cs),
            Started = cs.Started == true ? "Yes" : "No"
        }).ToList();

        _gridContainers.DataSource = containers;

        // Conditions
        var conditions = _pod.Status?.Conditions?.Select(c => new
        {
            Type = c.Type,
            Status = c.Status,
            LastTransition = c.LastTransitionTime?.ToString("yyyy-MM-dd HH:mm:ss"),
            Reason = c.Reason ?? "",
            Message = c.Message ?? ""
        }).ToList();

        _gridConditions.DataSource = conditions;

        // Volumes
        var volumes = _pod.Spec?.Volumes?.Select(v => new
        {
            Name = v.Name,
            Type = GetVolumeType(v),
            Source = GetVolumeSource(v)
        }).ToList();

        _gridVolumes.DataSource = volumes;

        // Metadata (raw labels & annotations)
        var metadata = $@"Labels:
{string.Join("\n", _pod.Metadata?.Labels?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}

Annotations:
{string.Join("\n", _pod.Metadata?.Annotations?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}

Owner References:
{string.Join("\n", _pod.Metadata?.OwnerReferences?.Select(o => $"  {o.Kind}/{o.Name} (UID: {o.Uid})") ?? Enumerable.Empty<string>())}
";
        _txtMetadata.Text = metadata;
    }

    private string GetContainerState(V1ContainerStatus cs)
    {
        if (cs.State?.Running != null)
            return $"Running (started: {cs.State.Running.StartedAt?.ToString("HH:mm:ss")})";
        if (cs.State?.Waiting != null)
            return $"Waiting ({cs.State.Waiting.Reason})";
        if (cs.State?.Terminated != null)
            return $"Terminated ({cs.State.Terminated.Reason})";
        return "Unknown";
    }

    private string GetVolumeType(V1Volume v)
    {
        if (v.ConfigMap != null) return "ConfigMap";
        if (v.Secret != null) return "Secret";
        if (v.EmptyDir != null) return "EmptyDir";
        if (v.PersistentVolumeClaim != null) return "PersistentVolumeClaim";
        if (v.HostPath != null) return "HostPath";
        if (v.Projected != null) return "Projected";
        if (v.DownwardAPI != null) return "DownwardAPI";
        return "Other";
    }

    private string GetVolumeSource(V1Volume v)
    {
        if (v.ConfigMap != null) return v.ConfigMap.Name ?? "";
        if (v.Secret != null) return v.Secret.SecretName ?? "";
        if (v.EmptyDir != null) return v.EmptyDir.Medium ?? "Memory";
        if (v.PersistentVolumeClaim != null) return v.PersistentVolumeClaim.ClaimName ?? "";
        if (v.HostPath != null) return v.HostPath.Path ?? "";
        return "N/A";
    }
}
