
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using ARM_BlazorServer.Models;
using ARM_BlazorServer.DTOs;

namespace ARM_BlazorServer.Repositories
{
    public class AUserRepository : IAUserRepository
    {
        private readonly IDbContextFactory<ARMDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public AUserRepository(IDbContextFactory<ARMDbContext> contextFactory,IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
		        public async Task<IEnumerable<AUserDTO>> GetAllAUsersAsync(int maxRows= 400)
        {
            using var context = _contextFactory.CreateDbContext();
            var AUsers= await context.AUsers
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Take(maxRows)
                .ToListAsync();
            IEnumerable<AUserDTO> AUsersDTO = _mapper.Map<List<AUser>, IEnumerable<AUserDTO>>(AUsers);
            return AUsersDTO;
        }
        public async Task<IEnumerable<AUserDTO>> SearchAUsersAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var AUsers= await context.AUsers
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<AUserDTO> AUsersDTO = _mapper.Map<List<AUser>, IEnumerable<AUserDTO>>(AUsers);
            return AUsersDTO;
        }

        public async Task<AUserDTO?> GetAUserByIdAsync(int UserId)
        {
            using var context = _contextFactory.CreateDbContext();
            var result =await context.AUsers.AsNoTracking()
              .FirstOrDefaultAsync(c => c.UserId == UserId);
            if (result == null) return null;
            AUserDTO aUserDTO=_mapper.Map<AUser,AUserDTO>(result);
            return aUserDTO;
        }

        public async Task<AUserDTO?> AddAUserAsync(AUserDTO aUserDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            AUser aUser = _mapper.Map<AUserDTO, AUser>(aUserDTO);
            var addedEntity = context.AUsers.Add(aUser);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            AUserDTO resultDTO=_mapper.Map<AUser, AUserDTO>(aUser);
            return resultDTO;
        }

        public async Task<AUserDTO?> UpdateAUserAsync(AUserDTO aUserDTO)
        {
            AUser aUser=_mapper.Map<AUserDTO, AUser>(aUserDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundAUser = await context.AUsers.AsNoTracking().FirstOrDefaultAsync(e => e.UserId == aUser.UserId);

                if (foundAUser != null)
                {
                    var mappedAUser = _mapper.Map<AUser>(aUser);
                    context.AUsers.Update(mappedAUser);
                    await context.SaveChangesAsync();
                    AUserDTO resultDTO = _mapper.Map<AUser, AUserDTO>(mappedAUser);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteAUserAsync(int UserId)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundAUser = context.AUsers.FirstOrDefault(e => e.UserId == UserId);
            if (foundAUser == null)
            {
                return;
            }
            context.AUsers.Remove(foundAUser);
            await context.SaveChangesAsync();
        }
    }
}