using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrewViewServer.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BrewViewServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VinmonopolController : Controller
    {
        private readonly IBrewRepository m_brewRepository;

        public VinmonopolController(IBrewRepository brewRepository)
        {
            m_brewRepository = brewRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await m_brewRepository.UpdateDatabase());
        }
    }
}
