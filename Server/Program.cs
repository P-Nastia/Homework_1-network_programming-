// See https://aka.ms/new-console-template for more information
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var localHost = await Dns.GetHostEntryAsync(Dns.GetHostName());
int i = 0;
foreach (var item in localHost.AddressList)
{
    Console.WriteLine($"{++i}.{item}");
}


Console.WriteLine("Обери IP-адресу: ->_");
int numberIP = int.Parse(Console.ReadLine());
Console.WriteLine("Обери порт: ->_");
int serverPort = int.Parse(Console.ReadLine());
Console.Title = localHost.AddressList[numberIP - 1].ToString() + $":{serverPort}";

var serverIP = localHost.AddressList[numberIP - 1];

IPEndPoint iPEndPoint = new IPEndPoint(serverIP, serverPort);
Socket server = new Socket(AddressFamily.InterNetwork,
    SocketType.Stream, ProtocolType.Tcp);

try
{
    server.Bind(iPEndPoint);
    server.Listen(10);
    while (true)
    {
        Console.WriteLine("Сервер очікує");
        Socket client = server.Accept();
        Console.WriteLine($"Клієнт -> {client.RemoteEndPoint}");
        int bytes = 0; 
        byte[] buffer = new byte[1024];
        do
        {
            bytes = client.Receive(buffer);
            Console.WriteLine($"Повідомлення: '{Encoding.Unicode.GetString(buffer)}'");
        } while (client.Available > 0);

        buffer = Encoding.Unicode.GetBytes($"{DateTime.Now}");
        client.Send(buffer);

        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Проблема {ex.Message}");
}

Console.ReadKey();