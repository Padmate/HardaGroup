using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using HardaGroup.Models;

namespace HardaGroup.Web.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel:BaseModel
    {
        [Required(ErrorMessage = "请输入当前密码")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "请输入新密码")]
        [StringLength(15, ErrorMessage = "密码必须至少包含6个字符。", MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z0-9]{6,15}$", ErrorMessage = "密码必须为6-15位字母和数字组成")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,15}$", ErrorMessage = "密码必须为大小写字母和数字的组合，不能使用特殊字符，长度在8-15之间")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "请再次输入新密码")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "电话号码")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "电话号码")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}