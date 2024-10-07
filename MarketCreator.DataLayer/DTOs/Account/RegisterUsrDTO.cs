

using System.ComponentModel.DataAnnotations;

namespace MarketCreator.DataLayer.DTOs.Account
{
    public class RegisterUsrDTO
    {


        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? FirstName { get; set; } = string.Empty;


        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? LastName { get; set; } = string.Empty;


 

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "  {۰} اشتباه است")]
        public string? Mobile { get; set; } = string.Empty;


        [Display(Name = "پسورد")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        public string Password { get; set; } = string.Empty;



        [Display(Name = "تایید پسورد")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [Compare("Password",ErrorMessage ="پسوردهای وارد شده یکسان نمی باشند")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
