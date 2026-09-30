namespace CinemaCatalog.Infrastructure.Firebase;

public class FirebaseSettings
{
    public const string SectionName = "Firebase";

    public string ProjectId { get; set; } = string.Empty;
    
    public string CredentialsPath { get; set; } = string.Empty;
}