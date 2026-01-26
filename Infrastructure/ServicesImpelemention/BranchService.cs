using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domin;
using Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class BranchService : IBranchService
    {
        private readonly ApplicationDbContext _context;

        public BranchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            return await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Branch> GetBranchById(int branchId)
        {
            return await _context.Branches
                .Where(b => b.BranchId == branchId && b.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Branch>> SearchBranchesByName(string term)
        {
            return await _context.Branches
                .Where(b => b.IsActive && 
                       (b.Name.Contains(term) || b.Code.Contains(term)))
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<List<Branch>> SearchBranchesByCode(string term)
        {
            return await _context.Branches
                .Where(b => b.IsActive && 
                       (b.Code.Contains(term) || b.Name.Contains(term)))
                .OrderBy(b => b.Code)
                .ToListAsync();
        }
    }
}
