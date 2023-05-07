using Microsoft.AspNetCore.Mvc.Rendering;

namespace Carwash.Helpers
{
    public interface IDropDownListHelper
    {
        Task<IEnumerable<SelectListItem>> GetDDLServicesAsync();
        Task<IEnumerable<SelectListItem>> GetDDLVehiclesAsync();

    }
}
