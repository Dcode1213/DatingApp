using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);        //return an entity
        Task<AppUser> GetUsersWithLikes(int userId);                            //return an AppUser Entity
        //Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
