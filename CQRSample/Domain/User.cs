using CQRSample.Common;
using FluentValidation;
using System.Security.Permissions;

namespace CQRSample.Domain
{
    public class User : BaseEntity
    {
       public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
      //  public bool IsActive { get; set; } = true;
     //   public DateTime DateAdded { get; set; } = DateTime.Now;
    //    public string AddedBy { get; set; }
     //   public string Reason { get; set; }
    }   


     public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.UserName)
                                    .NotEmpty().WithMessage("Username is Required!")
                                    .MinimumLength(3).WithMessage("Username must be at least 3 character long.")
                                    .Must(BeValidName).WithMessage("Username must be start an uppercase letter.");


            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required");
            
        }

        private bool BeValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return true; 

            return char.IsUpper(name[0]);
        }


        

    }


}
