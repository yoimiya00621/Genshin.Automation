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

namespace Genshin.Automation
{
    public partial class RectangleSelectorFrm : Form
    {
        public Rectangle selectionRectangle;
        private Point startPoint;
        private bool isDragging = false;
        RectangleSelectorViewFrm viewFrm = new RectangleSelectorViewFrm();
        public RectangleSelectorFrm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.Paint += OnPaint;
            viewFrm.rectangle = new Rectangle();
            viewFrm.Show();
        }

        private void RectangleSelectorFrm_Load(object sender, EventArgs e)
        {

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
            this.Opacity = 0.1;
            this.Cursor = Cursors.Cross;
        }

        private void OnMouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.startPoint = e.Location;
                this.isDragging = true;
            }
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.isDragging)
                {
                    Point currentPoint = e.Location;
                    this.selectionRectangle = new Rectangle(
                        Math.Min(this.startPoint.X, currentPoint.X),
                        Math.Min(this.startPoint.Y, currentPoint.Y),
                        Math.Abs(this.startPoint.X - currentPoint.X),
                        Math.Abs(this.startPoint.Y - currentPoint.Y)
                    );
                    viewFrm.rectangle = this.selectionRectangle;
                    this.Invalidate();
                    return;
                }
            }
            viewFrm.rectangle = new Rectangle(e.Location.X - 20, e.Location.Y - 20, 40, 40);

        }

        private void OnMouseUp(object? sender, MouseEventArgs e)
        {
            this.isDragging = false;
            if (e.Button == MouseButtons.Left)
            {

                // 获取框选矩形
                Rectangle selectedRect = GetSelectionRectangle();

                // 在这里使用 selectedRect 变量来执行任何您想要的操作
                // ...
                viewFrm.rectangle = this.selectionRectangle;
                viewFrm.Close();
                this.Close();
            }
        }

        private void OnPaint(object? sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.White, this.selectionRectangle);
        }

        public Rectangle GetSelectionRectangle()
        {
            RectangleF scaledRect = new RectangleF(
                   this.selectionRectangle.X / (float)this.Width,
                   this.selectionRectangle.Y / (float)this.Height,
                   this.selectionRectangle.Width / (float)this.Width,
                   this.selectionRectangle.Height / (float)this.Height
               );

            float scaleX = Screen.PrimaryScreen.Bounds.Width;
            float scaleY = Screen.PrimaryScreen.Bounds.Height;
            scaledRect = new RectangleF(scaledRect.X * scaleX, scaledRect.Y * scaleY, scaledRect.Width * scaleX, scaledRect.Height * scaleY);

            return Rectangle.Round(scaledRect);
        }


    }
}
