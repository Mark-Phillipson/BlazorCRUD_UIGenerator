
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using ARM_BlazorServer.Models;
using ARM_BlazorServer.DTOs;

namespace ARM_BlazorServer.Repositories
{
    public class ATableEditRepository : IATableEditRepository
    {
        private readonly IDbContextFactory<ARMDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public ATableEditRepository(IDbContextFactory<ARMDbContext> contextFactory,IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
		        public async Task<IEnumerable<ATableEditDTO>> GetAllATableEditsAsync(int maxRows= 400)
        {
            using var context = _contextFactory.CreateDbContext();
            var ATableEdits= await context.ATableEdits
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Take(maxRows)
                .ToListAsync();
            IEnumerable<ATableEditDTO> ATableEditsDTO = _mapper.Map<List<ATableEdit>, IEnumerable<ATableEditDTO>>(ATableEdits);
            return ATableEditsDTO;
        }
        public async Task<IEnumerable<ATableEditDTO>> SearchATableEditsAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var ATableEdits= await context.ATableEdits
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<ATableEditDTO> ATableEditsDTO = _mapper.Map<List<ATableEdit>, IEnumerable<ATableEditDTO>>(ATableEdits);
            return ATableEditsDTO;
        }

        public async Task<ATableEditDTO?> GetATableEditByIdAsync(int TableEditId)
        {
            using var context = _contextFactory.CreateDbContext();
            var result =await context.ATableEdits.AsNoTracking()
              .FirstOrDefaultAsync(c => c.TableEditId == TableEditId);
            if (result == null) return null;
            ATableEditDTO aTableEditDTO=_mapper.Map<ATableEdit,ATableEditDTO>(result);
            return aTableEditDTO;
        }

        public async Task<ATableEditDTO?> AddATableEditAsync(ATableEditDTO aTableEditDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            ATableEdit aTableEdit = _mapper.Map<ATableEditDTO, ATableEdit>(aTableEditDTO);
            var addedEntity = context.ATableEdits.Add(aTableEdit);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            ATableEditDTO resultDTO=_mapper.Map<ATableEdit, ATableEditDTO>(aTableEdit);
            return resultDTO;
        }

        public async Task<ATableEditDTO?> UpdateATableEditAsync(ATableEditDTO aTableEditDTO)
        {
            ATableEdit aTableEdit=_mapper.Map<ATableEditDTO, ATableEdit>(aTableEditDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundATableEdit = await context.ATableEdits.AsNoTracking().FirstOrDefaultAsync(e => e.TableEditId == aTableEdit.TableEditId);

                if (foundATableEdit != null)
                {
                    var mappedATableEdit = _mapper.Map<ATableEdit>(aTableEdit);
                    context.ATableEdits.Update(mappedATableEdit);
                    await context.SaveChangesAsync();
                    ATableEditDTO resultDTO = _mapper.Map<ATableEdit, ATableEditDTO>(mappedATableEdit);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteATableEditAsync(int TableEditId)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundATableEdit = context.ATableEdits.FirstOrDefault(e => e.TableEditId == TableEditId);
            if (foundATableEdit == null)
            {
                return;
            }
            context.ATableEdits.Remove(foundATableEdit);
            await context.SaveChangesAsync();
        }
    }
}