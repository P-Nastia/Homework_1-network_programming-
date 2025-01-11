using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Security;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket socketServer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if(IPAddress.Parse(this.ipAddressTB.Text) != null && !String.IsNullOrEmpty(this.portTB.Text))
            {
                var ip = IPAddress.Parse(this.ipAddressTB.Text);
                var port = int.Parse(this.portTB.Text);
                try
                {
                    var serverEndPoint = new IPEndPoint(ip, port);

                    socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socketServer.Connect(serverEndPoint);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (!String.IsNullOrEmpty(this.messageTB.Text))
            {
                try
                {
                    this.answerTB.Text = "";
                    var data = Encoding.Unicode.GetBytes(this.messageTB.Text);

                    socketServer.Send(data);

                    data = new byte[1024];
                    int bytes = 0;
                    do
                    {
                        bytes = socketServer.Receive(data);
                        this.answerTB.Text = Encoding.Unicode.GetString(data);

                    } while (socketServer.Available > 0);
                    this.messageTB.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}