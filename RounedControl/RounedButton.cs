using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedButton : Button
    {
        public delegate void ButtonClickEventHanddler(bool click, RounedButton btn);
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
        private Color unCheckBoxBackColor = Color.LightGray;
        private Color checkBoxBackColor = Color.LightSkyBlue;

        public int Radius { get => radius; set { radius = value; Invalidate(); } }
        public bool Clicked { get => clicked; set { clicked = value; ClickChangeColor(); } }
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
        public int BorderSize { get => borderSize; set { borderSize = value; Invalidate(); } }
        public StringAlignment TextAlignFormat { get => textAlignFormat; set { textAlignFormat = value; Invalidate(); } }
        public bool BorderVisible { get => borderVisible; set { borderVisible = value; Invalidate(); } }
        public ButtonStyle SelectButtonStyle { get => buttonStyle; set => buttonStyle = value; }
        public Color UnCheckBoxBackColor { get => unCheckBoxBackColor; set => unCheckBoxBackColor = value; }
        public Color CheckBoxBackColor { get => checkBoxBackColor; set => checkBoxBackColor = value; }
        [Bindable(true)]
        public Size ImageSize { get; set; }
        [Bindable(true)]
        public bool HoverBorderUse { get; set; } = true;

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
                var textlen = TextRenderer.MeasureText(Text, Font);
                using (var brush = new SolidBrush(BackColor))
                    pevent.Graphics.FillPath(brush, myPath);

                if(Image != null)
                {
                    if(ImageSize.Width == 0 && ImageSize.Height == 0)
                        ImageSize = new Size(Image.Width, Image.Height);
                    if (ImageAlign == ContentAlignment.MiddleLeft)
                    {
                        pevent.Graphics.DrawImage(Image, x: 10, y: (Height - ImageSize.Height) / 2, width: ImageSize.Width, height: ImageSize.Height);
                    }
                    else if(ImageAlign == ContentAlignment.MiddleCenter)
                    {
                        if(TextImageRelation == TextImageRelation.ImageBeforeText)
                        {
                            pevent.Graphics.DrawImage(Image, x: (Width - ImageSize.Width - textlen.Width) / 2, y: (Height - ImageSize.Height) / 2, width: ImageSize.Width, height: ImageSize.Height);
                        }
                        else
                        {
                            pevent.Graphics.DrawImage(Image, x: (Width - ImageSize.Width) / 2, y: (Height - ImageSize.Height) / 2, width: ImageSize.Width, height: ImageSize.Height);
                        }
                    }
                }

                var textFormat = new StringFormat();
                textFormat.LineAlignment = TextAlignFormat;
                if(TextImageRelation == TextImageRelation.ImageBeforeText)
                {
                    var rec = ClientRectangle;
                    var imageX = (Width - ImageSize.Width - textlen.Width) / 2;
                    if (ImageAlign == ContentAlignment.MiddleCenter)
                    {
                        rec.X = imageX + ImageSize.Width;
                    }
                    else
                    {
                        rec.X = ImageSize.Width + 10;
                    }
                    pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), rec, textFormat);
                }
                else
                {
                    textFormat.Alignment = TextAlignFormat;
                    pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), ClientRectangle, textFormat);
                }
                

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
                    BackColor = CheckBoxBackColor;
                    leavColor = BackColor;
                }
                else
                {
                    BackColor = UnCheckBoxBackColor;
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
            if (Enabled && HoverBorderUse)
            {
                leavBorderSize = BorderSize;
                leavBorderColor = BorderColor;
                leavBorderVisible = BorderVisible;
                MouseHoverAction();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (Enabled && HoverBorderUse)
            {
                MouseLeaveAction();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if(Height+10 < Font.Height || Height + 10 < ImageSize.Height)
            {
                var height = Font.Height < ImageSize.Height ? ImageSize.Height : Font.Height;
                Height = height;
            }
            var textLen = TextRenderer.MeasureText(Text, Font);
            if (Width + 10 < textLen.Width || (Image != null && Width + 10 < (textLen.Width + ImageSize.Width + 10)))
            {
                var width = Image != null ? textLen.Width + ImageSize.Width + 10 : textLen.Width;
                Width = width;
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
