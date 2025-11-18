using System;
using System.Collections.Generic;
using System.Linq;
using RemotingObjects.Models;

namespace RemotingObjects
{
    /// <summary>
    /// Implementation of IProjectManager with Marshal-By-Reference semantics.
    /// This class inherits from MarshalByRefObject to enable .NET Remoting.
    /// </summary>
    public class ProjectManagerImplement : MarshalByRefObject, IProjectManager
    {
        private readonly List<Project> _projects;

        /// <summary>
        /// Initializes a new instance of the ProjectManagerImplement class.
        /// Populates the project collection with sample data.
        /// </summary>
        public ProjectManagerImplement()
        {
            // Initialize with sample project data
            _projects = new List<Project>
            {
                new Project("PRJ001", "E-Commerce Platform Development", "Ha Noi",
                           new DateTime(2024, 1, 15), "Nguyen Van A"),
                new Project("PRJ002", "Mobile Banking Application", "Ho Chi Minh",
                           new DateTime(2024, 2, 1), "Tran Thi B"),
                new Project("PRJ003", "Warehouse Management System", "Da Nang",
                           new DateTime(2024, 1, 20), "Le Van C"),
                new Project("PRJ004", "Customer Relationship Management", "Ha Noi",
                           new DateTime(2024, 3, 10), "Pham Thi D"),
                new Project("PRJ005", "Online Learning Platform", "Ho Chi Minh",
                           new DateTime(2024, 2, 15), "Hoang Van E"),
                new Project("PRJ006", "Hotel Booking System", "Da Nang",
                           new DateTime(2024, 4, 1), "Nguyen Thi F"),
                new Project("PRJ007", "Inventory Management Tool", "Ha Noi",
                           new DateTime(2024, 3, 5), "Tran Van G"),
                new Project("PRJ008", "Healthcare Management System", "Can Tho",
                           new DateTime(2024, 2, 20), "Le Thi H"),
                new Project("PRJ009", "Real Estate Portal", "Ho Chi Minh",
                           new DateTime(2024, 4, 15), "Pham Van I"),
                new Project("PRJ010", "Supply Chain Analytics", "Ha Noi",
                           new DateTime(2024, 3, 25), "Hoang Thi K")
            };
        }

        /// <summary>
        /// Retrieves all projects from the data source.
        /// </summary>
        /// <returns>An array containing all projects.</returns>
        public Project[] GetAllProjects()
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAllProjects() called - Returning {_projects.Count} projects");
            return _projects.ToArray();
        }

        /// <summary>
        /// Retrieves all projects filtered by location.
        /// </summary>
        /// <param name="location">The location to filter by.</param>
        /// <returns>An array containing all projects at the specified location.</returns>
        public Project[] GetAllProjectsByLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAllProjectsByLocation() called with empty location - Returning all projects");
                return GetAllProjects();
            }

            var filtered = _projects
                .Where(p => p.Location.Equals(location, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetAllProjectsByLocation('{location}') called - Returning {filtered.Length} projects");
            return filtered;
        }

        /// <summary>
        /// Adds a new project to the data source.
        /// </summary>
        /// <param name="project">The project to add.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool AddProject(Project project)
        {
            if (project == null)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] AddProject() called with null project");
                return false;
            }

            // Check if project number already exists
            if (_projects.Any(p => p.ProjectNumber == project.ProjectNumber))
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] AddProject() failed - Project {project.ProjectNumber} already exists");
                return false;
            }

            _projects.Add(project);
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] AddProject() - Added project {project.ProjectNumber}: {project.ProjectName}");
            return true;
        }

        /// <summary>
        /// Updates an existing project in the data source.
        /// </summary>
        /// <param name="project">The project with updated information.</param>
        /// <returns>True if successful, false if project not found.</returns>
        public bool UpdateProject(Project project)
        {
            if (project == null)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] UpdateProject() called with null project");
                return false;
            }

            var existingProject = _projects.FirstOrDefault(p => p.ProjectNumber == project.ProjectNumber);
            if (existingProject == null)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] UpdateProject() failed - Project {project.ProjectNumber} not found");
                return false;
            }

            // Update properties
            existingProject.ProjectName = project.ProjectName;
            existingProject.Location = project.Location;
            existingProject.StartDate = project.StartDate;
            existingProject.ProjectManager = project.ProjectManager;

            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] UpdateProject() - Updated project {project.ProjectNumber}");
            return true;
        }

        /// <summary>
        /// Deletes a project from the data source.
        /// </summary>
        /// <param name="projectNumber">The project number to delete.</param>
        /// <returns>True if successful, false if project not found.</returns>
        public bool DeleteProject(string projectNumber)
        {
            if (string.IsNullOrWhiteSpace(projectNumber))
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] DeleteProject() called with empty project number");
                return false;
            }

            var project = _projects.FirstOrDefault(p => p.ProjectNumber == projectNumber);
            if (project == null)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] DeleteProject() failed - Project {projectNumber} not found");
                return false;
            }

            _projects.Remove(project);
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] DeleteProject() - Deleted project {projectNumber}: {project.ProjectName}");
            return true;
        }

        /// <summary>
        /// Gets a single project by its project number.
        /// </summary>
        /// <param name="projectNumber">The project number to search for.</param>
        /// <returns>The project if found, null otherwise.</returns>
        public Project GetProjectByNumber(string projectNumber)
        {
            if (string.IsNullOrWhiteSpace(projectNumber))
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetProjectByNumber() called with empty project number");
                return null;
            }

            var project = _projects.FirstOrDefault(p => p.ProjectNumber == projectNumber);
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] GetProjectByNumber('{projectNumber}') - {(project != null ? "Found" : "Not found")}");
            return project;
        }
    }
}
