
using MarketCreator.DataLayer.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace MarketCreator.DataLayer.Entities.Account
{
    public class User : BaseEntity
    {


        [Display(Name ="نام")]
        [Required(ErrorMessage ="{0} نمی تواند خالی باشد")]
        [MaxLength(50,ErrorMessage ="{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? FirstName { get; set; } = string.Empty;


        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? LastName { get; set; } = string.Empty;


        [Display(Name = "ایمیل")] 
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage ="{0} وارد شده درست نمی باشد")]
        public string? Email { get; set; } = string.Empty;


        [Display(Name = "کد فعالسازی ایمیل")] 
        [MaxLength(4)]
        public string? EmailActiveCode { get; set; } = string.Empty;

        
        [Display(Name = "ایمیل فعال/غیرفعال")] 
        public bool EmailActivated { get; set; }

        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "  {۰} اشتباه است")]
        public string? Mobile { get; set; } = string.Empty;

        [Display(Name = "کد فعالسازی موبایل")] 
        [MaxLength(4)]
        public string? MobileActiveCode { get; set; } = string.Empty;

        [Display(Name = "موبایل فعال/غیرفعال")] 
        public bool MobileActivated { get; set; }

        [Display(Name = "بلاک شده/نشده")] 
        public bool IsActive { get; set; }

        [Display(Name = "تصویر")] 
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string? Avatar { get; set; } = string.Empty;

        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

    }
}
