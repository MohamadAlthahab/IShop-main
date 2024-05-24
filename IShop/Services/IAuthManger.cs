using IShop.Models;
using System.Threading.Tasks;

namespace IShop.Services
{
    public interface IAuthManger
    {
        Task<bool> ValidateUser(LoginUserDTO userDTO);
        Task<string> CreateToken();
    }
}
