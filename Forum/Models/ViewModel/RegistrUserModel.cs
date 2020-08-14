using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Forum.Models
{
    public class RegistrUserModel
    {
        [Display(Name = "Полное имя пользователя")]
        [StringLength(50, ErrorMessage = @"Значение {0} должно содержать не менее {0} символов.", MinimumLength = 1)]
        public string FIO { get; set; }

        [Required]
        [Display(Name = "Логин пользователя")]
        [StringLength(20, ErrorMessage = @"Значение {0} должно содержать не менее {0} символов.", MinimumLength = 2)]
        public string LogIn { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(100, ErrorMessage = @"Значение {0} должно содержать не менее {0} символов.", MinimumLength = 3)]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Повторите ввод пароля")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [StringLength(100, ErrorMessage = @"Значение {0} должно содержать не менее {0} символов.", MinimumLength = 3)]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Роль")]
        public string Role { get; set; }


        public List<Role> GetListRole()
        {
            Entities entities = new Entities();
            return entities.Role.ToList();
        }
    }
}