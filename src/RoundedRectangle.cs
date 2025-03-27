using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenSaver
{
    [Flags]
    public enum RectangleCorners
    {
        None = 0,
        TopLeft = 1,
        TopRight = 2,
        BottomLeft = 4,
        BottomRight = 8,
        All = TopLeft | TopRight | BottomLeft | BottomRight
    }

    public abstract class RoundedRectangle
    {
        public static GraphicsPath Create(int x, int y, int width, int height, int radius, RectangleCorners corners)
        {
            int xw = x + width;
            int yh = y + height;
            int xwr = xw - radius;
            int yhr = yh - radius;
            int xr = x + radius;
            int yr = y + radius;
            int r2 = radius * 2;
            int xwr2 = xw - r2;
            int yhr2 = yh - r2;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();

            // Top Left Corner
            if ((corners & RectangleCorners.TopLeft) == RectangleCorners.TopLeft)
            {
                path.AddArc(x, y, r2, r2, 180, 90);
            }
            else
            {
                path.AddLine(x, yr, x, y);
                path.AddLine(x, y, xr, y);
            }

            // Top Edge
            path.AddLine(xr, y, xwr, y);

            // Top Right Corner
            if ((corners & RectangleCorners.TopRight) == RectangleCorners.TopRight)
            {
                path.AddArc(xwr2, y, r2, r2, 270, 90);
            }
            else
            {
                path.AddLine(xwr, y, xw, y);
                path.AddLine(xw, y, xw, yr);
            }

            // Right Edge
            path.AddLine(xw, yr, xw, yhr);

            // Bottom Right Corner
            if ((corners & RectangleCorners.BottomRight) == RectangleCorners.BottomRight)
            {
                path.AddArc(xwr2, yhr2, r2, r2, 0, 90);
            }
            else
            {
                path.AddLine(xw, yhr, xw, yh);
                path.AddLine(xw, yh, xwr, yh);
            }

            // Bottom Edge
            path.AddLine(xwr, yh, xr, yh);

            // Bottom Left Corner
            if ((corners & RectangleCorners.BottomLeft) == RectangleCorners.BottomLeft)
            {
                path.AddArc(x, yhr2, r2, r2, 90, 90);
            }
            else
            {
                path.AddLine(xr, yh, x, yh);
                path.AddLine(x, yh, x, yhr);
            }

            // Left Edge
            path.AddLine(x, yhr, x, yr);

            path.CloseFigure();
            return path;
        }

        public static GraphicsPath Create(Rectangle rect, int radius, RectangleCorners corners)
        {
            return Create(rect.X, rect.Y, rect.Width, rect.Height, radius, corners);
        }

        public static GraphicsPath Create(int x, int y, int width, int height, int radius)
        {
            return Create(x, y, width, height, radius, RectangleCorners.All);
        }

        public static GraphicsPath Create(Rectangle rect, int radius)
        {
            return Create(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static GraphicsPath Create(int x, int y, int width, int height)
        {
            return Create(x, y, width, height, 5);
        }

        public static GraphicsPath Create(Rectangle rect)
        {
            return Create(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
