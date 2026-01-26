using Microsoft.AspNetCore.Mvc;

namespace HubStoreV2.Controllers
{
    public class BaseController : Controller
    {
        protected int? GetSelectedBranchId()
        {
            if (Request.Cookies.TryGetValue("SelectedBranch", out var branchCookie) && int.TryParse(branchCookie, out var branchId))
            {
                return branchId;
            }
            return null;
        }
    }
}