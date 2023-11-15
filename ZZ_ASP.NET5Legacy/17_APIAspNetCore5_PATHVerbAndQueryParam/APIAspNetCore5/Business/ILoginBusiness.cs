using APIAspNetCore5.Data.VO;

namespace APIAspNetCore5.Business
{
    public interface ILoginBusiness
    {
        object FindByLogin(UserVO user);
    }
}