// Program.cs
using System.Configuration;
using System.Text.Json;
using K8sManager.Controllers;     // CleanExplorerController
// Config & infra
using K8sManager.Infrastructure;  // K8sClientFactory, DapperConnectionFactory
// Services & UI
using K8sManager.Services;        // K8sService, K8sExplorerService, SessionService, SettingsService, AuditService
using K8sManager.Views;           // MainViewLens, LoginView

namespace K8sManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var logFile = Path.Combine(AppContext.BaseDirectory, "error.log");

            try
            {
                File.WriteAllText(logFile, $"=== APPLICATION START ===\nTime: {DateTime.Now}\n\n");

                // Register global exception handlers
                Application.ThreadException += (s, e) =>
                {
                    File.AppendAllText(logFile, $"\n=== ThreadException ===\n{e.Exception}\n");
                    MessageBox.Show($"Thread Exception:\n{e.Exception.Message}\n\nSee error.log for details",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    File.AppendAllText(logFile, $"\n=== UnhandledException ===\n{e.ExceptionObject}\n");
                    MessageBox.Show($"Unhandled Exception:\n{e.ExceptionObject}\n\nSee error.log for details",
                        "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };

                File.AppendAllText(logFile, "Exception handlers registered\n");

                ApplicationConfiguration.Initialize();
                File.AppendAllText(logFile, "ApplicationConfiguration.Initialize() completed\n");

                // ---- 1) Load appsettings.json (nếu thiếu thì dùng default) ----
                AppConfig cfg;
                try
                {
                    var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                    if (File.Exists(path))
                    {
                        var json = File.ReadAllText(path);
                        cfg = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                        File.AppendAllText(logFile, "Config loaded from appsettings.json\n");
                    }
                    else
                    {
                        cfg = new AppConfig(); // fallback
                        File.AppendAllText(logFile, "Using default config (appsettings.json not found)\n");
                    }
                }
                catch (Exception ex)
                {
                    File.AppendAllText(logFile, $"Config load error: {ex}\n");
                    MessageBox.Show($"Config load error: {ex.Message}\nUsing defaults.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cfg = new AppConfig();
                }

                string kubePath = cfg.Kubernetes.KubeconfigPath;
                if (string.IsNullOrWhiteSpace(kubePath))
                {
                    kubePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        ".kube",
                        "config"
                    );
                }
                File.AppendAllText(logFile, $"Kubeconfig path: {kubePath}\n");

                // Check if kubeconfig exists
                if (!File.Exists(kubePath))
                {
                    File.AppendAllText(logFile, "WARNING: Kubeconfig file not found\n");
                    MessageBox.Show($"Kubeconfig file not found at:\n{kubePath}\n\nThe application will start but may not connect to any cluster.",
                        "Kubeconfig Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                File.AppendAllText(logFile, "Creating database connection factory...\n");
                // ---- 2) Khởi tạo Database connection factory ----
                // Đọc connection string từ App.config
                string? connectionString = null;
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings["CompanyDb"]?.ConnectionString;
                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        File.AppendAllText(logFile, "Connection string 'CompanyDb' not found in App.config, trying appsettings.json...\n");
                        connectionString = cfg.ConnectionStrings?.Default;
                    }
                }
                catch (Exception configEx)
                {
                    File.AppendAllText(logFile, $"Error reading App.config: {configEx.Message}\n");
                    connectionString = cfg.ConnectionStrings?.Default;
                }

                // Fallback to default if still empty
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=K8sManager;Integrated Security=True;TrustServerCertificate=True";
                    File.AppendAllText(logFile, "Using default connection string\n");
                }

                File.AppendAllText(logFile, $"Connection string: {connectionString}\n");
                var dbFactory = new DapperConnectionFactory(connectionString);
                File.AppendAllText(logFile, "Database factory created\n");

                // ---- TEST DATABASE CONNECTION ----
                File.AppendAllText(logFile, "Testing database connection...\n");
                try
                {
                    using (var testConn = dbFactory.Create())
                    {
                        testConn.Open();
                        File.AppendAllText(logFile, "Database connection successful!\n");
                        testConn.Close();
                    }
                }
                catch (Exception dbEx)
                {
                    var errorMsg = $"❌ DATABASE CONNECTION FAILED!\n\n" +
                                   $"Connection String:\n{connectionString}\n\n" +
                                   $"Error:\n{dbEx.Message}\n\n" +
                                   $"Possible causes:\n" +
                                   $"1. SQL Server LocalDB is not installed or not running\n" +
                                   $"2. Database 'K8sManager' does not exist\n" +
                                   $"3. Connection string is incorrect\n\n" +
                                   $"Solutions:\n" +
                                   $"1. Start SQL Server: sqllocaldb start MSSQLLocalDB\n" +
                                   $"2. Create database: Run Database/create_tables.sql\n" +
                                   $"3. Check appsettings.json connection string\n\n" +
                                   $"See error.log for details.";

                    File.AppendAllText(logFile, $"Database connection error: {dbEx}\n");
                    MessageBox.Show(errorMsg, "Database Connection Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                File.AppendAllText(logFile, "Creating API client...\n");
                // ---- 3) Khởi tạo API Client (SOA Architecture) ----
                var apiBaseUrl = "http://localhost:5000"; // Default API URL
                var apiClient = new ApiClient(apiBaseUrl);
                File.AppendAllText(logFile, $"API Client created (BaseUrl: {apiBaseUrl})\n");

                File.AppendAllText(logFile, "Creating API services...\n");
                // ---- 4) Khởi tạo API Services (wrappers around ApiClient) ----
                var sessionService = new ApiSessionService(apiClient);
                var auditService = new ApiAuditService(apiClient);
                var templateService = new ApiTemplateService(apiClient);
                var favoriteService = new ApiFavoriteService(apiClient);
                var settingsService = new ApiSettingsService(apiClient);
                File.AppendAllText(logFile, "API Services created\n");

                // ---- 5) Show login form ----
                File.AppendAllText(logFile, "Showing login dialog...\n");
                string? sessionToken = null;
                string? currentUsername = null;
                int? currentUserId = null;
                using (var loginForm = new SimpleLoginView(sessionService))
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        File.AppendAllText(logFile, "Login cancelled by user\n");
                        return; // User cancelled login
                    }

                    // Get session info from ApiSessionService
                    sessionToken = sessionService.CurrentUser?.SessionToken;
                    currentUserId = sessionService.CurrentUser?.UserId;
                    currentUsername = sessionService.CurrentUser?.User?.Username;

                    File.AppendAllText(logFile, $"Login successful. User ID: {currentUserId}, Username: {currentUsername}\n");
                    File.AppendAllText(logFile, $"User ID: {currentUserId}\n");
                }
                File.AppendAllText(logFile, $"Login successful for user: {currentUsername}\n");

                File.AppendAllText(logFile, "Creating K8s factory and services...\n");
                // ---- 6) Khởi tạo K8s client & services ----
                var k8sFactory = new K8sClientFactory(kubePath, cfg.Kubernetes.ContextName);
                var k8sService = new K8sService(k8sFactory);
                var explorerService = new K8sExplorerService(k8sFactory);
                var shellService = new K8sShellService(k8sFactory);  // Add shell service
                File.AppendAllText(logFile, "K8s services created successfully\n");

                File.AppendAllText(logFile, "Creating MainViewLens...\n");
                // ---- 7) Khởi chạy Lens-like UI with services ----
                var lens = new MainViewLens();
                lens.SessionToken = sessionToken;
                lens.CurrentUsername = currentUsername;
                lens.SessionService = sessionService;
                lens.AuditService = auditService;
                lens.SettingsService = settingsService;
                lens.TemplateService = templateService;
                lens.FavoriteService = favoriteService;
                lens.K8sExplorerService = explorerService;
                lens.ShellService = shellService;  // Add shell service to view
                File.AppendAllText(logFile, "MainViewLens created successfully\\n");

                // Create CleanExplorerController (not ExplorerController - that's the old one)
                File.AppendAllText(logFile, "Creating CleanExplorerController...\n");
                var controller = new CleanExplorerController(
                    lens,
                    k8sService,
                    explorerService,
                    k8sFactory,
                    auditService
                );
                if (currentUserId.HasValue)
                {
                    controller.SetCurrentUser(currentUserId.Value);
                }

                // CRITICAL FIX: Assign controller to view
                lens.Controller = controller;

                File.AppendAllText(logFile, "ExplorerController created successfully\n");

                // Initialize auto-refresh feature
                File.AppendAllText(logFile, "Initializing auto-refresh...\n");
                lens.InitializeAutoRefresh();
                File.AppendAllText(logFile, "Auto-refresh initialized\n");

                // Initialize in background BEFORE showing the form
                File.AppendAllText(logFile, "Starting background initialization...\n");
                System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        File.AppendAllText(logFile, "Calling InitializeAsync...\n");
                        await controller.InitializeAsync().ConfigureAwait(false);
                        File.AppendAllText(logFile, "InitializeAsync completed successfully\n");
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText(logFile, $"InitAsync error: {ex}\n");
                        try
                        {
                            lens.Invoke(() =>
                            {
                                MessageBox.Show(lens,
                                    $"Failed to load Kubernetes data:\n{ex.Message}\n\nThe application will continue but may not show cluster data.",
                                    "Initialization Warning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            });
                        }
                        catch { }
                    }
                });

                File.AppendAllText(logFile, "Starting Application.Run...\n");
                Application.Run(lens);
                File.AppendAllText(logFile, "Application.Run completed\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, $"Fatal error: {ex}\n");
                MessageBox.Show($"Fatal application error:\n{ex.Message}\n\nStack trace:\n{ex.StackTrace}\n\nSee error.log for details.",
                    "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}
