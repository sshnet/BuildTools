using LibGit2Sharp;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SshNet.BuildTools.Git
{
    public class Push : Task
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
        /// Gets or sets the username and password credentials to use for pushing to the remote repository.
        /// </summary>
        /// <value>
        /// The username and password credentials to use for pushing to the remote repository.
        /// </value>
        public ITaskItem Credentials { get; set; }

        public override bool Execute()
        {
            using (var repo = new Repository(WorkingDirectory))
            {
                repo.Network.Push(repo.Head, CreatePushOptions());
            }

            return true;
        }

        private PushOptions CreatePushOptions()
        {
            if (Credentials == null)
                return new PushOptions();

            var userName = Credentials.ItemSpec;
            var password = Credentials.GetMetadata("Password");
            return new PushOptions
            {
                CredentialsProvider =
                    (url, userNameFromUrl, types) =>
                        new UsernamePasswordCredentials {Username = userName, Password = password}
            };
        }
    }
}
