using PathInterview.Entities.Entity;

namespace PathInterview.Core.Security
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user); 
    }
}

