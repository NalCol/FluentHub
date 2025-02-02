namespace FluentHub.Octokit.Models.v4
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Autogenerated input type of DeleteProjectV2Workflow
    /// </summary>
    public class DeleteProjectV2WorkflowInput
    {
        /// <summary>
        /// The ID of the workflow to be removed.
        /// </summary>
        public ID WorkflowId { get; set; }

        /// <summary>
        /// A unique identifier for the client performing the mutation.
        /// </summary>
        public string ClientMutationId { get; set; }
    }
}