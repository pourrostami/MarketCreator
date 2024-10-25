
using MarketCreator.DataLayer.DTOs.Account;
using MarketCreator.DataLayer.Entities.Account;

namespace MarketCreator.Application.Services.Interfaces
{
    public interface IUserService:IAsyncDisposable
    {
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByMobileAsync(string mobile);

        Task<ResultRegisterUser> RegisterUserAsync(RegisterUsrDTO register);


        Task<ResultLoginUser> LoginUserAsync(LoginUserDTO obj);
    }
}
