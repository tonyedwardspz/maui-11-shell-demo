namespace ShellDemo.Views;
public class BookingsPage : WorkshopPage
{
    public BookingsPage() : base("04 · Route playground")
    {
        Heading("A URI becomes page data");
        Note("The destination displays the actual IQueryAttributable payload, including CLR types. Back returns here. Failed navigation is displayed above.");
        var id = new Entry { Text = "12345", Placeholder = "Booking ID" };
        Body.Add(id);
        Action("Open booking using entered ID", () => Shell.Current.GoToAsync($"//bookings/booking/{Uri.EscapeDataString(id.Text ?? "")}"));
        Code("""
Routing.RegisterRoute("booking/{bookingId}", typeof(BookingDetailsPage));
await Shell.Current.GoToAsync("//bookings/booking/12345");
""");
        Action("Run all route checks", async () =>
        {
            Status.Text = "Running…";
            var result = await Services.RouteChecks.RunAsync();
            Status.Text = result;
            await DisplayAlertAsync("Route validation", result, "OK");
        });
        foreach (var test in Services.RouteScenarios.All)
            Go(test.Name + (test.ExpectedFailure ? " · expect rejection" : ""), test.Route);
    }
}
