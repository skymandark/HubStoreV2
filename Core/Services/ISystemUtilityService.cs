using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ISystemUtilityService
    {
        Task<CompanyInfo> GetCompanyInfo();
    }

    public class CompanyInfo
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }
        public string CommercialRegister { get; set; }
    }
}
