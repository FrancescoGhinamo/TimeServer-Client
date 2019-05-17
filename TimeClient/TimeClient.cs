using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TimeClient
{
    public class TimeClient
    {

        public void SendRequest()
        {
            TcpClient me = null;
            try
            {
                Console.Write("\n\nInsert host name: ");
                string hostName = Console.ReadLine();
                Console.Write("\n\nInsert port: ");
                int port = int.Parse(Console.ReadLine());

                IPAddress addr = null;
                IPHostEntry ipInfo = Dns.GetHostEntry(hostName);
                for (int i = 0; i < ipInfo.AddressList.Length; i++)
                {
                    if (ipInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        addr = ipInfo.AddressList[i];
                    }
                }

                me = new TcpClient();
                Console.WriteLine("\t\tAwaiting connection");
                me.Connect(addr, port);

                StreamWriter writer = new StreamWriter(me.GetStream());
                StreamReader reader = new StreamReader(me.GetStream());
                writer.AutoFlush = true;
                Console.Write("\t\tSend a message to the server: ");
                writer.WriteLine(Console.ReadLine());
                string res = reader.ReadLine();
                Console.WriteLine("\t\tServer response: " + res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                me?.Close();
            }
            
        }

        static void Main(string[] args)
        {
            TimeClient c = new TimeClient();
            c.SendRequest();
            Console.ReadKey();
        }
    }
}
