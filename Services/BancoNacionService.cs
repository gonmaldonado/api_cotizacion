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

        public async Task<IActionResult> Cotizaciones()
        {

            var action = CotizacionesBN();
            var jsonResult = action.Result as JsonResult;
            var value = jsonResult.Value;
            var jsStr = JsonConvert.SerializeObject(value);
            var cotizacionBN = JsonConvert.DeserializeObject<List<CotizacionBN>>(jsStr);

            List<Cotizaciones> Listcotizacion = new List<Cotizaciones>();

            Cotizaciones cotizacion = new Cotizaciones()
            {
                FechaCotizacionDolarBilleteBNA = cotizacionBN[0].FechaBilletes.ToString("yyyy-MM-dd"),
                CotizacionCompraDolarBilleteBNA = cotizacionBN[0].CompraBilletes.ToString(),
                CotizacionVentaDolarBilleteBNA = cotizacionBN[0].VentaBilletes.ToString(),

                FechaCotizacionDolarDivisaBNA = cotizacionBN[0].FechaDivisas.ToString("yyyy-MM-dd"),
                CotizacionCompraDolarDivisaBNA = cotizacionBN[0].CompraDivisas.ToString(),
                CotizacionVentaDolarDivisaBNA = cotizacionBN[0].VentaDivisas.ToString(),

                FechaCotizacionEuroBilleteBNA = cotizacionBN[0].FechaEuroBillete.ToString("yyyy-MM-dd"),
                CotizacionCompraEuroBilleteBNA = cotizacionBN[0].CompraEuroBilletes.ToString(),
                CotizacionVentaEuroBilleteBNA = cotizacionBN[0].VentaEuroBilletes.ToString(),

                FechaCotizacionEuroDivisaBNA = cotizacionBN[0].FechaEuroDivisas.ToString("yyyy-MM-dd"),
                CotizacionCompraEuroDivisaBNA = cotizacionBN[0].CompraEuroDivisas.ToString(),
                CotizacionVentaEuroDivisaBNA = cotizacionBN[0].VentaEuroDivisas.ToString(),

            };
            Listcotizacion.Add(cotizacion);

            return new JsonResult(Listcotizacion);
        }

        public async Task<IActionResult> CotizacionesBN()
        {
            var HtmlDocument = new HtmlWeb().Load("https://www.bna.com.ar/Personas").DocumentNode.SelectNodes("(//td)");
            var HtmlDocumentFecha = new HtmlWeb().Load("https://www.bna.com.ar/Personas").DocumentNode.SelectNodes("(//th)");

            List<CotizacionBN> cotizaciones = new List<CotizacionBN>();

            var fechaCotizacionBilletes = Convert.ToDateTime(HtmlDocumentFecha[0].InnerText, CultureInfo.GetCultureInfo("es-AR")).ToUniversalTime();
            var fechaCotizacionDivisas = Convert.ToDateTime(HtmlDocumentFecha[3].InnerText, CultureInfo.GetCultureInfo("es-AR")).ToUniversalTime();

            String strCotizacionCompraBilletes = HtmlDocument[1].InnerText;
            String strCotizacionVentaBilletes = HtmlDocument[2].InnerText;
            String strCotizacionCompraEuroBillete = HtmlDocument[4].InnerText;
            String strCotizacionVentaEuroBillete = HtmlDocument[5].InnerText;
            String strCotizacionCompraDivisas = HtmlDocument[10].InnerText.Replace(".", ",");
            String strCotizacionventaDivisas = HtmlDocument[11].InnerText.Replace(".", ",");
            String strCotizacionCompraEuroDivisas = HtmlDocument[16].InnerText.Replace(".", ",");
            String strCotizacionventaEuroDivisas = HtmlDocument[17].InnerText.Replace(".", ",");


            CotizacionBN cotizacion = new CotizacionBN()
            {
                FechaBilletes = fechaCotizacionBilletes,
                CompraBilletes = Convert.ToDecimal(strCotizacionCompraBilletes, CultureInfo.GetCultureInfo("es-AR")),
                VentaBilletes = Convert.ToDecimal(strCotizacionVentaBilletes, CultureInfo.GetCultureInfo("es-AR")),
                FechaDivisas = fechaCotizacionDivisas,
                CompraDivisas = Convert.ToDecimal(strCotizacionCompraDivisas, CultureInfo.GetCultureInfo("es-AR")),
                VentaDivisas = Convert.ToDecimal(strCotizacionventaDivisas, CultureInfo.GetCultureInfo("es-AR")),
                FechaEuroBillete = fechaCotizacionBilletes,
                CompraEuroBilletes = Convert.ToDecimal(strCotizacionCompraEuroBillete, CultureInfo.GetCultureInfo("es-AR")),
                VentaEuroBilletes = Convert.ToDecimal(strCotizacionVentaEuroBillete, CultureInfo.GetCultureInfo("es-AR")),
                FechaEuroDivisas = fechaCotizacionDivisas,
                CompraEuroDivisas = Convert.ToDecimal(strCotizacionCompraEuroDivisas, CultureInfo.GetCultureInfo("es-AR")),
                VentaEuroDivisas = Convert.ToDecimal(strCotizacionventaEuroDivisas, CultureInfo.GetCultureInfo("es-AR"))
            };

            cotizaciones.Add(cotizacion);

            return new JsonResult(cotizaciones);

        }

    }
}
