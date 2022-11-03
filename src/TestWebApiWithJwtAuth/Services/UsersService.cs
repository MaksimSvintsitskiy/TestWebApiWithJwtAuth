using TestWebApiWithJwtAuth.Domain;

namespace TestWebApiWithJwtAuth.Services;

internal class UsersService : IUsersService
{
    private readonly List<User> _users = new();

    User? IUsersService.GetUserById(int userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId);
    }

    User? IUsersService.GetUserByLoginPassword(string login, string password)
    {
        return _users.FirstOrDefault(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase) && string.Equals(u.Password, password, StringComparison.OrdinalIgnoreCase));
    }

    bool IUsersService.IsUserExist(string login)
    {
        return _users.Any(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));
    }

    void IUsersService.SaveUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (user.Id == 0)
        {
            var nextId = _users.Count > 0 ? _users.Max(u => u.Id) : 0;

            user.Id = nextId + 1;

            _users.Add(user);

            return;
        }

        var userById = _users.FirstOrDefault(u => u.Id == user.Id);

        if (userById != null)
        {
            userById.Email = user.Email;
            userById.Name = user.Name;
            userById.Phone = user.Phone;
            userById.Surname = user.Surname;
            userById.Password = user.Password;
        }
    }
}