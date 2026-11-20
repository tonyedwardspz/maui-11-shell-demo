using ShellDemo.ViewModels;
using ShellDemo.Views;
namespace ShellDemo;
public partial class AppShell : Shell
{
    public AppShellViewModel State { get; }
    public AppShell(AppShellViewModel state)
    {
        InitializeComponent();
        State = state;
        BindingContext = state;
        Routing.RegisterRoute("booking/{bookingId}", typeof(BookingDetailsPage));
        // Use a different literal prefix so constrained and unconstrained routes do not compete.
        Routing.RegisterRoute("integer/{bookingId:int}", typeof(BookingDetailsPage));
        Routing.RegisterRoute("customer/{customerId:guid}", typeof(ParameterPage));
        Routing.RegisterRoute("long/{value:long}", typeof(ParameterPage));
        Routing.RegisterRoute("double/{value:double}", typeof(ParameterPage));
        Routing.RegisterRoute("bool/{value:bool}", typeof(ParameterPage));
        Routing.RegisterRoute("alpha/{value:alpha}", typeof(ParameterPage));
        Routing.RegisterRoute("search/{category?}", typeof(ParameterPage));
        Routing.RegisterRoute("results/{page=1}", typeof(ParameterPage));
        Routing.RegisterRoute("files/{*path}", typeof(ParameterPage));
        Routing.RegisterRoute("booking-{bookingId}-summary", typeof(BookingDetailsPage));
        Routing.RegisterRoute("booking/summary", typeof(LiteralSummaryPage));
        Routing.RegisterRoute("bookingdetails", typeof(BookingDetailsPage));
        Routing.RegisterRoute("nextdetail", typeof(ParameterPage));
    }
}
