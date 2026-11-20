using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace ShellDemo.ViewModels;
public enum BadgeMode { Count, Dot, Hidden, Text }
public partial class AppShellViewModel : ObservableObject
{
    [ObservableProperty, NotifyPropertyChangedFor(nameof(NotificationBadge))]
    private int unreadNotificationCount = 5;
    [ObservableProperty, NotifyPropertyChangedFor(nameof(NotificationBadge))]
    private BadgeMode mode;
    public string? NotificationBadge => Mode switch
    {
        BadgeMode.Dot => "",
        BadgeMode.Hidden => null,
        BadgeMode.Text => "New",
        _ => UnreadNotificationCount > 0 ? UnreadNotificationCount.ToString() : null
    };
    [RelayCommand] private void AddNotification() { Mode = BadgeMode.Count; UnreadNotificationCount++; }
    [RelayCommand] private void MarkAllRead() { UnreadNotificationCount = 0; Mode = BadgeMode.Count; }
    [RelayCommand] private void ShowDot() => Mode = BadgeMode.Dot;
    [RelayCommand] private void HideBadge() => Mode = BadgeMode.Hidden;
    [RelayCommand] private void ShowText() => Mode = BadgeMode.Text;
    [RelayCommand] private void ShowLongCount() { UnreadNotificationCount = 1234; Mode = BadgeMode.Count; }
    [RelayCommand] private async Task OpenNotificationsAsync() => await Shell.Current.GoToAsync("//notifications");
}
