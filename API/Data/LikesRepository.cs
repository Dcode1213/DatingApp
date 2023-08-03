using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext context;

        public LikesRepository(DataContext context)
        {
            this.context=context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await context.Likes.FindAsync(sourceUserId, targetUserId);
        }

        
        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = context.Users.OrderBy(u => u.UserName).AsQueryable(); //get a list of users in database ordered by username -- nothing gone happen currently
            var likes = context.Likes.AsQueryable();  //not execute because used AsQueryable

            if(likesParams.predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser);            //filter out user based on what's inside the liked list
            }

            if(likesParams.predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,                                              //Mappping manually
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });
            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.pageNumber, likesParams.pageSize);
         }   

        public async Task<AppUser> GetUsersWithLikes(int userId)
        {
            return await context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }

        
    }
}
