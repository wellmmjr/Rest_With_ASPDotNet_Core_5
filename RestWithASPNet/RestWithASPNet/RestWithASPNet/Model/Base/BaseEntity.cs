using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNet.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
