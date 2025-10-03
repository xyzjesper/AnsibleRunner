using System.CommandLine;
using AnsibleRunner.SubCommands;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        
        var rootCommand = new RootCommand
        {
            Description = "Ansible Runner to simple deploy Docker Compose. Backup Server and help me to work with ansible.",
            Subcommands =
            {
                new ComposeSubCommand()
            }
        };
        
        var parseResult = rootCommand.Parse(args);
        return await parseResult.InvokeAsync();
    }
}