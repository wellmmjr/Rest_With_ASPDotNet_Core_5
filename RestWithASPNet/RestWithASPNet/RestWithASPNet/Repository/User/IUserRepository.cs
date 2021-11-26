using RestWithASPNet.Data.VO;


namespace RestWithASPNet.Repository.User
{
    public interface IUserRepository
    {
        Model.User ValidateCredentials(UserVO userVO);

        Model.User ValidateCredentials(string username);

        bool RevokeToken(string username);

        Model.User RefreshUserInfo(Model.User user);
    }
}
