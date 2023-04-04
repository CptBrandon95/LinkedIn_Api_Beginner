namespace LinkedIn_Api_Beginner.Models
{
    public class ProductQueryParameters : QueryParameters
    {
        public decimal? MinPrice  { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
