using MusicServer.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicServer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();

        User Get(int userID);

        void Create(User user);
    }
}
