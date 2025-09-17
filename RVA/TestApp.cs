using System.Windows;

namespace TestApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var window = new Window();
            window.Title = "Test WPF App";
            window.Width = 300;
            window.Height = 200;
            window.Show();
        }
    }
}