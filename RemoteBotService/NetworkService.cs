using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RemoteBotService
{
    public interface INetworkService : IRoverElement
    {
        void Start(string roverIp);
        void Stop();
        void SendMessage(string message);

        List<INetworkListener> NetworkListenerList { get; set; }
        bool IsConnected { get; }
    }

    public class NetworkService : INetworkService
    {
        public List<INetworkListener> NetworkListenerList { get; set; }
        private TcpClient _client;
        Thread _listenerThread;
        private bool _isRunning;
        private const string SEPARATOR = "||";
        public bool IsConnected
        {
            get
            {
                return _client != null && _client.Connected;
            }
        }

        public NetworkService(List<INetworkListener> networkListenerList)
        {
            NetworkListenerList = networkListenerList;
        }

        public void Start(string roverIp)
        {
            try
            {
                Int32 port = 50005;

                _client = new TcpClient(roverIp, port);
                _listenerThread = new Thread(Listen);
                _isRunning = true;
                _listenerThread.Start();

            }
            catch (Exception ex)
            {

                throw ex;
            }


            /*
            // Receive the TcpServer.response. 

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);
            */
        }

        public void Listen()
        {
            string messageQueue = String.Empty;
            while (_isRunning)
            {
                Thread.Sleep(10);
                // Buffer to store the response bytes.
                Byte[] data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                if (_client.Connected)
                {
                    var stream = _client.GetStream();
                    bool noMoreMessage = false;
                    try
                    {
                        while (!noMoreMessage)
                        {
                            Int32 bytes = stream.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            if (!String.IsNullOrWhiteSpace(responseData))
                                messageQueue += responseData;
                            int separatorPos = messageQueue.IndexOf(SEPARATOR, StringComparison.InvariantCulture);
                            if (separatorPos > 0)
                            {
                                string message = messageQueue.Substring(0, separatorPos);
                                messageQueue = messageQueue.Substring(separatorPos + 2);
                                ParseMessage(message);
                            }
                            else
                            {
                                noMoreMessage = true;
                            }
                        }
                        //Console.WriteLine("Received: {0}", responseData);
                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }
                }



            }

        }

        private void ParseMessage(string message)
        {
            NetworkListenerList.ForEach(l => l.ReceiveNetworkMessage(message));
        }

        public void Stop()
        {
            // Close everything.
            if (_client != null)
            {
                try
                {
                    NetworkStream stream = _client.GetStream();
                    stream.Close();

                }
                catch (Exception)
                {
                }
                _client.Close();
            }
        }

        public void SendMessage(string message)
        {
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message + "||");

                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();
                if (_client != null)
                {
                    NetworkStream stream = _client.GetStream();

                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);   
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public void Initialize(string roverIp)
        {
            Start(roverIp);
        }

        public void LoadContent(ContentManager content)
        {

        }

        public void Update(GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, GameTime gameTime)
        {

        }

        public void UnloadContent()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Dispose()
        {
            this._isRunning = false;
            Stop();
            if (_listenerThread != null)
                _listenerThread.Join();
        }
    }

    public interface INetworkListener
    {
        void ReceiveNetworkMessage(string responseData);
    }
}
