using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NEWSHORE_AIR.Interfaces;
using NEWSHORE_UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.DataAccess
{
  public static class Data 
  {

    public static List<SelectListItem> Origins(string[] origenes)
    {
      try
      {
        string url = "https://recruiting-api.newshore.es/api/flights/2";
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
        //Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
        //_logger.LogError(Message);
      }
      return default;
    }

    public static List<SelectListItem> Destinations(string[] destinos)
    {
      try
      {
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
        //Message = $"Index HomeController Error {DateTime.Now.ToLongDateString()} {DateTime.UtcNow.ToLongTimeString()}  Error:  " + ex.Message;
        //_logger.LogError(Message);
      }
      return default;
    }
  }
}
