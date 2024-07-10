using DataLibrary.Dtos.GetAll;
using System.Data;

namespace DataLibrary.Repositry
{
    public interface IGenericRepositry<T> where T : class
    {
        IQueryable<T> GetAllWithInclude(GetAllDto getAllDto);
        Task<List<T>> GetAllAsync(GetAllDto getAllDto);
        Task<T> AddAsync(T item);
        Task<T> GetByIdAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        IDbTransaction BeginTransaction();
        public void SaveChanges();
        IQueryable<T> GetByIdWithInclude();

    }
}
