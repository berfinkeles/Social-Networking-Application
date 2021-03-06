namespace client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.button_connect = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_send = new System.Windows.Forms.Button();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_feed = new System.Windows.Forms.Button();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.button_users = new System.Windows.Forms.Button();
            this.textBox_follow = new System.Windows.Forms.TextBox();
            this.button_follow = new System.Windows.Forms.Button();
            this.checkBox_onlyfollows = new System.Windows.Forms.CheckBox();
            this.checkBox_mysweets = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_delete = new System.Windows.Forms.TextBox();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_reqFollows = new System.Windows.Forms.Button();
            this.button_reqFollowers = new System.Windows.Forms.Button();
            this.textBox_block = new System.Windows.Forms.TextBox();
            this.button_block = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(88, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(134, 98);
            this.textBox_ip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(172, 31);
            this.textBox_ip.TabIndex = 2;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(134, 152);
            this.textBox_port.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(172, 31);
            this.textBox_port.TabIndex = 3;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(134, 280);
            this.button_connect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(140, 42);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(729, 84);
            this.logs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(400, 482);
            this.logs.TabIndex = 5;
            this.logs.Text = "";
            // 
            // textBox_message
            // 
            this.textBox_message.Enabled = false;
            this.textBox_message.Location = new System.Drawing.Point(132, 527);
            this.textBox_message.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(192, 31);
            this.textBox_message.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 531);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Message:";
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(332, 519);
            this.button_send.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(130, 50);
            this.button_send.TabIndex = 8;
            this.button_send.Text = "send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // textBox_username
            // 
            this.textBox_username.Location = new System.Drawing.Point(134, 212);
            this.textBox_username.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_username.Name = "textBox_username";
            this.textBox_username.Size = new System.Drawing.Size(172, 31);
            this.textBox_username.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 216);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "username:";
            // 
            // button_feed
            // 
            this.button_feed.Enabled = false;
            this.button_feed.Location = new System.Drawing.Point(729, 33);
            this.button_feed.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_feed.Name = "button_feed";
            this.button_feed.Size = new System.Drawing.Size(400, 48);
            this.button_feed.TabIndex = 12;
            this.button_feed.Text = "Request Feed";
            this.button_feed.UseVisualStyleBackColor = true;
            this.button_feed.Click += new System.EventHandler(this.button_feed_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.Enabled = false;
            this.button_disconnect.Location = new System.Drawing.Point(298, 280);
            this.button_disconnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(140, 42);
            this.button_disconnect.TabIndex = 13;
            this.button_disconnect.Text = "disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // button_users
            // 
            this.button_users.Enabled = false;
            this.button_users.Location = new System.Drawing.Point(134, 348);
            this.button_users.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_users.Name = "button_users";
            this.button_users.Size = new System.Drawing.Size(304, 41);
            this.button_users.TabIndex = 14;
            this.button_users.Text = "Users";
            this.button_users.UseVisualStyleBackColor = true;
            this.button_users.Click += new System.EventHandler(this.button_users_Click);
            // 
            // textBox_follow
            // 
            this.textBox_follow.Enabled = false;
            this.textBox_follow.Location = new System.Drawing.Point(729, 594);
            this.textBox_follow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_follow.Name = "textBox_follow";
            this.textBox_follow.Size = new System.Drawing.Size(192, 31);
            this.textBox_follow.TabIndex = 15;
            // 
            // button_follow
            // 
            this.button_follow.Enabled = false;
            this.button_follow.Location = new System.Drawing.Point(962, 584);
            this.button_follow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_follow.Name = "button_follow";
            this.button_follow.Size = new System.Drawing.Size(168, 35);
            this.button_follow.TabIndex = 16;
            this.button_follow.Text = "follow";
            this.button_follow.UseVisualStyleBackColor = true;
            this.button_follow.Click += new System.EventHandler(this.button_follow_Click);
            // 
            // checkBox_onlyfollows
            // 
            this.checkBox_onlyfollows.AutoSize = true;
            this.checkBox_onlyfollows.Enabled = false;
            this.checkBox_onlyfollows.Location = new System.Drawing.Point(537, 42);
            this.checkBox_onlyfollows.Name = "checkBox_onlyfollows";
            this.checkBox_onlyfollows.Size = new System.Drawing.Size(167, 29);
            this.checkBox_onlyfollows.TabIndex = 17;
            this.checkBox_onlyfollows.Text = "Only Follows";
            this.checkBox_onlyfollows.UseVisualStyleBackColor = true;
            // 
            // checkBox_mysweets
            // 
            this.checkBox_mysweets.AutoSize = true;
            this.checkBox_mysweets.Enabled = false;
            this.checkBox_mysweets.Location = new System.Drawing.Point(537, 88);
            this.checkBox_mysweets.Name = "checkBox_mysweets";
            this.checkBox_mysweets.Size = new System.Drawing.Size(149, 29);
            this.checkBox_mysweets.TabIndex = 18;
            this.checkBox_mysweets.Text = "My Sweets";
            this.checkBox_mysweets.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 594);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 25);
            this.label5.TabIndex = 19;
            this.label5.Text = "Message ID:";
            // 
            // textBox_delete
            // 
            this.textBox_delete.Enabled = false;
            this.textBox_delete.Location = new System.Drawing.Point(154, 589);
            this.textBox_delete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_delete.Name = "textBox_delete";
            this.textBox_delete.Size = new System.Drawing.Size(169, 31);
            this.textBox_delete.TabIndex = 20;
            // 
            // button_delete
            // 
            this.button_delete.Enabled = false;
            this.button_delete.Location = new System.Drawing.Point(332, 581);
            this.button_delete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(130, 50);
            this.button_delete.TabIndex = 21;
            this.button_delete.Text = "delete";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_reqFollows
            // 
            this.button_reqFollows.Enabled = false;
            this.button_reqFollows.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_reqFollows.Location = new System.Drawing.Point(537, 138);
            this.button_reqFollows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_reqFollows.Name = "button_reqFollows";
            this.button_reqFollows.Size = new System.Drawing.Size(146, 48);
            this.button_reqFollows.TabIndex = 22;
            this.button_reqFollows.Text = "Follows";
            this.button_reqFollows.UseVisualStyleBackColor = true;
            this.button_reqFollows.Click += new System.EventHandler(this.button_reqFollows_Click);
            // 
            // button_reqFollowers
            // 
            this.button_reqFollowers.Enabled = false;
            this.button_reqFollowers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_reqFollowers.Location = new System.Drawing.Point(537, 197);
            this.button_reqFollowers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_reqFollowers.Name = "button_reqFollowers";
            this.button_reqFollowers.Size = new System.Drawing.Size(146, 50);
            this.button_reqFollowers.TabIndex = 23;
            this.button_reqFollowers.Text = "Followers";
            this.button_reqFollowers.UseVisualStyleBackColor = true;
            this.button_reqFollowers.Click += new System.EventHandler(this.button_reqFollowers_Click);
            // 
            // textBox_block
            // 
            this.textBox_block.Enabled = false;
            this.textBox_block.Location = new System.Drawing.Point(729, 639);
            this.textBox_block.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_block.Name = "textBox_block";
            this.textBox_block.Size = new System.Drawing.Size(192, 31);
            this.textBox_block.TabIndex = 24;
            // 
            // button_block
            // 
            this.button_block.Enabled = false;
            this.button_block.Location = new System.Drawing.Point(962, 635);
            this.button_block.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_block.Name = "button_block";
            this.button_block.Size = new System.Drawing.Size(168, 35);
            this.button_block.TabIndex = 25;
            this.button_block.Text = "block";
            this.button_block.UseVisualStyleBackColor = true;
            this.button_block.Click += new System.EventHandler(this.button_block_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1161, 692);
            this.Controls.Add(this.button_block);
            this.Controls.Add(this.textBox_block);
            this.Controls.Add(this.button_reqFollowers);
            this.Controls.Add(this.button_reqFollows);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.textBox_delete);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox_mysweets);
            this.Controls.Add(this.checkBox_onlyfollows);
            this.Controls.Add(this.button_follow);
            this.Controls.Add(this.textBox_follow);
            this.Controls.Add(this.button_users);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.button_feed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_feed;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.Button button_users;
        private System.Windows.Forms.TextBox textBox_follow;
        private System.Windows.Forms.Button button_follow;
        private System.Windows.Forms.CheckBox checkBox_onlyfollows;
        private System.Windows.Forms.CheckBox checkBox_mysweets;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_delete;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_reqFollows;
        private System.Windows.Forms.Button button_reqFollowers;
        private System.Windows.Forms.TextBox textBox_block;
        private System.Windows.Forms.Button button_block;
    }
}

