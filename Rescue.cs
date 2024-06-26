﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.XInput;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Rescue
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hwc, IntPtr hwp);

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            output.SelectionStart = output.Text.Length;
            // scroll it automatically
            output.ScrollToCaret();
        }

        public class Debouncer
        {
            private readonly TimeSpan debounceInterval;
            private DateTime lastInvokeTime = DateTime.MinValue;
            private Action action;

            public Debouncer(TimeSpan debounceInterval)
            {
                this.debounceInterval = debounceInterval;
            }

            public void Debounce(Action action)
            {
                lock (this)
                {
                    this.action = action;
                    var currentTime = DateTime.Now;
                    var timeSinceLastInvoke = currentTime - lastInvokeTime;
                    if (timeSinceLastInvoke >= debounceInterval)
                    {
                        lastInvokeTime = currentTime;
                        ThreadPool.QueueUserWorkItem(state => action());
                    }
                }
            }
        }

        /*public class Program
        {
            public static void Main(string[] args)
            {
                // Create a debouncer with a debounce interval of 200 milliseconds
                var debouncer = new Debouncer(TimeSpan.FromMilliseconds(250));

                // Example usage: simulating button clicks
                for (int i = 0; i < 10; i++)
                {
                    // Simulate button click event
                    debouncer.Debounce(() =>
                    {
                        Console.WriteLine("Button clicked.");
                    });

                    // Sleep for a short interval to simulate rapid clicks
                    Thread.Sleep(100);
                }

                Console.ReadLine();
            }
        }*/

        public async void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Replace "arduino_server_ip" and "port_number" with the actual IP address and port number of your Arduino server
                string serverIP = ipAddress.Text;
                int port = int.Parse(portTxt.Text);




                // Connect to the server
                TcpClient client = new TcpClient(serverIP, port);
                output.Text += "\n---------Connected to Arduino server.---------\n";

                // Send data to the server
                string message = "Hello";
                byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                await Task.Delay(500);
                Controller controller = new Controller(UserIndex.One);

                byte[] buffer = new byte[256];
                StringBuilder receivedData = new StringBuilder();

                // Check if the controller is connected

                var debouncer = new Debouncer(TimeSpan.FromMilliseconds(300));

                if (controller.IsConnected)
                {
                    output.Text += "Controller connected.\n";
                    bool checkbool = false;
                    bool checkbool2 = false;
                    bool checkbool3 = false;
                    bool checkbool4 = false;

                    bool flipperbool = false;
                    bool flipperbool2 = false;
                    bool flipperbool3 = false;
                    bool flipperbool4 = false;

                    while (true)
                    {
                        // Get the current state of the controller
                        State state = controller.GetState();

                        // Check if the controller is still connected
                        if (!state.PacketNumber.Equals(0))
                        {
                            // Print out the values of the thumbsticks
                            if ((state.Gamepad.LeftThumbX) >= -32767)
                            {
                                message = "2";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                debouncer.Debounce(() => { stream.Write(data, 0, data.Length); });
                            }
                            /*if ((state.Gamepad.LeftThumbX) >= 32767)
                            {
                                message = "3";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                debouncer.Debounce(() => { stream.Write(data, 0, data.Length); });
                            }*/
                            if ((state.Gamepad.LeftThumbY) >= 32767)
                            {
                                message = "H";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                debouncer.Debounce(() => { stream.Write(data, 0, data.Length); });
                            }/*`  
                            if ((state.Gamepad.RightThumbX) != 0)
                            {
                                message = "Right Thumbstick X: " + state.Gamepad.RightThumbX + "\n";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                            }
                            if ((state.Gamepad.RightThumbY) != 0)
                            {
                                message = "Right Thumbstick Y: " + state.Gamepad.RightThumbY + "\n";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                            }*/


                            if ((state.Gamepad.LeftTrigger) > 25)
                            {
                                if (checkbool == false)
                                {
                                    message = "0";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool = true;
                                }

                            }

                            if ((state.Gamepad.LeftTrigger) < 24)
                            {
                                if (checkbool == true)
                                {
                                    message = "S";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool = false;
                                }

                            }

                            if ((state.Gamepad.RightTrigger) > 25)
                            {
                                if (checkbool2 == false)
                                {
                                    message = "Q";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool2 = true;
                                }

                            }

                            //------------------ Stop ------------------//

                            if ((state.Gamepad.RightTrigger) < 24)
                            {
                                if (checkbool2 == true)
                                {
                                    message = "S";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool2 = false;
                                }

                            }

                            // Check if the A button is pressed
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.A) != 0)
                            {
                                message = "A";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                                flipperbool = true;
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.A) == 0)
                            {
                                if (flipperbool == true)
                                {
                                    message = "s";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    flipperbool = false;
                                }
                                
                            }

                            // Check if the B button is pressed
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.B) != 0)
                            {
                                message = "B";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                                flipperbool2 = true;
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.B) == 0)
                            {
                                if (flipperbool2 == true)
                                {
                                    message = "s";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    flipperbool2 = false;
                                }
                            }

                            // Check if the Y button is pressed
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0)
                            {
                                message = "Y";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                                flipperbool3 = true;
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.Y) == 0)
                            {
                                if (flipperbool3 == true)
                                {
                                    message = "s";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    flipperbool3 = false;
                                }
                                
                            }

                            // Check if the X button is pressed
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.X) != 0)
                            {
                                message = "X";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                                flipperbool4 = true;
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.X) == 0)
                            {
                                if (flipperbool4 == true)
                                {
                                    message = "s";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    flipperbool4 = false;
                                }
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0)
                            {
                                message = "Z";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0)
                            {
                                message = "C";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0)
                            {
                                message = "U";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0)
                            {
                                message = "D";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0)
                            {
                                message = "1";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                debouncer.Debounce(() => { stream.Write(data, 0, data.Length); });
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0)
                            {
                                message = "]";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                debouncer.Debounce(() => { stream.Write(data, 0, data.Length); });
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) != 0)
                            {
                                if(checkbool3 == false)
                                {
                                    message = "L";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool3 = true;
                                }
                                
                            }
                            if ((state.Gamepad.Buttons & GamepadButtonFlags.LeftShoulder) == 0)
                            {
                                if(checkbool3 == true)
                                {
                                    message = "S";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool3 = false;
                                }
                                
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) != 0)
                            {
                                if (checkbool4 == false)
                                {
                                    message = "R";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool4 = true;
                                }
                                
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.RightShoulder) == 0)
                            {
                                if (checkbool4 == true)
                                {
                                    message = "S";
                                    data = System.Text.Encoding.ASCII.GetBytes(message);
                                    stream = client.GetStream();
                                    stream.Write(data, 0, data.Length);
                                    checkbool4 = false;
                                }
                                
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.Start) != 0)
                            {
                                message = "Start Button Pressed.\n";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                            }

                            if ((state.Gamepad.Buttons & GamepadButtonFlags.Back) != 0)
                            {
                                message = "Back Button Pressed Exitting...\n";
                                data = System.Text.Encoding.ASCII.GetBytes(message);
                                stream = client.GetStream();
                                stream.Write(data, 0, data.Length);
                                output.Text += "\n---------Disconnecting Controller, Exit by user.---------\n";

                                System.Threading.Thread.Sleep(200);

                                stream.Close();
                                client.Close();
                                break;
                            }

                            // Add more button checks as needed

                            // Wait for a short amount of time to reduce CPU usage

                            System.Threading.Thread.Sleep(1);
                        }
                        else
                        {
                            output.Text += "Controller disconnected.\n";
                            break;
                        }

                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                        receivedData.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));

                        // Check for end of message
                        if (receivedData.ToString().Contains("\n\r"))
                        {
                            string responseData = receivedData.ToString().Trim();
                            //output.Text += "\nReceived: {0}" + responseData + "\n";
                            receivedData.Clear();
                        }

                        if (receivedData.ToString().Contains("Speed"))
                        {
                            string responseData = receivedData.ToString().Trim();
                            output.Text += "\nReceived: {0}" + responseData;
                        }
                        /*if (receivedData.ToString().Contains("Bus Volatage"))
                        {
                            string responseData = receivedData.ToString().Trim();
                            busV.Text += responseData;
                        }
                        if (receivedData.ToString().Contains("Load Volatage"))
                        {
                            string responseData = receivedData.ToString().Trim();
                            loadV.Text += responseData;
                        }
                        if (receivedData.ToString().Contains("Current"))
                        {
                            string responseData = receivedData.ToString().Trim();
                            current.Text += responseData;
                        }
                        if (receivedData.ToString().Contains("Power"))
                        {
                            string responseData = receivedData.ToString().Trim();
                            pwr.Text += responseData;
                        }*/
                    }

                }
            }
            catch {
                output.Text += "\n Error: Could not connect to Arduino Server";
            }
        }
        static void SendMessage(NetworkStream stream, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            Controller controller = new Controller(UserIndex.One);

            // Check if the controller is connected
            if (controller.IsConnected)
            {
                output.Text += "Controller connected.\n";
                System.Threading.Thread.Sleep(500);

                while (true)
                {
                    // Get the current state of the controller
                    State state = controller.GetState();

                    // Check if the controller is still connected
                    if (!state.PacketNumber.Equals(0))
                    {
                        // Print out the values of the thumbsticks
                        if ((state.Gamepad.LeftThumbX) != 0)
                        {
                            output.Text += "Left Thumbstick X: " + state.Gamepad.LeftThumbX + "\n";
                        }
                        if ((state.Gamepad.LeftThumbY) != 0)
                        {
                            output.Text += "Left Thumbstick Y: " + state.Gamepad.LeftThumbY + "\n";
                        }
                        if ((state.Gamepad.RightThumbX) != 0)
                        {
                            output.Text += "Right Thumbstick X: " + state.Gamepad.RightThumbX + "\n";
                        }
                        if ((state.Gamepad.RightThumbY) != 0)
                        {
                            output.Text += "Right Thumbstick Y: " + state.Gamepad.RightThumbY + "\n";
                        }

                        //------------------ 9 Backward Speeds Section Starts Here ------------------//
                        

                        // Check if the A button is pressed
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.A) != 0)
                        {
                            output.Text += "A button pressed.\n";
                        }

                        // Check if the B button is pressed
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.B) != 0)
                        {
                            output.Text += "B button pressed.\n";
                        }

                        // Check if the Y button is pressed
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Y) != 0)
                        {
                            output.Text += "Y button pressed.\n";
                        }

                        // Check if the X button is pressed
                        if ((state.Gamepad.Buttons & GamepadButtonFlags.X) != 0)
                        {
                            output.Text += "X button pressed.\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadLeft) != 0)
                        {
                            output.Text += "DL\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadRight) != 0)
                        {
                            output.Text += "DR\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadUp) != 0)
                        {
                            output.Text += "DU\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.DPadDown) != 0)
                        {
                            output.Text += "DD\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.LeftThumb) != 0)
                        {
                            output.Text += "Left Thumbstick Button Pressed.\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.RightThumb) != 0)
                        {
                            output.Text += "Right Thumbstick Button Pressed.\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Start) != 0)
                        {
                            output.Text += "Start Button Pressed.\n";
                        }

                        if ((state.Gamepad.Buttons & GamepadButtonFlags.Back) != 0)
                        {
                            output.Text += "Back Button Pressed Exitting...\n";
                            break;
                        }

                        // Add more button checks as needed

                        // Wait for a short amount of time to reduce CPU usage
                        System.Threading.Thread.Sleep(1);
                    }
                    else
                    {
                        output.Text += "Controller disconnected.\n";
                        break;
                    }
                }
            }
            else
            {
                output.Text += "Controller not connected.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            output.Text = "";
        }

        private void ipAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                webView21.Source = new Uri(txtUrl2.Text);
            }
            catch (UriFormatException)
            {
                webView21.Source = new Uri("https://" + txtUrl2.Text);
            }
        }

        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void webView21_Click_1(object sender, EventArgs e)
        {

        }

        private void ForwardUrl2_Click_1(object sender, EventArgs e)
        {
            webView21.GoForward();
        }

        private void BackUrl2_Click_1(object sender, EventArgs e)
        {
            webView21.GoBack();
        }

        private void reload2_Click_1(object sender, EventArgs e)
        {
            webView21.Reload();
        }

        private void reload1_Click_1(object sender, EventArgs e)
        {
            webView22.Reload();
        }

        private void BackUrl1_Click_1(object sender, EventArgs e)
        {
            webView22.GoBack();
        }

        private void ForwardUrl1_Click_1(object sender, EventArgs e)
        {
            webView22.GoForward();
        }

        private void url1Enter_Click(object sender, EventArgs e)
        {
            try
            {
                webView22.Source = new Uri(txtUrl2.Text);
            }
            catch (UriFormatException)
            {
                webView22.Source = new Uri("https://" + txtUrl1.Text);
            }
        }

        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUrl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    webView21.Source = new Uri(txtUrl2.Text);
                }
                catch (UriFormatException)
                {
                    webView21.Source = new Uri("https://" + txtUrl2.Text);
                }
            }
        }
        private void txtUrl2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tableLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtUrl1_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtUrl1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    webView22.Source = new Uri(txtUrl1.Text);
                }
                catch (UriFormatException)
                {
                    webView22.Source = new Uri("https://" + txtUrl1.Text);
                }
            }
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}