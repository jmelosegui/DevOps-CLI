// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Jmelosegui.DevOpsCLI.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Jmelosegui.DevOpsCLI.ApiClients;
    using McMaster.Extensions.CommandLineUtils;
    using Microsoft.Extensions.Logging;

    [Command("list", Description = "Get a list of variable groups.")]
    public class VariableGroupListCommand : CommandBase
    {
        public VariableGroupListCommand(ILogger<VariableGroupListCommand> logger)
            : base(logger)
        {
        }

        [Required]
        [Option("-p|--project", "Tfs project name", CommandOptionType.SingleValue)]
        public string ProjectName { get; set; }

        protected override int OnExecute(CommandLineApplication app)
        {
            base.OnExecute(app);

            while (string.IsNullOrEmpty(this.ProjectName))
            {
                this.ProjectName = Prompt.GetString("> ProjectName:", null, ConsoleColor.DarkGray);
            }

            var list = this.DevOpsClient.VariableGroup.GetAllAsync(this.ProjectName).Result;

            Console.WriteLine();

            foreach (var variableGroup in list)
            {
                Console.WriteLine($"{variableGroup.Name} ({variableGroup.Id})");
            }

            return ExitCodes.Ok;
        }
    }
}