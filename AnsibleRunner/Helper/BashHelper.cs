using System.Diagnostics;

namespace AnsibleRunner.Helper;

public class BashHelper
{
    public static async Task<string> RunCommand(string command)
    {
        var process = new Process();

        var processStartInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = @"/bin/bash",
            Arguments = $"-c \"{command}\"",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        process.StartInfo = processStartInfo;
        process.Start();
        await process.WaitForExitAsync();

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        return string.IsNullOrEmpty(error) ? output : error;
    }
}