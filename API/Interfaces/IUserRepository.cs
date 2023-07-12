using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> getUserByIdAsync(int id);
        Task<AppUser> getUserByUserNameAsync(string username);
        Task<IEnumerable<MemberDto>> getMembersAsync();
        Task<MemberDto> GetMemberAsync(string username); 
    }
}
