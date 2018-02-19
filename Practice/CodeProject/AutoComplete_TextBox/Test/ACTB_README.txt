https://www.codeproject.com/Articles/44920/A-Reusable-WPF-Autocomplete-TextBox
https://www.codeproject.com/Articles/273978/A-Reusable-WPF-Autocomplete-TextBox-Part

* Fixit *
Put ViewModel.cs class back into ACTB project if it can easily re-used.
ACTB project should be used w/ DI for it's QueryList so it's completely re-usable.
SuggestEntries never gets anything added (needed for persisting via xml serialize/de-serialize)
When Enter key is hit, add to SuggestEntries and get all (e.g., m3u files).
See if it works to move code from Window1 code-behind into a VM.
