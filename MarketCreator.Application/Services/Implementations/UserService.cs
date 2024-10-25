

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


        #region Register

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
        #endregion


        #region Login
        public async Task<ResultLoginUser> LoginUserAsync(LoginUserDTO obj)
        {
            //اول سرچ با موبایل انجام میدیم که اینطور موبایلی ثبت شده یا نه
            var  user = await GetUserByMobileAsync(obj.Mobile);

            if(user==null) return ResultLoginUser.NotFound;

            //اگر اینطور موبایلی باشه ولی پسوردش غلط باشه پس ابتدا پسورد رو چک میکنیم
            //چجوری کار میکنه؟
            //پسورد ورودی رو میفرستیم
            //در کنارش پسورد هش و سالت  یوزری که از دیتابیس بدست آوردیم
            //اگر پسورد ورودی با هش و سالت دیتابیس یعنی کاربری که با موبایلش بدستش آوردیم ترکیبشون ترو بده 
            //یعنی درسته پسورد همونه
            bool result = VerifyPasswordHash(obj.Password, user.PasswordHash, user.PasswordSalt);
            if(!result) return ResultLoginUser.NotFound;

            //اگر از دو مرحله ی بالایی عبور کرد  یعنی هم همچون موبایلی داریم با همچون پسوردی
            //پس نوبت بعدی چک کردن موبایل اکتیویتد هست اگر فالز باشه یعنی موبایل تایید نشده موقع ثبت نام
            if(!user.MobileActivated) return ResultLoginUser.NotActivatedMobile;

            //اگه مراحل بالایی رو عبور کرده باشه یعنی آقا درسته و ادامه ی لاگین شدن
            return ResultLoginUser.Success;
        }
        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if( _userRepository!=null )
                await _userRepository.DisposeAsync();
        }
        #endregion
    }
}
