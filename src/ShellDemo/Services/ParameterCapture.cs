namespace ShellDemo.Services;
// Observes real delivery; no route matching or parameter defaults are simulated by the sample.
public static class ParameterCapture
{
    public static string Page { get; private set; } = "";
    public static Dictionary<string, object> Values { get; private set; } = new();
    public static void Clear() { Page = ""; Values = new(); }
    public static void Record(string page, IDictionary<string, object> values) { Page = page; Values = new(values); }
    public static string Describe(IDictionary<string, object> values) => values.Count == 0
        ? "No parameters received."
        : string.Join("\n", values.Select(p => $"{p.Key} = {p.Value} ({p.Value?.GetType().Name ?? "null"})"));
}
public static class LabOptions
{
    public static bool FadeTransitions { get; set; }
    public static bool ReselectToRoot { get; set; } = true;
    public static string LastNativeEvent { get; set; } = "No native callback yet.";
}
