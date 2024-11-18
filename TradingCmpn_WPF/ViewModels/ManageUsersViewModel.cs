using TradingCmpn_WPF.ViewModels;

public class ManageUsersViewModel : ViewModelBase
{
    private readonly IUserService _userService;

    public ManageUsersViewModel(IUserService userService)
    {
        _userService = userService;
    }
}