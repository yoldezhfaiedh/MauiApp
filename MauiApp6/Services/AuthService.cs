//public class AuthService
//{
//    public bool IsAuthenticated { get; private set; }
//    public string UserName { get; private set; }

//    public event Action OnChange;

//    public void Login(string userName)
//    {
//        IsAuthenticated = true;
//        UserName = userName;
//        NotifyStateChanged();
//    }

//    public void Logout()
//    {
//        IsAuthenticated = false;
//        UserName = string.Empty;
//        NotifyStateChanged();
//    }

//    private void NotifyStateChanged() => OnChange?.Invoke();
//}


public class AuthService
{
    public UserModel CurrentUser { get; private set; }

    public void SetUser(UserModel user)
    {
        CurrentUser = user;
    }

    public string GetUserRole()
    {
        return CurrentUser?.Role ?? "User";
    }

    public bool IsAuthenticated => CurrentUser != null;
}
