using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;
using System.Text.RegularExpressions;
using AnsibleRunner.Helper;
using AnsibleRunner.Models;
using AnsibleRunner.Options.Compose;
using Spectre.Console;

namespace AnsibleRunner.Actions.Compose;

public class DeployComposeAction
{
    public static async Task Run(ParseResult parseResult, UrlOption urlOption)
    {
        var parsedUrlOption = parseResult.GetValue(urlOption);
        if (parsedUrlOption == null)
        {
            AnsiConsole.MarkupLine("[gray]Please parse a --url in.[/]");
        }

        if (parsedUrlOption != null)
        {
            var rec = new Regex("https://.").Match(parsedUrlOption);
            if (!rec.Success)
            {
                AnsiConsole.MarkupLine("[red]Invalid url option specified.[/]");
                return;
            }
        }


        var httpClient = new HttpClient();
        var data = await httpClient.GetAsync(parsedUrlOption);
        if (data.IsSuccessStatusCode)
        {
            var json = JsonSerializer.Deserialize<ComposeConfigModel>(await data.Content.ReadAsStringAsync());

            if (json == null)
            {
                AnsiConsole.MarkupLine("[red]No data returned.[/]");
            }

            AnsiConsole.MarkupLine($"[white]Successfully deployed the project [/]\n");

            foreach (var composeDataObject in json.Data)
            {
                var directory = new DirectoryInfo($"/compose/{composeDataObject.Name}");
                if (!directory.Exists)
                {
                    AnsiConsole.MarkupLine($"[red]Directory {directory.FullName} does not exist.[/]");
                    directory.Create();
                    AnsiConsole.MarkupLine($"[white]Creating new directory [/]\n");
                }

                var curlCommand = await BashHelper.RunCommand(
                    $"cd /compose/{composeDataObject.Name}/ && curl -L {composeDataObject.Url} > docker-compose.yml");

                // var dockerCommand = await BashHelper.RunCommand($"cd /compose/{composeDataObject.Name} && docker compose up -d");

                Console.WriteLine(curlCommand);
                
                AnsiConsole.MarkupLine($"[white]{directory.FullName} deployed.[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]An error occured while deploying the file.[/]");
        }
    }
}