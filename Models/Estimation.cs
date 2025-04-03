using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("estimation")]
public class Estimation : BaseModel
{
    internal User? User;

    [PrimaryKey("id", false)]
    public int id { get; set; }
    public int? user_id { get; set; }
    public string estimation { get; set; }
    public string type { get; set; }
    public string category { get; set; }
    public string reason { get; set; }

}
