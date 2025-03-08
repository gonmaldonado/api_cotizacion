using api_cotizacion.services;
using api_cotizacion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api_cotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionController : ControllerBase
    {
        private readonly IBancoNacionService _bancoNacionService;

        public CotizacionController(IBancoNacionService bancoNacionService) 
        { _bancoNacionService = bancoNacionService; }



        [HttpGet]
        [Route("BancoNacion")]
        public async Task<object> ListAsyncCotizaciones()
        {
            return await _bancoNacionService.Cotizaciones();
        }
    }
}
