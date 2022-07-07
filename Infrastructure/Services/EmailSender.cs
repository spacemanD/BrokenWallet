using System.Net;
using System.Net.Mail;
using System.Text;
using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using Domain.Entities;
using Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly UserManager<AppUser> _manager;
        private readonly IOptions<SmtpSettings> _config;

        public EmailSender(UserManager<AppUser> manager, IOptions<SmtpSettings> config)
        {
            _manager = manager;
            _config = config;
        }

        public async Task<Result<Unit>> ChangePassword(ForgotPasswordDto model)
        {
            var user = await _manager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Result<Unit>.Failure("Email is not existing");
            }

            var password = GetRandomPassword(6);

            var token = await _manager.GeneratePasswordResetTokenAsync(user);

            var result = await _manager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
            {
                return Result<Unit>.Failure(result?.Errors.ToString()!);
            }

            ResetPassword(new AppUser
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                PasswordHash = password
            });

            return Result<Unit>.Success(Unit.Value);
        }

        private void ResetPassword(AppUser user)
        {
            using var mail = new MailMessage();
            mail.From = new MailAddress("brokenWallet@gmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Password Recovery";
            mail.Body = $"Hi, {user.DisplayName}!\nYour new password: {user.PasswordHash}.";

            using var smtpClient = new SmtpClient(_config.Value.Host, int.Parse(_config.Value.Port));
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_config.Value.Username, _config.Value.Password);
            smtpClient.EnableSsl = true;
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            smtpClient.Send(mail);
        }

        private static string GetRandomPassword(int length)
        {
            const string letters = @"ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string digits = @"0123456789";
            const string specials = @"!@#$%^&*?";

            var builder = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < length; i++)
            {
                var firstIndex = random.Next(letters.Length);
                builder.Append(letters[firstIndex]);
                var secondIndex = random.Next(digits.Length);
                builder.Append(digits[secondIndex]);
                var thirdIndex = random.Next(specials.Length);
                builder.Append(specials[thirdIndex]);
            }

            return builder.ToString();
        }
    }
}