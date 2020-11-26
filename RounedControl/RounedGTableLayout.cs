using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing;

namespace RounedControl
{
    public class RounedGTableLayout : TableLayoutPanel
    {
        int radius = 30;
        int border = 0;


        public RounedGTableLayout()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Dock = DockStyle.Fill;
            BackColor = Color.White;
        }

        public int Radius { get => radius; set { radius = value; Invalidate(); } }
        public int Border { get => border; set { border = value; Invalidate(); } }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath myPath = new GraphicsPath())
            {
                //(0,0일때를 대비하여 BoaderWidth 만큼 좌료를 띄어줘서 보더가 안보이는걸 방지)
                float x = border;  //panel의 x좌표
                float y = border;  //panel의 y좌표
                                   //작은 상자 크기
                float miniRect = Radius;
                //메인 사각형
                RectangleF rect = ClientRectangle;
                //Bottom | Right
                float startAngle3 = 0.0F;
                float sweepAngle3 = 90.0F;
                //Bottom | Left
                float startAngle4 = 90.0F;
                float sweepAngle4 = 90.0F;
                //작은 사각형
                RectangleF arc = new RectangleF(new PointF(x, y), new SizeF(miniRect, miniRect));

                //그리기 시작
                myPath.StartFigure();
                //좌측 상단
                myPath.AddLine(new PointF(arc.X, arc.Y), new PointF(rect.Right - x, arc.Y));
                //좌표 이동 x좌표의 우측 끝을 간뒤 mini 상자의 크기와 보더 넓이 만큼 좌표를 뒤로 이동
                arc.X = rect.Right - x - miniRect;
                //우측 상단
                myPath.AddLine(new PointF(rect.Right - x, arc.Y), new PointF(rect.Right - x, rect.Bottom - miniRect - y));
                //좌표 이동 y좌표의 하단 끝을 간뒤 mini 상자의 크기와 보더 넓이 만큼 좌표를 뒤로 이동
                arc.Y = rect.Bottom - miniRect - y;
                //우측 하단
                myPath.AddArc(arc, startAngle3, sweepAngle3);
                //좌표이동 X좌표의 좌측 끝으로 이동
                arc.X = rect.Left + x;
                //좌측 하단
                myPath.AddArc(arc, startAngle4, sweepAngle4);
                //그리기 종료
                myPath.AddLine(new PointF(arc.X, arc.Y), new PointF(x, y));
                myPath.CloseFigure();

                using(Pen pen = new Pen(BackColor, border))
                    e.Graphics.DrawPath(pen, myPath);

                Region = new Region(myPath);
            }
        }
    }
}
