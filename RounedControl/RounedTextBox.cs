using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedTextBox : UserControl
    {
        private StringBuilder textBuilder = new StringBuilder();
        private readonly Color SELECT_COLOR = Color.SkyBlue;
        private readonly Color UNSELECT_COLOR = Color.Black;

        #region 속성
        private Color downForeColor;
        private int radius = 30;
        private int borderSize = 5;
        private Color borderColor = Color.Black;
        private bool borderVisible = true;
        private StringAlignment textAlign = StringAlignment.Center;

        public int Radius { get => radius; set { radius = value; Invalidate(); } }
        public int BorderSize { get => borderSize; set { borderSize = value; Invalidate(); } }
        public Color BorderColor { get => borderColor; set { borderColor = value; Invalidate(); } }
        public bool BorderVisible { get => borderVisible; set { borderVisible = value; Invalidate(); } }
        public StringAlignment TextAlign { get => textAlign; set { textAlign = value; Invalidate(); } }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text { get; set; } = "";

        [Bindable(true)]
        public string HintText { get; set; } = "Hint";
        [Bindable(true)]
        public Color HintTextColor { get; set; } = Color.LightGray;
        #endregion

        public delegate void InputEnterEventHanddler(RounedTextBox rounedTextBox);
        public event InputEnterEventHanddler InputEnterEvent;

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        public RounedTextBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserMouse, true);
            Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (Enabled)
            {
                Focus();
                textBuilder.Clear();
                Text = HintText;
                downForeColor = ForeColor;
                ForeColor = HintTextColor;
                BorderColor = SELECT_COLOR;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (Enabled)
            {
                BorderColor = SELECT_COLOR;
                Cursor = Cursors.Hand;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (Enabled)
            {
                if (Focused)
                {
                    this.Parent.Focus();
                    return;
                }
                BorderColor = UNSELECT_COLOR;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (Focused && Enabled)
            {
                if(e.KeyChar == (char)Keys.Back)
                {
                    if(textBuilder.Length > 0)
                        textBuilder.Remove(textBuilder.Length - 1, 1);
                }
                else if(e.KeyChar == (char)Keys.Space)
                {
                    textBuilder.Append(" ");
                }
                else if(e.KeyChar == (char)Keys.Enter)
                {
                    BorderColor = UNSELECT_COLOR;
                    InputEnterEvent(this);
                }
                else
                {
                    textBuilder.Append(e.KeyChar);
                }
                Text = textBuilder.ToString();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var myPath = CreateRounedControl.CreateRounedButtonControl(e.Graphics, this, ClientRectangle, Radius, BorderSize))
            {
                using (var brush = new SolidBrush(BackColor))
                    e.Graphics.FillPath(brush, myPath);

                if (BorderVisible)
                {
                    using (var pen = new Pen(BorderColor, BorderSize))
                        e.Graphics.DrawPath(pen, myPath);
                }

                var rectF = new RectangleF();
                rectF.Size = new SizeF((float)ClientRectangle.Width, (float)ClientRectangle.Height);
                rectF.Location = new PointF((float)ClientRectangle.X, (float)ClientRectangle.Y);
                var stringFromat = new StringFormat();
                stringFromat.Alignment = TextAlign;
                stringFromat.LineAlignment = TextAlign;
                if(Text == "" || Text == HintText)
                {
                    e.Graphics.DrawString(HintText, Font, new SolidBrush(HintTextColor), rectF, stringFromat);
                }
                else
                {
                    if (ForeColor == HintTextColor)
                        ForeColor = downForeColor;
                    e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), rectF, stringFromat);
                }
            }
        }
    }
}
