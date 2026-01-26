using System.Threading.Tasks;
using Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ServicesImpelemention
{
    public class SystemUtilityService : ISystemUtilityService
    {
        private readonly ApplicationDbContext _context;

        public SystemUtilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyInfo> GetCompanyInfo()
        {
            // This would typically come from a CompanySettings table
            // For now, returning default values - you should update this based on your actual company settings
            return new CompanyInfo
            {
                CompanyName = "شركة HubStore",
                Address = "العنوان: شارع الملك فهد، الرياض، المملكة العربية السعودية",
                Phone = "966-12-3456789",
                Fax = "966-12-3456790",
                Email = "info@hubstore.com",
                Website = "www.hubstore.com",
                TaxNumber = "1234567890",
                CommercialRegister = "1234567890"
            };
        }
    }
}
