using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Model.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Order between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
