using APIAspNetCore5.Model;
using System.Collections.Generic;

namespace APIAspNetCore5.Business
{
    public interface IBookBusiness
    {
        Book Create(Book book);
        Book FindById(long id);
        List<Book> FindAll();
        Book Update(Book book);
        void Delete(long id);
    }
}
