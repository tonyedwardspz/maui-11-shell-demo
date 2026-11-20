namespace ShellDemo.Services;
public record RouteScenario(string Name, string Route, string Page = "ParameterPage", string? Key = null, string? Value = null, bool ExpectedFailure = false);
public static class RouteScenarios
{
    public static readonly RouteScenario[] All =
    [
        new("Required booking", "//bookings/booking/12345", "BookingDetailsPage", "bookingId", "12345"),
        new("Escaped space", "//bookings/booking/coastal%20stay", "BookingDetailsPage", "bookingId", "coastal stay"),
        new("Integer constraint", "//bookings/integer/12345", "BookingDetailsPage", "bookingId", "12345"),
        new("Invalid integer", "//bookings/integer/abc", ExpectedFailure: true),
        new("GUID constraint", "//bookings/customer/12345678-1234-1234-1234-123456789abc", Key: "customerId", Value: "12345678-1234-1234-1234-123456789abc"),
        new("Long constraint", "//bookings/long/9223372036854775807", Key: "value", Value: "9223372036854775807"),
        new("Double constraint", "//bookings/double/12.5", Key: "value", Value: "12.5"),
        new("Boolean constraint", "//bookings/bool/true", Key: "value", Value: "true"),
        new("Alpha constraint", "//bookings/alpha/coastal", Key: "value", Value: "coastal"),
        new("Optional absent", "//home/search"),
        new("Optional supplied", "//home/search/coastal", Key: "category", Value: "coastal"),
        new("Default page omitted", "//home/results", Key: "page", Value: "1"),
        new("Default page supplied", "//home/results/2", Key: "page", Value: "2"),
        new("Catch-all path", "//home/files/documents/2026/report.pdf", Key: "path", Value: "documents/2026/report.pdf"),
        new("Mixed segment", "//bookings/booking-12345-summary", "BookingDetailsPage", "bookingId", "12345"),
        new("Literal precedence", "//bookings/booking/summary", "LiteralSummaryPage"),
        new("Path wins over query", "//bookings/booking/12345?bookingId=999", "BookingDetailsPage", "bookingId", "12345"),
        new("Relative template unsupported", "booking/12345", ExpectedFailure: true),
        new("Traditional query route", "bookingdetails?bookingId=67890", "BookingDetailsPage", "bookingId", "67890")
    ];
}
public static class RouteChecks
{
    private static bool running;
    public static async Task<string> RunAsync()
    {
        if (running) return "Checks already running.";
        running = true;
        var results = new List<string>();
        try
        {
            results.AddRange(await BadgeChecks.RunAsync());
            foreach (var test in RouteScenarios.All)
            {
                await Shell.Current.GoToAsync("//bookings", false);
                ParameterCapture.Clear();
                try
                {
                    await Shell.Current.GoToAsync(test.Route, false);
                    var actualPage = Shell.Current.CurrentPage.GetType().Name;
                    var matches = !test.ExpectedFailure && actualPage == test.Page;
                    if (test.Key is not null)
                        matches &= ParameterCapture.Values.TryGetValue(test.Key, out var value) && value is string s && s == test.Value;
                    results.Add($"{(matches ? "PASS" : "FAIL")} {test.Name}: {actualPage}; {ParameterCapture.Describe(ParameterCapture.Values)}");
                }
                catch (ArgumentException ex)
                { results.Add($"{(test.ExpectedFailure ? "PASS" : "FAIL")} {test.Name}: {ex.GetType().Name}: {ex.Message}"); }
                catch (Exception ex) { results.Add($"FAIL {test.Name}: {ex.GetType().Name}: {ex.Message}"); }
            }
        }
        finally { running = false; await Shell.Current.GoToAsync("//bookings", false); }
        var report = string.Join("\n", results);
        System.Diagnostics.Debug.WriteLine(report);
        await File.WriteAllTextAsync(Path.Combine(FileSystem.AppDataDirectory, "route-checks.txt"), report);
        return report;
    }
}
