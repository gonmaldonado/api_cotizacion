using Microsoft.AspNetCore.Mvc;
using api_cotizacion.Interfaces;
using api_cotizacion.Entities;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Globalization;

namespace api_cotizacion.services
{
    public class BancoNacionService :IBancoNacionService
    {

        public BancoNacionService()
        {
          
        }
        public async Task<List<Item>> CotizacionesBN()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            using (var client = new HttpClient(handler))
            {
                string url = "https://www.bna.com.ar/Personas";
                string htmlContent = await client.GetStringAsync(url);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var HtmlDocument = htmlDoc.DocumentNode.SelectNodes("(//td)");
                var HtmlDocumentFecha = htmlDoc.DocumentNode.SelectNodes("(//th)");
                var fechaCotizacionBilletes = Convert.ToDateTime(HtmlDocumentFecha[0].InnerText.Trim(), CultureInfo.GetCultureInfo("es-AR")).ToUniversalTime();
                var fechaCotizacionDivisas = Convert.ToDateTime(HtmlDocumentFecha[3].InnerText.Trim(), CultureInfo.GetCultureInfo("es-AR")).ToUniversalTime();

                // Limpieza de datos
                string Limpiar(string input) => input.Trim().Replace(".", ",").Replace("\u00a0", ""); // Elimina espacios no separables

                // Obtener y limpiar cotizaciones
                string strCotizacionCompraBilletes = Limpiar(HtmlDocument[1].InnerText);
                string strCotizacionVentaBilletes = Limpiar(HtmlDocument[2].InnerText);
                string strCotizacionCompraEuroBillete = Limpiar(HtmlDocument[4].InnerText);
                string strCotizacionVentaEuroBillete = Limpiar(HtmlDocument[5].InnerText);
                string strCotizacionCompraDivisas = Limpiar(HtmlDocument[10].InnerText);
                string strCotizacionventaDivisas = Limpiar(HtmlDocument[11].InnerText);
                string strCotizacionCompraEuroDivisas = Limpiar(HtmlDocument[16].InnerText);
                string strCotizacionventaEuroDivisas = Limpiar(HtmlDocument[17].InnerText);

                List<Item> cotizaciones = new List<Item>();

                // Carga del objeto
                Item DolarBillete = new Item()
                { Cotizacion = "Dolar Billete",
                    Fecha = fechaCotizacionBilletes,
                    Compra = Convert.ToDecimal(strCotizacionCompraBilletes, CultureInfo.GetCultureInfo("es-AR")),
                    Venta = Convert.ToDecimal(strCotizacionVentaBilletes, CultureInfo.GetCultureInfo("es-AR"))
                }; 
                cotizaciones.Add(DolarBillete);

                Item DolarDivisa = new Item()
                {
                    Cotizacion = "Dolar Divisa",
                    Fecha = fechaCotizacionDivisas,
                    Compra = Convert.ToDecimal(strCotizacionCompraDivisas, CultureInfo.GetCultureInfo("es-AR")),
                    Venta = Convert.ToDecimal(strCotizacionventaDivisas, CultureInfo.GetCultureInfo("es-AR"))
                };
                cotizaciones.Add(DolarDivisa);

                Item EuroBillete = new Item()
                {
                    Cotizacion = "Euro Billete",
                    Fecha = fechaCotizacionBilletes,
                    Compra = Convert.ToDecimal(strCotizacionCompraEuroBillete, CultureInfo.GetCultureInfo("es-AR")),
                    Venta = Convert.ToDecimal(strCotizacionVentaEuroBillete, CultureInfo.GetCultureInfo("es-AR"))
                };
                cotizaciones.Add(EuroBillete);

                Item EuroDivisa = new Item()
                {
                    Cotizacion = "Euro Divisa",
                    Fecha = fechaCotizacionDivisas,
                    Compra = Convert.ToDecimal(strCotizacionCompraEuroDivisas, CultureInfo.GetCultureInfo("es-AR")),
                    Venta = Convert.ToDecimal(strCotizacionventaEuroDivisas, CultureInfo.GetCultureInfo("es-AR"))
                };
                cotizaciones.Add(EuroDivisa);

                return cotizaciones;
            }
        }
        public async Task<IActionResult> Cotizaciones()
        {
            return new JsonResult(CotizacionesBN());
        }

    }
}
