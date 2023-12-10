using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CODL.Models;

public class CodewarsUser
{
    private readonly Guid _id = Guid.NewGuid();
    [Key]   
    public Guid Id
    {
        get { return _id;}
        private set { }
    }
    [Required]
    public string Username { get; set; }
    public readonly DateTime _createdAt = DateTime.Now;
    public DateTime CreatedAt { get { return _createdAt; }}
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class CodeChallengeStats
{
    public uint totalAuthored { get; set; }
    public uint totalCompleted { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class CodewarsUserInfo
{
    public string id { get; set; }
    public string username { get; set; }
    public string name { get; set; }
    public int honor { get; set; }
    public string clan { get; set; }
    public IEnumerable<string> skills { get; set; }
    public Ranks ranks { get; set; }
    public CodeChallengeStats codeChallenges { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class CodewarsUserResponse : CodewarsUserInfo
{
    public IEnumerable<CommentResponse> comments { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}