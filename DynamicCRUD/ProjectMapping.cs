public class ProjectMapping
{
    public List<Project>? Projects { get; set; }
}

public class Project
{
    public string? DatabaseName { get; set; }
    public Namespaces? Namespaces { get; set; }
    public Folders? Folders { get; set; }
}

public class Namespaces
{
    public string? RazorNamespace { get; set; }
    public string? DtoNamespace { get; set; }
    public string? DataServiceNamespace { get; set; }
    public string? RepositoryNamespace { get; set; }
}

public class Folders
{
    public string? RazorFolder { get; set; }
    public string? DtoFolder { get; set; }
    public string? DataServiceFolder { get; set; }
    public string? RepositoryFolder { get; set; }
}