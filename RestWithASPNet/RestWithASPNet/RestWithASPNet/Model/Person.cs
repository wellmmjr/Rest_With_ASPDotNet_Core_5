using RestWithASPNet.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNet.Model
{

    [Table("person")]
    public class Person : BaseEntity
    {        
        [Column("first_name")]
        public string FirstName { get; set; }
        
        [Column("second_name")]
        public string SecondNamed { get; set; }
        
        [Column("address")]
        public string Address { get; set; }
        
        [Column("gender")]
        public string Gender { get; set; }
        
        [Column("enabled")]
        public bool Enabled { get; set; }
    }
}
