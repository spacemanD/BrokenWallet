using Application.Core;
using Application.Profiles;
using MediatR;

namespace Infrastructure.Interfaces
{
    public interface IEmailSender
    {
        Task<Result<Unit>> ChangePassword(ForgotPasswordDto model);
    }
}