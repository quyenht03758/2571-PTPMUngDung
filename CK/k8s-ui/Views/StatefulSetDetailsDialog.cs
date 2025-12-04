using k8s.Models;
using System.ComponentModel;
using MaterialSkin.Controls;

namespace K8sManager.Views
{
    /// <summary>
    /// Detailed view dialog for StatefulSet with 6 tabs
    /// </summary>
    [DesignerCategory("Form")]
    public partial class StatefulSetDetailsDialog : MaterialForm
    {
        private readonly V1StatefulSet _statefulSet;
        private readonly Func<V1StatefulSet, System.Threading.Tasks.Task> _onScale;
        private readonly Func<V1StatefulSet, System.Threading.Tasks.Task> _onRestart;
        private readonly Func<V1StatefulSet, System.Threading.Tasks.Task> _onViewPods;

        public StatefulSetDetailsDialog(
            V1StatefulSet statefulSet,
            Func<V1StatefulSet, System.Threading.Tasks.Task> onScale,
            Func<V1StatefulSet, System.Threading.Tasks.Task> onRestart,
            Func<V1StatefulSet, System.Threading.Tasks.Task> onViewPods)
        {
            InitializeComponent();
            
            _statefulSet = statefulSet;
            _onScale = onScale;
            _onRestart = onRestart;
            _onViewPods = onViewPods;

            InitializeComponents();
            LoadData();
        }

        private void InitializeComponents()
        {
            Text = $"StatefulSet: {_statefulSet.Metadata?.Name}";
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

            // Bottom panel with action buttons
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                Padding = new Padding(15, 10, 15, 15)
            };

            var btnScale = new MaterialButton
            {
                Text = "Scale",
                Location = new Point(15, 15),
                AutoSize = true
            };
            btnScale.Click += async (s, e) => await _onScale(_statefulSet).ConfigureAwait(false);

            var btnRestart = new MaterialButton
            {
                Text = "Restart",
                Location = new Point(120, 15),
                AutoSize = true
            };
            btnRestart.Click += async (s, e) => await _onRestart(_statefulSet).ConfigureAwait(false);

            var btnViewPods = new MaterialButton
            {
                Text = "View Pods",
                Location = new Point(225, 15),
                AutoSize = true
            };
            btnViewPods.Click += async (s, e) => await _onViewPods(_statefulSet).ConfigureAwait(false);

            var btnClose = new MaterialButton
            {
                Text = "Close",
                Location = new Point(350, 15),
                AutoSize = true
            };
            btnClose.Click += (s, e) => Close();

            bottomPanel.Controls.AddRange(new Control[] { btnScale, btnRestart, btnViewPods, btnClose });

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

            var spec = _statefulSet.Spec;
            var status = _statefulSet.Status;
            var metadata = _statefulSet.Metadata;

            textBox.AppendText($"Name: {metadata?.Name}\n");
            textBox.AppendText($"Namespace: {metadata?.NamespaceProperty}\n");
            textBox.AppendText($"Service Name: {spec?.ServiceName}\n\n");

            textBox.AppendText("=== Replicas ===\n");
            textBox.AppendText($"Desired: {spec?.Replicas ?? 0}\n");
            textBox.AppendText($"Current: {status?.Replicas ?? 0}\n");
            textBox.AppendText($"Ready: {status?.ReadyReplicas ?? 0}\n");
            textBox.AppendText($"Updated: {status?.UpdatedReplicas ?? 0}\n");
            textBox.AppendText($"Available: {status?.AvailableReplicas ?? 0}\n\n");

            textBox.AppendText("=== Selector ===\n");
            if (spec?.Selector?.MatchLabels != null)
            {
                foreach (var label in spec.Selector.MatchLabels)
                {
                    textBox.AppendText($"  {label.Key}: {label.Value}\n");
                }
            }

            textBox.AppendText("\n=== Pod Management Policy ===\n");
            textBox.AppendText($"Policy: {spec?.PodManagementPolicy ?? "OrderedReady"}\n\n");

            textBox.AppendText("=== Additional Info ===\n");
            textBox.AppendText($"Generation: {metadata?.Generation}\n");
            textBox.AppendText($"Current Revision: {status?.CurrentRevision}\n");
            textBox.AppendText($"Update Revision: {status?.UpdateRevision}\n");
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

            var containers = _statefulSet.Spec?.Template?.Spec?.Containers;
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

            var conditions = _statefulSet.Status?.Conditions;
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

            var strategy = _statefulSet.Spec?.UpdateStrategy;

            textBox.AppendText("=== Update Strategy ===\n\n");
            textBox.AppendText($"Type: {strategy?.Type ?? "RollingUpdate"}\n\n");

            if (strategy?.Type == "RollingUpdate" && strategy.RollingUpdate != null)
            {
                textBox.AppendText("=== Rolling Update Configuration ===\n");
                textBox.AppendText($"Partition: {strategy.RollingUpdate.Partition ?? 0}\n");
                textBox.AppendText("  (Only pods with ordinal >= partition will be updated)\n\n");
            }
            else if (strategy?.Type == "OnDelete")
            {
                textBox.AppendText("=== OnDelete Configuration ===\n");
                textBox.AppendText("Pods are only updated when they are manually deleted.\n");
                textBox.AppendText("This allows manual control over the update process.\n\n");
            }

            textBox.AppendText("=== Additional Settings ===\n");
            textBox.AppendText($"Revision History Limit: {_statefulSet.Spec?.RevisionHistoryLimit ?? 10}\n");
            textBox.AppendText($"Min Ready Seconds: {_statefulSet.Spec?.MinReadySeconds ?? 0}\n");
            textBox.AppendText($"Pod Management Policy: {_statefulSet.Spec?.PodManagementPolicy ?? "OrderedReady"}\n\n");

            textBox.AppendText("=== Pod Management Policies ===\n");
            textBox.AppendText("• OrderedReady: Pods are created/updated in order (0, 1, 2...)\n");
            textBox.AppendText("• Parallel: Pods are created/updated simultaneously\n");

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

            // Pod volumes
            var volumes = _statefulSet.Spec?.Template?.Spec?.Volumes;
            if (volumes != null)
            {
                foreach (var volume in volumes)
                {
                    var type = GetVolumeType(volume);
                    var source = GetVolumeSource(volume);
                    grid.Rows.Add(volume.Name, type, source);
                }
            }

            // Volume Claim Templates (unique to StatefulSet)
            var vctPanel = new Panel { Dock = DockStyle.Bottom, Height = 200, Padding = new Padding(10) };
            var vctLabel = new Label
            {
                Text = "Volume Claim Templates:",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Height = 25
            };

            var vctGrid = new DataGridView
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

            vctGrid.Columns.Add("Name", "Name");
            vctGrid.Columns.Add("StorageClass", "Storage Class");
            vctGrid.Columns.Add("AccessModes", "Access Modes");
            vctGrid.Columns.Add("Storage", "Storage");

            var vcts = _statefulSet.Spec?.VolumeClaimTemplates;
            if (vcts != null)
            {
                foreach (var vct in vcts)
                {
                    var storageClass = vct.Spec?.StorageClassName ?? "default";
                    var accessModes = vct.Spec?.AccessModes != null
                        ? string.Join(", ", vct.Spec.AccessModes)
                        : "N/A";
                    var storage = vct.Spec?.Resources?.Requests?.ContainsKey("storage") == true
                        ? vct.Spec.Resources.Requests["storage"].ToString()
                        : "N/A";

                    vctGrid.Rows.Add(vct.Metadata?.Name, storageClass, accessModes, storage);
                }
            }

            vctPanel.Controls.Add(vctGrid);
            vctPanel.Controls.Add(vctLabel);

            tab.Controls.Add(grid);
            tab.Controls.Add(vctPanel);
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

            var metadata = _statefulSet.Metadata;

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

            var podTemplate = _statefulSet.Spec?.Template?.Metadata;
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
