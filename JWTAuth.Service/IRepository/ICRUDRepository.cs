using JWTAuth.Domain.Entities;

namespace JWTAuth.Service.IRepository
{
    public interface ICRUDRepository
    {
        List<User> GetAllUsers();
        void Create(RegisterUser user);
        void Update(User user);
        void Delete(string name);
    }
}
