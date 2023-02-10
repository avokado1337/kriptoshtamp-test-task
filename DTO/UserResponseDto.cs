using KriptoshtampTestTask.Models;

namespace KriptoshtampTestTask.DTO
{
    public class UserResponseDto
    {
        public List<User> Users { get; set; }
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
        public string Param { get; set; }
        public string OrderBy { get; set; }
    }
}
