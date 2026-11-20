namespace ShellDemo.Views;
// Small shared layout helpers keep the native demos readable. Navigation remains real Shell navigation.
public class WorkshopPage : ContentPage
{
    protected VerticalStackLayout Body { get; } = new() { Padding = 24, Spacing = 14 };
    protected Label Status { get; } = new() { TextColor = Colors.DarkSlateBlue };
    public WorkshopPage(string title)
    {
        Title = title;
        Content = new ScrollView { Content = Body };
        Body.Add(Status);
    }
    protected void Heading(string text) => Body.Add(new Label { Text = text, FontSize = 26, FontAttributes = FontAttributes.Bold });
    protected void Note(string text) => Body.Add(new Label { Text = text });
    protected void Code(string text) => Body.Add(new Border
    {
        Padding = 12, BackgroundColor = Color.FromArgb("#EAF0F5"),
        Content = new Label { Text = text, FontFamily = "monospace", TextColor = Color.FromArgb("#102A43"), FontSize = 13 }
    });
    protected void Action(string text, Func<Task> action)
    {
        var button = new Button { Text = text, HorizontalOptions = LayoutOptions.Fill };
        button.Clicked += async (_, _) =>
        {
            button.IsEnabled = false;
            try { await action(); }
            catch (Exception ex) { Status.Text = $"{ex.GetType().Name}: {ex.Message}"; }
            finally { button.IsEnabled = true; }
        };
        Body.Add(button);
    }
    protected void Go(string text, string route, bool animate = true) => Action(text, () => Shell.Current.GoToAsync(route, animate));
}
