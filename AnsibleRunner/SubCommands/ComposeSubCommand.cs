using System.CommandLine;
using AnsibleRunner.Actions.Compose;
using AnsibleRunner.Options.Compose;

namespace AnsibleRunner.SubCommands;

public class ComposeSubCommand : Command
{
    public ComposeSubCommand() : base("compose", "Deploy a .json file for Docker Compose.")
    {
        var urlOption = new UrlOption();
        Options.Add(urlOption);
        SetAction(async (result) => await DeployComposeAction.Run(result, urlOption));
    }
}