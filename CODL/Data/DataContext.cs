using CODL.Models;
using Microsoft.EntityFrameworkCore;

namespace CODL.Data;

public class DataContext : DbContext
{
    public DbSet<AppUser?> AppUsers { get; set; }
    public DbSet<CodewarsUser> CodewarsUsers { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CodewarsUser>().HasData(
            new CodewarsUser  { Username = "walker00" },
            new CodewarsUser  { Username = "Avatarkonlanz" },
            new CodewarsUser {Username = "walker00"},
            new CodewarsUser {Username = "Avatarkonlanz"},
            new CodewarsUser {Username = "Avatarastroash"},
            new CodewarsUser {Username = "aweperi"},
            new CodewarsUser {Username = "EveNyarango"},
            new CodewarsUser {Username = "kwamesarfo"},
            new CodewarsUser {Username = "edwardtsatsu"},
            new CodewarsUser {Username = "randyayittah"},
            new CodewarsUser {Username = "konlan"},
            new CodewarsUser {Username = "ofoe-fiergbor"},
            new CodewarsUser {Username = "Henosagy"},
            new CodewarsUser {Username = "Kpakpo"},
            new CodewarsUser {Username = "Twumasi"},
            new CodewarsUser {Username = "explorer_"},
            new CodewarsUser {Username = "frederick.arthur"},
            new CodewarsUser {Username = "VincentChrisbone"},
            new CodewarsUser {Username = "henosagy"},
            new CodewarsUser {Username = "lameiraatt"},
            new CodewarsUser {Username = "Phinehas"},
            new CodewarsUser {Username = "AlPrince"},
            new CodewarsUser {Username = "dawudmohammed"},
            new CodewarsUser {Username = "Fred16"},
            new CodewarsUser {Username = "JoanaMamley"},
            new CodewarsUser {Username = "theJaneOh"},
            new CodewarsUser {Username = "fytm"},
            new CodewarsUser {Username = "flexninja21"},
            new CodewarsUser {Username = "kkolA17"},
            new CodewarsUser {Username = "deitybounce@blazer"},
            new CodewarsUser {Username = "mills.k"},
            new CodewarsUser {Username = "joy_122"},
            new CodewarsUser {Username = "KwesiJoe"},
            new CodewarsUser {Username = "btdjangbah001"},
            new CodewarsUser {Username = "selorm_l"},
            new CodewarsUser {Username = "georgehans1"},
            new CodewarsUser {Username = "ellieGem"},
            new CodewarsUser {Username = "Regardless"},
            new CodewarsUser {Username = "LindaSeyram"},
            new CodewarsUser {Username = "AlbertPrince"},
            new CodewarsUser {Username = "i-bm"},
            new CodewarsUser {Username = "emmanuel-G"},
            new CodewarsUser {Username = "Jojo-Jowey"},
            new CodewarsUser {Username = "MManuelXI"},
            new CodewarsUser {Username = "mrbauer"},
            new CodewarsUser {Username = "sarpong4"},
            new CodewarsUser {Username = "tkayy"},
            new CodewarsUser {Username = "jsofosu"},
            new CodewarsUser {Username = "ckacquah"},
            new CodewarsUser {Username = "joesunesis"},
            new CodewarsUser {Username = "elvis.segba"},
            new CodewarsUser {Username = "stepheneyt"},
            new CodewarsUser {Username = "afsanat"},
            new CodewarsUser {Username = "iv-tt"},
            new CodewarsUser {Username = "lamptey07"},
            new CodewarsUser {Username = "swithin"},
            new CodewarsUser {Username = "patrova"},
            new CodewarsUser {Username = "kweks45"},
            new CodewarsUser {Username = "emmanuelamet"},
            new CodewarsUser {Username = "YoofiBP"},
            new CodewarsUser {Username = "jeff777"},
            new CodewarsUser {Username = "hussein6065"},
            new CodewarsUser {Username = "richkode"},
            new CodewarsUser {Username = "attara"},
            new CodewarsUser {Username = "wun_butias"},
            new CodewarsUser {Username = "michael.essien"},
            new CodewarsUser {Username = "revup.a"},
            new CodewarsUser {Username = "afwcole"},
            new CodewarsUser {Username = "Audrey Mengue"},
            new CodewarsUser {Username = "Caleb_Fianu"},
            new CodewarsUser {Username = "Kickif"},
            new CodewarsUser {Username = "jojoe-ainoo"},
            new CodewarsUser {Username = "nathaniel.assan"},
            new CodewarsUser {Username = "aiks_code"}
        );
    }
}