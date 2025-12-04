using MaterialSkin.Controls;

using System.ComponentModel;
namespace K8sManager.Views;

/// <summary>
/// Dialog for viewing Pod logs
/// </summary>
[DesignerCategory("Form")]
public partial class LogsDialog : MaterialForm
{
    private TextBox _txtLogs = null!;
    private MaterialButton _btnRefresh = null!;
    private MaterialButton _btnClose = null!;
    private ComboBox _cmbContainer = null!;
    private NumericUpDown _numTailLines = null!;
    private Panel _topPanel = null!;
    private Label _lblContainer = null!;
    private Label _lblTailLines = null!;
    
    private readonly Func<string?, int, Task<string>> _loadLogsFunc;

    public LogsDialog(string podName, string[] containers, Func<string?, int, Task<string>> loadLogsFunc)
    {
        InitializeComponent();
        
        _loadLogsFunc = loadLogsFunc;

        Text = $"Logs - {podName}";
        
        _cmbContainer.Items.Add("(All)");
        foreach (var c in containers)
            _cmbContainer.Items.Add(c);
        _cmbContainer.SelectedIndex = 0;

        _btnRefresh.Click += async (s, e) => await LoadLogsAsync().ConfigureAwait(false);
        _btnClose.Click += (s, e) => Close();

        Load += async (s, e) => await LoadLogsAsync().ConfigureAwait(false);
    }

    private async Task LoadLogsAsync()
    {
        try
        {
            _btnRefresh.Enabled = false;
            _txtLogs.Text = "Loading logs...";

            var container = _cmbContainer.SelectedIndex == 0
                ? null
                : _cmbContainer.SelectedItem?.ToString();
            var tailLines = (int)_numTailLines.Value;

            var logs = await _loadLogsFunc(container, tailLines).ConfigureAwait(false);
            _txtLogs.Text = logs;

            // Scroll to bottom
            _txtLogs.SelectionStart = _txtLogs.Text.Length;
            _txtLogs.ScrollToCaret();
        }
        catch (Exception ex)
        {
            _txtLogs.Text = $"Error loading logs:\n{ex.Message}";
        }
        finally
        {
            _btnRefresh.Enabled = true;
        }
    }
}
