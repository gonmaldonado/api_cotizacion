﻿using Microsoft.AspNetCore.Mvc;

namespace api_cotizacion.Interfaces
{
    public interface IBancoNacionService
    {
        public Task<IActionResult> Cotizaciones();
    }
}
