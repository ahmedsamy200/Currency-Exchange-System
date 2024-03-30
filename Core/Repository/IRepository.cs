using System;
using System.Collections.Generic;
using System.Text;
using Core.Domian;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(long? id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<ExchageHistory>> GetAllHistories();
        void SaveChanges();
    }
}
