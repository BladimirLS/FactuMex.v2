using System.Threading.Tasks;
using CallCenter.DBModel;

namespace CallCenter.Data
{
    public class DataRepository<T> : IDataRepository<T> where T: class
    {
        private readonly FactuMexContext _context;

        public DataRepository(FactuMexContext context)
        {
            _context = context;
        }


        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> SaveAsync(T entity)
        {
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
