using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrewViewServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BrewViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VinmonopolController : Controller
    {
        private readonly IVinmonopolRepository m_vinmonopolRepository;

        public VinmonopolController(IVinmonopolRepository vinmonopolRepository)
        {
            m_vinmonopolRepository = vinmonopolRepository;
        }

        [HttpPost]
        [Route("updateall")]
        public async Task<IActionResult> UpdateAll()
        {
            return Ok(await m_vinmonopolRepository.GetAllProducts());
        }
         
        [HttpPost]
        [Route("updatedsince")]
        public async Task<IActionResult> UpdatedSince(DateTime date)
        {
            return Ok(await m_vinmonopolRepository.GetProductsUpdatedSince(date));
        }
    }
}
