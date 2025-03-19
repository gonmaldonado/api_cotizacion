using System;
namespace api_cotizacion.Entities
{
    public class Item
    {
        public string? Cotizacion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Compra { get; set; }
        public decimal Venta { get; set; }
    }
}
