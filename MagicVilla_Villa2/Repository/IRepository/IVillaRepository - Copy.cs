using MagicVilla_Villa2.Model;
using System.Linq.Expressions;

namespace MagicVilla_Villa2.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa> 
    {
      
        Task<Villa> UpdateAsync(Villa entity);
     
        
    }
}
