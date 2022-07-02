using System.Net;
using System.Net.Mail;
using System.Text;
using Application.Core;
using Application.Profiles;
using Domain;
using Domain.Entities;
using Infrastructure.Interfaces;
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

            string password = GetRandomPassword(6);

            var token = await _manager.GeneratePasswordResetTokenAsync(user);

            var result = await _manager.ResetPasswordAsync(user, token, password);

            if (result.Succeeded)
            {
                ResetPassword(new AppUser()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    PasswordHash = password
                });

                return Result<Unit>.Success(Unit.Value);
            }

            return Result<Unit>.Failure(result?.Errors.ToString());
        }

        private void ResetPassword(AppUser user)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("brokenWallet@gmail.com");
                mail.To.Add(user.Email);
                mail.Subject = "Password Recovery";
                mail.Body = "Hi: " + user.DisplayName + " \tYour new password: " + user.PasswordHash;

                using (SmtpClient smtp = new SmtpClient(_config.Value.Host, int.Parse(_config.Value.Port)))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_config.Value.Username, _config.Value.Password);
                    smtp.EnableSsl = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    smtp.Send(mail);
                }
            }
        }

        private static string GetRandomPassword(int length)
        {
            const string letters = @"ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string digits = @"0123456789";
            const string specials = @"!@#$%^&*?";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
 
            for (int i = 0; i < length; i++)
            {
                var index = rnd.Next(letters.Length);
                sb.Append(letters[index]);
                var index2 = rnd.Next(digits.Length);
                sb.Append(digits[index2]);
                var index3 = rnd.Next(specials.Length);
                sb.Append(specials[index3]);
            }

            return sb.ToString();
        }
    }
}
