using KriptoshtampTestTask.DTO;
using KriptoshtampTestTask.Models;

namespace KriptoshtampTestTask.Contracts
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<bool> Delete(int id);
        Task<UserResponseDto> GetAllUsers(int page, string param, string orderBy);
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetUsersByFullName(string? name, DateTime? date);
        Task<bool> EditUser(User user);
    }
}
