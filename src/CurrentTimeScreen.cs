using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScreenSaver
{
    internal class CurrentTimeScreen : TimeScreen
    {
        private readonly bool _display24HourTime;
        private readonly bool _isPreviewMode;
        private readonly bool _showSeconds = false;

        private const int SplitWidth = 4;
        private const double BoxSeparationPercent = 0.05; // 5% separation between boxes
        private const bool DrawGuideLines = false;

        private Font _largeFont;
        private Font _smallFont;

        private Font LargeFont => _largeFont ?? (_largeFont = new Font(FontFamily, _boxSize.Percent(85), FontStyle.Bold, GraphicsUnit.Pixel));
        private Font SmallFont => _smallFont ?? (_smallFont = new Font(FontFamily, _boxSize.Percent(9), FontStyle.Bold, GraphicsUnit.Pixel));

        private readonly Brush _fontBrush = new SolidBrush(Color.FromArgb(255, 183, 183, 183));
        private readonly Pen _splitPen = new Pen(Color.Black, SplitWidth);

        private readonly int _boxSize;
        private readonly int _separatorWidth;
        private readonly int _startingX;
        private readonly int _startingY;

        public CurrentTimeScreen(Form form, bool display24HourTime, bool isPreviewMode, int scalePercent)
        {
            _display24HourTime = display24HourTime;
            _isPreviewMode = isPreviewMode;
            _form = form;
            
            // The border is between 5% and 30% of the screen:
            // Scale of 0 = 5%, Scale of 100 = 30%
            int borderPercent = (100 - scalePercent) / 4 + 5;
            
            int boxSizeWidth = CalcBoxSize(form.Width, borderPercent, 2);
            int boxSizeHeight = CalcBoxSize(form.Height, borderPercent, 1);
            
            _boxSize = Math.Min(boxSizeWidth, boxSizeHeight);
            _separatorWidth = Convert.ToInt32(_boxSize * BoxSeparationPercent);

            _startingX = CalcOffset(form.Width, 2, _boxSize, _separatorWidth);
            _startingY = CalcOffset(form.Height, 1, _boxSize, 0);
        }

        private int CalcBoxSize(int total, int borderPercent, int boxCount)
        {
            int borderSize = total.Percent(borderPercent);
            int
