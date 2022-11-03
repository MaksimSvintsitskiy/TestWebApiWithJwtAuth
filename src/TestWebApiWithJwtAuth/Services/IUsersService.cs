using TestWebApiWithJwtAuth.Domain;

namespace TestWebApiWithJwtAuth.Services;

public interface IUsersService
{
    User? GetUserById(int userId);

    User? GetUserByLoginPassword(string login, string password);

    bool IsUserExist(string login);

    void SaveUser(User user);
}