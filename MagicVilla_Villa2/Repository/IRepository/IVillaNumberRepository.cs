using MagicVilla_Villa2.Model;
using System.Linq.Expressions;

namespace MagicVilla_Villa2.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber> 
    {
      
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
     
        
    }
}
