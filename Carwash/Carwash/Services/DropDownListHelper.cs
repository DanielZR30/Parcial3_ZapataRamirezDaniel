using Carwash.DAL;
using Carwash.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Carwash.Services
{

    public class DropDownListHelper: IDropDownListHelper
    {
        private readonly DatabaseContext _context;
        public DropDownListHelper(DatabaseContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetDDLServicesAsync()
        {
            
            List<SelectListItem> listServices = await _context.Services
                .OrderBy(s => s.Price)
                .Select(s => new SelectListItem
                {  
                    Text = s.Name,
                    Value = s.Id.ToString()
                })
                .ToListAsync();

            listServices.Insert(0, new SelectListItem
            {
                Text = "Selecione una categoría...",
                Value = Guid.Empty.ToString(), 
                Selected = true 
            });

            return listServices;
        }

        public Task<IEnumerable<SelectListItem>> GetDDLVehiclesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
