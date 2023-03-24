using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Genshin.Automation
{
    public partial class RectangleSelectorViewFrm : Form
    {
        private Rectangle _rectangle;

        public Rectangle rectangle
        {
            get { return _rectangle; }
            set
            {
                if (_rectangle != value)
                {
                    _rectangle = value;

                    // 触发弹窗提醒
                    CaptureRectangle(value);
                }
            }
        }
        Bitmap ?bitmap;
        public RectangleSelectorViewFrm()
        {
            InitializeComponent();
            this.TopMost = true;
            this.DoubleBuffered = true;
        }
        public void CaptureRectangle(Rectangle rect)
        {
            if (rect.Width != 0 && rect.Height != 0)
            {
                pictureBox1.Image = null;
                bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(rect.Location, Point.Empty, rect.Size, CopyPixelOperation.SourceCopy);
                }
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    Pen pen = new Pen(Color.Red, 1);

                    int centerX = bitmap.Width / 2;
                    int centerY = bitmap.Height / 2;

                    // 画横线
                    g.DrawLine(pen, centerX - 50, centerY, centerX + 50, centerY);

                    // 画竖线
                    g.DrawLine(pen, centerX, centerY - 50, centerX, centerY + 50);
                }
                pictureBox1.Image = bitmap;
            }

        }
    }
}
