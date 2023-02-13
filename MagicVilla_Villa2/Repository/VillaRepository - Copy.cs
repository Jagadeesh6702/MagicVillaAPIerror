using MagicVilla_Villa2.Data;
using MagicVilla_Villa2.Model;
using MagicVilla_Villa2.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_Villa2.Repository
{
    public class VillaRepository :Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db)
        { 
            _db = db;
        }
    

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedTime= DateTime.Now;
            _db.Villas.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
