﻿// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Jmelosegui.DevOps.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Jmelosegui.DevOps.Client.Models;
    using Jmelosegui.DevOps.Client.Models.Requests;

    public sealed class GitApiClient : IGitApiClient
    {
        public GitApiClient(IConnection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Gets the underlying connection.
        /// </summary>
        public IConnection Connection { get; private set; }

        /// <summary>
        /// Retrieve git commits for a project.
        /// </summary>
        /// <param name="projectName">Project ID or project name.</param>
        /// <param name="commitListRequest">Payload used in the request.</param>
        /// <returns>Returns an IEnumerable of <see cref="CommitRef"/>.</returns>
        public async Task<IEnumerable<CommitRef>> GetCommits(string projectName, CommitListRequest commitListRequest = null)
        {
            var parameters = new Dictionary<string, object>();

            FluentDictionary.For(parameters)
                            .Add("api-version", "4.1")
                            .Add("searchCriteria.$top", commitListRequest.Top, () => commitListRequest.Top > 0)
                            .Add("searchCriteria.itemVersion.version", commitListRequest.SearchCriteriaVersion, () => !string.IsNullOrEmpty(commitListRequest.SearchCriteriaVersion));

            var endPoint = new Uri($"{projectName}/_apis/git/repositories/{commitListRequest.RepositoryId}/commits", UriKind.Relative);

            var response = await this.Connection.Get<GenericCollectionResponse<CommitRef>>(endPoint, parameters, null)
                               .ConfigureAwait(false);

            return response.Body.Values;
        }
    }
}
