using System;
using System.Windows;
using ArVision.Core.Logging;
using ArVision.Service.Client;
using ArVision.Service.Pharma.Shared.DTO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string CLASS_NAME = nameof(MainWindow);
        PharmaServiceProxy serviceClient;
        public MainWindow()
        {
            InitializeComponent();
            string methodName = LogManager.GetCurrentMethodName(CLASS_NAME);

            btnGetVersion.Click += btnGetVersion_Click;
            btnDosomething.Click += btnDosomething_Click;
            serviceClient = new PharmaServiceFactory().GetPharmaServiceProxy("127.0.0.1",9080);
        }
        private void btnGetVersion_Click(object sender, RoutedEventArgs e)
        {
            ApiVersion version = serviceClient.GetVersion();
            txtVersion.Text = version.VersionNumber;
        }
        private void btnDosomething_Click(object sender, RoutedEventArgs e)
        {
            SampleInputData data = new SampleInputData(){ IntergerData = Int32.Parse(txtIntData.Text), StringData = txtStringData.Text };
            serviceClient.Test(data); 

        }
    }
}
