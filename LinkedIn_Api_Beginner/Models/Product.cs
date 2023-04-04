using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinkedIn_Api_Beginner.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Sku { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvaiable { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [JsonIgnore] // --> telling the system not to serialize it 
        public virtual Category? Category { get; set; }
    }
}
