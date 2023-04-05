namespace LinkedIn_Api_Beginner.Models
{
    public class ProductQueryParameters : QueryParameters
    {
        public decimal? MinPrice  { get; set; } // ?--> makes the MinPrice nullable 
        public decimal? MaxPrice { get; set; }

        // Searching for Items
        public string Name { get; set; } = string.Empty; // Initializing it with an empty string
        public string?  Sku { get; set; } = string.Empty;
    }
}
