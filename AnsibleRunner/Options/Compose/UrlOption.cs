using System.CommandLine;

namespace AnsibleRunner.Options.Compose;

public class UrlOption : Option<string>
{
    
    public UrlOption() : base("--url", [])
    {
        Description = "Foreground color of text displayed on the console.";
        Required = true;
        
    }
}