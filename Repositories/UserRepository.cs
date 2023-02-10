using KriptoshtampTestTask.Contracts;
using KriptoshtampTestTask.Data;
using KriptoshtampTestTask.DTO;
using KriptoshtampTestTask.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace KriptoshtampTestTask.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserResponseDto> GetAllUsers(int page, string? param, string? orderBy)
        {
            var pageResult = 10f;
            var pageCount = Math.Ceiling(_context.Users.Count() / pageResult);


            var users = await _context.Users
                .Skip((page - 1) * (int)pageResult)
                .Take((int)pageResult)
                .ToListAsync();
            var response = new UserResponseDto
            {
                Users = users,
                CurrentPage = page,
                Pages = (int)pageCount
            };
            if (param != null && orderBy != null)
            {
                var propertyInfo = typeof(User).GetProperty(param);
                if (orderBy == "asc")
                {
                    response.Users = response.Users.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                }
                else
                {
                    response.Users = response.Users.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                }
            }
            return response;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetUsersByFullName(string? name = null, DateTime? date = null)
        {
            IQueryable<User> query = _context.Users;

            if (!string.IsNullOrEmpty(name)) 
            {
                query = query.Where(x => x.Name.Contains(name) 
                    || x.Surname.Contains(name) 
                    || x.MiddleName.Contains(name));
            }
            if (date != null)
            {
                query = query.Where(x => x.DateOfBirth.Equals(date));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> EditUser(User user)
        {
            var tempUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (tempUser != null)
            {
                tempUser.Name = user.Name;
                tempUser.Surname = user.Surname;
                tempUser.MiddleName = user.MiddleName;
                tempUser.Gender = user.Gender;
                tempUser.DateOfBirth = user.DateOfBirth;
                tempUser.Country = user.Country;
                tempUser.City = user.City;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
