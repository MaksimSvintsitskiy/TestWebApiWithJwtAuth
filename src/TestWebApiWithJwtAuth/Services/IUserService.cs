using TestWebApiWithJwtAuth.Domain;

namespace TestWebApiWithJwtAuth.Services;

public interface IUserService
{
    User? GetUserById(int userId);

    User? GetUserByLoginPassword(string login, string password);

    bool IsUserExist(string login);

    int SaveUser(User user);
}