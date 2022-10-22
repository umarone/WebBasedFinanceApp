using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace MudasirRehmanAlp.Helpers
{
    public class HtmlHelperCodeGenerate
    {
        public string GenerateQRCode(string CodeContent)
        {
            string Code = "";
            var _content = CodeContent;
            int _width = 200;
            int _height = 200;

            var _barcodeWriterPixelData = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options =
                {
                    Height=_height,
                    Width=_width,
                    Margin=0
                }
            };

            var _pixeldata = _barcodeWriterPixelData.Write(_content);
            using (var bitmap = new Bitmap(_pixeldata.Width, _pixeldata.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var memoryStream = new MemoryStream())
                {
                    var _bitmapdata = bitmap.LockBits(new Rectangle(0, 0,
                       _pixeldata.Width, _pixeldata.Height),
                       System.Drawing.Imaging.ImageLockMode.WriteOnly,
                       System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(
                            _pixeldata.Pixels, 0, _bitmapdata.Scan0, _pixeldata.Pixels.Length);

                    }
                    finally
                    {
                        bitmap.UnlockBits(_bitmapdata);
                    }
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    Code = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return Code;
        }
        public string GenerateBarCode(string CodeContent)
        {
            string Code = "";
            var _content = CodeContent;
            int _width = 300;
            int _height = 35;
            var _barcodeWriterPixelData = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options =
                {
                    Height=_height,
                    Width=_width,
                    Margin=0
                }
            };

            var _pixeldata = _barcodeWriterPixelData.Write(_content);
            using (var bitmap = new Bitmap(_pixeldata.Width, _pixeldata.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                using (var memoryStream = new MemoryStream())
                {
                    var _bitmapdata = bitmap.LockBits(new Rectangle(0, 0,
                       _pixeldata.Width, _pixeldata.Height),
                       System.Drawing.Imaging.ImageLockMode.WriteOnly,
                       System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(
                            _pixeldata.Pixels, 0, _bitmapdata.Scan0, _pixeldata.Pixels.Length);

                    }
                    finally
                    {
                        bitmap.UnlockBits(_bitmapdata);
                    }
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    Code = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return Code;
        }
    }
}