// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Jmelosegui.DevOpsCLI.Commands
{
    using System;
    using System.Collections.Generic;
    using Jmelosegui.DevOpsCLI.ApiClients;
    using Jmelosegui.DevOpsCLI.Models;
    using McMaster.Extensions.CommandLineUtils;
    using Microsoft.Extensions.Logging;

    [Command("list", Description = "Get a list of build definitions.")]
    public class BuildListCommand : CommandBase
    {
        public BuildListCommand(ILogger<BuildListCommand> logger)
            : base(logger)
        {
        }

        [Option("-p|--project", "Tfs project name", CommandOptionType.SingleValue)]
        public string ProjectName { get; set; }

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);

            while (string.IsNullOrEmpty(this.ProjectName))
            {
                this.ProjectName = Prompt.GetString("> ProjectName:", null, ConsoleColor.DarkGray);
            }

            IEnumerable<BuildDefinition> results = this.DevOpsClient.BuildDefinition.GetAllAsync(this.ProjectName).Result;

            Console.WriteLine();

            foreach (var buildDefinitionEntry in results)
            {
                Console.WriteLine($"{buildDefinitionEntry.Name} ({buildDefinitionEntry.Id})");
            }

            return ExitCodes.Ok;
        }
    }
}