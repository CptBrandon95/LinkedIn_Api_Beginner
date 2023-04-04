namespace LinkedIn_Api_Beginner.Models
{
    /*
     A query class to handle more advanced data retrievals
     */
    public class QueryParameters
    {
        const int _maxSize = 100;
        private int _size = 50;
        public int Page { get; set; } = 1; // Initializing the page 1 which is our default page
        public int Size 
        {
            get
            { return _size; }
            set 
            { _size = Math.Min(_maxSize, value); }
        }
    }
}
