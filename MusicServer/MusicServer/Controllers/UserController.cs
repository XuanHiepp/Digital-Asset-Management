using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicServer.Models.Database;
using MusicServer.Models.Database.Queries;
using MusicServer.Repositories.Interfaces;
using MusicServer.Services.Interfaces;

namespace MusicServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMusicService musicService;

        public UserController(IUserRepository userRepository,
            IMusicService musicService)
        {
            this.userRepository = userRepository;
            this.musicService = musicService;
        }

        // GET: api/User
        [HttpGet]
        public List<User> Get()
        {
            try
            {
                var users = userRepository.GetAll();
                List<User> result = new List<User>();
                foreach (var user in users)
                {
                    result.Add(user);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetUserInfo/{id}")]
        public User GetUserInfo(int id)
        {
            try
            {
                var users = userRepository.GetAll();
                User result = new User();
                result = userRepository.Get(id);
                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetMusicAssetWithUser/{userID}")]
        public List<MusicQueryData> GetMusicAssetWithUser(uint userID)
        {
            try
            {
                var result = musicService.GetMusicListWithUserID(userID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetMusicAssetWithUserBuyer/{buyerID}")]
        public List<MusicQueryData> GetMusicAssetWithUserBuyer(uint buyerID)
        {
            try
            {
                var result = musicService.GetMusicListWithUserIDWithBuying(buyerID);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
