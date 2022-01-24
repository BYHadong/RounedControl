using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace RounedControl
{
    public partial class RounedRangeTrackBar : UserControl
    {
        public enum ShowDataType
        {
            Time,
            Number
        }
        public enum ClickData
        {
            Hi,
            Low,
            None
        }
        /// <summary>
        /// Mouse Up 이벤트에 발동됨
        /// </summary>
        /// <param name="he">0일때 최대 범위를 표시 </param>
        /// <param name="low">0일때 최소 범위를 표시 </param>
        public delegate void ValueChangeEventHandler(int he, int low, ClickData data);
        public event ValueChangeEventHandler ValueChangeEvent;

        bool hiIsDown = false;
        bool lowIsDown = false;

        Point hiIsDownPoint;
        Point lowIsDownPoint;

        #region 속성

        int max = 100;
        int min = 0;
        int heVal = 30;
        int lowVal = 30;
        int lineWidth = 5;
        int itemWidth = 4;
        int itemBoarderWidth = 2;
        int showItemWidth = 10;
        Color itemBackColor = Color.White;
        Color lineBackColor = Color.White;
        Color nonLineBackColor = Color.Tomato;

        bool showTopBottomItem = false;

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
                if (value < min)
                {
                    max = min + 1;
                    Invalidate();
                }
                else
                {
                    max = value;
                    Invalidate();
                }
            }
        }
        [Description("최소값"), Category()]
        public int MinValue
        {
            get { return min; }
            set
            {
                if (value > max)
                {
                    min = max - 1;
                    Invalidate();
                }
                else
                {
                    min = value;
                    Invalidate();
                }
            }
        }

        [Description("높은 값"), Category()]
        public int HiValue
        {
            get { return heVal; }
            set
            {
                heVal = value; Invalidate();
            }
        }

        [Description("낮은 값"), Category()]
        public int LowValue
        {
            get { return lowVal; }
            set
            {
                lowVal = value; Invalidate();
            }
        }

        [Bindable(true)]
        public Color ItemBackColor { get => itemBackColor; set => itemBackColor = value; }
        [Bindable(true)]
        public Color LineBackColor { get => lineBackColor; set => lineBackColor = value; }
        [Bindable(true)]
        public Color NonLineBackColor { get => nonLineBackColor; set => nonLineBackColor = value; }
        [Bindable(true)]
        public bool ShowTopBottomItem { get => showTopBottomItem; set => showTopBottomItem = value; }
        [Bindable(true)]
        public int ShowItemWidth { get => showItemWidth; set => showItemWidth = value; }
        [Bindable(true)]
        public ShowDataType DataType { get; set; } = ShowDataType.Time;

        #endregion
        public RounedRangeTrackBar()
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
                    grp.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                    grp.SmoothingMode = SmoothingMode.AntiAlias;

                    var width = Width - ItemWidth;
                    var height = Height / 2;
                    Pen dataLine = new Pen(LineBackColor, LineWidth);
                    Pen nonDataLine = new Pen(NonLineBackColor, LineWidth);
                    Pen dataLineHeader = new Pen(Color.Black, ItemBoarderWidth);

                    var currentHiValue = GetCurrentValueToInt(HiValue, (Math.Abs(MinValue) + Math.Abs(MaxValue)), width);
                    var currentLowValue = GetCurrentValueToInt(LowValue, (Math.Abs(MinValue) + Math.Abs(MaxValue)), width);
                    var hiValueLangth = width - currentHiValue;

                    var headerHeight = (LineWidth * 2);

                    //Low Line
                    grp.DrawLine(dataLine, 0, height, currentLowValue, height);
                    //nonDataLine
                    grp.DrawLine(nonDataLine, currentLowValue + ItemWidth, height, hiValueLangth, height);
                    //Hi Line
                    grp.DrawLine(dataLine, Width, height, hiValueLangth + ItemWidth, height);

                    grp.FillRectangle(new SolidBrush(LineBackColor), new Rectangle(currentLowValue, height - (LineWidth), ItemWidth, headerHeight));
                    grp.DrawRectangle(dataLineHeader, new Rectangle(currentLowValue, height - (LineWidth), ItemWidth, headerHeight));

                    grp.FillRectangle(new SolidBrush(LineBackColor), new Rectangle(hiValueLangth, height - (LineWidth), ItemWidth, headerHeight));
                    grp.DrawRectangle(dataLineHeader, new Rectangle(hiValueLangth, height - (LineWidth), ItemWidth, headerHeight));

                    //Point를 잡아 끌어 당기거나 할 수 있게 만들어줌
                    lowIsDownPoint = new Point(currentLowValue + (ItemWidth / 4), height - (headerHeight / 2));
                    hiIsDownPoint = new Point(hiValueLangth + (ItemWidth / 4), height - (headerHeight / 2));

                    if (ShowTopBottomItem && DataType == ShowDataType.Time)
                    {
                        var font = new Font("맑은 고딕", 12, FontStyle.Bold);

                        var lowTime = GetLowCureentValueToDateTime().ToString("HH:mm");
                        var lowItemX = currentLowValue - (ItemWidth);
                        var lowItemY = height - (headerHeight * 2 - (LineWidth));

                        var hiTime = GetHiCureentValueToDateTime().ToString("HH:mm");
                        var hiItemX = hiValueLangth - (ItemWidth);
                        var hiItemY = height + (headerHeight - (LineWidth / 2));


                        var lowEllipse = CreateTragle(e.Graphics, lowItemX, lowItemY);
                        var hiEllipse = CreateTragle(e.Graphics, hiItemX, hiItemY, true);

                        Console.WriteLine($"H Item X : {hiItemX} // L Item X : {lowItemX}");
                        if ((lowItemX + (ShowItemWidth / 2)) > width)
                        {
                            //Low Item Set
                            Console.WriteLine($"Low > Width");
                        }
                        if ((hiItemX + (ShowItemWidth / 2)) > width)
                        {
                            //Hi Item Set
                            Console.WriteLine($"Hi > Width");
                        }

                        if ((lowItemX) < (ShowItemWidth / 2))
                        {
                            //Low Item Set
                            Console.WriteLine($"Low < 0");
                        }
                        if ((hiItemX) < (ShowItemWidth / 2))
                        {
                            //Hi Item Set
                            Console.WriteLine($"Hi < 0");
                        }

                        grp.FillPath(new SolidBrush(ItemBackColor), lowEllipse);
                        grp.DrawPath(new Pen(Color.Black, ItemBoarderWidth), lowEllipse);

                        grp.FillPath(new SolidBrush(ItemBackColor), hiEllipse);
                        grp.DrawPath(new Pen(Color.Black, ItemBoarderWidth), hiEllipse);

                        var lowTimeTextLength = grp.MeasureString(lowTime, font, new PointF(lowItemX, lowItemY), StringFormat.GenericTypographic);
                        grp.DrawString(lowTime, font, new SolidBrush(Color.Black), new Point(lowItemX + (ShowItemWidth), lowItemY + ((ShowItemWidth / 2) - ((int)lowTimeTextLength.Height / 2))));

                        var hiTimeTextLength = grp.MeasureString(hiTime, font, new PointF(hiItemX, hiItemY), StringFormat.GenericTypographic);
                        grp.DrawString(hiTime, font, new SolidBrush(Color.Black), new Point(hiItemX - ((int)hiTimeTextLength.Width) - 5, hiItemY + ((ShowItemWidth / 2) - ((int)hiTimeTextLength.Height / 2))));

                        lowIsDownPoint = new Point(lowItemX, height - (headerHeight + LineWidth));
                        hiIsDownPoint = new Point(hiItemX, height + (LineWidth + (LineWidth / 2)));
                    }
                    else if(DataType == ShowDataType.Number && ShowTopBottomItem)
                    {
                        var font = new Font("맑은 고딕", 12, FontStyle.Bold);

                        var lowTime = (LowValue - Math.Abs(MinValue)).ToString();
                        var lowItemX = currentLowValue - (ItemWidth);
                        var lowItemY = height - (headerHeight * 2 - (LineWidth));

                        var hiTime = (Math.Abs(MaxValue) - HiValue).ToString();
                        var hiItemX = hiValueLangth - (ItemWidth);
                        var hiItemY = height + (headerHeight - (LineWidth / 2));


                        var lowEllipse = CreateTragle(e.Graphics, lowItemX, lowItemY);
                        var hiEllipse = CreateTragle(e.Graphics, hiItemX, hiItemY, true);

                        Console.WriteLine($"H Item X : {hiItemX} // L Item X : {lowItemX}");
                        if ((lowItemX + (ShowItemWidth / 2)) > width)
                        {
                            //Low Item Set
                            Console.WriteLine($"Low > Width");
                        }
                        if ((hiItemX + (ShowItemWidth / 2)) > width)
                        {
                            //Hi Item Set
                            Console.WriteLine($"Hi > Width");
                        }

                        if ((lowItemX) < (ShowItemWidth / 2))
                        {
                            //Low Item Set
                            Console.WriteLine($"Low < 0");
                        }
                        if ((hiItemX) < (ShowItemWidth / 2))
                        {
                            //Hi Item Set
                            Console.WriteLine($"Hi < 0");
                        }

                        grp.FillPath(new SolidBrush(ItemBackColor), lowEllipse);
                        grp.DrawPath(new Pen(Color.Black, ItemBoarderWidth), lowEllipse);

                        grp.FillPath(new SolidBrush(ItemBackColor), hiEllipse);
                        grp.DrawPath(new Pen(Color.Black, ItemBoarderWidth), hiEllipse);

                        var lowTimeTextLength = grp.MeasureString(lowTime, font, new PointF(lowItemX, lowItemY), StringFormat.GenericTypographic);
                        grp.DrawString(lowTime, font, new SolidBrush(Color.Black), new Point(lowItemX + (ShowItemWidth), lowItemY + ((ShowItemWidth / 2) - ((int)lowTimeTextLength.Height / 2))));

                        var hiTimeTextLength = grp.MeasureString(hiTime, font, new PointF(hiItemX, hiItemY), StringFormat.GenericTypographic);
                        grp.DrawString(hiTime, font, new SolidBrush(Color.Black), new Point(hiItemX - ((int)hiTimeTextLength.Width) - 5, hiItemY + ((ShowItemWidth / 2) - ((int)hiTimeTextLength.Height / 2))));

                        lowIsDownPoint = new Point(lowItemX, height - (headerHeight + LineWidth));
                        hiIsDownPoint = new Point(hiItemX, height + (LineWidth + (LineWidth / 2)));
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

        private GraphicsPath CreateTragle(Graphics graphics, int x, int y, bool rebsers = false)
        {
            var path = new GraphicsPath();
            if (rebsers)
            {
                path.AddLine(new Point(x, y + (ShowItemWidth)), new Point(x + ShowItemWidth, y + (ShowItemWidth)));
                path.AddLine(new Point(x + ShowItemWidth, y + (ShowItemWidth)), new Point(x + (ShowItemWidth / 2), y));
                path.AddLine(new Point(x + (ShowItemWidth / 2), y), new Point(x, y + (ShowItemWidth)));
            }
            else
            {
                path.AddLine(new Point(x, y), new Point(x + ShowItemWidth, y));
                path.AddLine(new Point(x + ShowItemWidth, y), new Point(x + (ShowItemWidth / 2), y + (ShowItemWidth)));
                path.AddLine(new Point(x + (ShowItemWidth / 2), y + (ShowItemWidth)), new Point(x, y));
            }
            return path;
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (ShowTopBottomItem)
            {
                var downRac = new Rectangle(e.X - (ShowItemWidth), e.Y - (ShowItemWidth), ShowItemWidth * 2, ShowItemWidth * 2);
                var hiRectangle = new Rectangle(hiIsDownPoint, new Size(ShowItemWidth, ShowItemWidth));
                var lowRectangle = new Rectangle(lowIsDownPoint, new Size(ShowItemWidth, ShowItemWidth));

                if (downRac.Contains(hiRectangle))
                {
                    hiIsDown = true;
                }
                else if (downRac.Contains(lowRectangle))
                {
                    lowIsDown = true;
                }
            }
            else
            {
                var downRac = new Rectangle(e.X - (ItemWidth), e.Y - ((LineWidth * 2)), ItemWidth * 2, LineWidth * 3);
                if (downRac.Contains(hiIsDownPoint))
                {
                    hiIsDown = true;
                }
                else if (downRac.Contains(lowIsDownPoint))
                {
                    lowIsDown = true;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var clickData = ClickData.None;
            if (hiIsDown)
            {
                clickData = ClickData.Hi;
            }
            else if (lowIsDown)
            {
                clickData = ClickData.Low;
            }
            if (hiIsDown || lowIsDown)
            {
                MoveProgress(e.X, clickData);
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            hiIsDown = false;
            lowIsDown = false;
        }

        private void MoveProgress(int X, ClickData clickData)
        {
            try
            {
                var width = (Width - ItemWidth);

                if (X <= 0)
                    X = 0;
                if (X >= width)
                    X = width;

                if (hiIsDown)
                {
                    var calX = width - X;
                    HiValue = GetCurrentValueToInt(calX, width, (Math.Abs(MinValue) + Math.Abs(MaxValue)));
                    ValueChangeEvent(HiValue, LowValue, clickData);
                }
                else if (lowIsDown)
                {
                    LowValue = GetCurrentValueToInt(X, width, (Math.Abs(MinValue) + Math.Abs(MaxValue)));
                    ValueChangeEvent(HiValue, LowValue, clickData);
                }

                if ((MaxValue - (HiValue + LowValue)) <= 0)
                {
                    if (hiIsDown)
                    {
                        LowValue = GetCurrentValueToInt(X, width, (Math.Abs(MinValue) + Math.Abs(MaxValue)));
                        ValueChangeEvent(HiValue, LowValue, clickData);
                    }
                    else if (lowIsDown)
                    {
                        var calX = width - X;
                        HiValue = GetCurrentValueToInt(calX, width, (Math.Abs(MinValue) + Math.Abs(MaxValue)));
                        ValueChangeEvent(HiValue, LowValue, clickData);
                    }
                }
            }
            catch
            { }
        }

        //          var currentLowValue = GetCurrentValueToInt(LowValue, MinValue, width);
        private int GetCurrentValueToInt(int a1, int a2, int b)
        {
            //a1 : 원래 값
            //a2 : 원래 최대값
            //b : 변환 최대값
            //return : 변환 값
            return a1 * b / a2;
        }
        public DateTime GetHiCureentValueToDateTime()
        {
            var nowDate = DateTime.Now;
            nowDate = nowDate.Add(new TimeSpan(-nowDate.Hour, -nowDate.Minute, -nowDate.Second));
            var value = MaxValue - HiValue;
            nowDate = nowDate.AddMinutes(value);
            return nowDate;
        }

        public DateTime GetLowCureentValueToDateTime()
        {
            var nowDate = DateTime.Now;
            nowDate = nowDate.Add(new TimeSpan(-nowDate.Hour, -nowDate.Minute, -nowDate.Second));
            nowDate = nowDate.AddMinutes(LowValue);
            return nowDate;
        }
    }
}
