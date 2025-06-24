using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TralalaGame
{
    //Kalau mau nambah font
    public class TextFont : Label
    {
        public TextFont() { }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            base.OnPaint(e);
        }
    }
}
