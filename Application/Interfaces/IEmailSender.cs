using Application.Core;
using Application.Profiles;
using MediatR;

namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task<Result<Unit>> ChangePassword(ForgotPasswordDto model);
    }
}