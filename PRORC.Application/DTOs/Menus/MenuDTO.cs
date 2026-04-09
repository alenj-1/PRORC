namespace PRORC.Application.DTOs.Menus
{
    public class MenuDto
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
