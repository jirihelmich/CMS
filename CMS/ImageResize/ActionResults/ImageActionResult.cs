
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;

namespace ImageResize.ActionResults
{
    /// <summary>
    /// To save and retrieve image in different sizes.
    /// </summary>
    public class ImageActionResult : ActionResult
    {

        private Image _SourceImage;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <param name="SourceImage">resized image details</param>
        public ImageActionResult(Image SourceImage)
        {
            this._SourceImage = SourceImage;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this.GenerateImage(context);
        }

        /// <summary>
        /// save different size of image
        /// </summary>
        /// <param name="context"></param>
        private void GenerateImage(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "image/jpeg";
            this._SourceImage.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
            this._SourceImage.Dispose();
        }
    }
}
