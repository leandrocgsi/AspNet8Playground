using APIAspNetCore5.Model.Base;
using System.Collections.Generic;

namespace APIAspNetCore5.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T item);
        T FindById(long id);
        List<T> FindAll();
        T Update(T item);
        void Delete(long id);

        bool Exists(long? id);
    }
}
