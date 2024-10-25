
using System.ComponentModel.DataAnnotations;

namespace MarketCreator.DataLayer.DTOs.Account
{
    public class LoginUserDTO
    {
        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [RegularExpression(@"(^(09|9)[0-9]\d{8}$)", ErrorMessage = "موبایل وارد شده صحیح نمی باشد")]
        public string? Mobile { get; set; } = string.Empty;


        [Display(Name = "پسورد")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        public string Password { get; set; } = string.Empty;

        [Display(Name ="مرا بخاطر بسپار")]
        public bool RememberMe { get; set; }
    }

    //حالاتی که موقع لاگین پیش میاد

    public enum ResultLoginUser
    {
        Success,
        NotFound,
        NotActivatedMobile
    }

}
