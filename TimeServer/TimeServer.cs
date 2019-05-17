using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TimeServer
{
    public class TimeServer
    {
        private IPAddress IpAddress;

        private int Port;

        public TimeServer(int port)
        {
            Console.WriteLine("Host name: " + Dns.GetHostName());
            this.Port = port;
            try
            {
                IPHostEntry ipInfo = Dns.GetHostEntry(Dns.GetHostName());
                for(int i = 0; i < ipInfo.AddressList.Length; i++)
                {
                    if(ipInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        IpAddress = ipInfo.AddressList[i];
                    }
                }
                
            }
            catch (Exception) { }
            
            
        }

        public async void Run()
        {
            TcpClient client = null;
            try
            {
                while(true)
                {
                    TcpListener listener = new TcpListener(IpAddress, Port);
                    listener.Start();
                    Console.WriteLine("\t\tAwaiting client");
                    client = await listener.AcceptTcpClientAsync();

                    StreamReader reader = new StreamReader(client.GetStream());
                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.AutoFlush = true;
                    string msg = await reader.ReadLineAsync();

                    string date = DateTime.Now.ToString("dd/MM/yyyy");

                    await writer.WriteLineAsync(date);
                    Console.WriteLine("\t\tClient satisfied");
                    client?.Close();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client?.Close();
            }
            
        }

        static void Main(string[] args)
        {
            TimeServer s = new TimeServer(8080);
            Console.WriteLine("Starting server service");
            s.Run();

            Console.WriteLine("Method run async");
            Console.ReadKey();
        }
    }
}
