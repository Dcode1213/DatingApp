using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> getUserByIdAsync(int id);
        Task<AppUser> getUserByUserNameAsync(string username);
        Task<PagedList<MemberDto>> getMembersAsync(UserParams userParams);
        Task<MemberDto> GetMemberAsync(string username); 
    }
}
    