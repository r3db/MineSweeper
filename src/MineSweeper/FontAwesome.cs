using System;
using System.Drawing;
using System.Drawing.Text;
using System.Threading;

namespace Delete1
{
    internal static class FontAwesome
    {
        private static readonly ThreadLocal<FontFamily> _fontFamily = new ThreadLocal<FontFamily>(() => LoadFontFamily(@"../../fontawesome-webfont.ttf").Families[0]);

        private static PrivateFontCollection LoadFontFamily(string fontFamily)
        {
            var fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(fontFamily);
            return fontCollection;
        }

        public static FontFamily FontFamily => _fontFamily.Value;
    }
}