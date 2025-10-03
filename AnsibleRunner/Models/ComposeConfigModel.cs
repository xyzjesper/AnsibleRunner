namespace AnsibleRunner.Models;

public class ComposeConfigModel
{
    public ComposeDataObject[] Data { get; set; }
}

public class ComposeDataObject
{
    public string Name { get; set; }
    public string Url { get; set; }
}