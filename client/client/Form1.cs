using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client
{
    public partial class Form1 : Form
    {

        bool terminating = false;
        bool connected = false;
        Socket clientSocket;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }
        private void send_message(string message)
        {
            Byte[] buffer = new Byte[10000000];
            buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
        }

        private string receive_response() // recieve 1 mesasge
        {
            Byte[] buffer = new Byte[10000000];
            clientSocket.Receive(buffer);
            string incomingMessage = Encoding.Default.GetString(buffer);
            incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
            return incomingMessage;
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;
            string username = textBox_username.Text;

            int portNum;
            if(Int32.TryParse(textBox_port.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    send_message(username);

                    string server_response = receive_response();
                    if (server_response == "authorized")
                    {
                        button_connect.Enabled = false;
                        textBox_message.Enabled = true;
                        textBox_delete.Enabled = true;
                        textBox_follow.Enabled = true;
                        button_disconnect.Enabled = true;
                        button_send.Enabled = true;
                        button_delete.Enabled = true;
                        button_feed.Enabled = true;
                        button_users.Enabled = true;
                        button_follow.Enabled = true;
                        checkBox_onlyfollows.Enabled = true;
                        checkBox_mysweets.Enabled = true;
                        terminating = false;

                        connected = true;
                        logs.AppendText("Connected to the server!\n");

                        Thread receiveThread = new Thread(Receive);
                        receiveThread.Start();
                    }
                    else if(server_response == "already connected")
                    {
                        logs.AppendText("This user is already connected to the server!\n");
                    }
                    else if (server_response == "not authorized")
                    {
                        logs.AppendText("You are not registered to the system!\n");
                    }
                    
                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                logs.AppendText("Check the port!\n");
            }

        }


        private void Receive()
        {
            while(connected)
            {
                try
                {
                    Byte[] buffer = new Byte[10000000];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if(incomingMessage.Contains("F-E-E-D"))
                    {
                        if( incomingMessage.Length > 8)
                        {
                            // prints the sweets coming from the server
                            logs.AppendText(incomingMessage.Substring(7));
                            logs.ScrollToCaret();
                        }
                        else
                        {
                            // no sweets to show
                            logs.AppendText("No sweets to show...\n");
                            logs.ScrollToCaret();
                        }
                    }
                    
                    else if(incomingMessage.Contains("U-S-E-R-L-I-S-T"))
                    {
                        logs.AppendText("*********************\n");
                        logs.AppendText(incomingMessage.Substring(15));
                        logs.AppendText("*********************\n");
                        logs.ScrollToCaret();
                    }
                    else if (incomingMessage.Contains("N-O-T-I-N-D-B-FOLLOW"))
                    {
                        logs.AppendText("Requested user to follow is not in the database\n");
                        logs.AppendText("Please enter a valid username\n");
                        logs.ScrollToCaret();
                    }
                    else if (incomingMessage.Contains("A-L-R-F-O-L"))
                    {
                        logs.AppendText("You already follow this user\n");
                        logs.ScrollToCaret();
                    }
                    else if (incomingMessage.Contains("F-O-L-YOURSELF"))
                    {
                        logs.AppendText("You can not follow yourself!\n");
                        logs.ScrollToCaret();
                    }
                    else if (incomingMessage.Contains("S-U-C-C-F-O-L"))
                    {
                        logs.AppendText("Successfully followed user!\n");
                        logs.ScrollToCaret();
                    }
                    else if (incomingMessage.Contains("D-E-L-E-T-E-SUCC"))
                    {
                        logs.AppendText("Successfully deleted sweet!\n");
                        logs.ScrollToCaret();
                    }
                    else if (incomingMessage.Contains("D-E-L-E-T-E-WRONG-ID"))
                    {
                        logs.AppendText("Sweet doesn't exist, Check the message id!\n");
                        logs.ScrollToCaret();
                    }
                    else if (incomingMessage.Contains("D-E-L-E-T-E-WRONG-OWNER"))
                    {
                        logs.AppendText("You are not authorized to delete this sweet!\n");
                        logs.ScrollToCaret();
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                        button_connect.Enabled = true;
                        button_disconnect.Enabled = false;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            string message = textBox_message.Text;

            if(message != "" && message.Length <= 64)
            {
                Byte[] buffer = Encoding.Default.GetBytes(message);
                clientSocket.Send(buffer);
                logs.AppendText("Sweet sent successfully!\n");
            }

        }

        private void button_feed_Click(object sender, EventArgs e)
        {
            string feed_message = "";
            if (checkBox_onlyfollows.Checked && checkBox_mysweets.Checked)
            {
                logs.AppendText("Please select only one option...\n");
            }
            else if (checkBox_onlyfollows.Checked)
            {
                feed_message = "R-E-Q-U-E-S-T-F";
                logs.AppendText("Requested for Sweet Feed From Followings...\n");
            }
            else if (checkBox_mysweets.Checked)
            {
                feed_message = "R-E-Q-U-E-S-T-MYS";
                logs.AppendText("Requested for own sweets...\n");
            }
            else
            {
                feed_message = "R-E-Q-U-E-S-T";
                logs.AppendText("Requested for Sweet Feed...\n");
            }
            try
            {
                if( feed_message != "")
                {
                    Byte[] buffer = Encoding.Default.GetBytes(feed_message);
                    clientSocket.Send(buffer);
                }
            }
            catch
            {
                logs.AppendText("There was a problem while requesting sweet feeds\n");
            }
            
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            string message = "D-I-S-C-O-N-N-E-C-T";
            Byte[] buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
            connected = false;
            terminating = true;
            button_connect.Enabled = true;
            button_disconnect.Enabled = false;
            button_send.Enabled = false;
            button_feed.Enabled = false;
            button_users.Enabled = false;
            textBox_message.Enabled = false;
            checkBox_onlyfollows.Enabled = false;
            checkBox_mysweets.Enabled = false;

            clientSocket.Disconnect(false);
            logs.AppendText("Disconnected\n");
            logs.ScrollToCaret();
        }

        private void button_users_Click(object sender, EventArgs e)
        {
            string message = "R-E-Q-U-S-E-R";
            Byte[] buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
            logs.AppendText("Requested users\n");
            logs.ScrollToCaret();
        }

        private void button_follow_Click(object sender, EventArgs e)
        {
            string message = "F-O-L-L-O-W" + textBox_follow.Text;
            Byte[] buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
            logs.AppendText("Follow request sent!\n");
            logs.ScrollToCaret();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            string id = textBox_delete.Text;
            string message = "D-E-L-E-T-E-MES" + id;
            Byte[] buffer = Encoding.Default.GetBytes(message);
            clientSocket.Send(buffer);
            logs.AppendText("Delete request sent!\n");
            logs.ScrollToCaret();
        }
    }
}
