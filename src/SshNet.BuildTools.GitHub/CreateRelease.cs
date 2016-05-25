using Microsoft.Build.Framework;
using Octokit;
using Task = Microsoft.Build.Utilities.Task;

namespace SshNet.BuildTools.GitHub
{
    public class CreateRelease : Task
    {
        /// <summary>
        /// Gets or sets the username and password credentials to use.
        /// </summary>
        /// <value>
        /// The username and password credentials to use.
        /// </value>
        public ITaskItem Credentials { get; set; }

        /// <summary>
        /// Gets or sets the repository's owner.
        /// </summary>
        /// <value>
        /// The repository's owner.
        /// </value>
        [Required]
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the name of the repository.
        /// </summary>
        /// <value>
        /// The name of the repository.
        /// </value>
        [Required]
        public string Repository { get; set; }

        [Required]
        public ITaskItem Release { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the release.
        /// </summary>
        /// <value>
        /// The identifier of the release.
        /// </value>
        [Output]
        public int Id { get; set; }

        public override bool Execute()
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue(GetType().Assembly.GetName().Name))
            {
                Credentials = CreateCredentials()
            };

            var release = client.Repository.Release.Create(Owner, Repository, CreateNewRelease()).Result;
            Id = release.Id;

            return true;
        }

        private Credentials CreateCredentials()
        {
            if (Credentials == null)
                return null;

            var userName = Credentials.ItemSpec;
            var password = Credentials.GetMetadata("Password");
            return new Credentials(userName, password);
        }

        private NewRelease CreateNewRelease()
        {
            var name = Release.ItemSpec;
            var tag = Release.GetMetadata("TagName");

            var newRelease = new NewRelease(tag) {Name = name};

            var targetCommitSha = Release.GetMetadata("CommitSha");
            if (!string.IsNullOrEmpty(targetCommitSha))
                newRelease.TargetCommitish = targetCommitSha;

            var prerelease = Release.GetMetadata("Prerelease");
            if (!string.IsNullOrEmpty(prerelease))
                newRelease.Prerelease = bool.Parse(prerelease);

            var draft = Release.GetMetadata("Draft");
            if (!string.IsNullOrEmpty(draft))
                newRelease.Draft = bool.Parse(draft);

            return newRelease;
        }
    }
}
