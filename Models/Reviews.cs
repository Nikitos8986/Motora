using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("reviews")]
public class Reviews : BaseModel
{
    [PrimaryKey("id", false)]
    public int id { get; set; }
    public int? user_id { get; set; }
    public string title { get; set; }
    public string text { get; set; }
    public string category { get; set; }
    public string estimation { get; set; }
    public bool privates { get; set; }
}
