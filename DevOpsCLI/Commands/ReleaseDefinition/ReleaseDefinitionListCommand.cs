// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Jmelosegui.DevOpsCLI.Commands
{
    using System;
    using System.Collections.Generic;
    using McMaster.Extensions.CommandLineUtils;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    [Command("list", Description = "Get a list of release definitions.")]
    public class ReleaseDefinitionListCommand : CommandBase
    {
        public ReleaseDefinitionListCommand(ILogger<ReleaseDefinitionListCommand> logger)
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

            IEnumerable<Models.ReleaseDefinition> releaseDefinitions = this.DevOpsClient.ReleaseDefinition.GetAllAsync(this.ProjectName).Result;

            Console.WriteLine();

            Console.WriteLine(JsonConvert.SerializeObject(releaseDefinitions, Formatting.Indented));

            return ExitCodes.Ok;
        }
    }
}
