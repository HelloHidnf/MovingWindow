namespace MovingWindow
{
    public partial class Window : Form
    {
        int speedx = 3;
        int speedy = 3;
        Screen[] screens;
        Screen screen;
        Screen ?nextscreen;

        private async Task<DialogResult> MoveWindow()
        {
            File.WriteAllText("log.txt", $"{screens.GetValue(0)}\n\n{screens.GetValue(1)}");
            Form frm = ActiveForm;
            while (frm != null)
            {
                await Task.Delay(10);
                frm.Left += speedx;
                frm.Top += speedy;
                if (frm.Left <= screen.Bounds.Left && speedx < 0)
                {
                    nextscreen = Array.Find(screens, x => x.Bounds.Right == screen.Bounds.Left);
                    if(nextscreen != null && nextscreen.Bounds.Top < frm.Bounds.Top && nextscreen.Bounds.Bottom > frm.Bounds.Bottom)
                    {
                        screen = nextscreen;
                    }
                    else
                    {
                        speedx = -speedx;
                    }
                }
                if (frm.Right >= screen.Bounds.Right && speedx > 0)
                {
                    nextscreen = Array.Find(screens, x => x.Bounds.Left == screen.Bounds.Right);
                    if (nextscreen != null && nextscreen.Bounds.Top < frm.Bounds.Top && nextscreen.Bounds.Bottom > frm.Bounds.Bottom)
                    {
                        screen = nextscreen;
                    }
                    else
                    {
                        speedx = -speedx;
                    }
                }
                if (frm.Top <= screen.Bounds.Top && speedy < 0)
                {
                    nextscreen = Array.Find(screens, x => x.Bounds.Bottom == screen.Bounds.Top);
                    if (nextscreen != null && nextscreen.Bounds.Left < frm.Bounds.Left && nextscreen.Bounds.Right > frm.Bounds.Right)
                    {
                        screen = nextscreen;
                    }
                    else
                    {
                        speedy = -speedy;
                    }
                }
                if (frm.Bottom >= screen.Bounds.Bottom && speedy > 0)
                {
                    nextscreen = Array.Find(screens, x => x.Bounds.Top == screen.Bounds.Bottom);
                    if (nextscreen != null && nextscreen.Bounds.Left < frm.Bounds.Left && nextscreen.Bounds.Right > frm.Bounds.Right)
                    {
                        screen = nextscreen;
                    }
                    else
                    {
                        speedy = -speedy;
                    }
                }
            }
            return DialogResult;
        }

        public Window()
        {
            InitializeComponent();
            screen = Screen.FromControl(this);
            screens = Screen.AllScreens;
        }

        private void Window_Shown(object sender, EventArgs e)
        {
            pictureBox1.AllowDrop = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Image";
                ofd.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.ImageLocation = ofd.FileName;
                }
            }
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Activate();
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            Array data = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                string image = data.GetValue(0).ToString();
                pictureBox1.ImageLocation = image;
            }
        }

        private void pictureBox1_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            button1.Dispose();
            ActiveForm.ControlBox = false;
            timer1.Start();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            await MoveWindow();
        }
    }
}