using TestWebApiWithJwtAuth.Domain;

namespace TestWebApiWithJwtAuth.Services;

internal class UserService : IUserService
{
    private readonly List<User> _users = new();

    User? IUserService.GetUserById(int userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId);
    }

    User? IUserService.GetUserByLoginPassword(string login, string password)
    {
        return _users.FirstOrDefault(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase) && string.Equals(u.Email, password, StringComparison.OrdinalIgnoreCase));
    }

    bool IUserService.IsUserExist(string login)
    {
        return _users.Any(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));
    }

    int IUserService.SaveUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userById = _users.FirstOrDefault(u => u.Id == user.Id);

        if (userById == null || user.Id == 0)
        {
            var nextId = _users.Count > 0 ? _users.Max(u => u.Id) : 1;

            user.Id = nextId;

            _users.Add(user);
        }
        else
        {
            userById.Email = user.Email;
            userById.Name = user.Name;
            userById.Phone = user.Phone;
            userById.Surname = user.Surname;
        }

        return user.Id;
    }
}