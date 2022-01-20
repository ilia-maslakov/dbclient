using FluentValidation;
using dbclient.data.EF;
namespace dbclient.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            var errmsg = "Incorrect data in [{PropertyName}]: value [{PropertyValue}]";
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage(errmsg);
            RuleFor(x => x.Surname).NotNull().NotEmpty().WithMessage(errmsg);
        }
    }
}
