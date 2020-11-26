using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedLabel : Label
    {
        private int radius = 15;
        private int borderSize = 5;
        private Color borderColor = Color.Black;
        private bool borderVisible = true;
        private Color boxColor = Color.White;

        public int Radius { get => radius; set { radius = value; Invalidate(); } }
        public int BorderSize { get => borderSize; set { borderSize = value; Invalidate(); } }
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
        public bool BorderVisible { get => borderVisible; set { borderVisible = value; Invalidate(); } }

        public Color BoxColor { get => boxColor; set => boxColor = value; }

        public RounedLabel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
            TextAlign = ContentAlignment.MiddleCenter;
            BackColor = Color.Transparent;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = ClientRectangle;
            using (var myPath = CreateRounedControl.CreateRounedBasicControl(rect, Radius, BorderSize))
            {
                using (var brush = new SolidBrush(BoxColor))
                    e.Graphics.FillPath(brush, myPath);

                if (BorderVisible)
                {
                    using (var pen = new Pen(BorderColor, BorderSize))
                        e.Graphics.DrawPath(pen, myPath);
                }

                e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), rect, TextAlignFormat());
                TextRenderer.MeasureText(Text, Font, rect.Size);
            }
        }

        /// <summary>
        /// Label의 Text의 위치를 지정해줌
        /// </summary>
        /// <returns>위치값</returns>
        StringFormat TextAlignFormat()
        {
            var stringFormat = new StringFormat();
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                    {
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        return stringFormat;
                    }
                case ContentAlignment.TopCenter:
                    {
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        return stringFormat;
                    }
                case ContentAlignment.TopRight:
                    {
                        stringFormat.Alignment = StringAlignment.Near;
                        stringFormat.LineAlignment = StringAlignment.Far;
                        return stringFormat;
                    }
                case ContentAlignment.MiddleLeft:
                    {
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        return stringFormat;
                    }
                case ContentAlignment.MiddleRight:
                    {
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Far;
                        return stringFormat;
                    }
                case ContentAlignment.BottomLeft:
                    {
                        stringFormat.Alignment = StringAlignment.Far;
                        stringFormat.LineAlignment = StringAlignment.Near;
                        return stringFormat;
                    }
                case ContentAlignment.BottomCenter:
                    {
                        stringFormat.Alignment = StringAlignment.Far;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        return stringFormat;
                    }
                case ContentAlignment.BottomRight:
                    {
                        stringFormat.Alignment = StringAlignment.Far;
                        stringFormat.LineAlignment = StringAlignment.Far;
                        return stringFormat;
                    }
                default: //Middel Center
                    {
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        return stringFormat;
                    }
            }
        }
    }
}