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
        List<string> feed = new List<string>();

        bool terminating = false;
        bool listening = false;

        int id_counter = 1;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void read_file() // reads the user-db.txt and stores contents in users list
        {
            string line;
            string workingDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "user-db.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                users.Add(line);
            }
            file.Close();
        }

        private void read_messages() // reads messages.txt and stores in feed list
        {
            string line;
            string workingDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "messages.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                feed.Add(line);
            }
            file.Close();
        }

        private int get_lastID() // reads messages.txt and returns the id of last sweet
        {
            string workingDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "messages.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            if(new FileInfo(path).Length == 0)
            {
                file.Close();
                return 0;
            }
            else
            {
                string lastLine = File.ReadLines(path).Last();
                int stopIndex = lastLine.IndexOf(" ", lastLine.IndexOf(":"));
                int length = stopIndex - lastLine.IndexOf(":");
                string id = lastLine.Substring(lastLine.IndexOf(":")+1, length);
                int lastID = Int32.Parse(id);
                file.Close();
                return lastID;
            }
            
           
        }

        
        private void button_listen_Click(object sender, EventArgs e)
        {
            int serverPort;
            read_file(); // read user-db.txt

            string workingDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "messages.txt");
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            if (Int32.TryParse(textBox_port.Text, out serverPort))
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

                    if(is_authorized(newClient,ref client_name))
                    {
                        //user is in the database
                        if (!clientSocketsDictionary.ContainsKey(client_name)) // checks if the user already connected
                        {
                            
                            send_message(newClient, "authorized");
                            clientSocketsDictionary.Add(client_name, newClient);
                            connected_users.Add(client_name);
                            logs.AppendText(client_name + " is connected.\n");
                            logs.ScrollToCaret();

               
                        }
                        else
                        {
                            logs.AppendText(client_name + " is trying to connect again\n");
                            logs.ScrollToCaret();
                            send_message(newClient, "already connected");
                            newClient.Close();
                        }

                    }
                    else
                    {
                        logs.AppendText(client_name + " is trying to connect but not registered\n");
                        logs.ScrollToCaret();
                        send_message(newClient, "not authorized");
                        newClient.Close();
                    }

                    Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                    receiveThread.Start();
                    
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
            string name = connected_users[connected_users.Count() - 1]; //username of thisClient

            while (connected && !terminating)
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
                    // gets the message from client
                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (incomingMessage == "R-E-Q-U-E-S-T") // client requested sweet feed
                    {
                        logs.AppendText(username + " requested Sweet Feed\n");
                        string feed_messages = "F-E-E-D"; // this will be sent to client
                        try
                        {
                            read_messages(); // get contents of messages.txt
                            foreach (string message in feed)
                            {
                                if (message.Substring(0, message.IndexOf(' ')) != username)
                                {
                                    feed_messages = feed_messages + message + "\n";
                                }
                            }
                            send_message(thisClient, feed_messages);
                            feed.Clear(); // clear the feed list so that client doesn't get duplicate feeds
                        }
                        catch
                        {
                            logs.AppendText("An error occurred while preparing feed request!");
                        }

                    }
                    else if(incomingMessage == "D-I-S-C-O-N-N-E-C-T")
                    {
                        thisClient.Close();
                        clientSockets.Remove(thisClient);
                        connected_users.Remove(name);
                        clientSocketsDictionary.Remove(name);
                        
                    }
                    else if(incomingMessage.Length != 0) // client has posted a sweet
                    {
                        DateTime postedDate = DateTime.Now;
                  
                        Sweet sweet = new Sweet(username, incomingMessage,postedDate, id_counter);
                        id_counter += 1;
                        sweets.Add(sweet);

                        // get the project directory
                        string workingDirectory = Environment.CurrentDirectory;
                        var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "messages.txt");
                        int id = get_lastID()+1;
                        
                        // Write sweets to messages.txt file
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine(username + " - id:" + id + " - date:" + postedDate + ": " + incomingMessage);
                        }

                        logs.AppendText(username + " posted a sweet!\n");
                    }
                }
                catch
                {
                    if(!terminating)
                    {
                        logs.AppendText(name + " has disconnected\n");
                    }
                    thisClient.Close();
                    clientSockets.Remove(thisClient);
                    connected_users.Remove(name);
                    clientSocketsDictionary.Remove(name);
                    connected = false;
                }
            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // string workingDirectory = Environment.CurrentDirectory;
            // var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "messages.txt");

            // File.WriteAllText(path, String.Empty);
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        
        
    }
}
