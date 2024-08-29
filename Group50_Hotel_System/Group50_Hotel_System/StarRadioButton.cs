using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace YourNamespace // Replace with your actual namespace
{
    public class StarRadioButton : RadioButton
    {
        private bool _checked;

        public StarRadioButton()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Point[] starPoints = CreateStarPoints(new Point(this.Width / 2, this.Height / 2), this.Width / 2 - 5, this.Width / 4);
            using (SolidBrush brush = new SolidBrush(this._checked ? Color.Gold : Color.Gray))
            {
                g.FillPolygon(brush, starPoints);
            }

            using (Pen pen = new Pen(Color.Black, 1))
            {
                g.DrawPolygon(pen, starPoints);
            }

            TextRenderer.DrawText(g, this.Text, this.Font, new Point(this.Width, (this.Height - this.Font.Height) / 2), this.ForeColor);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this._checked = !this._checked;
            this.Invalidate();
        }

        private Point[] CreateStarPoints(Point center, int outerRadius, int innerRadius)
        {
            Point[] points = new Point[10];
            double angle = Math.PI / 5;
            for (int i = 0; i < 10; i++)
            {
                int r = (i % 2 == 0) ? outerRadius : innerRadius;
                points[i] = new Point(
                    center.X + (int)(r * Math.Cos(i * angle)),
                    center.Y - (int)(r * Math.Sin(i * angle)));
            }
            return points;
        }
    }
}
