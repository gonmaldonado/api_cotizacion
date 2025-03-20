using api_cotizacion.Entities;
using Microsoft.AspNetCore.Mvc;

namespace api_cotizacion.Interfaces
{
    public interface IBancoNacionService
    {
        public Task<ActionResult<Cotizacion>> Cotizaciones();
    }
}
