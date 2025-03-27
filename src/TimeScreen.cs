using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenSaver
{
    internal abstract class TimeScreen
    {
        protected Form _form;
        private Graphics _graphics;
        private PrivateFontCollection _pfc;
        private FontFamily _fontFamily;

        protected abstract byte[] GetFontResource();
        internal abstract void Draw();

        protected Graphics Gfx
        {
            get
            {
                if (_graphics == null)
                {
                    _graphics = _form.CreateGraphics();
                    _graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    _graphics.SmoothingMode = SmoothingMode.HighQuality;
                }
                return _graphics;
            }
        }

        protected FontFamily FontFamily
        {
            get
            {
                if (_fontFamily == null)
                {
                    if (_pfc == null)
                    {
                        _pfc = InitFontCollection();
                    }
                    _fontFamily = _pfc.Families[0];
                }
                return _fontFamily;
            }
        }

        protected int GetFontAscentPercent()
        {
            var ascent = FontFamily.GetCellAscent(FontStyle.Regular);
            var emHeight = FontFamily.GetEmHeight(FontStyle.Regular);
            return ascent * 100 / emHeight;
        }

        private PrivateFontCollection InitFontCollection()
        {
            var pfc = new PrivateFontCollection();
            AddFont(pfc, GetFontResource());
            return pfc;
        }

        protected static readonly Color BackColorTop = Color.FromArgb(255, 18, 18, 18);
        protected static readonly Color BackColorBottom = Color.FromArgb(255, 10, 10, 10);
        protected static readonly Brush FontBrush = new SolidBrush(Color.FromArgb(255, 183, 183, 183));

        protected static void AddFont(PrivateFontCollection pfc, byte[] fontResource)
        {
            IntPtr ptr = Marshal.AllocCoTaskMem(fontResource.Length);
            Marshal.Copy(fontResource, 0, ptr, fontResource.Length);
            pfc.AddMemoryFont(ptr, fontResource.Length);
            Marshal.FreeCoTaskMem(ptr);
        }

        protected string FormatAmPm(DateTime time)
        {
            return time.Hour >= 12 ? "PM" : "AM";
        }

        internal void DisposeResources()
        {
            _fontFamily?.Dispose();
            _fontFamily = null;
            _pfc?.Dispose();
            _pfc = null;
        }
    }
}
