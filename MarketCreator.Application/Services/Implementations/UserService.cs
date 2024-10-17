

using MarketCreator.Application.Services.Interfaces;
using MarketCreator.DataLayer.DTOs.Account;
using MarketCreator.DataLayer.Entities.Account;
using MarketCreator.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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

        #region Password Hash && Salt

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac =new  HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac =new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        #endregion

        public async Task<ResultRegisterUser> RegisterUserAsync(RegisterUsrDTO register)
        {
            var user = await GetUserByMobileAsync(register.Mobile.Trim());
            if (user == null)
            {
                CreatePasswordHash(register.Password.Trim(), out byte[]  passwordHash, out byte[] passwordSalt);

                await _userRepository.AddEntityAsync(new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Mobile = register.Mobile,
                    MobileActiveCode = new Random().Next(1000,9999).ToString(),   
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                });

                await _userRepository.SaveChangesAsync();
            } 

            if(user!=null && !user.MobileActivated)
            {
                await _userRepository.DeletePermanentAsync(user.Id);
                CreatePasswordHash(register.Password.Trim(), out byte[] passwordHash, out byte[] passwordSalt);

                await _userRepository.AddEntityAsync(new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Mobile = register.Mobile,
                    MobileActiveCode = new Random().Next(1000, 9999).ToString(),
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                });

                await _userRepository.SaveChangesAsync(); 
            }

            if(user != null && user.MobileActivated)
                return ResultRegisterUser.MobileExists;

            return ResultRegisterUser.Success;
        }



        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if( _userRepository!=null )
                await _userRepository.DisposeAsync();
        }
        #endregion
    }
}
