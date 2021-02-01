using System.ComponentModel.DataAnnotations.Schema;

namespace APIAspNetCore5.Model
{
    [Table("users")]
    public class User
    {
        [Column("ID")]
        public long? Id { get; set; }

        [Column("Login")]
        public string Login { get; set; }

        [Column("AccessKey")]
        public string AccessKey { get; set; }
    }
}
