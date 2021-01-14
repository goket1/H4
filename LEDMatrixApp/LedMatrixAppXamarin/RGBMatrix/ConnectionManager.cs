using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace RGBMatrix
{
    class ConnectionManager
    {
        string hostName;
        int port;

        TcpClient tcpClient;

        public ConnectionManager(string hostName, int port)
        {
            this.hostName = hostName;
            this.port = port;
        }

        public void Connect()
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                tcpClient = new TcpClient(hostName, port);

            }
            catch (ArgumentNullException e)
            {
                System.Diagnostics.Debug.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                System.Diagnostics.Debug.WriteLine("SocketException: {0}", e);
            }
        }

        public void SendData(String message)
        {
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = tcpClient.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                System.Diagnostics.Debug.WriteLine("Sent: {0}", message);
            }

            catch (ArgumentNullException e)
            {
                System.Diagnostics.Debug.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                System.Diagnostics.Debug.WriteLine("SocketException: {0}", e);
            }
        }

        public void CloseConnection()
        {
            // Close everything.
            tcpClient.GetStream().Close();
            tcpClient.Close();
        }

        ~ConnectionManager()
        {
            CloseConnection();
        }
    }
}