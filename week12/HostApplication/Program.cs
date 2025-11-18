using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using RemotingObjects;

namespace HostApplication
{
    /// <summary>
    /// Host application for .NET Remoting server.
    /// Configures HTTP channel and hosts the ProjectManagerImplement remote object.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Project Management - Host Server";
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║   PROJECT MANAGEMENT SYSTEM - REMOTING HOST SERVER   ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine();

                // Create HTTP channel on port 8080
                HttpChannel channel = new HttpChannel(8080);

                // Register the channel with ChannelServices
                ChannelServices.RegisterChannel(channel, false);
                Console.WriteLine("[INFO] HTTP Channel registered on port 8080");

                // Register the remote object
                // WellKnownObjectMode.Singleton: Single instance serves all clients
                // WellKnownObjectMode.SingleCall: New instance per client call
                RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof(ProjectManagerImplement),
                    "ProjectManager",
                    WellKnownObjectMode.Singleton
                );

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[SUCCESS] ProjectManager remote object registered");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Remote Object URL: http://localhost:8080/ProjectManager");
                Console.WriteLine("Mode: Singleton (shared instance)");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Server is running... Press ENTER to stop.");
                Console.ResetColor();
                Console.WriteLine();

                // Keep the server running
                Console.ReadLine();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[INFO] Shutting down server...");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                       ERROR                            ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Type: {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }
    }
}
