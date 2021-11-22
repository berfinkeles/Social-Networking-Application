using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        class Sweet
        {
            public string username;
            public string message;
            public DateTime date;
            public int id;

            public Sweet(string username, string message, DateTime date,int id)
            {
                this.username = username;
                this.message = message;
                this.date = date;
                this.id = id;
            }
        }

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>();
        Dictionary<string, Socket> clientSocketsDictionary = new Dictionary<string, Socket>();
        List<string> users = new List<string>(); // usernames in the "user-db.txt"
        List<string> connected_users = new List<string>(); // names of connected users
        List<Sweet> sweets = new List<Sweet>();

        bool terminating = false;
        bool listening = false;

        int id_counter = 1;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void read_file()
        {
            string line;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "user-db.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\LENOVO\cs408 project\user-db.txt");
            while ((line = file.ReadLine()) != null)
            {
                users.Add(line);
            }
            file.Close();
        }





        private void button_listen_Click(object sender, EventArgs e)
        {
            int serverPort;
            read_file();
              
            if(Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(3);

                listening = true;
                button_listen.Enabled = false;
                

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }

        private string receive_message(Socket clientSocket) // receives only one message
        {
            Byte[] buffer = new Byte[10000000];
            clientSocket.Receive(buffer);
            string incomingMessage = Encoding.Default.GetString(buffer);
            incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
            return incomingMessage;
        }

        private bool is_authorized(Socket thisClient, ref string name) // check if user is in database
        {
            try
            {
                string incomingMessage = receive_message(thisClient);

                if (users.Contains(incomingMessage)) // check if name is registered
                {
                    name = incomingMessage;
                    return true;
                }
                else
                {
                    name = incomingMessage;
                    return false;
                }
            }
            catch (Exception ex)
            {
                logs.AppendText("Fail: " + ex.ToString() + "\n");
                logs.ScrollToCaret();
                throw;
            }
        }

        private void send_message(Socket clientSocket, string message) // takes socket and message then sends the message to that socket
        {
            Byte[] buffer = new Byte[10000000];
            buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
        }

        private void Accept()
        {
            while(listening)
            {
                try
                {
                    string client_name = "";
                    Socket newClient = serverSocket.Accept();
                    // clientSockets.Add(newClient);

                    if(is_authorized(newClient,ref client_name))
                    {
                        //user is in the database
                        if (!clientSocketsDictionary.ContainsKey(client_name)) // checks if the user already connected
                        {
                            
                            send_message(newClient, "authorized\n");
                            clientSocketsDictionary.Add(client_name, newClient);
                            connected_users.Add(client_name);
                            logs.AppendText(client_name + " is connected.\n");
                            logs.ScrollToCaret();

               
                            //Thread receiveThread = new Thread(Receive);
                            //receiveThread.Start();
                        }
                        else
                        {
                            logs.AppendText(client_name + " is trying to connect again\n");
                            logs.ScrollToCaret();
                            //send_message(newClient, "already connected");
                            newClient.Close();
                        }

                    }
                    else
                    {
                        logs.AppendText(client_name + " is trying to connect but not registered\n");
                        logs.ScrollToCaret();
                        //send_message(newClient, "not authorized");
                        newClient.Close();
                    }

                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    receiveThread.Start();

                    //Thread receiveThread = new Thread(Receive);
                    //receiveThread.Start();
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }

        private void Receive(Socket thisClient) // updated
        {
            bool connected = true;

            while(connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[10000000];
                    thisClient.Receive(buffer);
                    string username="";

                    foreach (string keyVar in clientSocketsDictionary.Keys)
                    {
                        if (clientSocketsDictionary[keyVar] == thisClient)
                        {
                            username = keyVar;
                        }
                    }
                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (incomingMessage == "R-E-Q-U-E-S-T")
                    {
                        string feed_messages = "F-E-E-D";
                        try
                        { 
                            for (int i = 0; i <= sweets.Count; i++)
                            {
                                if (sweets[i].username != username)
                                {
                                    feed_messages += sweets[i].id.ToString() + "- " + sweets[i].date.ToString() + " " + sweets[i].username + " : " + sweets[i].message + " \n";

                                }
                            }
                            send_message(thisClient, feed_messages);
                        }
                        catch
                        {
                            logs.AppendText("ups");
                        }

                    }
                    else
                    {
                        DateTime postedDate = DateTime.Now;
                  
                        Sweet sweet = new Sweet(username, incomingMessage,postedDate, id_counter);
                        id_counter += 1;
                        sweets.Add(sweet);

                        string myfile = @"C:\Users\LENOVO\cs408 project\messages.txt";
                        string path = System.AppDomain.CurrentDomain.BaseDirectory;
                        path += "messages.txt";
                        //logs.AppendText(path);

                        // Appending the given texts
                        using (StreamWriter sw = File.AppendText(myfile))
                        {
                            sw.WriteLine(username + " - " + postedDate + ": " + incomingMessage);
                        }

                        logs.AppendText(username + " posted a sweet!\n");
                    }
                }
                catch
                {
                    if(!terminating)
                    {
                        logs.AppendText("A client has disconnected\n");
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string myfile = @"C:\Users\LENOVO\cs408 project\messages.txt";
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            path += "messages.txt";


            File.WriteAllText(myfile, String.Empty);
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        
        
    }
}
