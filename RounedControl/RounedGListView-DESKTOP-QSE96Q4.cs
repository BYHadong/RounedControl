using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace RounedControl
{
    public class RounedGListView : ListView
    {
        private const int VERTICAL_SCROLL_BAR_WIDTH_SUB = 4;
        private const int COLUMN_WIDTH_AUTO= 30;
        private int border = 0;
        private Color itemStartBackColor = Color.White;
        private Color itemEndBackColor = Color.LightGray;
        private Color itemTextColor = Color.Black;
        private Color columnsBackColor = Color.LightSkyBlue;
        private Color selectItemColor = Color.LightSkyBlue;
        private Color columnFontColor = Color.LightGray;
        private Font headerFontStyle = new Font("맑은 고딕", 10);
        private bool createRoot = false;

        public int BorderWidth { get => border; set { border = value; Invalidate(); } }
        public Color ItemStartBackColor { get => itemStartBackColor; set { itemStartBackColor = value; Invalidate(); } }
        public Color ItemEndBackColor { get => itemEndBackColor; set { itemEndBackColor = value; Invalidate(); } }
        public Color ColumnsBackColor { get => columnsBackColor; set { columnsBackColor = value; Invalidate(); } }
        public Color SelectItemColor { get => selectItemColor; set { selectItemColor = value; Invalidate(); } }
        public Font HeaderFontStyle { get => headerFontStyle; set { headerFontStyle = value; Invalidate(); } }
        public Color ColumnFontColor { get => columnFontColor; set { columnFontColor = value; Invalidate(); } }
        public Color ItemTextColor { get => itemTextColor; set { itemTextColor = value; Invalidate(); } }
        public HorizontalAlignment ItemTextAlignment { get; set; } = HorizontalAlignment.Left;
        public HorizontalAlignment ColumnTextAlignment { get; set; } = HorizontalAlignment.Left;

        public RounedGListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserMouse | ControlStyles.AllPaintingInWmPaint, true);
            OwnerDraw = true;
            View = View.Details;
            FullRowSelect = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics graphics = e.Graphics)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.DrawRectangle(new Pen(BackColor, BorderWidth), ClientRectangle);
            }
        }
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            base.OnDrawColumnHeader(e);
            using (StringFormat sf = new StringFormat())
            {
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                }
                e.Graphics.FillRectangle(new SolidBrush(ColumnsBackColor), e.Bounds);

                var rec = e.Bounds;
                rec.Y = (rec.Height - HeaderFontStyle.Height) / 2;
                e.Graphics.DrawString(e.Header.Text, HeaderFontStyle, new SolidBrush(ColumnFontColor), rec, sf);
            }
            return;
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(SelectItemColor), e.Bounds);
            }
            else
            {
                var backColor = e.ItemIndex % 2 == 0 ? ItemStartBackColor : ItemEndBackColor;
                //그라데이션을 줄 수 있음
                using (var brush = new LinearGradientBrush(e.Bounds, backColor, backColor, LinearGradientMode.Horizontal))
                    e.Graphics.FillRectangle(brush, e.Bounds);
            }
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            base.OnDrawSubItem(e);
            TextFormatFlags flags = TextFormatFlags.Left;
            using (StringFormat sf = new StringFormat())
            {
                switch (ItemTextAlignment)
                {
                    case HorizontalAlignment.Center:
                        {
                            sf.Alignment = StringAlignment.Center;
                            flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                            break;
                        }
                    case HorizontalAlignment.Right:
                        {
                            sf.Alignment = StringAlignment.Center;
                            flags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
                            break;
                        }
                }
                double subItemValue;
                if (e.ColumnIndex > 0 && double.TryParse(e.SubItem.Text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out subItemValue) && subItemValue < 0)
                {
                    if ((e.ItemState & ListViewItemStates.Selected) == 0)
                    {
                        e.DrawBackground();
                    }
                    e.Graphics.DrawString(e.SubItem.Text, Font, new SolidBrush(ItemTextColor), e.Bounds, sf);
                    return;
                }
                e.DrawText(flags);
            }
        }

        public void AddDetailLIst(string[] listData)
        {
            if (createRoot && View == View.Details)
            {
                var listItem = new List<string>(listData);
                listItem.Insert(0, "");
                Items.Add(new ListViewItem(listItem.ToArray()));
            }
            else
            {
                var listItem = new ListViewItem(listData);
                Items.Add(listItem);
            }
        }

        public void CreateRootItem()
        {
            Columns.Insert(0, new ColumnHeader()
            {
                Name = "root",
                Text = "root",
                Width = 0
            });
            createRoot = true;
            foreach (ColumnHeader column in Columns)
            {
                if (column.Text != "root")
                {
                    column.TextAlign = ColumnTextAlignment;
                    column.Width = (Width - SystemInformation.VerticalScrollBarWidth - VERTICAL_SCROLL_BAR_WIDTH_SUB) / (Columns.Count - 1);
                }
            }
            Invalidate();
        }

        public void ColumnAutoSizeSet()
        {
            var width = 0;
            foreach (ColumnHeader column in Columns)
            {
                column.AutoResize(ColumnHeaderAutoResizeStyle.None);
                if (column.Text != "root")
                {
                    var itemTextLength = ItemTextTopLegnth(column.Index);

                    var columnTextFormat = TextRenderer.MeasureText(column.Text, HeaderFontStyle);
                    var columnTextLength = columnTextFormat.Width;

                    if (column.Index == Columns.Count - 1)
                    {
                        column.Width = Width - width;
                    }
                    else if (columnTextLength > (itemTextLength + COLUMN_WIDTH_AUTO))
                    {
                        column.Width = columnTextLength + COLUMN_WIDTH_AUTO;
                        width += column.Width;
                    }
                    else
                    {
                        column.Width = itemTextLength + COLUMN_WIDTH_AUTO;
                        width += column.Width;
                    }
                }
            }
        }

        public int ItemTextTopLegnth(int idx)
        {
            var maxLength = 0;
            foreach (ListViewItem item in Items)
            {
                for (int i = 0; i < item.SubItems.Count; i++)
                {
                    if (i == idx)
                    {
                        var subItem = item.SubItems[i];
                        var textFormat = TextRenderer.MeasureText(subItem.Text, Font);
                        maxLength = maxLength < textFormat.Width ? textFormat.Width : maxLength;
                    }
                }
            }
            return maxLength;
        }

        public void ColumnHeaderWidthAutoChange()
        {
            foreach (ColumnHeader column in Columns)
            {
                var columnText = TextRenderer.MeasureText(column.Text, HeaderFontStyle);
                if (column.Text != "root")
                {
                    column.TextAlign = ColumnTextAlignment;
                    column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                    if(columnText.Width > column.Width)
                    {
                        column.Width = columnText.Width + COLUMN_WIDTH_AUTO;
                    }
                }
            }
        }
    }
}
