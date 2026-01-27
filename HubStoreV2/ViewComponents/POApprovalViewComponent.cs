using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HubStoreV2.ViewComponents
{
    public class POApprovalViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int typeId)
        {
            ViewBag.TypeId = typeId;
            return View();
        }
    }
}
