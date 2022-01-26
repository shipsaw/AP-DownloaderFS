using System.Windows;
using ApDownloader.UI.Views;

namespace ApDownloader.UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ContentView.Content = new LoginView();
    }

    private void LoginView(object sender, RoutedEventArgs e)
    {
        ContentView.Content = new LoginView();
    }

    private void DownloadView(object sender, RoutedEventArgs e)
    {
        ContentView.Content = new DownloadView();
    }

    private void InstallView(object sender, RoutedEventArgs e)
    {
        ContentView.Content = new InstallView();
    }

    private void OptionsView(object sender, RoutedEventArgs e)
    {
        ContentView.Content = new OptionsView();
    }
}