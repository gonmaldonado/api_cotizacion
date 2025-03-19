using System;
namespace api_cotizacion.Entities
{
    public class Cotizacion
    {
        public List<Item>? Cotizaciones { get; set; }
    }
    public class Item
    {
        public string? Cotizacion { get; set; }
        public string? Fecha { get; set; }
        public decimal Compra { get; set; }
        public decimal Venta { get; set; }
    }
    public class CotizacionBN
    {
        public DateTime FechaBilletes { get; set; }
        public decimal CompraBilletes { get; set; }
        public decimal VentaBilletes { get; set; }
        public DateTime FechaDivisas { get; set; }
        public decimal CompraDivisas { get; set; }
        public decimal VentaDivisas { get; set; }
        public DateTime FechaEuroBillete { get; set; }
        public decimal CompraEuroBilletes { get; set; }
        public decimal VentaEuroBilletes { get; set; }
        public DateTime FechaEuroDivisas { get; set; }
        public decimal CompraEuroDivisas { get; set; }
        public decimal VentaEuroDivisas { get; set; }
    }
}
