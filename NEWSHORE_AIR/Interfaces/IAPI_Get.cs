using NEWSHORE_AIR.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_AIR.Interfaces
{
  public interface IAPI_Get
  {
    List<ResponseNewShoreAPI> Get(string url);
    List<string> Origins(string? url = null);
    List<string> Destinations(string? url = null);
    Journeyy Rutas(string? url = null, string? origen = null, string? destino = null);
    Journeyy RutasRegreso(string? url = null, string? origen = null, string? destino = null);
    }
}
