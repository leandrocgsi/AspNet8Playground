using RestWithASPNETErudio.Data.VO;
using System.Collections.Generic;

namespace RestWithASPNETErudio.Business
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
        PersonVO FindByID(long id);
        List<PersonVO> FindAll();
        PersonVO Update(PersonVO person);
        void Delete(long id);
    }
}
