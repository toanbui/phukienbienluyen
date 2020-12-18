using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Threading.Tasks;

namespace Utilities
{
    public class ImageBusinnes
    {
        public enum Dimensions
        {
            Width,
            Height
        }
        public enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }
        public static Image ScaleByPercent(Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        public static MemoryStream ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            float nPercent = 0;

            switch (Dimension)
            {
                case Dimensions.Width:
                    nPercent = ((float)Size / (float)sourceWidth);
                    break;
                default:
                    nPercent = ((float)Size / (float)sourceHeight);
                    break;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

            grPhoto.Dispose();

            MemoryStream mem = new MemoryStream();
            bmPhoto.Save(mem, ImageFormat.Png);
            return mem;
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        public static MemoryStream Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = (int)(Height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = (int)((Height - (sourceHeight * nPercent)) / 2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = (int)(Width - (sourceWidth * nPercent));
                        break;
                    default:
                        destX = (int)((Width - (sourceWidth * nPercent)) / 2);
                        break;
                }
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.MakeTransparent();
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            if (Config.AllowWaterMark)
            {
                var text = Config.WaterMark;
                var font = new Font("Arial", 20, FontStyle.Regular);
                var angle = 35;

                SizeF textSize = GetEvenTextImageSize(text, font);

                SizeF imageSize;

                if (angle == 0)
                    imageSize = textSize;
                else
                    imageSize = GetRotatedTextImageSize(textSize, angle);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

                float scale = bmPhoto.Width / textSize.Width;
                grPhoto.ScaleTransform(scale, scale);
                grPhoto.SmoothingMode = SmoothingMode.HighQuality;
                grPhoto.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

                SizeF textContainerSize = grPhoto.VisibleClipBounds.Size;
                grPhoto.TranslateTransform(textContainerSize.Width / 2, textContainerSize.Height / 2);
                grPhoto.RotateTransform(angle);
                grPhoto.DrawString(text, font, new SolidBrush(Color.FromArgb(100, Color.Red)) , -(textSize.Width / 2), -(textSize.Height / 2));

                imgPhoto.Dispose();
                grPhoto.Dispose();
            }
            else
            {
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);
                imgPhoto.Dispose();
                grPhoto.Dispose();
            }


            MemoryStream mem = new MemoryStream();
            bmPhoto.Save(mem, ImageFormat.Png);
            return mem;
        }
        public static async Task<MemoryStream> CropAsync(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
        {
            return await Task.Run(() => Crop(imgPhoto, Width, Height, Anchor));
        }
        private static SizeF GetEvenTextImageSize(string text, Font font)
        {
            using (var image = new Bitmap(1, 1, PixelFormat.Format32bppArgb))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    return graphics.MeasureString(text, font);
                }
            }
        }
        private static SizeF GetRotatedTextImageSize(SizeF fontSize, int angle)
        {

            // Source: http://www.codeproject.com/KB/graphics/rotateimage.aspx

            double theta = angle * Math.PI / 180.0;

            while (theta < 0.0)
                theta += 2 * Math.PI;

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            if ((theta >= 0.0 && theta < Math.PI / 2.0) || (theta >= Math.PI && theta < (Math.PI + (Math.PI / 2.0))))
            {
                adjacentTop = Math.Abs(Math.Cos(theta)) * fontSize.Width;
                oppositeTop = Math.Abs(Math.Sin(theta)) * fontSize.Width;
                adjacentBottom = Math.Abs(Math.Cos(theta)) * fontSize.Height;
                oppositeBottom = Math.Abs(Math.Sin(theta)) * fontSize.Height;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(theta)) * fontSize.Height;
                oppositeTop = Math.Abs(Math.Cos(theta)) * fontSize.Height;
                adjacentBottom = Math.Abs(Math.Sin(theta)) * fontSize.Width;
                oppositeBottom = Math.Abs(Math.Cos(theta)) * fontSize.Width;
            }

            int nWidth = (int)Math.Ceiling(adjacentTop + oppositeBottom);
            int nHeight = (int)Math.Ceiling(adjacentBottom + oppositeTop);

            return new SizeF(nWidth, nHeight);

        }
        public MemoryStream AddWaterMarkRotate(Bitmap bmp, string text)
        {
            var font = new Font("Arial", 20, FontStyle.Regular);
            var angle = 35;

            SizeF textSize = GetEvenTextImageSize(text, font);

            SizeF imageSize;

            if (angle == 0)
                imageSize = textSize;
            else
                imageSize = GetRotatedTextImageSize(textSize, angle);

            using (bmp)
            {

                using (var graphics = Graphics.FromImage(bmp))
                {

                    float scale = bmp.Width / textSize.Width;
                    graphics.ScaleTransform(scale, scale);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

                    SizeF textContainerSize = graphics.VisibleClipBounds.Size;
                    graphics.TranslateTransform(textContainerSize.Width / 2, textContainerSize.Height / 2);
                    graphics.RotateTransform(angle);

                    graphics.DrawString(text, font, Brushes.Black, -(textSize.Width / 2), -(textSize.Height / 2));

                }

                var stream = new MemoryStream();
                bmp.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
        }
        public static Bitmap AddWaterMarkRotateBmp(Bitmap bmp, string text)
        {
            var font = new Font("Arial", 20, FontStyle.Regular);
            var angle = 35;

            SizeF textSize = GetEvenTextImageSize(text, font);

            SizeF imageSize;

            if (angle == 0)
                imageSize = textSize;
            else
                imageSize = GetRotatedTextImageSize(textSize, angle);

            using (bmp)
            {

                using (var graphics = Graphics.FromImage(bmp))
                {

                    float scale = bmp.Width / textSize.Width;
                    graphics.ScaleTransform(scale, scale);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

                    SizeF textContainerSize = graphics.VisibleClipBounds.Size;
                    graphics.TranslateTransform(textContainerSize.Width / 2, textContainerSize.Height / 2);
                    graphics.RotateTransform(angle);

                    graphics.DrawString(text, font, Brushes.Black, -(textSize.Width / 2), -(textSize.Height / 2));

                }
                return bmp;
            }
        }
    }
}
