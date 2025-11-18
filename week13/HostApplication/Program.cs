using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace HostApplication
{
    /// <summary>
    /// Console host application for the Social Insurance WCF Service.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PrintHeader();

            ServiceHost host = null;

            try
            {
                // Create service host
                host = new ServiceHost(typeof(SocialInsuranceService.SocialInsuranceServiceImpl));

                // Open the service
                host.Open();

                Console.WriteLine("[INFO] Service host started successfully");
                Console.WriteLine($"[SUCCESS] Service is now listening on the configured endpoints\n");

                // Display endpoints
                Console.WriteLine("Configured Endpoints:");
                foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
                {
                    Console.WriteLine($"  - Address: {endpoint.Address}");
                    Console.WriteLine($"    Binding: {endpoint.Binding.Name}");
                    Console.WriteLine($"    Contract: {endpoint.Contract.Name}\n");
                }

                Console.WriteLine("═══════════════════════════════════════════════════════════");
                Console.WriteLine("Server is running... Press ENTER to stop.");
                Console.WriteLine("═══════════════════════════════════════════════════════════\n");

                try
                {
                    Console.ReadLine();
                }
                catch (InvalidOperationException)
                {
                    // Running in background mode - keep server alive
                    System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                }

                Console.WriteLine("\n[INFO] Shutting down server...");
                host.Close();
                Console.WriteLine("[SUCCESS] Server stopped successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] Failed to start service host");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"\nStack Trace:\n{ex.StackTrace}");

                if (host != null && host.State == CommunicationState.Faulted)
                {
                    host.Abort();
                }
            }

            try
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            catch (InvalidOperationException)
            {
                // Running in background - cannot read key, just exit
            }
        }

        static void PrintHeader()
        {
            try
            {
                Console.Clear();
            }
            catch (System.IO.IOException)
            {
                // Console.Clear() fails when output is redirected or no console is available
                // This is expected when running in background mode - just skip clearing
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║      SOCIAL INSURANCE MANAGEMENT - WCF HOST SERVER       ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
