using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedGListView : ListView
    {
        private int border = 0;
        private int radius = 30;

        public RounedGListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public int Border { get => border; set { border = value; Invalidate(); } }
        public int Radius { get => radius; set { radius = value; Invalidate(); } }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath myPath = CreateRounedControl.GetRounedGroupBoxinPanel(ClientRectangle, Radius, Border))
            {
                using (Pen pen = new Pen(BackColor, Border))
                    e.Graphics.DrawPath(pen, myPath);

                Region = new Region(myPath);
            }
        }
    }
}
