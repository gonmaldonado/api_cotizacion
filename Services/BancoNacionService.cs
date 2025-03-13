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
            var cotizacionBN = CotizacionesBN();

            List<Cotizaciones> Listcotizacion = new List<Cotizaciones>();

            Cotizaciones cotizacion = new Cotizaciones()
            {
                FechaCotizacionDolarBilleteBNA = cotizacionBN.Result.FechaBilletes.ToString("yyyy-MM-dd"),
                CotizacionCompraDolarBilleteBNA = cotizacionBN.Result.CompraBilletes.ToString(),
                CotizacionVentaDolarBilleteBNA = cotizacionBN.Result.VentaBilletes.ToString(),

                FechaCotizacionDolarDivisaBNA = cotizacionBN.Result.FechaDivisas.ToString("yyyy-MM-dd"),
                CotizacionCompraDolarDivisaBNA = cotizacionBN.Result.CompraDivisas.ToString(),
                CotizacionVentaDolarDivisaBNA = cotizacionBN.Result.VentaDivisas.ToString(),

                FechaCotizacionEuroBilleteBNA = cotizacionBN.Result.FechaEuroBillete.ToString("yyyy-MM-dd"),
                CotizacionCompraEuroBilleteBNA = cotizacionBN.Result.CompraEuroBilletes.ToString(),
                CotizacionVentaEuroBilleteBNA = cotizacionBN.Result.VentaEuroBilletes.ToString(),

                FechaCotizacionEuroDivisaBNA = cotizacionBN.Result.FechaEuroDivisas.ToString("yyyy-MM-dd"),
                CotizacionCompraEuroDivisaBNA = cotizacionBN.Result.CompraEuroDivisas.ToString(),
                CotizacionVentaEuroDivisaBNA = cotizacionBN.Result.VentaEuroDivisas.ToString(),

            };
            Listcotizacion.Add(cotizacion);

            return new JsonResult(Listcotizacion);
        }

        //public async Task<IActionResult> CotizacionesBN()
        //{
        //    var HtmlDocument = new HtmlWeb().Load("https://www.bna.com.ar/Personas").DocumentNode.SelectNodes("(//td)");
        //    var HtmlDocumentFecha = new HtmlWeb().Load("https://www.bna.com.ar/Personas").DocumentNode.SelectNodes("(//th)");

        //    List<CotizacionBN> cotizaciones = new List<CotizacionBN>();

        //    var fechaCotizacionBilletes = Convert.ToDateTime(HtmlDocumentFecha[0].InnerText, CultureInfo.GetCultureInfo("es-AR")).ToUniversalTime();
        //    var fechaCotizacionDivisas = Convert.ToDateTime(HtmlDocumentFecha[3].InnerText, CultureInfo.GetCultureInfo("es-AR")).ToUniversalTime();

        //    String strCotizacionCompraBilletes = HtmlDocument[1].InnerText;
        //    String strCotizacionVentaBilletes = HtmlDocument[2].InnerText;
        //    String strCotizacionCompraEuroBillete = HtmlDocument[4].InnerText;
        //    String strCotizacionVentaEuroBillete = HtmlDocument[5].InnerText;
        //    String strCotizacionCompraDivisas = HtmlDocument[10].InnerText.Replace(".", ",");
        //    String strCotizacionventaDivisas = HtmlDocument[11].InnerText.Replace(".", ",");
        //    String strCotizacionCompraEuroDivisas = HtmlDocument[16].InnerText.Replace(".", ",");
        //    String strCotizacionventaEuroDivisas = HtmlDocument[17].InnerText.Replace(".", ",");


        //    CotizacionBN cotizacion = new CotizacionBN()
        //    {
        //        FechaBilletes = fechaCotizacionBilletes,
        //        CompraBilletes = Convert.ToDecimal(strCotizacionCompraBilletes, CultureInfo.GetCultureInfo("es-AR")),
        //        VentaBilletes = Convert.ToDecimal(strCotizacionVentaBilletes, CultureInfo.GetCultureInfo("es-AR")),
        //        FechaDivisas = fechaCotizacionDivisas,
        //        CompraDivisas = Convert.ToDecimal(strCotizacionCompraDivisas, CultureInfo.GetCultureInfo("es-AR")),
        //        VentaDivisas = Convert.ToDecimal(strCotizacionventaDivisas, CultureInfo.GetCultureInfo("es-AR")),
        //        FechaEuroBillete = fechaCotizacionBilletes,
        //        CompraEuroBilletes = Convert.ToDecimal(strCotizacionCompraEuroBillete, CultureInfo.GetCultureInfo("es-AR")),
        //        VentaEuroBilletes = Convert.ToDecimal(strCotizacionVentaEuroBillete, CultureInfo.GetCultureInfo("es-AR")),
        //        FechaEuroDivisas = fechaCotizacionDivisas,
        //        CompraEuroDivisas = Convert.ToDecimal(strCotizacionCompraEuroDivisas, CultureInfo.GetCultureInfo("es-AR")),
        //        VentaEuroDivisas = Convert.ToDecimal(strCotizacionventaEuroDivisas, CultureInfo.GetCultureInfo("es-AR"))
        //    };

        //    cotizaciones.Add(cotizacion);

        //    return new JsonResult(cotizaciones);

        //}
        public async Task<CotizacionBN> CotizacionesBN()
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

                // Carga del objeto
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
                return cotizacion;
            }
        }

    }
}
