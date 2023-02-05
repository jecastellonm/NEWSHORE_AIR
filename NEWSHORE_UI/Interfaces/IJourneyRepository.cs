using NEWSHORE_AIR.DataAccess;
using NEWSHORE_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Interfaces
{
  public interface IJourneyRepository: IGenericRepository<Journeyy>
  {
    Task<List<Journeyy>> GetAll();
    Journeyy GetbyIdOriginDestination(string origin, string destino);
  }
}
