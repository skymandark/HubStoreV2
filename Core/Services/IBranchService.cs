using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domin;

namespace Core.Services
{
    public interface IBranchService
    {
        Task<List<Branch>> GetAllBranches();
        Task<Branch> GetBranchById(int branchId);
        Task<List<Branch>> SearchBranchesByName(string term);
        Task<List<Branch>> SearchBranchesByCode(string term);
    }
}
