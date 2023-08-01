using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            this.context=context;
            this.mapper=mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        { 
            return await context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(mapper.ConfigurationProvider  )
                .SingleOrDefaultAsync();          

            //.Select(user => new MemberDto
            // {
            //     Id = user.Id,
            //     UserName = user.UserName,            //without using autoMapper
            //     KnownAs = user.KnownAs,  

            // })
        }

        public async Task<PagedList<MemberDto>> getMembersAsync(UserParams userParams)
        {
            //var query = context.Users   
            //    .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            //    .AsNoTracking();
            var query = context.Users.AsQueryable();
            query =  query.Where(u => u.UserName != userParams.CurrentUsername);  
            query = query.Where(u => u.Gender == userParams.Gender);                //for filtering gender

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));     //filtering ages

            query =  query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(query.AsNoTracking().ProjectTo<MemberDto>(mapper.ConfigurationProvider),
                userParams.pageNumber, userParams.PageSize); //return page lists

            //return await context.Users                 
            //    .ProjectTo<MemberDto>(mapper.ConfigurationProvider)     //for manulay write changes and above used for query
            //    .ToListAsync();
        }

        public async Task<AppUser> getUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> getUserByUserNameAsync(string username)
        {
            return await context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;      //if 0 then return false
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified; //just informing to ef tracker that an entity has been updated
        }
    }
}
