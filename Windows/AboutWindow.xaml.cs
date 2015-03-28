using System.Reflection;
using System.Windows;

namespace VideoJam.Windows
{
    /// <summary>
    ///     Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public sealed partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();

            VersionTextBlock.Text = "version " + Assembly.GetExecutingAssembly().GetName().Version;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}