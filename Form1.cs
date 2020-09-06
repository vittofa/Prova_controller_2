using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Gaming.Input;

namespace Prova_controller_2
{
    public partial class Form1 : Form
    {

        obj Rec = new obj(DefaultBackColor);
        Gamepad Controller;
        Timer tim = new Timer();


        GamepadVibration gv = new GamepadVibration();
        int Vibration = 0;
        int VibrationOLD = 0;
        
        
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            tim.Tick += T_Tick;
            tim.Interval = 1;
            tim.Start();


        }

        private async void T_Tick(object sender, EventArgs e)
        {
            if(Gamepad.Gamepads.Count() > 0)
            {
                Controller = Gamepad.Gamepads.First();
                var Reading = Controller.GetCurrentReading();
                label1.Text = Reading.LeftThumbstickX.ToString();
                label2.Text = Reading.LeftThumbstickY.ToString();

                label8.Text = Reading.RightThumbstickX.ToString();
                label7.Text = Reading.RightThumbstickY.ToString();

                label11.Text = Reading.RightTrigger.ToString();
                label12.Text = Reading.LeftTrigger.ToString();

                int speed = trackBar1.Value;

                if(Reading.LeftThumbstickX > 0.1)
                {
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x + (int)(Math.Sqrt(speed * Reading.LeftThumbstickX)), Rec.y = Rec.y + 0, this.Width, this.Height))
                    {
                        //await Vibrate();
                        Vibration = 1;
                    }
                    else
                    {
                        Vibration = 0;
                    }
                }
                else if (Reading.LeftThumbstickX < -0.1)
                {
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x - (int)(Math.Sqrt((-speed) *Reading.LeftThumbstickX)), Rec.y = Rec.y + 0, this.Width, this.Height))
                    {
                        //await Vibrate();
                        Vibration = 1;
                    }
                    else
                    {
                        Vibration = 0;
                    }
                }

                if (Reading.LeftThumbstickY > 0.1)
                {
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x + 0, Rec.y = Rec.y - (int)(Math.Sqrt(speed * Reading.LeftThumbstickY)), this.Width, this.Height))
                    {
                        //await Vibrate();
                        Vibration = 1;
                    }
                    else
                    {
                        Vibration = 0;
                    }
                }
                else if (Reading.LeftThumbstickY < -0.1)
                {
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x + 0, Rec.y = Rec.y + (int)(Math.Sqrt((-speed) * Reading.LeftThumbstickY)), this.Width, this.Height))
                    {
                        //await Vibrate();
                        Vibration = 1;
                    }
                    else
                    {
                        Vibration = 0;
                    }
                }

                if (Reading.Buttons == GamepadButtons.DPadUp)
                {
                    await Log("Button DPadUp pressed");
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x + 0, Rec.y = Rec.y - 10, this.Width, this.Height))
                    {

                    }
                }
                if (Reading.Buttons == GamepadButtons.DPadRight)
                {
                    await Log("Button DPadRight pressed");
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x + 10, Rec.y = Rec.y + 0, this.Width, this.Height))
                    {

                    }
                }
                if (Reading.Buttons == GamepadButtons.DPadDown)
                {
                    await Log("Button DPadDown pressed");
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x + 0, Rec.y = Rec.y + 10, this.Width, this.Height))
                    {

                    }
                }
                if (Reading.Buttons == GamepadButtons.DPadLeft)
                {
                    await Log("Button DPadLeft pressed");
                    if(Rec.moveObj(this.CreateGraphics(), Rec.x = Rec.x - 10, Rec.y = Rec.y + 0, this.Width, this.Height))
                    {

                    }
                }

                if (Reading.Buttons == GamepadButtons.A)
                {
                    await Log("Button A pressed");
                    Rec.size += 10;
                    if (Rec.size > 500)
                    {
                        Rec.size = 500;
                    }
                    Rec.increaseSize(this.CreateGraphics(), Rec.size);
                }
                if (Reading.Buttons == GamepadButtons.B)
                {
                    await Log("Button B pressed");
                    Rec.size -= 10;
                    if (Rec.size < 5)
                    {
                        Rec.size = 5;
                    }
                    Rec.increaseSize(this.CreateGraphics(), Rec.size);
                }
                if (Reading.Buttons == GamepadButtons.X)
                {
                    await Log("Button X pressed");
                    await Log("Width: " + this.Width.ToString() + " Height: " + this.Height);
                }
                if (Reading.Buttons == GamepadButtons.Y)
                {
                    await Log("Button Y pressed");
                }
                
            }
        }

        private async void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            await Log("Controller Removed");
            toolStripStatusLabel1.Text = "Joystick disconnected";
        }

        private async void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            await Log("Controller Added");
            toolStripStatusLabel1.Text = "Joystick connected";
        }

        private async Task Log(string str)
        {
            Task t = Task.Run(() =>
            {
                Debug.WriteLine(DateTime.Now.ToShortTimeString() + ": " + str);
            });
            await t;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            this.WindowState = FormWindowState.Normal;
            Rec.CreateShape(this.CreateGraphics(), 250, 250, Brushes.Red, 25);
            Task taskVibrazione = new Task(this.Vibrate);
            taskVibrazione.Start();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //await Vibrate();
        }

        [STAThread]
        private void Vibrate()
        {
            while (true)
            {
                if (Controller != null)
                {
                    if(Vibration != VibrationOLD)
                    {
                        gv.LeftMotor = Vibration;
                        gv.RightMotor = Vibration;
                        Controller.Vibration = gv;
                    }
                }
            }
        }

    }

    class obj
    {
        private Graphics g;
        private Color defaultFormColor;
        public int x;
        public int y;
        public int size;
        private Brush b;
        
        public obj(Color defaultFormColor)
        {
            this.defaultFormColor = defaultFormColor;
        }

        public void CreateShape(Graphics G, int X, int Y, Brush B, int Size)
        {
            G.FillRectangle(B, X, Y, Size, Size);
            this.x = X;
            this.y = Y;
            this.b = B;
            this.g = G;
            this.size = Size;
        }

        public bool moveObj(Graphics G, int X, int Y, int width, int height)
        {
            bool retVal = false;
            if (X < 0 )
            {
                X = 0;
                this.x = 0;
                retVal = true;
            }
            if(Y < 0)
            {
                Y = 0;
                this.y = 0;
                retVal = true;
            }
            if(X + this.size > (width - 15))
            {
                X = width- 15 - this.size;
                this.x = X;
                retVal = true;
            }
            if (Y + this.size > (height - 60))
            {
                Y = height - 60 - this.size;
                this.y = Y;
                retVal = true;
            }

            //Draw new Rectangle
            G.Clear(this.defaultFormColor);
            CreateShape(G, X, Y, this.b, this.size);
            return retVal;
        }

        public void increaseSize(Graphics G, int SIZE)
        {
            G.Clear(this.defaultFormColor);
            CreateShape(G, this.x, this.y, this.b, SIZE);
        }

    }
}
