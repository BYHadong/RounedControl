using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RounedControl
{
    public static class CreateRounedControl
    {
        public static GraphicsPath CreateRounedGroupBoxControl(Graphics graphics, Control control, Rectangle rect, int radius, int border)
        {
            GroupBoxRenderer.DrawParentBackground(graphics, rect, control);
            return CreateRounedRectangle(rect, radius, border);
        }

        public static GraphicsPath CreateRounedButtonControl(Graphics graphics, Control control, Rectangle rect, int radius, int border)
        {
            ButtonRenderer.DrawParentBackground(graphics, rect, control);
            return CreateRounedRectangle(rect, radius, border);
        }

        public static GraphicsPath CreateRounedBasicControl(Rectangle rect, int radius, int border)
        {
            return CreateRounedRectangle(rect, radius, border);
        }

        private static GraphicsPath CreateRounedRectangle(Rectangle rect, int radius, int border)
        {
            GraphicsPath myPath = new GraphicsPath();
            //boader를 그려준다.
            int x, y, miniRect;
            x = border;  //panel의 x좌표
            y = border;  //panel의 y좌표
            //작은 상자 크기
            miniRect = radius;

            float startAngle1, startAngle2, startAngle3, startAngle4, sweepAngle;
            sweepAngle = 90.0F;
            //Top | Left
            startAngle1 = 180.0F;
            // Top | Right
            startAngle2 = 270.0F;
            //Bottom | Right
            startAngle3 = 0.0F;
            //Bottom | Left
            startAngle4 = 90.0F;
            //작은 사각형
            Rectangle arc = new Rectangle(new Point(x, y), new Size(miniRect, miniRect));

            //그리기 시작
            myPath.StartFigure();
            //좌측 상단 
            myPath.AddArc(arc, startAngle1, sweepAngle);
            //좌표 이동 x좌표의 우측 끝을 간뒤 mini 상자의 크기와 보더 넓이 만큼 좌표를 뒤로 이동
            arc.X = rect.Right - miniRect - x;
            //우측 상단
            myPath.AddArc(arc, startAngle2, sweepAngle);
            //좌표 이동 y좌표의 하단 끝을 간뒤 mini 상자의 크기와 보더 넓이 만큼 좌표를 뒤로 이동
            arc.Y = rect.Bottom - miniRect - y;
            //우측 하단
            myPath.AddArc(arc, startAngle3, sweepAngle);
            //좌표이동 X좌표의 좌측 끝으로 이동
            arc.X = rect.Left + x;
            //좌측 하단
            myPath.AddArc(arc, startAngle4, sweepAngle);
            //그리기 종료
            myPath.CloseFigure();
            return myPath;
        }

        public static GraphicsPath GetEllipase(Rectangle rect)
        {
            if (rect.Width != rect.Height)
            {
                return null;
            }
            GraphicsPath myPath = new GraphicsPath();
            //boader를 그려준다.
            int x, y, miniRect;
            x = 0;  //panel의 x좌표
            y = 0;  //panel의 y좌표
            //작은 상자 크기
            miniRect = rect.Height;
            //각도
            float startAngle1, startAngle2, startAngle3, startAngle4, sweepAngle;
            sweepAngle = 90.0F;
            //Top | Left
            startAngle1 = 180.0F;
            // Top | Right
            startAngle2 = 270.0F;
            //Bottom | Right
            startAngle3 = 0.0F;
            //Bottom | Left
            startAngle4 = 90.0F;
            //작은 사각형
            Rectangle arc = new Rectangle(new Point(x, y), new Size(miniRect, miniRect));

            //그리기 시작
            myPath.StartFigure();
            //좌측 상단 
            myPath.AddArc(arc, startAngle1, sweepAngle);
            //좌표 이동 x좌표의 우측 끝을 간뒤 mini 상자의 크기와 보더 넓이 만큼 좌표를 뒤로 이동
            arc.X = rect.Right - miniRect - x;
            //우측 상단
            myPath.AddArc(arc, startAngle2, sweepAngle);
            //좌표 이동 y좌표의 하단 끝을 간뒤 mini 상자의 크기와 보더 넓이 만큼 좌표를 뒤로 이동
            arc.Y = rect.Bottom - miniRect - y;
            //우측 하단
            myPath.AddArc(arc, startAngle3, sweepAngle);
            //좌표이동 X좌표의 좌측 끝으로 이동
            arc.X = rect.Left + x;
            //좌측 하단
            myPath.AddArc(arc, startAngle4, sweepAngle);
            //그리기 종료
            myPath.CloseFigure();
            return myPath;
        }

        public static GraphicsPath GetRounedGroupBoxinPanel(Rectangle rect, int radius, int border)
        {
            GraphicsPath myPath = new GraphicsPath();
            //(0,0일때를 대비하여 BoaderWidth 만큼 좌료를 띄어줘서 보더가 안보이는걸 방지)
            int x = border;  //panel의 x좌표
            int y = border;  //panel의 y좌표
                             //작은 상자 크기
            int miniRect = radius;
            //Bottom | Right
            int startAngle3 = 0;
            int sweepAngle3 = 90;
            //Bottom | Left
            int startAngle4 = 90;
            int sweepAngle4 = 90;
            //작은 사각형
            Rectangle arc = new Rectangle(new Point(x, y), new Size(miniRect, miniRect));

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

            return myPath;
        }
    }
}
