// Get the number of selected objects
var count = 0;
Application.ActiveModel?.SelectedObjects(objects => count = objects?.Count ?? 0);

// Let the user know
Application.ShowMessageBox($"Looks like you have {count} objects selected");