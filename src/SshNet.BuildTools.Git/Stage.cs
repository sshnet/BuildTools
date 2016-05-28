using System.Linq;
using LibGit2Sharp;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SshNet.BuildTools.Git
{
    /// <summary>
    /// Promotes changes to the staging area of a given Git repository.
    /// </summary>
    public class Stage : Task
    {
        /// <summary>
        /// Gets or sets the working directory of the Git repository.
        /// </summary>
        /// <value>
        /// The working directory of the Git repository.
        /// </value>
        [Required]
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Stages changes in the specified working directory.
        /// </summary>
        public override bool Execute()
        {
            using (var repo = new Repository(WorkingDirectory))
            {
                var status = repo.RetrieveStatus();
                var filePaths = status.Modified.Select(mods => mods.FilePath);
                repo.Stage(filePaths);
            }

            return true;
        }
    }
}
