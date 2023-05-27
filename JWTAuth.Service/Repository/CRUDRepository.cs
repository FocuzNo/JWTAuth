using JWTAuth.Domain;
using JWTAuth.Domain.Entities;
using JWTAuth.Service.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Service.Repository
{
    public class CRUDRepository : ICRUDRepository
    {
        private readonly DataContext _dataContext;
        private readonly static User user = new();

        public CRUDRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List <User> GetAllUsers()
        {
            return _dataContext.Users.ToList();
        }

        public void Create(RegisterUser registerUser)
        {
            string passworHash = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);

            user.Username = registerUser.Username;
            user.PasswordHash = passworHash;

            _dataContext.Add(registerUser);
            _dataContext.SaveChanges();
        }

        public void Update(User user)
        {
            string passworHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            user.PasswordHash = passworHash;
            _dataContext.Entry(user).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }

        public void Delete(string? name)
        {
            User? user = _dataContext.Users.FirstOrDefault(x => x.Username == name);
            _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();
        }
    }
}
