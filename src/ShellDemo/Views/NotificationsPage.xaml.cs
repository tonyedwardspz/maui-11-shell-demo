namespace ShellDemo.Views;
public partial class NotificationsPage : ContentPage
{
    public NotificationsPage() { InitializeComponent(); BindingContext = ((AppShell)Shell.Current).State; }
}
