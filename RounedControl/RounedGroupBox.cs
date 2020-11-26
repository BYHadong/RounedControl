using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedGroupBox : GroupBox
    {
        public enum TitleTextAlign
        {
            Center,
            Left,
            Right
        }

        #region 속성
        private Color textColor = Color.Black;
        private Color borderColor = Color.Gray;
        private int borderWidth = 3;
        private Color textBackColor = Color.Gray;
        private int radius = 30;
        private TitleTextAlign textAlign = TitleTextAlign.Center;
        
        /// <summary>
        /// 상단 타이틀 박스의 텍스트 색상을 설정
        /// </summary>
        public Color TextColor { get => textColor; set { textColor = value; Invalidate(); } }
        /// <summary>
        /// 상단 타이틀 박스의 배경 색상을 설정
        /// </summary>
        public Color TextBackColor { get => textBackColor; set { textBackColor = value; Invalidate(); } }
        /// 테두리의 색상을 설정
        /// </summary>
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
        /// <summary>
        /// 테두리의 굵기를 설정
        /// </summary>
        public int BorderWidth { get => borderWidth; set { borderWidth = value; Invalidate(); } }
        /// <summary>
        /// 둥근 정도를 설정
        /// </summary>
        public int Radius { get => radius; set { radius = value; Invalidate(); } }
        /// <summary>
        /// 상단 타이틀의 제목의 위치를 수정
        /// </summary>
        public TitleTextAlign TextAlign { get => textAlign; set { textAlign = value; Invalidate(); } }
        #endregion

        #region 상수
        //Const
        /// <summary>
        /// 마진값
        /// </summary>
        private const int DEFAULT_MARGIN = 3;
        #endregion

        //padding 값(Dock 설정)
        private int paddingTop = 0;
        private int paddingBottom = 0;
        private int paddingLeft = 0;
        private int paddingRight = 0;


        public RounedGroupBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
            Dock = DockStyle.Fill;
            Width = 200;
            Height = 200;
        }

        #region override
        /// <summary>
        /// Dock 설정
        /// </summary>
        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            if (Padding.Top < paddingTop || (Padding.Left | Padding.Right | Padding.Bottom) < paddingBottom)
            {
                Padding = new Padding(paddingLeft, paddingTop, paddingRight, paddingBottom);
                return;
            }
            int left, top, right, bottom;
            left = Padding.Left;
            top = Padding.Top;
            right = Padding.Right;
            bottom = Padding.Bottom;
            Padding = new Padding(left, top, right, bottom);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = ClientRectangle;
            using (var myPath = CreateRounedControl.CreateRounedGroupBoxControl(e.Graphics, this, ClientRectangle, Radius, BorderWidth))
            {
                int x, y, cwidth, cheight;
                x = rect.X;
                y = rect.Y - (BorderWidth / 2);
                cwidth = rect.Width;
                cheight = Font.Height + (BorderWidth * 2) + DEFAULT_MARGIN;
                var piont = new Point(x, y);
                var size = new Size(cwidth, cheight);
                //상단 텍스트 쪽 상자
                rect = new Rectangle(piont, size);
                SetDisplayRectangle(rect);
                //Content Box
                using (var brush = new SolidBrush(BackColor))
                    e.Graphics.FillPath(brush, myPath);
                var clip = e.Graphics.ClipBounds;
                e.Graphics.SetClip(rect);
                //Title Box
                using (var brush = new SolidBrush(TextBackColor))
                    e.Graphics.FillPath(brush, myPath);
                //Text 위치
                switch (TextAlign)
                {
                    case TitleTextAlign.Center:
                        TextRenderer.DrawText(e.Graphics, Text, Font, rect, TextColor, (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter));
                        break;

                    case TitleTextAlign.Left:
                        var rectsu = rect;
                        rectsu.X = DEFAULT_MARGIN + BorderWidth * 2;
                        TextRenderer.DrawText(e.Graphics, Text, Font, rectsu, TextColor, (TextFormatFlags.Left | TextFormatFlags.VerticalCenter));
                        break;
                    case TitleTextAlign.Right:
                        rectsu = rect;
                        rectsu.Width = rect.Width - DEFAULT_MARGIN - BorderWidth * 2;
                        TextRenderer.DrawText(e.Graphics, Text, Font, rectsu, TextColor, (TextFormatFlags.Right | TextFormatFlags.VerticalCenter));
                        break;
                    default:
                        TextRenderer.DrawText(e.Graphics, Text, Font, rect, TextColor, (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter));
                        break;
                }
                e.Graphics.SetClip(clip);
                //Rouned
                using (var pen = new Pen(BorderColor, BorderWidth))
                    e.Graphics.DrawPath(pen, myPath);
            }
        }
        #endregion

        /// <summary>
        /// Dock의 설정영역을 설정 해줌
        /// </summary>
        /// <param name="rect">상단 텍스트박스 영역</param>
        private void SetDisplayRectangle(Rectangle rect)
        {
            var paddingValue = new Padding();
            paddingValue.Left = BorderWidth + DEFAULT_MARGIN;
            paddingValue.Right = BorderWidth + DEFAULT_MARGIN;
            paddingValue.Bottom = BorderWidth + DEFAULT_MARGIN;
            paddingValue.Top = rect.Height / 2 + BorderWidth;
            //padding값이 초기 값이면 초기화 or BorderWidth값이 바뀔때 or rect의 높이가 바뀔때
            if (paddingTop == 0 && paddingLeft == 0 || paddingLeft != (BorderWidth + DEFAULT_MARGIN) || paddingTop != (rect.Height / 2 + BorderWidth))
            {
                paddingTop = paddingValue.Top;
                paddingBottom = paddingValue.Bottom;
                paddingLeft = paddingValue.Left;
                paddingRight = paddingValue.Right;
                Padding = paddingValue;
            }
        }
    }
}
