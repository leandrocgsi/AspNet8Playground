using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
