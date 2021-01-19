using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedGListView : ListView
    {
        private int border = 0;
        private int radius = 30;
        private bool changItemColor = true;
        private Color itemStartBackColor = Color.White;
        private Color itemEndBackColor = Color.White;
        private Color columnsBackColor = Color.LightSkyBlue;
        private Color selectItemColor = Color.LightSkyBlue;

        public RounedGListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserMouse, true);
            OwnerDraw = true;
            View = View.Details;
            FullRowSelect = true;
        }

        public int Border { get => border; set { border = value; Invalidate(); } }
        public int Radius { get => radius; set { radius = value; Invalidate(); } }
        public Color ItemStartBackColor { get => itemStartBackColor; set { itemStartBackColor = value; Invalidate(); } }
        public Color ItemEndBackColor { get => itemEndBackColor; set { itemEndBackColor = value; Invalidate(); } }
        public Color ColumnsBackColor { get => columnsBackColor; set { columnsBackColor = value; Invalidate(); } }
        public Color SelectItemColor { get => selectItemColor; set { selectItemColor = value; Invalidate(); } }

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

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            var clickedItem = GetItemAt(5, e.Y);
            if(clickedItem != null)
            {
                clickedItem.Selected = true;
                clickedItem.Focused = true;
            }
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            base.OnDrawColumnHeader(e);
            e.Graphics.FillRectangle(new SolidBrush(ColumnsBackColor), e.Bounds);
            e.DrawText(TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            base.OnDrawItem(e);
            e.Item.UseItemStyleForSubItems = true;
            if((e.State & ListViewItemStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(SelectItemColor), e.Bounds);
                e.DrawFocusRectangle();
            }
            else
            {
                using(var brush = new LinearGradientBrush(e.Bounds, ItemStartBackColor, ItemEndBackColor, LinearGradientMode.Horizontal))
                    e.Graphics.FillRectangle(brush, e.Bounds);
                changItemColor = !changItemColor;
            }
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            base.OnDrawSubItem(e);
            TextFormatFlags flags = TextFormatFlags.Left;
            using(StringFormat sf = new StringFormat())
            {

            }

            e.DrawText(flags);
        }

        public void AddDetailLIst(string[] listData)
        {
            var listItem = new ListViewItem(listData);
            Items.Add(listItem);
        }
    }
}
