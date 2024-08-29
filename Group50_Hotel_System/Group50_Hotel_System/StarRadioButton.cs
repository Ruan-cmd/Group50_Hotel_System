using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Group50_Hotel_System
{
    public class StarRadioButton : RadioButton
    {
        public StarRadioButton()
        {
            this.Appearance = Appearance.Button;
            this.AutoSize = false;
            this.Size = new Size(40, 40); // Adjust size for the star
            this.FlatStyle = FlatStyle.Flat;
            this.CheckedChanged += new EventHandler(StarRadioButton_CheckedChanged);
        }

        private void StarRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // Redraw the control to update its appearance when the Checked state changes
            this.Invalidate();

            if (this.Checked)
            {
                // Uncheck other radio buttons in the same parent container
                foreach (Control control in this.Parent.Controls)
                {
                    if (control is StarRadioButton && control != this)
                    {
                        ((StarRadioButton)control).Checked = false;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            // Clear the background
            pevent.Graphics.Clear(this.Parent.BackColor);

            // Determine the colors for the gradient
            Color startColor = this.Checked ? Color.Gold : Color.Gray;
            Color endColor = this.Checked ? Color.Yellow : Color.Gray;

            // Draw the star with a gradient fill and a border
            int starSize = this.Height - 5; // Set star size relative to the height
            Rectangle starRect = new Rectangle(0, 0, starSize, starSize); // Adjust the star size and position

            using (GraphicsPath path = new GraphicsPath())
            {
                Point[] starPoints = GetStarPoints(starRect);
                path.AddPolygon(starPoints);

                // Use a PathGradientBrush to create a radial gradient
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = startColor;
                    brush.SurroundColors = new Color[] { endColor };
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    pevent.Graphics.FillPolygon(brush, starPoints);
                }

                // Draw the border
                using (Pen borderPen = new Pen(Color.Black, 1)) // Black border with a width of 1
                {
                    pevent.Graphics.DrawPolygon(borderPen, starPoints);
                }
            }

            // Draw the text next to the star
            int textXPosition = starSize + 5; // Set text position after the star with padding
            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, new Point(textXPosition, (this.Height - this.Font.Height) / 2), this.ForeColor);
        }

        private Point[] GetStarPoints(Rectangle rect)
        {
            int width = rect.Width;
            int height = rect.Height;
            int centerX = rect.X + width / 2;
            int centerY = rect.Y + height / 2;
            int outerRadius = Math.Min(width, height) / 2;
            int innerRadius = outerRadius / 2;

            PointF[] points = new PointF[10];
            double angle = -Math.PI / 2;
            double angleIncrement = Math.PI / 5;

            for (int i = 0; i < 10; i++)
            {
                int radius = i % 2 == 0 ? outerRadius : innerRadius;
                points[i] = new PointF(
                    centerX + (float)(radius * Math.Cos(angle)),
                    centerY + (float)(radius * Math.Sin(angle))
                );
                angle += angleIncrement;
            }

            return Array.ConvertAll(points, Point.Round);
        }
    }
}
