namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// An object which can have its data claimed or claim data from another.
    /// </summary>
    public class Claimable
    {
        /// <summary>
        /// A placeholder user for attribution of imported data on GitHub.
        /// </summary>
        public Mannequin Mannequin { get; set; }

        /// <summary>
        /// A user is an individual's account on GitHub that owns repositories and can make new content.
        /// </summary>
        public User User { get; set; }
    }
}