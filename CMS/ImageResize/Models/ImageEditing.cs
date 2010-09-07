using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImageResize.Models
{
    public class ImageEditing
    {

        private string _OriginalPath;

        /// <summary>
        /// Initializes a new instance of the ImageEditing class
        /// </summary>
        /// <param name="Path">To store image original path</param>
        public ImageEditing(string Path)
        {
            this._OriginalPath = Path;
        }

        /// <summary>
        /// To resize images to different sizes
        /// </summary>
        /// <param name="width">image width</param>
        /// <param name="height">image height </param>
        /// <returns>resized image</returns>
        public Image Resize(int width, int height)
        {
            return this.Resize(width, height, System.Drawing.Image.FromFile(this._OriginalPath));
        }

        /// <summary>
        /// To resize image with respect to given ratio.
        /// </summary>
        /// <param name="ratio">ratio of the image.</param>
        /// <param name="path">source of the image.</param>
        /// <returns>resized ratio of the image</returns>
        public Image Resize(int ratio)
        {
            Image tempimage = System.Drawing.Image.FromFile(this._OriginalPath);
            return this.Resize((int)(tempimage.Width * 0.01 * ratio), (int)(tempimage.Height * 0.01 * ratio), System.Drawing.Image.FromFile(this._OriginalPath));
        }

        /// <summary>
        /// To resize images to different sizes
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">image width </param>
        /// <param name="Path">path(url) of the image </param>
        /// <returns>resized image</returns>
        public Image Resize(int width, int height, string Path)
        {
            return this.Resize(width, height, System.Drawing.Image.FromFile(Path));
        }

        /// <summary>
        /// To resize images to different sizes
        /// </summary>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="ImageInfo">image informations</param>
        /// <returns>resized image</returns>
        public Image Resize(int width, int height, Image ImageInfo)
        {
            decimal HeigthWidthRatio = (decimal)ImageInfo.Height / (decimal)ImageInfo.Width;

            if (width > ImageInfo.Width)
            {
                width = ImageInfo.Width;
            }

            if ((decimal)ImageInfo.Width / width > (decimal)ImageInfo.Height / height)
            {
                height = (int)((decimal)width * HeigthWidthRatio);
            }
            else
            {
                width = (int)((decimal)height / HeigthWidthRatio);
            }

            return this.GenerateResizedImage(width, height, ImageInfo);
        }

        /// <summary>
        /// To geneterate thumbnail image
        /// </summary>
        /// <param name="width">image width</param>
        /// <param name="height"> image height</param>
        /// <param name="Imageinfo"> image informations</param>
        /// <returns>thumbnail image</returns>
        private Image GenerateResizedImage(int width, int height, Image Imageinfo)
        {
            if (width <= 0 | height <= 0)
            {
                throw new Exception("Either width or heigth has invalid value");
            }

            try
            {
                System.Drawing.Image ThumbNail = new Bitmap(width, height, Imageinfo.PixelFormat);
                Graphics Graphic = Graphics.FromImage(ThumbNail);

                Graphic.CompositingQuality = CompositingQuality.HighQuality;
                Graphic.SmoothingMode = SmoothingMode.HighQuality;
                Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Rectangle Rectangle = new Rectangle(0, 0, width, height);
                Graphic.DrawImage(Imageinfo, Rectangle);

                return ThumbNail;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                Imageinfo.Dispose();
            }
        }
    }
}
