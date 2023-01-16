using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEWSHORE_AIR.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.DataAccess
{
    public class Data
    {
        private static IConfiguration _configuration;
        private static ILogger<Data> _logger;
        private static IAPI_Get _api_Get;
        public Data(IConfiguration configuration, IAPI_Get api_Get, ILogger<Data> logger)
        {
            _configuration = configuration;
            _api_Get = api_Get;
            _logger = logger;
        }
        public static string Message { get; set; }

        public static List<SelectListItem> Origins(string? url = null)
        {
            try
            {
                if (url == null)
                    url = _configuration["Rutas:MultipyRetorno"];
                //string url = _configuration["Rutas:MultipyRetorno"];
                var origenes = _api_Get.Origins().ToArray();
                List<SelectListItem> lstorigenes = new List<SelectListItem>();
                for (int i = 0; i < origenes.Length; i++)
                {
                    lstorigenes.Add(new SelectListItem() { Text = origenes[i], Value = i + 1.ToString() });
                }
                //SelectList listItems = new SelectList(origenes, "", "");
                return lstorigenes;
            }
            catch (Exception ex)
            {
                Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
                _logger.LogInformation(Message);
            }
            return null;
        }

        public static List<SelectListItem> Destinations(string? url = null)
        {
            try
            {
                if (url == null)
                    url = _configuration["Rutas:MultipyRetorno"];
                //string url = _configuration["Rutas:MultipyRetorno"];
                var destinos = _api_Get.Destinations().ToArray();
                List<SelectListItem> lstdestinos = new List<SelectListItem>();
                for (int i = 0; i < destinos.Length; i++)
                {
                    lstdestinos.Add(new SelectListItem() { Text = destinos[i], Value = i + 1.ToString() });
                }
                //SelectList listItems = new SelectList(origenes, "", "");
                return lstdestinos;
            }
            catch (Exception ex)
            {
                Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
                _logger.LogInformation(Message);
            }
            return null;
        }


    }
}
