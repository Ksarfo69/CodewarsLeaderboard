using System.Text.Json;

namespace CODL.Models;

public class HonorRanking
{
    public string username {get; set;}
    public string name {get; set;}
    public string clan {get; set;}
    public int honor {get; set;}
    public string rank {get; set;}
    public IEnumerable<string> languages {get; set;}
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class OverallLanguagesRanking
{
    public IEnumerable<ScoreRanking> scores {get; set;}
    public ISet<string> languages {get; set;}
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class LanguageRanking
{
    public string username {get; set;}
    public string name {get; set;}
    public string clan {get; set;}
    public int score {get; set;}
    public string rank {get; set;}
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class Stats 
{
    public int rank {get; set;}
    public string name {get; set;} //e.g 4 kyu 
    public string color {get; set;}
    public int score {get; set;}
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
} 

public class ScoreRanking 
{
    public string username {get; set;}
    public string name {get; set;}
    public string clan {get; set;}
    public int score {get; set;}
    public string rank {get; set;}
    public IEnumerable<string> languages {get; set;}
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class Ranks 
{
    public Stats overall {get; set;}
    public IDictionary<string, Stats> languages {get; set;}
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}