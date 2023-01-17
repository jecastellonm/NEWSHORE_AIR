using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Interfaces
{
  interface IData
  {
    static List<SelectListItem> Origins() { return null; }
    List<SelectListItem> Destinations(string? url = "");

  }
}
