using System;

namespace RemotingObjects.Models
{
    /// <summary>
    /// Represents a project with Marshal-By-Value semantics for .NET Remoting.
    /// This class is serializable and will be copied when passed across remoting boundaries.
    /// </summary>
    [Serializable]
    public class Project
    {
        /// <summary>
        /// Gets or sets the project number (unique identifier).
        /// </summary>
        public string ProjectNumber { get; set; }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the project location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the project start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the project manager's name.
        /// </summary>
        public string ProjectManager { get; set; }

        /// <summary>
        /// Initializes a new instance of the Project class.
        /// </summary>
        public Project()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Project class with specified values.
        /// </summary>
        public Project(string projectNumber, string projectName, string location,
                      DateTime startDate, string projectManager)
        {
            ProjectNumber = projectNumber;
            ProjectName = projectName;
            Location = location;
            StartDate = startDate;
            ProjectManager = projectManager;
        }
    }
}
