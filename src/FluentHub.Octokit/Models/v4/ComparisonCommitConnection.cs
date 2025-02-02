namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// The connection type for Commit.
    /// </summary>
    public class ComparisonCommitConnection
    {
        /// <summary>
        /// The total count of authors and co-authors across all commits.
        /// </summary>
        public int AuthorCount { get; set; }

        /// <summary>
        /// A list of edges.
        /// </summary>
        public List<CommitEdge> Edges { get; set; }

        /// <summary>
        /// A list of nodes.
        /// </summary>
        public List<Commit> Nodes { get; set; }

        /// <summary>
        /// Information to aid in pagination.
        /// </summary>
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// Identifies the total count of items in the connection.
        /// </summary>
        public int TotalCount { get; set; }
    }
}