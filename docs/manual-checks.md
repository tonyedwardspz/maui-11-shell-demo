# Reader and native UI checks

Run on each intended platform. Automated route checks do not verify native badge pixels, animations, gestures, spoken labels, or Windows rendering.

1. **Badges (sections 1–2).** Open Inbox. Confirm initial count 5 appears on tab and toolbar. Add notifications, mark all read, show dot, hide badge, show New, show 1234. Confirm the two badges update together. Check the Windows non-numeric dot behaviour. Test light/dark modes and large text.
2. **Overflow (3).** Native lab: invoke Search, then Settings, Feedback, About. Confirm ordering and the resulting status text. Do not assume icons in secondary menus are portable.
3. **Routes (4).** Run every individual route, then Run all route checks. Inspect received key, value, CLR type and destination. The optional-absent case should have no category; the default-absent case should deliver page="1". Try invalid integer, Unicode, spaces and encoded delimiters in the booking entry.
4. **Architecture and appearance (5–6).** Inspect handler type in Native lab. Android should show the native subtitle; compare the article’s red-title attempt with Shell.TitleColor (see the validation report). Apple handler mode should display a red tab-bar border. Compare legacy builds. Ensure navigation remains usable and appearance updates when switching tabs.
5. **Reselect (7).** Android: open booking, push another detail, reselect Bookings. Confirm the current section returns to its root. Tap repeatedly during transitions. Switch between tabs to check independent stack preservation. Disable the reselect switch and compare base behaviour.
6. **Back (8).** Activate the custom toolbar back command. With VoiceOver/TalkBack, verify "Return to bookings" is announced. Separately test Android system back, iOS back gestures and programmatic pop. The custom toolbar command is not a universal back interceptor.
7. **Transitions (9).** Compare animated and instant pushes from Native lab, then enable Android fade and repeat push/pop. Verify system back and rapid taps. On Apple the sample deliberately retains native transitions.
8. **Migration (10).** Clean and build both handler and renderer modes. Check tab selection, navigation stacks, overflow, page lifecycle, appearance, rotation and accessibility. A compile-only check cannot establish native runtime parity.
9. **TabbedPage (11).** Open the standalone demo. Inbox starts with badge 3. Show dot, hide it and return to the workshop. Confirm the restored Shell remains usable. The demo swaps the window root so TabbedPage is never nested inside Shell.
10. **C# template (11).** In a scratch directory, run `dotnet new maui --ui csharp`; compare its Shell definition with this app's XAML Shell and C# exercise pages.
