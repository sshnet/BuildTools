using System;
using LibGit2Sharp;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SshNet.BuildTools.Git
{
    /// <summary>
    /// Commits changes in a given Git repository.
    /// </summary>
    public class Commit : Task
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
        /// Gets or sets the commit message.
        /// </summary>
        /// <value>
        /// The commit message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the full SHA-1 object name of the commit.
        /// </summary>
        /// <value>
        /// The full SHA-1 object name of the commit.
        /// </value>
        [Output]
        public string Sha1 { get; set; }

        /// <summary>
        /// Commits changes in the specified repository.
        /// </summary>
        public override bool Execute()
        {
            using (var repo = new Repository(WorkingDirectory))
            {
                Signature committer, author = committer = repo.Config.BuildSignature(DateTimeOffset.Now);

                var commit = repo.Commit(Message, author, committer);
                Sha1 = commit.Sha;
            }

            return true;
        }
    }
}
