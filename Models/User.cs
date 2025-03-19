using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("users")]
public class User : BaseModel
{
    [PrimaryKey("id", false)]
    public int id { get; set; }
    public string your_name { get; set; }
    public string email { get; set; }
    public string password { get; set; }

}
