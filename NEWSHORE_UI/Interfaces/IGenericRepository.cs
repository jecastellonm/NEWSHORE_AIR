using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NEWSHORE_UI.Interfaces
{
  public interface IGenericRepository<T> where T: class 
  {
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> Add(T entity);
  }
}
