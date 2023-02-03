using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PathInterview.Core.Result;
using PathInterview.Core.Security;
using PathInterview.Entities.Dto.Auth.Request;
using PathInterview.Entities.Entity;
using PathInterview.Infrastructure.Abstract.Query;
using PathInterview.Infrastructure.Abstract.Service;

namespace PathInterview.Infrastructure.Concrete.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _identityUserManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthQuery _authQuery;
        private readonly ITokenHelper _tokenHelper;

        public AuthService(UserManager<User> identityUserManager, IAuthQuery authQuery, SignInManager<User> signInManager, ITokenHelper tokenHelper)
        {
            _identityUserManager = identityUserManager;
            _authQuery = authQuery;
            _signInManager = signInManager;
            _tokenHelper = tokenHelper;
        }

        public async Task<DataResult> RegisterAsync(RegisterRequest model)
        {
            DataResult dataResult = new DataResult();
            
            (bool success, string message) = await CreateUser(model, model.Password);
            
            if (success)
            {
                dataResult.Data = message;
                return dataResult;
            }

            dataResult.ErrorMessageList.Add(message);
            return dataResult;
        }

        public async Task<DataResult> LoginAsync(LoginRequest model)
        {
            DataResult dataResult = new();

            User user = await _authQuery.Get(c => c.Email.Equals(model.Email));

            if (user is null)
            {
                dataResult.ErrorMessageList.Add("Kullanıcı bulunamadı");
                return dataResult;
            }
            
            SignInResult signInCheck = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, true);

            if (!signInCheck.Succeeded)
            {
                dataResult.ErrorMessageList.Add("Hatalı email veya şifre");
                return dataResult;
            }
            
            AccessToken token = _tokenHelper.CreateToken(user);

            if (string.IsNullOrEmpty(token.Token))
            {
                dataResult.ErrorMessageList.Add("İşlem başarısız");
                return dataResult;
            }

            dataResult.Data = token.Token;
            return dataResult;
        }

        private async Task<(bool success, string message)> CreateUser(RegisterRequest model, string password)
        {
            User data = await _authQuery.Get(c => c.Email.Equals(model.Email));

            if (data is not null)
            {
                return (false, "Bu kullanıcı zaten var");
            }
            
            User entity = new()
            {
                Email = model.Email,
                IsDeleted = false,
                IsStatus = true,
                UserName = $"{model.FirstName}.{model.LastName}",
                CreatedAt = DateTime.Now,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false
            };
            
            IdentityResult result = await _identityUserManager.CreateAsync(entity, password);

            if (result.Succeeded)
                return (true, "İşlem başarılı");
            
            return (false,  "Bir hata oluştu");
        }
    }
}