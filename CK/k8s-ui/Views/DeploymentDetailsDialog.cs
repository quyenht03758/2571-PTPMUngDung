using k8s.Models;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views;

/// <summary>
/// Detailed Deployment information dialog (like Lens Desktop)
/// </summary>
[DesignerCategory("Form")]
public partial class DeploymentDetailsDialog : MaterialForm
{
    private readonly V1Deployment _deployment;
    private readonly TabControl _tabs;
    private readonly RichTextBox _txtOverview;
    private readonly DataGridView _gridContainers;
    private readonly DataGridView _gridConditions;
    private readonly DataGridView _gridVolumes;
    private readonly RichTextBox _txtMetadata;
    private readonly RichTextBox _txtStrategy;
    private readonly MaterialButton _btnClose;
    private readonly MaterialButton _btnScale;
    private readonly MaterialButton _btnRestart;
    private readonly MaterialButton _btnViewPods;
    private readonly Action<V1Deployment>? _onScale;
    private readonly Action<V1Deployment>? _onRestart;
    private readonly Action<V1Deployment>? _onViewPods;

    public DeploymentDetailsDialog(
        V1Deployment deployment,
        Action<V1Deployment>? onScale = null,
        Action<V1Deployment>? onRestart = null,
        Action<V1Deployment>? onViewPods = null)
    {
        _deployment = deployment;
        _onScale = onScale;
        _onRestart = onRestart;
        _onViewPods = onViewPods;

        Text = $"Deployment Details - {deployment.Metadata?.Name}";
        Size = new Size(1100, 750);
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = true;
        MinimumSize = new Size(900, 650);

        // Tabs
        _tabs = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f)
        };

        var tabOverview = new TabPage("Overview");
        var tabContainers = new TabPage("Containers");
        var tabConditions = new TabPage("Conditions");
        var tabStrategy = new TabPage("Strategy");
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
            new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Container Name", DataPropertyName = "Name", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "Image", HeaderText = "Image", DataPropertyName = "Image", Width = 350 },
            new DataGridViewTextBoxColumn { Name = "Ports", HeaderText = "Ports", DataPropertyName = "Ports", Width = 150 },
            new DataGridViewTextBoxColumn { Name = "Resources", HeaderText = "Resources", DataPropertyName = "Resources", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
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
            new DataGridViewTextBoxColumn { Name = "LastUpdate", HeaderText = "Last Update", DataPropertyName = "LastUpdate", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "LastTransition", HeaderText = "Last Transition", DataPropertyName = "LastTransition", Width = 200 },
            new DataGridViewTextBoxColumn { Name = "Reason", HeaderText = "Reason", DataPropertyName = "Reason", Width = 150 },
            new DataGridViewTextBoxColumn { Name = "Message", HeaderText = "Message", DataPropertyName = "Message", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
        });
        tabConditions.Controls.Add(_gridConditions);

        // Strategy
        _txtStrategy = new RichTextBox
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            Font = new Font("Consolas", 12f),
            BackColor = Color.White
        };
        tabStrategy.Controls.Add(_txtStrategy);

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

        _tabs.TabPages.AddRange(new[] { tabOverview, tabContainers, tabConditions, tabStrategy, tabVolumes, tabMetadata });

        // Bottom panel
        var bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            Padding = new Padding(10)
        };

        _btnScale = new MaterialButton
        {
            Text = "Scale",
            Location = new Point(10, 12),
            Size = new Size(100, 36),
            UseAccentColor = false,
            Enabled = _onScale != null
        };
        _btnScale.Click += (s, e) => _onScale?.Invoke(_deployment);

        _btnRestart = new MaterialButton
        {
            Text = "Restart",
            Location = new Point(120, 12),
            Size = new Size(100, 36),
            UseAccentColor = false,
            Enabled = _onRestart != null
        };
        _btnRestart.Click += (s, e) => _onRestart?.Invoke(_deployment);

        _btnViewPods = new MaterialButton
        {
            Text = "View Pods",
            Location = new Point(230, 12),
            Size = new Size(120, 36),
            UseAccentColor = false,
            Enabled = _onViewPods != null
        };
        _btnViewPods.Click += (s, e) => _onViewPods?.Invoke(_deployment);

        _btnClose = new MaterialButton
        {
            Text = "Close",
            Location = new Point(360, 12),
            Size = new Size(100, 36),
            UseAccentColor = false
        };
        _btnClose.Click += (s, e) => Close();

        bottomPanel.Controls.AddRange(new Control[] { _btnScale, _btnRestart, _btnViewPods, _btnClose });

        Controls.Add(_tabs);
        Controls.Add(bottomPanel);

        Load += (s, e) => LoadDeploymentDetails();
    }

    private void LoadDeploymentDetails()
    {
        // Overview
        var replicas = _deployment.Spec?.Replicas ?? 0;
        var readyReplicas = _deployment.Status?.ReadyReplicas ?? 0;
        var availableReplicas = _deployment.Status?.AvailableReplicas ?? 0;
        var updatedReplicas = _deployment.Status?.UpdatedReplicas ?? 0;

        var overview = $@"Deployment: {_deployment.Metadata?.Name}
Namespace: {_deployment.Metadata?.NamespaceProperty}

Status:
  Replicas: {readyReplicas}/{replicas} ready
  Available: {availableReplicas}
  Updated: {updatedReplicas}
  Unavailable: {_deployment.Status?.UnavailableReplicas ?? 0}

Strategy:
  Type: {_deployment.Spec?.Strategy?.Type ?? "RollingUpdate"}
  
Selector:
  {string.Join("\n  ", _deployment.Spec?.Selector?.MatchLabels?.Select(kv => $"{kv.Key}={kv.Value}") ?? Enumerable.Empty<string>())}

Created: {_deployment.Metadata?.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss UTC")}
Generation: {_deployment.Metadata?.Generation}
UID: {_deployment.Metadata?.Uid}

Labels:
{string.Join("\n", _deployment.Metadata?.Labels?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}
";
        _txtOverview.Text = overview;

        // Containers
        var containers = _deployment.Spec?.Template?.Spec?.Containers?.Select(c => new
        {
            Name = c.Name,
            Image = c.Image,
            Ports = string.Join(", ", c.Ports?.Select(p => $"{p.ContainerPort}/{p.Protocol}") ?? Enumerable.Empty<string>()),
            Resources = GetResourcesInfo(c)
        }).ToList();

        _gridContainers.DataSource = containers;

        // Conditions
        var conditions = _deployment.Status?.Conditions?.Select(c => new
        {
            Type = c.Type,
            Status = c.Status,
            LastUpdate = c.LastUpdateTime?.ToString("yyyy-MM-dd HH:mm:ss"),
            LastTransition = c.LastTransitionTime?.ToString("yyyy-MM-dd HH:mm:ss"),
            Reason = c.Reason ?? "",
            Message = c.Message ?? ""
        }).ToList();

        _gridConditions.DataSource = conditions;

        // Strategy Details
        var strategy = _deployment.Spec?.Strategy;
        var strategyText = $@"Deployment Strategy
==================

Type: {strategy?.Type ?? "RollingUpdate"}

";

        if (strategy?.Type == "RollingUpdate" && strategy.RollingUpdate != null)
        {
            strategyText += $@"Rolling Update Configuration:
  Max Unavailable: {strategy.RollingUpdate.MaxUnavailable?.ToString() ?? "25%"}
  Max Surge: {strategy.RollingUpdate.MaxSurge?.ToString() ?? "25%"}

Description:
  Max Unavailable: Maximum number of pods that can be unavailable during the update
  Max Surge: Maximum number of pods that can be created above the desired number
";
        }
        else if (strategy?.Type == "Recreate")
        {
            strategyText += @"Recreate Strategy:
  All existing pods are killed before new ones are created
  Results in downtime but ensures clean slate
";
        }

        strategyText += $@"

Revision History:
  Revision History Limit: {_deployment.Spec?.RevisionHistoryLimit ?? 10}
  
Pod Template:
  Min Ready Seconds: {_deployment.Spec?.MinReadySeconds ?? 0}
  Progress Deadline: {_deployment.Spec?.ProgressDeadlineSeconds ?? 600}s
";

        _txtStrategy.Text = strategyText;

        // Volumes
        var volumes = _deployment.Spec?.Template?.Spec?.Volumes?.Select(v => new
        {
            Name = v.Name,
            Type = GetVolumeType(v),
            Source = GetVolumeSource(v)
        }).ToList();

        _gridVolumes.DataSource = volumes;

        // Metadata (raw labels & annotations)
        var metadata = $@"Labels:
{string.Join("\n", _deployment.Metadata?.Labels?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}

Annotations:
{string.Join("\n", _deployment.Metadata?.Annotations?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}

Pod Template Labels:
{string.Join("\n", _deployment.Spec?.Template?.Metadata?.Labels?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}

Pod Template Annotations:
{string.Join("\n", _deployment.Spec?.Template?.Metadata?.Annotations?.Select(kv => $"  {kv.Key}: {kv.Value}") ?? Enumerable.Empty<string>())}

Owner References:
{string.Join("\n", _deployment.Metadata?.OwnerReferences?.Select(o => $"  {o.Kind}/{o.Name} (UID: {o.Uid})") ?? Enumerable.Empty<string>())}
";
        _txtMetadata.Text = metadata;
    }

    private string GetResourcesInfo(V1Container container)
    {
        var limits = container.Resources?.Limits;
        var requests = container.Resources?.Requests;

        if (limits == null && requests == null)
            return "None";

        var info = "";

        if (requests != null)
        {
            var cpu = requests.ContainsKey("cpu") ? requests["cpu"].ToString() : "-";
            var memory = requests.ContainsKey("memory") ? requests["memory"].ToString() : "-";
            info += $"Req: CPU={cpu}, Mem={memory}";
        }

        if (limits != null)
        {
            if (!string.IsNullOrEmpty(info)) info += " | ";
            var cpu = limits.ContainsKey("cpu") ? limits["cpu"].ToString() : "-";
            var memory = limits.ContainsKey("memory") ? limits["memory"].ToString() : "-";
            info += $"Lim: CPU={cpu}, Mem={memory}";
        }

        return info;
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
