

using MarketCreator.Application.Services.Interfaces;
using MarketCreator.DataLayer.DTOs.Account;
using MarketCreator.DataLayer.Entities.Account;
using MarketCreator.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketCreator.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Constructor
        private readonly IGenericRepository<User> _userRepository;

        public UserService(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region Get User By Id or Mobile


        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> GetUserByMobileAsync(string mobile)
        {
            return await _userRepository.GetAll().AsQueryable()
                .FirstOrDefaultAsync(s => s.Mobile == mobile);
        }
        #endregion

        public async Task<ResultRegisterUser> RegisterUserAsync(RegisterUsrDTO register)
        {
            var user = await GetUserByMobileAsync(register.Mobile.Trim());
            if (user == null)
            {
                await _userRepository.AddEntityAsync(new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Mobile = register.Mobile,
                });

                await _userRepository.SaveChangesAsync();
            } 

            return ResultRegisterUser.Success;
        }



        #region Dispose

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
