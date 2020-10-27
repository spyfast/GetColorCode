using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GetColorCode
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point point);

        [DllImport("gdi32.dll", 
            CharSet = CharSet.Auto,
            SetLastError = true,
            ExactSpelling = true)]
        static extern int BitBlt(
            IntPtr intPrt,
            int x,
            int y,
            int width,
            int height,
            IntPtr intPrt2,
            int xSrc,
            int ySrc,
            int dwRop);

        Bitmap screen = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public MainForm()
        {
            InitializeComponent();
        }
        public Color GetPixelColor(Point point)
        {
            using (Graphics graphics = Graphics.FromImage(screen))
            {
                using (Graphics graphics1 = Graphics.FromHwnd(IntPtr.Zero))
                {
                    var src = graphics1.GetHdc();
                    var src2 = graphics.GetHdc();
                    var val = BitBlt(src2, 0, 0, 1, 1, src, point.X, point.Y,
                        (int)CopyPixelOperation.SourceCopy);
                    graphics1.ReleaseHdc();
                }
            }

            return screen.GetPixel(0, 0);
        }

        private void TimeUpdates_Tick(object sender, EventArgs e)
        {
            var point = new Point();
            GetCursorPos(ref point);

            var cursor = GetPixelColor(point);
            BackColor = cursor;
            label_CodeColor.Text = "Code color: " + BackColor.Name;
        }
    }
}
