using k8s.Models;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    /// <summary>
    /// Detailed view dialog for DaemonSet with 6 tabs
    /// </summary>
    [DesignerCategory("Form")]
    public partial class DaemonSetDetailsDialog : MaterialForm
    {
        private readonly V1DaemonSet _daemonSet;
        private readonly Func<V1DaemonSet, System.Threading.Tasks.Task> _onRestart;
        private readonly Func<V1DaemonSet, System.Threading.Tasks.Task> _onViewPods;

        public DaemonSetDetailsDialog(
            V1DaemonSet daemonSet,
            Func<V1DaemonSet, System.Threading.Tasks.Task> onRestart,
            Func<V1DaemonSet, System.Threading.Tasks.Task> onViewPods)
        {
            InitializeComponent();
            
            _daemonSet = daemonSet;
            _onRestart = onRestart;
            _onViewPods = onViewPods;

            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            Text = $"DaemonSet: {_daemonSet.Metadata?.Name}";
            Size = new Size(1100, 750);
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = true;
            MinimumSize = new Size(900, 600);

            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            // Add tabs
            tabControl.TabPages.Add(CreateOverviewTab());
            tabControl.TabPages.Add(CreateContainersTab());
            tabControl.TabPages.Add(CreateConditionsTab());
            tabControl.TabPages.Add(CreateUpdateStrategyTab());
            tabControl.TabPages.Add(CreateVolumesTab());
            tabControl.TabPages.Add(CreateMetadataTab());

            // Bottom panel with action buttons (no Scale for DaemonSet)
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(15, 10, 15, 15)
            };

            var btnRestart = new MaterialButton
            {
                Text = "Restart",
                Location = new Point(15, 15),
                AutoSize = true
            };
            btnRestart.Click += async (s, e) => await _onRestart(_daemonSet).ConfigureAwait(false);

            var btnViewPods = new MaterialButton
            {
                Text = "View Pods",
                Location = new Point(120, 15),
                AutoSize = true
            };
            btnViewPods.Click += async (s, e) => await _onViewPods(_daemonSet).ConfigureAwait(false);

            var btnClose = new MaterialButton
            {
                Text = "Close",
                Location = new Point(245, 15),
                AutoSize = true
            };
            btnClose.Click += (s, e) => Close();

            bottomPanel.Controls.AddRange(new Control[] { btnRestart, btnViewPods, btnClose });

            Controls.Add(tabControl);
            Controls.Add(bottomPanel);
        }

        private TabPage CreateOverviewTab()
        {
            var tab = new TabPage("Overview");
            var panel = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(15) };

            var textBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 12F),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            var spec = _daemonSet.Spec;
            var status = _daemonSet.Status;
            var metadata = _daemonSet.Metadata;

            textBox.AppendText($"Name: {metadata?.Name}\n");
            textBox.AppendText($"Namespace: {metadata?.NamespaceProperty}\n\n");

            textBox.AppendText("=== Status ===\n");
            textBox.AppendText($"Desired Number Scheduled: {status?.DesiredNumberScheduled ?? 0}\n");
            textBox.AppendText($"Current Number Scheduled: {status?.CurrentNumberScheduled ?? 0}\n");
            textBox.AppendText($"Number Ready: {status?.NumberReady ?? 0}\n");
            textBox.AppendText($"Number Available: {status?.NumberAvailable ?? 0}\n");
            textBox.AppendText($"Number Unavailable: {status?.NumberUnavailable ?? 0}\n");
            textBox.AppendText($"Updated Number Scheduled: {status?.UpdatedNumberScheduled ?? 0}\n");
            textBox.AppendText($"Number Misscheduled: {status?.NumberMisscheduled ?? 0}\n\n");

            textBox.AppendText("=== Selector ===\n");
            if (spec?.Selector?.MatchLabels != null)
            {
                foreach (var label in spec.Selector.MatchLabels)
                {
                    textBox.AppendText($"  {label.Key}: {label.Value}\n");
                }
            }

            textBox.AppendText("\n=== Additional Info ===\n");
            textBox.AppendText($"Generation: {metadata?.Generation}\n");
            textBox.AppendText($"Observed Generation: {status?.ObservedGeneration}\n");
            textBox.AppendText($"Creation Time: {metadata?.CreationTimestamp}\n");
            textBox.AppendText($"UID: {metadata?.Uid}\n\n");

            if (metadata?.Labels?.Any() == true)
            {
                textBox.AppendText("=== Labels ===\n");
                foreach (var label in metadata.Labels)
                {
                    textBox.AppendText($"  {label.Key}: {label.Value}\n");
                }
            }

            panel.Controls.Add(textBox);
            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateContainersTab()
        {
            var tab = new TabPage("Containers");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            grid.Columns.Add("Name", "Name");
            grid.Columns.Add("Image", "Image");
            grid.Columns.Add("Ports", "Ports");
            grid.Columns.Add("Resources", "Resources");

            var containers = _daemonSet.Spec?.Template?.Spec?.Containers;
            if (containers != null)
            {
                foreach (var container in containers)
                {
                    var ports = container.Ports != null
                        ? string.Join(", ", container.Ports.Select(p => $"{p.ContainerPort}/{p.Protocol}"))
                        : "None";

                    var resources = GetResourcesInfo(container.Resources);

                    grid.Rows.Add(
                        container.Name,
                        container.Image,
                        ports,
                        resources
                    );
                }
            }

            tab.Controls.Add(grid);
            return tab;
        }

        private TabPage CreateConditionsTab()
        {
            var tab = new TabPage("Conditions");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            grid.Columns.Add("Type", "Type");
            grid.Columns.Add("Status", "Status");
            grid.Columns.Add("LastTransitionTime", "Last Transition");
            grid.Columns.Add("Reason", "Reason");
            grid.Columns.Add("Message", "Message");

            var conditions = _daemonSet.Status?.Conditions;
            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    grid.Rows.Add(
                        condition.Type,
                        condition.Status,
                        condition.LastTransitionTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A",
                        condition.Reason ?? "",
                        condition.Message ?? ""
                    );
                }
            }

            tab.Controls.Add(grid);
            return tab;
        }

        private TabPage CreateUpdateStrategyTab()
        {
            var tab = new TabPage("Update Strategy");
            var textBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 12F),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(15)
            };

            var strategy = _daemonSet.Spec?.UpdateStrategy;

            textBox.AppendText("=== Update Strategy ===\n\n");
            textBox.AppendText($"Type: {strategy?.Type ?? "RollingUpdate"}\n\n");

            if (strategy?.Type == "RollingUpdate" && strategy.RollingUpdate != null)
            {
                textBox.AppendText("=== Rolling Update Configuration ===\n");

                var maxUnavailable = strategy.RollingUpdate.MaxUnavailable;
                if (maxUnavailable != null)
                {
                    textBox.AppendText($"Max Unavailable: {maxUnavailable}\n");
                    textBox.AppendText("  (Maximum number of pods that can be unavailable during update)\n\n");
                }
                else
                {
                    textBox.AppendText("Max Unavailable: 1 (default)\n\n");
                }
            }
            else if (strategy?.Type == "OnDelete")
            {
                textBox.AppendText("=== OnDelete Configuration ===\n");
                textBox.AppendText("Pods are only updated when they are manually deleted.\n");
                textBox.AppendText("This allows manual control over the update process.\n\n");
            }

            textBox.AppendText("=== Additional Settings ===\n");
            textBox.AppendText($"Revision History Limit: {_daemonSet.Spec?.RevisionHistoryLimit ?? 10}\n");
            textBox.AppendText($"Min Ready Seconds: {_daemonSet.Spec?.MinReadySeconds ?? 0}\n\n");

            textBox.AppendText("=== DaemonSet Characteristics ===\n");
            textBox.AppendText("• Ensures a pod runs on all (or selected) nodes\n");
            textBox.AppendText("• Automatically adds pods to new nodes\n");
            textBox.AppendText("• Removes pods when nodes are removed\n");
            textBox.AppendText("• Cannot be scaled (runs on all matching nodes)\n");

            tab.Controls.Add(textBox);
            return tab;
        }

        private TabPage CreateVolumesTab()
        {
            var tab = new TabPage("Volumes");
            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };

            grid.Columns.Add("Name", "Volume Name");
            grid.Columns.Add("Type", "Type");
            grid.Columns.Add("Source", "Source");

            var volumes = _daemonSet.Spec?.Template?.Spec?.Volumes;
            if (volumes != null)
            {
                foreach (var volume in volumes)
                {
                    var type = GetVolumeType(volume);
                    var source = GetVolumeSource(volume);
                    grid.Rows.Add(volume.Name, type, source);
                }
            }

            tab.Controls.Add(grid);
            return tab;
        }

        private TabPage CreateMetadataTab()
        {
            var tab = new TabPage("Metadata");
            var textBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 12F),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            var metadata = _daemonSet.Metadata;

            if (metadata?.Labels?.Any() == true)
            {
                textBox.AppendText("=== Labels ===\n");
                foreach (var label in metadata.Labels)
                {
                    textBox.AppendText($"{label.Key}: {label.Value}\n");
                }
                textBox.AppendText("\n");
            }

            if (metadata?.Annotations?.Any() == true)
            {
                textBox.AppendText("=== Annotations ===\n");
                foreach (var annotation in metadata.Annotations)
                {
                    textBox.AppendText($"{annotation.Key}: {annotation.Value}\n");
                }
                textBox.AppendText("\n");
            }

            var podTemplate = _daemonSet.Spec?.Template?.Metadata;
            if (podTemplate?.Labels?.Any() == true)
            {
                textBox.AppendText("=== Pod Template Labels ===\n");
                foreach (var label in podTemplate.Labels)
                {
                    textBox.AppendText($"{label.Key}: {label.Value}\n");
                }
                textBox.AppendText("\n");
            }

            if (podTemplate?.Annotations?.Any() == true)
            {
                textBox.AppendText("=== Pod Template Annotations ===\n");
                foreach (var annotation in podTemplate.Annotations)
                {
                    textBox.AppendText($"{annotation.Key}: {annotation.Value}\n");
                }
                textBox.AppendText("\n");
            }

            if (metadata?.OwnerReferences?.Any() == true)
            {
                textBox.AppendText("=== Owner References ===\n");
                foreach (var owner in metadata.OwnerReferences)
                {
                    textBox.AppendText($"Kind: {owner.Kind}\n");
                    textBox.AppendText($"Name: {owner.Name}\n");
                    textBox.AppendText($"UID: {owner.Uid}\n");
                    textBox.AppendText($"Controller: {owner.Controller}\n\n");
                }
            }

            tab.Controls.Add(textBox);
            return tab;
        }

        private void LoadData()
        {
            // Data is loaded via tabs
        }

        private string GetResourcesInfo(V1ResourceRequirements? resources)
        {
            if (resources == null) return "None";

            var parts = new System.Collections.Generic.List<string>();

            if (resources.Requests != null && resources.Requests.Any())
            {
                var cpu = resources.Requests.ContainsKey("cpu") ? resources.Requests["cpu"].ToString() : null;
                var memory = resources.Requests.ContainsKey("memory") ? resources.Requests["memory"].ToString() : null;
                var req = new System.Collections.Generic.List<string>();
                if (cpu != null) req.Add($"CPU={cpu}");
                if (memory != null) req.Add($"Mem={memory}");
                if (req.Any()) parts.Add("Req: " + string.Join(", ", req));
            }

            if (resources.Limits != null && resources.Limits.Any())
            {
                var cpu = resources.Limits.ContainsKey("cpu") ? resources.Limits["cpu"].ToString() : null;
                var memory = resources.Limits.ContainsKey("memory") ? resources.Limits["memory"].ToString() : null;
                var lim = new System.Collections.Generic.List<string>();
                if (cpu != null) lim.Add($"CPU={cpu}");
                if (memory != null) lim.Add($"Mem={memory}");
                if (lim.Any()) parts.Add("Lim: " + string.Join(", ", lim));
            }

            return parts.Any() ? string.Join(" | ", parts) : "None";
        }

        private string GetVolumeType(V1Volume volume)
        {
            if (volume.ConfigMap != null) return "ConfigMap";
            if (volume.Secret != null) return "Secret";
            if (volume.EmptyDir != null) return "EmptyDir";
            if (volume.PersistentVolumeClaim != null) return "PersistentVolumeClaim";
            if (volume.HostPath != null) return "HostPath";
            if (volume.Nfs != null) return "NFS";
            return "Other";
        }

        private string GetVolumeSource(V1Volume volume)
        {
            if (volume.ConfigMap != null) return volume.ConfigMap.Name ?? "N/A";
            if (volume.Secret != null) return volume.Secret.SecretName ?? "N/A";
            if (volume.EmptyDir != null) return "{}";
            if (volume.PersistentVolumeClaim != null) return volume.PersistentVolumeClaim.ClaimName ?? "N/A";
            if (volume.HostPath != null) return volume.HostPath.Path ?? "N/A";
            if (volume.Nfs != null) return $"{volume.Nfs.Server}:{volume.Nfs.Path}";
            return "N/A";
        }
    }
}
