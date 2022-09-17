using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace CodeAssesment.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please Enter Username..")]
        [Display(Name = "UserFirstName")]
        public string UserFirstName { get; set; }

        [Display(Name = "UserLast Name")]
        public string UserLastName { get; set; }
        
        [Required(ErrorMessage = "Please Enter Email...")]
        [Display(Name = "UserEmail")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile ...")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        [Display(Name = "UserMobile")]
        public int UserMobile { get; set; }

        public string UserImgProfile { get; set; }
        public string UserImgExt { get; set; }
        public string UserDocument { get; set; }
        public string UserDoc_Ext { get; set; }
        

    }
}
