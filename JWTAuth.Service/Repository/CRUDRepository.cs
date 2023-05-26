using JWTAuth.Domain;
using JWTAuth.Service.IRepository;

namespace JWTAuth.Service.Repository
{
    public class CRUDRepository : ICRUDRepository
    {
        private readonly DataContext _dataContext;

        public CRUDRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void GetAllUsers()
        {
            var user = _dataContext.Users.ToList();
        }
    }
}
