using RemotingObjects.Models;

namespace RemotingObjects
{
    /// <summary>
    /// Interface for project management operations exposed via .NET Remoting.
    /// Implementations should inherit from MarshalByRefObject for Marshal-By-Reference semantics.
    /// </summary>
    public interface IProjectManager
    {
        /// <summary>
        /// Retrieves all projects from the data source.
        /// </summary>
        /// <returns>An array containing all projects.</returns>
        Project[] GetAllProjects();

        /// <summary>
        /// Retrieves all projects filtered by location.
        /// </summary>
        /// <param name="location">The location to filter by.</param>
        /// <returns>An array containing all projects at the specified location.</returns>
        Project[] GetAllProjectsByLocation(string location);

        /// <summary>
        /// Adds a new project to the data source.
        /// </summary>
        /// <param name="project">The project to add.</param>
        /// <returns>True if successful, false otherwise.</returns>
        bool AddProject(Project project);

        /// <summary>
        /// Updates an existing project in the data source.
        /// </summary>
        /// <param name="project">The project with updated information.</param>
        /// <returns>True if successful, false if project not found.</returns>
        bool UpdateProject(Project project);

        /// <summary>
        /// Deletes a project from the data source.
        /// </summary>
        /// <param name="projectNumber">The project number to delete.</param>
        /// <returns>True if successful, false if project not found.</returns>
        bool DeleteProject(string projectNumber);

        /// <summary>
        /// Gets a single project by its project number.
        /// </summary>
        /// <param name="projectNumber">The project number to search for.</param>
        /// <returns>The project if found, null otherwise.</returns>
        Project GetProjectByNumber(string projectNumber);
    }
}
