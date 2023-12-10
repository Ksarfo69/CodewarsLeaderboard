using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CODL.Models;

public class Comment
{
    private readonly Guid _id = Guid.NewGuid();
    [Key]   
    public Guid Id
    {
        get { return _id;}
        private set { }
    }
    [Required]
    public string Text { get; set; }
    [Required]
    public AppUser Commenter { get; set; }
    [Required]
    public CodewarsUser Commentee { get; set; }
    public readonly DateTime _createdAt = DateTime.Now;
    public DateTime CreatedAt { get { return _createdAt; }}
    
    public CommentResponse ToResponse()
    {
        return new CommentResponse
        {
            CommenteeUsername = Commentee.Username,
            CommenterUsername = Commenter.Username,
            Text = Text,
            CreatedAt = CreatedAt
        };
    }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class CommentAdd
{
    public string CommenteeUsername { get; set; }
    public string Text { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class CommentResponse
{
    public string CommenterUsername { get; set; }
    public string CommenteeUsername { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}