using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedButton : Button
    {
        public delegate void ButtonClickEventHanddler(bool click, RounedButton name);
        public event ButtonClickEventHanddler RounedButton_ClickEvnet;

        public enum ButtonStyle
        {
            Button,
            CheckBox
        }

        public readonly Color CLICK_COLOR = Color.LightSkyBlue;
        public readonly Color NOT_CLICK_COLOR = Color.LightGray;
        public readonly Color HOVER_BUTTON_COLOR = Color.DarkGray;
        private readonly int HOVER_BORDER_SIZE = 4;

        public Color leavColor;
        private int radius = 30;
        private bool clicked = false;
        private Color borderColor = Color.Black;
        private int borderSize = 0;
        private bool borderVisible = false;
        private StringAlignment textAlignFormat = StringAlignment.Center;
        private ButtonStyle buttonStyle = ButtonStyle.Button;
        private int leavBorderSize;
        private Color leavBorderColor;
        private bool leavBorderVisible;
        private bool hoverEvent = false;

        public int Radius { get => radius; set { radius = value; Invalidate(); } }
        public bool Clicked { get => clicked; set { clicked = value; ClickChangeColor(); } }
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
        public int BorderSize { get => borderSize; set { borderSize = value; Invalidate(); } }
        public StringAlignment TextAlignFormat { get => textAlignFormat; set { textAlignFormat = value; Invalidate(); } }
        public bool BorderVisible { get => borderVisible; set { borderVisible = value; Invalidate(); } }
        public ButtonStyle SelectButtonStyle { get => buttonStyle; set => buttonStyle = value; }

        public RounedButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserMouse, Enabled);
            BackColor = Color.LightGray;
            leavColor = BackColor;
            ForeColor = Color.Black;
            Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var myPath = CreateRounedControl.CreateRounedButtonControl(pevent.Graphics, this, ClientRectangle, Radius, BorderSize))
            {
                using (var brush = new SolidBrush(BackColor))
                    pevent.Graphics.FillPath(brush, myPath);

                var textFormat = new StringFormat();
                textFormat.LineAlignment = TextAlignFormat;
                textFormat.Alignment = TextAlignFormat;
                pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle, textFormat);

                if (BorderVisible == true)
                {
                    using (var pen = new Pen(BorderColor, BorderSize))
                        pevent.Graphics.DrawPath(pen, myPath);
                }
            }
        }

        private void ClickChangeColor()
        {
            if (SelectButtonStyle == ButtonStyle.CheckBox)
            {
                if(Clicked == true)
                {
                    BackColor = CLICK_COLOR;
                    leavColor = BackColor;
                }
                else
                {
                    BackColor = NOT_CLICK_COLOR;
                    leavColor = BackColor;
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (Enabled)
            {
                Clicked = !Clicked;
                RounedButton_ClickEvnet?.Invoke(Clicked, this);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (Enabled)
            {
                leavBorderSize = BorderSize;
                leavBorderColor = BorderColor;
                leavBorderVisible = BorderVisible;
                MouseHoverAction();
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (Enabled)
            {
                MouseLeaveAction();
            }
        }

        void MouseHoverAction()
        {
            BorderColor = BackColor != CLICK_COLOR ? CLICK_COLOR : NOT_CLICK_COLOR;
            BorderVisible = true;
            BorderSize = HOVER_BORDER_SIZE;
        }

        void MouseLeaveAction()
        {
            BorderSize = leavBorderSize;
            BorderColor = leavBorderColor;
            BorderVisible = leavBorderVisible;
        }
    }
}
