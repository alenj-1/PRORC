namespace PRORC.Application.DTOs.Restaurants
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CuisineType { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Rating { get; set; }
        public bool IsActive { get; set; }
    }
}