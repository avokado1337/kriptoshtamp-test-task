using AutoMapper;
using KriptoshtampTestTask.Contracts;
using KriptoshtampTestTask.DTO;
using KriptoshtampTestTask.Models;
using KriptoshtampTestTask.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KriptoshtampTestTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = (UserRepository?)userRepository;
        }

        [HttpGet("/users/{page}/{param?}/{orderBy?}")]
        public async Task<UserResponseDto> GetAllUsers(int page = 1, string? param = "Id", string? orderBy = "asc")
        {
            return await _userRepository.GetAllUsers(page, param, orderBy);
        }
        [HttpGet("/user/{id}")]
        public async Task<UserDto> GetUserById(int id)
        {
            var config = new MapperConfiguration(config => config.CreateMap<User, UserDto>());

            var mapper = new Mapper(config);

            var userRepo = await _userRepository.GetUserById(id);

            var user = mapper.Map<UserDto>(userRepo);

            return user;
        }
        [HttpPost("/user")]
        public async Task<UserDto> CreateUser([FromBody] UserDto user)
        {
            var config = new MapperConfiguration(config => config.CreateMap<UserDto, User>());

            var mapper = new Mapper(config);

            var userMap = mapper.Map<UserDto, User>(user);

            await _userRepository.Create(userMap);
            return user;
        }
        [HttpDelete("/user/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        [HttpGet("users/search/{name?}/{birthdate?}")]
        public async Task<IEnumerable<User>> Search(string? name = null, DateTime? birthdate = null)
        {
            return await _userRepository.GetUsersByFullName(name, birthdate);
        }

        [HttpPut("/user")]
        public async Task<bool> Update([FromBody] UserDto userDto)
        {
            var config = new MapperConfiguration(config => config.CreateMap<UserDto, User>());

            var mapper = new Mapper(config);

            var userMap = mapper.Map<UserDto, User>(userDto);

            return await _userRepository.EditUser(userMap);
        }
    }
}
