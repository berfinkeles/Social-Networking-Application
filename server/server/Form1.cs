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
            var path_follow = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "follows.txt");
            // Initialize follows.txt
            if (!File.Exists(Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "follows.txt")))
            {

                using (FileStream fs = File.Create(path_follow))
                {
                    foreach(string usr in users)
                    {
                        Byte[] title = new UTF8Encoding(true).GetBytes(usr + " \n");
                        fs.Write(title, 0, title.Length);
                    }
                    
                }
            }

            

            // Initialize messages.txt
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
                            Thread receiveThread = new Thread(() => Receive(newClient)); // updated
                            receiveThread.Start();


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
                    // Getting the username of the current client
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
                            logs.AppendText("An error occurred while preparing feed request!\n");
                        }

                    }
                    else if (incomingMessage == "R-E-Q-U-E-S-T-F") // client requested sweet feed only from followers
                    {
                        logs.AppendText(username + " requested Sweet Feed From Their Followings\n");
                        string followings = get_followings(username);//can add the functionality to check for users with no followings bur didn't
                        string feed_messages = "F-E-E-D"; // this will be sent to client
                        try
                        {
                            read_messages(); // get contents of messages.txt
                            foreach (string message in feed)
                            {
                                string sweet_sender = message.Substring(0, message.IndexOf(' '));
                                if (sweet_sender != username && followings.Contains(sweet_sender))//if add every sweet that the requester follows
                                {
                                    feed_messages = feed_messages + message + "\n";
                                }
                            }
                            send_message(thisClient, feed_messages);
                            feed.Clear(); // clear the feed list so that client doesn't get duplicate feeds
                        }
                        catch
                        {
                            logs.AppendText("An error occurred while preparing feed request!\n");
                        }

                    }
                    else if (incomingMessage == "R-E-Q-U-E-S-T-MYS") // client requested her own sweets
                    {
                        logs.AppendText(username + " requested their own sweets\n");
                        string feed_messages = "F-E-E-D"; // this will be sent to client
                        try
                        {
                            read_messages(); // get contents of messages.txt
                            foreach (string message in feed)
                            {
                                string sweet_sender = message.Substring(0, message.IndexOf(' '));
                                if (sweet_sender == username) //if sweet is posted by the user
                                {
                                    feed_messages = feed_messages + message + "\n";
                                }
                            }
                            send_message(thisClient, feed_messages);
                            feed.Clear(); // clear the feed list so that client doesn't get duplicate feeds
                        }
                        catch
                        {
                            logs.AppendText("An error occurred while preparing feed request!\n");
                        }

                    }
                    else if(incomingMessage == "D-I-S-C-O-N-N-E-C-T")
                    {
                        thisClient.Close();
                        clientSockets.Remove(thisClient);
                        connected_users.Remove(name);
                        clientSocketsDictionary.Remove(name);
                        
                    }
                    else if(incomingMessage == "R-E-Q-U-S-E-R")
                    {
                        string userListMessage = "U-S-E-R-L-I-S-T";
                        foreach (string user in users)
                        {
                            userListMessage = userListMessage + user + "\n";
                        }
                        send_message(thisClient, userListMessage);
                    }
                    else if (incomingMessage.Contains("F-O-L-L-O-W"))
                    {
                        string user_to_follow = incomingMessage.Substring(11);
                        logs.AppendText(username + " requested to follow " + incomingMessage.Substring(11)+"\n");
                        if(username == user_to_follow)
                        {
                            logs.AppendText("User can not follow herself\n");
                            send_message(thisClient, "F-O-L-YOURSELF");
                        }
                        else
                        {
                            
                            bool inDatabase = false;
                            string workingDirectory = Environment.CurrentDirectory;
                            var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "follows.txt");
                            foreach (string user in users)
                            {
                                if(user == user_to_follow) // if user in ddatabase
                                {
                                    inDatabase = true;
                                    string[] arrLine = File.ReadAllLines(path);
                                    for(int i = 0; i < arrLine.Length; i++)
                                    {
                                        string array_user = arrLine[i];
                                        if(array_user.Substring(0, array_user.IndexOf(" ")) == username)
                                        {
                                            if (array_user.Contains(user_to_follow)) //contains
                                            {
                                                logs.AppendText(username+ " already follows " + user_to_follow + "\n");
                                                send_message(thisClient, "A-L-R-F-O-L");

                                            }
                                            else //add user to followings
                                            {
                                                arrLine[i] = arrLine[i] + " " + user_to_follow;
                                                File.WriteAllLines(path, arrLine);
                                                logs.AppendText(username + " succesfully followed " + user_to_follow + "\n");
                                                send_message(thisClient, "S-U-C-C-F-O-L");
                                                break;

                                            }
                                        }
                                    }

                                }

                            }

                            if(inDatabase == false)
                            {
                                string Message = "N-O-T-I-N-D-B-FOLLOW";
                                logs.AppendText("Requested user to follow is not in the database\n");
                                send_message(thisClient, Message);
                            }

                        }

                    }
                    else if(incomingMessage.Contains("D-E-L-E-T-E-MES"))
                    {
                        string id_to_delete = incomingMessage.Substring(15);
                        bool sweet_exist = false; // if sweet exists with that id
                        bool correct_owner = false; // if message to deleted is owned by the user
                        logs.AppendText(username + " requested to delete message with id: " + id_to_delete + " \n");
                        try
                        {
                            read_messages(); // get contents of messages.txt
                            foreach (string message in feed)
                            {
                                string username_text = message.Substring(0, message.IndexOf(' '));
                                string temp_line = message.Substring(message.IndexOf(':')+1);
                                string id_text = temp_line.Substring(0, temp_line.IndexOf(' '));
                                if (id_text == id_to_delete) // sweet exists with that id
                                {
                                    sweet_exist = true;
                                    if (username_text == username ) // sweet belongs to the user
                                    {
                                        // message will be deleted
                                        correct_owner = true;

                                        // get the project directory
                                        string workingDirectory = Environment.CurrentDirectory;
                                        var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "messages.txt");

                                        // Write all sweets except the one to be deleted
                                        string tempFile = Path.GetTempFileName();

                                        using (var sr = new StreamReader(path))
                                        using (StreamWriter sw = File.AppendText(tempFile))
                                        {
                                            string line;

                                            while ((line = sr.ReadLine()) != null)
                                            {
                                                if (line != message)
                                                    sw.WriteLine(line);
                                            }
                                        }
                                        File.Delete(path);
                                        File.Move(tempFile, path);

                                        send_message(thisClient, "D-E-L-E-T-E-SUCC");
                                        feed.Clear(); // clear the feed list so that client doesn't get duplicate feeds
                                        logs.AppendText("Delete Successfull!\n");
                                        logs.ScrollToCaret();
                                        break;
                                    }
                                }
                            }
                            if (!sweet_exist)
                            {
                                // sweet doesn't exist
                                logs.AppendText("Sweet doesn't exist!\n");
                                logs.ScrollToCaret();
                                send_message(thisClient, "D-E-L-E-T-E-WRONG-ID");
                                feed.Clear(); // clear the feed list so that client doesn't get duplicate feeds
                            }
                            else if (!correct_owner)
                            {
                                // sweet exists but owner is wrong
                                logs.AppendText("User is not authorized to delete this sweet!\n");
                                logs.ScrollToCaret();
                                send_message(thisClient, "D-E-L-E-T-E-WRONG-OWNER");
                                feed.Clear(); // clear the feed list so that client doesn't get duplicate feeds
                            }
                        }
                        catch
                        {
                            logs.AppendText("An error occurred while preparing feed request!\n");
                            logs.ScrollToCaret();
                        }
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

        private string get_followings(string username)
        {
            string workingDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName, "follows.txt");
            string followings = "";
            string[] arrLine = File.ReadAllLines(path);
            for (int i = 0; i < arrLine.Length; i++)
            {
                string array_user = arrLine[i];
                if (array_user.Substring(0, array_user.IndexOf(" ")) == username)//find the line that belongs to the user that makes the request
                {
                    followings = array_user.Substring(username.Length+1); 
                    //followings = follows_str.Split(' ');//add each following to the list
                    break;
                }

                
            }

            return followings;
        }



    }
}
