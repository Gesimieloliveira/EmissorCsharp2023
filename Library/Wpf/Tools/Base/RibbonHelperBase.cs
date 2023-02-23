using System;
using System.Windows;
using System.Windows.Media;

namespace FusionLibrary.Wpf.Tools.Base
{
    public abstract class RibbonHelperBase
    {
        protected static ImageSource CreateGlyph(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, Brush foreBrush)
        {
            if (fontFamily == null || string.IsNullOrEmpty(text)) return null;
            //premier essai, on charge la police directement
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);

            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                //si ça ne fonctionne pas (et pour le mode design dans certains cas) on ajoute l'uri pack://application
                typeface = new Typeface(new FontFamily(new Uri("pack://application:,,,"), fontFamily.Source), fontStyle, fontWeight, fontStretch);
                if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                    throw new InvalidOperationException("No glyphtypeface found");
            }

            //détermination des indices/tailles des caractères dans la police
            var glyphIndexes = new ushort[text.Length];
            var advanceWidths = new double[text.Length];

            for (var n = 0; n < text.Length; n++)
            {
                ushort glyphIndex;
                try
                {
                    glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

                }
                catch (Exception)
                {
                    glyphIndex = 42;
                }
                glyphIndexes[n] = glyphIndex;

                var width = glyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
                advanceWidths[n] = width;
            }

            try
            {

                //création de l'objet DrawingImage (compatible avec Imagesource) à partir d'un glyphrun
                var gr = new GlyphRun(glyphTypeface, 0, false, 1.0, glyphIndexes,
                    new Point(0, 0), advanceWidths, null, null, null, null, null, null);

                var glyphRunDrawing = new GlyphRunDrawing(foreBrush, gr);
                return new DrawingImage(glyphRunDrawing);
            }
            catch (Exception ex)
            {
                // ReSharper disable LocalizableElement
                Console.WriteLine("Error in generating Glyphrun : " + ex.Message);
                // ReSharper restore LocalizableElement
            }
            return null;
        }
    }
}