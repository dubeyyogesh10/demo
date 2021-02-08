using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Telerik.Windows.Examples.AutoSuggestBox.FirstLook
{
    public static class GlyphDataProvider
    {
        private static Dictionary<string, string> glyphCategroes = new Dictionary<string, string>()
        {
            {"e0", "NavigationLayout"},
            {"e1", "Action"},
            {"e2", "Media"},
            {"e3", "Toggle"},
            {"e4", "AlertNotification"},
            {"e5", "Image"},
            {"e6", "Editor"},
            {"e7", "Map"},
            {"e8", "Social"},
            {"e9", "File"},
        };

        public static List<GlyphInfo> GetGlyphData()
        {
            var glyphsDict = new ResourceDictionary();
            glyphsDict.Source = new Uri("/Telerik.Windows.Controls;component/Themes/FontResources.xaml", System.UriKind.RelativeOrAbsolute);
            var result = new List<GlyphInfo>();
            foreach (var glyphResource in glyphsDict.Keys)
            {
                if (!glyphResource.ToString().StartsWith("Glyph"))
                {
                    continue;
                }
                var glyphName = GetGlyphName(glyphResource.ToString());
                var glyph = glyphsDict[glyphResource];
                byte[] bytes = Encoding.Unicode.GetBytes(glyph.ToString());
                string categoryCode = bytes[1].ToString("x2");
                string glyphTextContent = GetGlyphText(glyph);
                GlyphInfo glyphInfo = new GlyphInfo() 
                { 
                    Name = glyphName, 
                    Content = glyph, 
                    TextContent = glyphTextContent,
                    Category = glyphCategroes[categoryCode].Trim(),
                    UnicodePoint = glyphTextContent.Substring(3, 4),
                    Description = glyphResource.ToString()
                };
                result.Add(glyphInfo);
            }

            return result;
        }

        private static string GetGlyphName(string input)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in input)
            {
                if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
                builder.Append(c);
            }
            input = builder.ToString().Substring(6);
            return input;
        }

        private static string GetGlyphText(object glyph)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(glyph.ToString());
            if (bytes.Length < 2)
            {
                return null;
            }

            if (glyph.ToString().StartsWith("&#x"))
            {
                return glyph.ToString();
            }

            string firstCode = bytes[0].ToString("x2");
            string categoryCode = bytes[1].ToString("x2");

            string leftOver = string.Empty;
            if (bytes.Length > 2)
            {
                leftOver = Encoding.Unicode.GetString(bytes, 2, bytes.Length - 2);
            }

            return "&#x" + categoryCode + firstCode + ";" + leftOver;
        }
    }
}
