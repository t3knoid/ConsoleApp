# ConsoleApp
Sample C# Code to Display Console Application Output in a WPF Form

# Example Use Case - Robocopy thread
The following example runs Robocopy in a separate thread and show its output using the ConsoleApp class.

```
Thread robocopyWindowThread = new Thread(new ThreadStart(() =>
{
  string param = string.Format("\"{0}\" \"{1}\" /E /NP /ETA /MT:32 /tee /log:\"{2}\"", sourceFolder, destinationFolder, logFile);
  ConsoleApp consoleapp = new ConsoleApp("robocopy.exe", param, true, false, false); // Use consoleapp to display Robocopy console
  consoleapp.Title = "Copying files";
  consoleapp.Closed += (s, e) =>
    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background); // Ensures thread completes when the window closes
    consoleapp.Show();
    consoleapp.Start(false);
    System.Windows.Threading.Dispatcher.Run();
}));
robocopyWindowThread.SetApartmentState(ApartmentState.STA);  // Set the apartment state required when calling a ConsoleApp class from a separate thread
robocopyWindowThread.IsBackground = true; // Make the thread a background thread            
robocopyWindowThread.Start(); // Start the thread
```           
