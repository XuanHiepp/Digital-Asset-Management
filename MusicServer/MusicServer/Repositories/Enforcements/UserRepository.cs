using MusicServer.Models.Database;
using MusicServer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Enforcements
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(MusicDBContext dbContext) : base(dbContext)
        {
        }

        List<User> IUserRepository.GetAll()
        {
            return dbContext.Users.ToList();
        }

        User IUserRepository.Get(int userID)
        {
            var result = dbContext.Users.Where(c => c.UserID == userID).SingleOrDefault();
            return result;
        }

        void IUserRepository.Create(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges(true);
        }
    }
}
