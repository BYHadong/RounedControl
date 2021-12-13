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

        //Event Var
        private Point mouseDownLocation;
        private Point mouseMoveLocation;

        #region 속성
        private Color textColor = Color.Black;
        private Color borderColor = Color.Gray;
        private int borderWidth = 5;
        private Color textBackColor = Color.Gray;
        private int radius = 30;
        private TitleTextAlign textAlign = TitleTextAlign.Center;
        private Font titleFont;
        private bool mainFrom = false;
        private Rectangle headerRectangle;

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

        public Font TitleFont { get => titleFont; set { titleFont = value; Invalidate(); } }
        public bool MainFrom { get => mainFrom; set => mainFrom = value; }
        public Rectangle HeaderRectangle { get => headerRectangle; }
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
            TitleFont = new Font("맑은 고딕", 12F, FontStyle.Bold);
            Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
            Width = 200;
            Height = 200;
        }

        #region override
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            var chiled = e.Control;
            if (chiled.Dock == DockStyle.Fill || (chiled.Anchor == AnchorStyles.Top && chiled.Anchor == AnchorStyles.Bottom && chiled.Anchor == AnchorStyles.Right && chiled.Anchor == AnchorStyles.Left))
            {
                chiled.Location = new Point(BorderWidth, HeaderRectangle.Height);
                chiled.Width = Width - BorderWidth;
                chiled.Height = Height - HeaderRectangle.Height;
            }
        }

        #region Mouse Event
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if ((mouseMoveLocation.X > BorderWidth || mouseMoveLocation.X < Width - BorderWidth - Margin.Right))
            {
                Cursor = Cursors.Default;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseMoveLocation = e.Location;

            if (e.Button != MouseButtons.Left)
            {
                if ((mouseMoveLocation.X <= HeaderRectangle.Width && mouseMoveLocation.Y <= HeaderRectangle.Height) && MainFrom)
                {
                    Cursor = Cursors.Hand;
                }

                if ((mouseMoveLocation.Y >= Height - BorderWidth - Margin.Bottom && mouseMoveLocation.X >= Width - BorderWidth) && MainFrom)
                {
                    Cursor = Cursors.SizeNWSE; // 좌상 우하
                }
                //else if ((mouseMoveLocation.Y <= BorderWidth && mouseMoveLocation.X >= Width - BorderWidth) ||
                //    (mouseMoveLocation.Y >= Height - BorderWidth && mouseMoveLocation.X <= BorderWidth)
                //    && MainFrom)
                //{
                //    Cursor = Cursors.SizeNESW; // 우상 좌하
                //}
                else if ((mouseMoveLocation.X >= Width - BorderWidth) && MainFrom)
                {
                    Cursor = Cursors.SizeWE; // 좌우
                }
                else if ((mouseMoveLocation.Y >= Height - BorderWidth) && MainFrom)
                {
                    Cursor = Cursors.SizeNS; // 상하
                }

                if(!(mouseMoveLocation.X <= HeaderRectangle.Width && mouseMoveLocation.Y <= HeaderRectangle.Height) && 
                    !(mouseMoveLocation.Y >= Height - BorderWidth - Margin.Bottom && mouseMoveLocation.X >= Width - BorderWidth) && 
                    !(mouseMoveLocation.X >= Width - BorderWidth) && 
                    !(mouseMoveLocation.Y >= Height - BorderWidth))
                {
                    Cursor = Cursors.Default;
                }

            }
            else if(e.Button == MouseButtons.Left)
            {
                if (Parent is Form)
                {
                    var parent = Parent as Form;
                    if (Cursor.Current == Cursors.Hand)
                    {
                        parent.Location = new Point(e.X + parent.Left - mouseDownLocation.X, e.Y + parent.Top - mouseDownLocation.Y);
                    }
                    //상하
                    else if (Cursor.Current == Cursors.SizeNS)
                    {
                        var width = Width;
                        var height = (mouseMoveLocation.Y);
                        parent.Size = new Size(width, height);
                    }
                    //좌우
                    else if (Cursor.Current == Cursors.SizeWE)
                    {
                        var width = (mouseMoveLocation.X);
                        var height = Height;
                        parent.Size = new Size(width, height);
                    }
                    //좌상 우하
                    else if (Cursor.Current == Cursors.SizeNWSE)
                    {
                        var width = (mouseMoveLocation.X);
                        var height = (mouseMoveLocation.Y);
                        parent.Size = new Size(width, height);
                    }
                }
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && MainFrom)
            {
                mouseDownLocation = e.Location;
            }
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.Button == MouseButtons.Left && Cursor == Cursors.Hand)
            {
                if (Parent is Form)
                {
                    var parent = Parent as Form;
                    if (parent.WindowState == FormWindowState.Maximized)
                    {
                        parent.WindowState = FormWindowState.Normal;
                        Width = parent.Width;
                        Height = parent.Height;
                    }
                    else
                    {
                        parent.WindowState = FormWindowState.Maximized;
                        Width = parent.Width;
                        Height = parent.Height;
                    }
                }
            }
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = ClientRectangle;
            using (var myPath = CreateRounedControl.CreateRounedGroupBoxControl(e.Graphics, this, ClientRectangle, Radius, BorderWidth))
            {
                int x, y, cwidth, cheight;
                x = rect.X;
                y = rect.Y;
                cwidth = rect.Width;
                cheight = TitleFont.Height + Font.Height + BorderWidth;
                var piont = new Point(x, y);
                var size = new Size(cwidth, cheight);
                //상단 텍스트 쪽 상자
                headerRectangle = new Rectangle(piont, size);
                SetDisplayRectangle(HeaderRectangle);
                //Content Box
                using (var brush = new SolidBrush(BackColor))
                    e.Graphics.FillPath(brush, myPath);
                var clip = e.Graphics.ClipBounds;
                e.Graphics.SetClip(HeaderRectangle);
                //Title Box
                using (var brush = new SolidBrush(TextBackColor))
                    e.Graphics.FillPath(brush, myPath);
                //Text 위치
                switch (TextAlign)
                {
                    case TitleTextAlign.Center:
                        TextRenderer.DrawText(e.Graphics, Text, TitleFont, HeaderRectangle, TextColor, (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter));
                        break;

                    case TitleTextAlign.Left:
                        var rectsu = HeaderRectangle;
                        rectsu.X = DEFAULT_MARGIN + BorderWidth * 2;
                        TextRenderer.DrawText(e.Graphics, Text, TitleFont, rectsu, TextColor, (TextFormatFlags.Left | TextFormatFlags.VerticalCenter));
                        break;
                    case TitleTextAlign.Right:
                        rectsu = HeaderRectangle;
                        rectsu.Width = HeaderRectangle.Width - DEFAULT_MARGIN - BorderWidth * 2;
                        TextRenderer.DrawText(e.Graphics, Text, TitleFont, rectsu, TextColor, (TextFormatFlags.Right | TextFormatFlags.VerticalCenter));
                        break;
                    default:
                        TextRenderer.DrawText(e.Graphics, Text, TitleFont, rect, TextColor, (TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter));
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
            paddingValue.Left = BorderWidth + Margin.Left;
            paddingValue.Right = BorderWidth + Margin.Right;
            paddingValue.Bottom = BorderWidth + Margin.Bottom;
            paddingValue.Top = TitleFont.Height + Margin.Top;
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
