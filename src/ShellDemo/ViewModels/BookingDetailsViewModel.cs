using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace ShellDemo.ViewModels;
public partial class BookingDetailsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string? bookingId;
    [ObservableProperty] private string payload = "No parameters received.";
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        BookingId = query.TryGetValue("bookingId", out var value) ? value?.ToString() : null;
        Payload = Services.ParameterCapture.Describe(query);
        Services.ParameterCapture.Record(nameof(Views.BookingDetailsPage), query);
    }
    [RelayCommand] private async Task BackAsync() => await Shell.Current.GoToAsync("..");
}
