// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    using Radios.RF24;
    using System;
    using System.Diagnostics;
    using System.Text;
    using Windows.UI.Core;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const byte CS_LINE = 0;
        private const byte CE_PIN = 16;
        private const byte IRQ_PIN = 21;
        private const byte CHANNEL = 1;
        private byte[] NODE1 = Encoding.UTF8.GetBytes("1Node");
        private byte[] NODE2 = Encoding.UTF8.GetBytes("2Node");

        public RF24 sender;
        bool isInitialized = false;

        public MainPage()
        {
            this.InitializeComponent();
            SendButton.Click += ButtonSend_Click;

            DataRate.ItemsSource = Enum.GetValues(typeof(DataRate));
            PowerLevel.ItemsSource = Enum.GetValues(typeof(PowerLevel));

            sender = new RF24();
            //receiver = new RF24();

            sender.OnDataReceived += Radio_OnDataReceived;
            sender.OnTransmitFailed += Radio_OnTransmitFailed;
            sender.OnTransmitSuccess += Radio_OnTransmitSuccess;
            
            isInitialized = true;            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await sender.Initialize(23, 0, 24, "SPI0");
            //await receiver.Initialize(5, 0, 6, "SPI1");

            sender.IsEnabled = true;
           // receiver.IsEnabled = true;

            sender.Channel = 76;
           // receiver.Channel = 76;

            sender.Address = NODE1;
           // receiver.Address = NODE2;

            Debug.WriteLine(sender.details());
           // Debug.WriteLine(receiver.details());
        }

        private void ButtonSend_Click(object sender1, RoutedEventArgs e)
        {
            var addr = Encoding.UTF8.GetBytes(SendToAddress.Text);
            Array.Reverse(addr);
            sender.SendTo(NODE2, Encoding.UTF8.GetBytes(SendBuffer.Text));
        }

        private void Radio_OnTransmitSuccess()
        {
            Debug.WriteLine("Transmit Succeeded!");
            if (isInitialized)
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => SendStatus.Text = "Transmit Succeeded");
        }

        private void Radio_OnTransmitFailed()
        {
            Debug.WriteLine("Transmit FAILED");
            if (isInitialized)
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => SendStatus.Text = "Transmit FAILED");
        }

        private void Radio_OnDataReceived(byte[] data)
        {
            Debug.WriteLine("Received: " + Encoding.UTF8.GetString(data));
            if (isInitialized)
                Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ReceiveBuffer.Text = Encoding.UTF8.GetString(data));
        }
    }
}
