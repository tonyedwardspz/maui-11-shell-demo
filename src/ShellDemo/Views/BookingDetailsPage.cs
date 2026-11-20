using ShellDemo.ViewModels;
namespace ShellDemo.Views;
public class BookingDetailsPage : WorkshopPage
{
    public BookingDetailsPage() : base("08 · Booking details")
    {
        var vm = new BookingDetailsViewModel();
        BindingContext = vm;
        Shell.SetBackgroundColor(this, Color.FromArgb("#102A43"));
        Shell.SetTitleColor(this, Colors.White);
        Shell.SetForegroundColor(this, Colors.White);
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IconOverride = "back.png", TextOverride = "Back to bookings",
            Command = vm.BackCommand, AccessibilityLabel = "Return to bookings"
        });
        Heading("Captured booking");
        var label = new Label { FontSize = 24 };
        label.SetBinding(Label.TextProperty, nameof(vm.BookingId)); Body.Add(label);
        var payload = new Label(); payload.SetBinding(Label.TextProperty, nameof(vm.Payload)); Body.Add(payload);
        Note("The payload is received by the view model using IQueryAttributable. Route constraints validate strings; they do not change their CLR type.");
        Code("""
public void ApplyQueryAttributes(IDictionary<string, object> query)
{
    BookingId = query.TryGetValue("bookingId", out var value)
        ? value?.ToString() : null;
}
""");
        Go("Push another detail (build a stack)", "nextdetail?level=2");
        Action("Pop this section to root", () => Shell.Current.Navigation.PopToRootAsync());
        Note("On Android, reselect the Bookings tab to return to its root. Test hardware back and gestures separately from the custom toolbar command.");
    }
}
public class ParameterPage : WorkshopPage, IQueryAttributable
{
    public ParameterPage() : base("04 · Received parameters")
    {
        Heading("Actual navigation payload");
        Go("Push another detail", "nextdetail?level=3");
        Action("Pop to root", () => Shell.Current.Navigation.PopToRootAsync());
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Status.Text = Services.ParameterCapture.Describe(query);
        Services.ParameterCapture.Record(nameof(ParameterPage), query);
    }
}
public class LiteralSummaryPage : WorkshopPage, IQueryAttributable
{
    public LiteralSummaryPage() : base("04 · Literal wins")
    { Heading("booking/summary"); Note("The literal route was selected instead of booking/{bookingId}."); }
    public void ApplyQueryAttributes(IDictionary<string, object> query) => Services.ParameterCapture.Record(nameof(LiteralSummaryPage), query);
}
