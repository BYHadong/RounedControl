using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RounedControl
{
    public partial class RounedTrackBar : UserControl
    {

        public delegate void ValueChangeEventHandler(int value);
        public event ValueChangeEventHandler ValueChangeEvent;

        bool isDown = false;
        Point isDownPoint;

        #region 속성
        int max = 100;
        int heVal = 50;
        int lineWidth = 5;
        int itemWidth = 15;
        int itemBoarderWidth = 2;


        Color itemBackColor = Color.DimGray;
        Color lineBackColor = Color.DimGray;
        Color nonLineBackColor = Color.Tomato;

        [Description("라인의 두께"), Category()]
        public int LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
                Invalidate();
            }
        }

        [Description("원의 두께"), Category()]
        public int ItemWidth
        {
            get
            {
                return itemWidth;
            }
            set
            {
                itemWidth = value;
                Invalidate();
            }
        }

        [Description("원의 두께"), Category()]
        public int ItemBoarderWidth
        {
            get
            {
                return itemBoarderWidth;
            }
            set
            {
                itemBoarderWidth = value;
                Invalidate();
            }
        }

        [Description("최대값"), Category()]
        public int MaxValue
        {
            get { return max; }
            set
            {
                if (max == 0)
                    max = 1;
                max = value; Invalidate();
            }
        }

        [Description("높은 값"), Category()]
        public int Value
        {
            get { return heVal; }
            set
            {
                if (heVal == 0)
                    heVal = 1;
                heVal = value; Invalidate();
            }
        }

        public Color ItemBackColor { get => itemBackColor; set => itemBackColor = value; }
        public Color LineBackColor { get => lineBackColor; set => lineBackColor = value; }
        public Color NonLineBackColor { get => nonLineBackColor; set => nonLineBackColor = value; }

        #endregion

        public RounedTrackBar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserMouse | ControlStyles.UserPaint, true);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            try
            {
                using (var grp = e.Graphics)
                {
                    grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    var width = Width - ItemWidth;
                    var height = Height / 2;
                    Pen dataLine = new Pen(LineBackColor, LineWidth);
                    Pen nonDataLine = new Pen(NonLineBackColor, LineWidth);
                    Pen dataLineHeader = new Pen(Color.Black, ItemBoarderWidth);

                    var currentValue = GetCurrentValueToInt(Value, MaxValue, width);
                    var valueLangth = width - currentValue;
                    var headerHeight = (LineWidth * 2);

                    grp.DrawLine(dataLine, 0, height, currentValue, height);
                    grp.DrawLine(nonDataLine, currentValue + ItemWidth, height, Width, height);

                    grp.FillRectangle(new SolidBrush(LineBackColor), new Rectangle(currentValue, height - (LineWidth), ItemWidth, headerHeight));
                    grp.DrawRectangle(dataLineHeader, new Rectangle(currentValue, height - (LineWidth), ItemWidth, headerHeight));

                    isDownPoint = new Point(currentValue, height - (LineWidth));
                }
            }
            catch
            {

            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            var headerHeight = (LineWidth * 2);
            var downRac = new Rectangle(0, e.Y - headerHeight, Width + ItemWidth, headerHeight * 2);
            if (downRac.Contains(isDownPoint))
            {
                isDown = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (isDown)
            {
                MoveProgress(e.X);
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isDown = false;
        }

        private void MoveProgress(int X)
        {
            try
            {
                if (X <= 0)
                    X = 0;
                if (X >= Width)
                    X = Width;

                if (isDown)
                {
                    Value = GetCurrentValueToInt(X, Width, MaxValue);
                    ValueChangeEvent(Value);
                }
            }
            catch
            { }
        }

        private int GetCurrentValueToInt(int a1, int a2, int b)
        {
            //a1 : 원래 값
            //a2 : 원래 최대값
            //b : 변환 최대값
            //return : 변환 값
            return a1 * b / a2;
        }

        public DateTime GetCureentValueToDateTime()
        {
            var nowDate = DateTime.Now;
            nowDate = nowDate.Add(new TimeSpan(-nowDate.Hour, -nowDate.Minute, -nowDate.Second));
            nowDate = nowDate.AddMinutes(Value);
            return nowDate;
        }
    }
}
