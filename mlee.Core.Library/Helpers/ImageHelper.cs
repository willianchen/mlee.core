using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace mlee.Core.Library.Helpers
{
    /// <summary>
    /// 图片缩放方式
    /// </summary>
    public enum EnumImageScaleMode
    {
        /// <summary>
        /// 不缩放(即不缩放)
        /// </summary>
        NoScale,
        /// <summary>
        /// 固定长宽(可能会拉伸)
        /// </summary>
        FixedWidthHeight,
        /// <summary>
        /// 固定宽度
        /// </summary>
        FixedWidth,
        /// <summary>
        /// 固定高度
        /// </summary>
        FixedHeight,
        /// <summary>
        /// 比例长宽
        /// </summary>
        ScaleWidthHeight
    }

    /// <summary>
    /// 图片位置
    /// </summary>
    public enum ImagePosition
    {
        /// <summary>
        /// 左上
        /// </summary>
        LeftTop,
        /// <summary>
        /// 左下
        /// </summary>
        LeftBottom,
        /// <summary>
        /// 右上
        /// </summary>
        RightTop,
        /// <summary>
        /// 右下
        /// </summary>
        RigthBottom,
        /// <summary>
        /// 顶部居中
        /// </summary>
        TopMiddle,
        /// <summary>
        /// 底部居中
        /// </summary>
        BottomMiddle,
        /// <summary>
        /// 中心
        /// </summary>
        Center
    }


    public class ImageHelper
    {
        //Jpg编码信息
        private static ImageCodecInfo _JpgEncode = ImageCodecInfo.GetImageEncoders().Where(t => t.MimeType == "image/jpeg").FirstOrDefault();
        private static ImageCodecInfo _PngEncode = ImageCodecInfo.GetImageEncoders().Where(t => t.MimeType == "image/png").FirstOrDefault();
        private static ImageCodecInfo _GifEncode = ImageCodecInfo.GetImageEncoders().Where(t => t.MimeType == "image/gif").FirstOrDefault();
        private static ImageCodecInfo _BmpEncode = ImageCodecInfo.GetImageEncoders().Where(t => t.MimeType == "image/bmp").FirstOrDefault();
        private static ImageCodecInfo _TifEncode = ImageCodecInfo.GetImageEncoders().Where(t => t.MimeType == "image/tiff").FirstOrDefault();

        /// <summary>
        /// 计算并返回缩放后的图片大打
        /// </summary>
        public static Size GetSize(Image Img, Size ToSize, EnumImageScaleMode Mode = EnumImageScaleMode.ScaleWidthHeight)
        {
            Size ImgSize = new Size(Img.Width, Img.Height);
            Size OkSize;
            int intWidth, intHeight;
            switch (Mode)
            {
                case EnumImageScaleMode.NoScale:
                    OkSize = ImgSize;
                    break;
                case EnumImageScaleMode.FixedWidthHeight:
                    OkSize = ToSize;
                    break;
                case EnumImageScaleMode.FixedWidth:
                    intHeight = (int)Math.Round((double)ImgSize.Height * ToSize.Width / ImgSize.Width);
                    OkSize = new Size(ToSize.Width, intHeight);
                    break;
                case EnumImageScaleMode.FixedHeight:
                    intWidth = (int)Math.Round((double)ImgSize.Width * ToSize.Height / ImgSize.Height);
                    OkSize = new Size(ToSize.Width, intWidth);
                    break;
                case EnumImageScaleMode.ScaleWidthHeight:
                default:
                    if (ImgSize.Width < ToSize.Width && ImgSize.Height < ToSize.Height)
                    {
                        OkSize = ImgSize;
                        break;
                    }
                    if ((double)ImgSize.Width / ImgSize.Height > (double)ToSize.Width / ToSize.Height)
                    {
                        intHeight = (int)Math.Round((double)ImgSize.Height / ImgSize.Width * ToSize.Width);
                        OkSize = new Size(ToSize.Width, intHeight);
                    }
                    else
                    {
                        intWidth = (int)Math.Round((double)ImgSize.Width / ImgSize.Height * ToSize.Height);
                        OkSize = new Size(intWidth, ToSize.Height);
                    }
                    break;
            }
            return OkSize;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="ImgFileName">图片文件名</param>
        /// <param name="OutSize">输出图片大小</param>
        /// <param name="Mode">缩方形式</param>
        /// <param name="IsFill">是否填充(默认用白色填充)</param>
        /// <returns>缩放后新图</returns>
        public static Image ToSize(string ImgFileName, Size OutSize, EnumImageScaleMode Mode = EnumImageScaleMode.ScaleWidthHeight, bool IsFill = false)
        {
            Color fillcolor = Color.White;
            return ToSize(Image.FromFile(ImgFileName), OutSize, Mode, IsFill, fillcolor);
        }
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="ImgFileName">图片文件名</param>
        /// <param name="Width">宽</param>
        /// <param name="Height">高</param>
        /// <param name="Mode">缩方形式</param>
        /// <param name="IsFill">是否填充(默认用白色填充)</param>
        /// <returns>缩放后新图</returns>
        public static Image ToSize(string ImgFileName, int Width, int Height, EnumImageScaleMode Mode = EnumImageScaleMode.ScaleWidthHeight, bool IsFill = false)
        {
            Color fillcolor = Color.White;
            return ToSize(Image.FromFile(ImgFileName), new Size { Width = Width, Height = Height }, Mode, IsFill, fillcolor);
        }
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="Strm">流对象</param>
        /// <param name="Width">宽</param>
        /// <param name="Height">高</param>
        /// <param name="Mode">缩方形式</param>
        /// <param name="IsFill">是否填充(默认用白色填充)</param>
        /// <returns>缩放后新图</returns>
        public static Image ToSize(Stream Strm, int Width, int Height, EnumImageScaleMode Mode = EnumImageScaleMode.ScaleWidthHeight, bool IsFill = false)
        {
            Color fillcolor = Color.White;
            return ToSize(Image.FromStream(Strm), new Size { Width = Width, Height = Height }, Mode, IsFill, fillcolor);
        }
        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="Strm">流对象</param>
        /// <param name="OutSize">输出图片大小</param>
        /// <param name="Mode">缩方形式</param>
        /// <param name="IsFill">是否填充(默认用白色填充)</param>
        /// <returns>缩放后新图</returns>
        public static Image ToSize(Stream Strm, Size OutSize, EnumImageScaleMode Mode = EnumImageScaleMode.ScaleWidthHeight, bool IsFill = false)
        {
            Color fillcolor = Color.White;
            return ToSize(Image.FromStream(Strm), OutSize, Mode, IsFill, fillcolor);
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="Img">图片对象</param>
        /// <param name="Width">宽</param>
        /// <param name="Height">高</param>
        /// <param name="Mode">缩方形式</param>
        /// <param name="IsFill">是否填充(默认用白色填充)</param>
        /// <returns>缩放后新图</returns>
        public static Image ToSize(Image Img, int Width, int Height, EnumImageScaleMode Mode = EnumImageScaleMode.ScaleWidthHeight, bool IsFill = false)
        {
            Color fillcolor = Color.White;
            return ToSize(Img, new Size { Width = Width, Height = Height }, Mode, IsFill, fillcolor);
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="Img">图片对象</param>
        /// <param name="OutSize">输出图片大小</param>
        /// <param name="Mode">缩方形式</param>
        /// <param name="IsFill">是否填充</param>
        /// <param name="FillColor">填充颜色(IsFill==true时生效)</param>
        /// <returns>缩放后新图</returns>
        public static Image ToSize(Image Img, Size OutSize, EnumImageScaleMode Mode = EnumImageScaleMode.ScaleWidthHeight, bool IsFill = false, Color? FillColor = null)
        {
            Size OkSize = GetSize(Img, OutSize, Mode);
            Bitmap bmpImg;
            Rectangle rctImg;
            if (IsFill)
            {
                bmpImg = new Bitmap(OutSize.Width, OutSize.Height);
                Point pot = new Point(-1, -1);
                pot.X += (int)Math.Round((double)(OutSize.Width - OkSize.Width) / 2);
                pot.Y += (int)Math.Round((double)(OutSize.Height - OkSize.Height) / 2);
                rctImg = new Rectangle(pot, OkSize);
            }
            else
            {
                bmpImg = new Bitmap(OkSize.Width, OkSize.Height);
                rctImg = new Rectangle(new Point(0, 0), OkSize);
            }
            Graphics g = Graphics.FromImage(bmpImg);
            //设置插值模式
            g.InterpolationMode = InterpolationMode.High;
            //设置平滑模式 
            g.SmoothingMode = SmoothingMode.HighQuality;
            if (FillColor.HasValue) { g.Clear(FillColor.Value); }
            //缩放图片
            g.DrawImage(Img, rctImg,
                        new Rectangle(new Point(0, 0), Img.Size),
                        GraphicsUnit.Pixel);
            return bmpImg;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="Img">图版对象</param>
        /// <param name="OutFileName">输出文件名</param>
        /// <param name="Quality">图片品质</param>
        /// <returns>是否成功保存</returns>
        public static bool Save(Image Img, string OutFileName, int Quality = 80)
        {
            //保存图片
            EncoderParameters eps = new EncoderParameters();
            eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)(Quality));
            try
            {
                var at = OutFileName.LastIndexOf('.');
                if (at == -1)
                {
                    Img.Save(OutFileName, _JpgEncode, eps);
                }
                else
                {
                    switch (OutFileName.Substring(at + 1))
                    {
                        case "jpg":
                            Img.Save(OutFileName, _JpgEncode, eps);
                            break;
                        case "png":
                            Img.Save(OutFileName, _PngEncode, eps);
                            break;
                        case "gif":
                            Img.Save(OutFileName, _GifEncode, eps);
                            break;
                        case "bmp":
                            Img.Save(OutFileName, _BmpEncode, eps);
                            break;
                        case "tif":
                            Img.Save(OutFileName, _TifEncode, eps);
                            break;
                        default:
                            Img.Save(OutFileName, _JpgEncode, eps);
                            break;
                    }
                }
                Img.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Img.Dispose();
                return false;
            }

        }

        /// <summary>
        /// 生成一个新的水印图片制作实例
        /// </summary>
        public ImageHelper() { }

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="ImageFullFileName">源图片文件名</param>
        /// <param name="WaterImageFileName">水印图片文件名</param>
        /// <param name="Alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <param name="Position">位置</param>
        /// <param name="PicturePath" >图片的路径</param>
        /// <returns>返回生成于指定文件夹下的水印文件名</returns>
        public string DrawImage(string ImageFullFileName, string WaterImageFileName, float Alpha, ImagePosition Position, string OutputTemplateImageFileName)
        {
            // 判断参数是否有效
            if (ImageFullFileName == string.Empty)
            {
                return "文件不能为空";
            }

            // 源图片，水印图片全路径
            string fileSourceExtension = System.IO.Path.GetExtension(ImageFullFileName).ToLower();

            // 判断文件是否存在,以及类型是否正确
            if (System.IO.File.Exists(ImageFullFileName) == false || (
                fileSourceExtension != ".gif" &&
                fileSourceExtension != ".jpg" &&
                fileSourceExtension != ".png")
                )
            {
                return "文件不存在或是非图片文件";
            }

            // 将需要加上水印的图片装载到Image对象中
            Image imgPhoto = Image.FromFile(ImageFullFileName);

            return DrawImage(imgPhoto, WaterImageFileName, Alpha, Position, OutputTemplateImageFileName);
        }

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="ImageFullFileName">源图片文件名</param>
        /// <param name="WaterImageFileName">水印图片文件名</param>
        /// <param name="Alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <param name="Position">位置</param>
        /// <param name="PicturePath" >图片的路径</param>
        /// <returns>返回生成于指定文件夹下的水印文件名</returns>
        public string DrawImage(Stream ImageStrm, string WaterImageFileName, float Alpha, ImagePosition Position, string OutputTemplateImageFileName)
        {
            // 判断参数是否有效
            if (WaterImageFileName == string.Empty || Alpha == 0.0)
            {
                return "文件不能为空";
            }

            // 源图片，水印图片全路径
            string fileWaterExtension = System.IO.Path.GetExtension(WaterImageFileName).ToLower();

            // 判断文件是否存在,以及类型是否正确
            if (System.IO.File.Exists(WaterImageFileName) == false || (
                fileWaterExtension != ".gif" &&
                fileWaterExtension != ".jpg" &&
                fileWaterExtension != ".png")
                )
            {
                return "文件不存在或是非图片文件";
            }

            // 目标图片名称及全路径
            string targetImage = OutputTemplateImageFileName;

            // 将需要加上水印的图片装载到Image对象中
            Image imgPhoto = Image.FromStream(ImageStrm);

            // 确定其长宽
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // 设定分辨率
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 定义一个绘图画面用来装载位图
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //同样，由于水印是图片，我们也需要定义一个Image来装载它
            Image imgWatermark = new Bitmap(WaterImageFileName);

            // 获取水印图片的高度和宽度
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。
            // 成员名称  说明 
            // AntiAlias   指定消除锯齿的呈现。 
            // Default    指定不消除锯齿。

            // HighQuality 指定高质量、低速度呈现。 
            // HighSpeed  指定高速度、低质量呈现。 
            // Invalid    指定一个无效模式。 
            // None     指定不消除锯齿。 
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;



            // 第一次描绘，将我们的底图描绘在绘图画面上
            grPhoto.DrawImage(imgPhoto,
                            new Rectangle(0, 0, phWidth, phHeight),
                            0,
                            0,
                            phWidth,
                            phHeight,
                            GraphicsUnit.Pixel);

            // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            // 继续，将水印图片装载到一个绘图画面grWatermark
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。
            ImageAttributes imageAttributes = new ImageAttributes();

            //Colormap: 定义转换颜色的映射
            ColorMap colorMap = new ColorMap();

            //我的水印图被定义成拥有绿色背景色的图片被替换成透明
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
    new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f}, // red红色
    new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f}, //green绿色
    new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f}, //blue蓝色    
    new float[] {0.0f, 0.0f, 0.0f, Alpha, 0.0f}, //透明度   
    new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};//

            // ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。
            // ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);


            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            //上面设置完颜色，下面开始设置位置
            int xPosOfWm;
            int yPosOfWm;

            switch (Position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;

                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 10;
                    break;
                default:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            // 第二次绘图，把水印印上去
            grWatermark.DrawImage(imgWatermark,
                new Rectangle(xPosOfWm,
                        yPosOfWm,
                        wmWidth,
                        wmHeight),
                        0,
                        0,
                        wmWidth,
                        wmHeight,
                        GraphicsUnit.Pixel,
                        imageAttributes);

            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            // 保存文件到服务器的文件夹里面
            imgPhoto.Save(targetImage, ImageFormat.Jpeg);
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            return string.Empty;
        }

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="ImageFullFileName">源图片文件名</param>
        /// <param name="WaterImageFileName">水印图片文件名</param>
        /// <param name="Alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <param name="Position">位置</param>
        /// <param name="PicturePath" >图片的路径</param>
        /// <returns>返回生成于指定文件夹下的水印文件名</returns>
        public string DrawImage(Image ImageObj, string WaterImageFileName, float Alpha, ImagePosition Position, string OutputTemplateImageFileName)
        {
            // 判断参数是否有效
            if (WaterImageFileName == string.Empty || Alpha == 0.0)
            {
                return "文件不能为空";
            }

            // 源图片，水印图片全路径
            string fileWaterExtension = System.IO.Path.GetExtension(WaterImageFileName).ToLower();

            // 判断文件是否存在,以及类型是否正确
            if (System.IO.File.Exists(WaterImageFileName) == false || (
                fileWaterExtension != ".gif" &&
                fileWaterExtension != ".jpg" &&
                fileWaterExtension != ".png")
                )
            {
                return "文件不存在或是非图片文件";
            }

            // 目标图片名称及全路径
            string targetImage = OutputTemplateImageFileName;

            // 确定其长宽
            int phWidth = ImageObj.Width;
            int phHeight = ImageObj.Height;

            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // 设定分辨率
            bmPhoto.SetResolution(ImageObj.HorizontalResolution, ImageObj.VerticalResolution);

            // 定义一个绘图画面用来装载位图
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //同样，由于水印是图片，我们也需要定义一个Image来装载它
            Image imgWatermark = new Bitmap(WaterImageFileName);

            // 获取水印图片的高度和宽度
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。
            // 成员名称  说明 
            // AntiAlias   指定消除锯齿的呈现。 
            // Default    指定不消除锯齿。

            // HighQuality 指定高质量、低速度呈现。 
            // HighSpeed  指定高速度、低质量呈现。 
            // Invalid    指定一个无效模式。 
            // None     指定不消除锯齿。 
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;



            // 第一次描绘，将我们的底图描绘在绘图画面上
            grPhoto.DrawImage(ImageObj,
                            new Rectangle(0, 0, phWidth, phHeight),
                            0,
                            0,
                            phWidth,
                            phHeight,
                            GraphicsUnit.Pixel);

            // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(ImageObj.HorizontalResolution, ImageObj.VerticalResolution);

            // 继续，将水印图片装载到一个绘图画面grWatermark
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。
            ImageAttributes imageAttributes = new ImageAttributes();

            //Colormap: 定义转换颜色的映射
            ColorMap colorMap = new ColorMap();

            //我的水印图被定义成拥有绿色背景色的图片被替换成透明
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
    new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f}, // red红色
    new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f}, //green绿色
    new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f}, //blue蓝色    
    new float[] {0.0f, 0.0f, 0.0f, Alpha, 0.0f}, //透明度   
    new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};//

            // ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。
            // ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);


            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            //上面设置完颜色，下面开始设置位置
            int xPosOfWm;
            int yPosOfWm;

            switch (Position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;

                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 10;
                    break;
                default:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            // 第二次绘图，把水印印上去
            grWatermark.DrawImage(imgWatermark,
                new Rectangle(xPosOfWm,
                        yPosOfWm,
                        wmWidth,
                        wmHeight),
                        0,
                        0,
                        wmWidth,
                        wmHeight,
                        GraphicsUnit.Pixel,
                        imageAttributes);

            ImageObj = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            // 保存文件到服务器的文件夹里面
            ImageObj.Save(targetImage, ImageFormat.Jpeg);
            ImageObj.Dispose();
            imgWatermark.Dispose();
            return string.Empty;
        }

        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="Img">图片对象</param>
        /// <param name="WaterImage">水印图片对象</param>
        /// <param name="Alpha">透明度(0.1-1.0数值越小透明度越高)</param>
        /// <param name="Position">位置</param>
        /// <returns>带水印图片对象</returns>
        public static Image DrawImage(Image Img, Image WaterImage, float Alpha, ImagePosition Position)
        {
            // 确定其长宽
            int phWidth = Img.Width;
            int phHeight = Img.Height;

            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            // 设定分辨率
            bmPhoto.SetResolution(Img.HorizontalResolution, Img.VerticalResolution);

            // 定义一个绘图画面用来装载位图
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            // 获取水印图片的高度和宽度
            int wmWidth = WaterImage.Width;
            int wmHeight = WaterImage.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。
            // 成员名称  说明 
            // AntiAlias   指定消除锯齿的呈现。 
            // Default    指定不消除锯齿。

            // HighQuality 指定高质量、低速度呈现。 
            // HighSpeed  指定高速度、低质量呈现。 
            // Invalid    指定一个无效模式。 
            // None     指定不消除锯齿。 
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;



            // 第一次描绘，将我们的底图描绘在绘图画面上
            grPhoto.DrawImage(Img,
                            new Rectangle(0, 0, phWidth, phHeight),
                            0,
                            0,
                            phWidth,
                            phHeight,
                            GraphicsUnit.Pixel);

            // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(Img.HorizontalResolution, Img.VerticalResolution);

            // 继续，将水印图片装载到一个绘图画面grWatermark
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。
            ImageAttributes imageAttributes = new ImageAttributes();

            //Colormap: 定义转换颜色的映射
            ColorMap colorMap = new ColorMap();

            //我的水印图被定义成拥有绿色背景色的图片被替换成透明
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
    new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f}, // red红色
    new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f}, //green绿色
    new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f}, //blue蓝色    
    new float[] {0.0f, 0.0f, 0.0f, Alpha, 0.0f}, //透明度   
    new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};//

            // ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。
            // ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);


            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            //上面设置完颜色，下面开始设置位置
            int xPosOfWm;
            int yPosOfWm;

            switch (Position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;

                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 10;
                    break;
                default:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            // 第二次绘图，把水印印上去
            grWatermark.DrawImage(WaterImage,
                new Rectangle(xPosOfWm,
                        yPosOfWm,
                        wmWidth,
                        wmHeight),
                        0,
                        0,
                        wmWidth,
                        wmHeight,
                        GraphicsUnit.Pixel,
                        imageAttributes);

            grPhoto.Dispose();
            grWatermark.Dispose();

            return bmWatermark;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="Bitbmp"></param>
        /// <param name="DestHeight"></param>
        /// <param name="DestWidth"></param>
        /// <returns></returns>
        public Image DrawThumbnail(string FileName, int DestHeight, int DestWidth)
        {
            Image imgPhoto = Image.FromFile(FileName);
            return imgPhoto;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="Bitbmp"></param>
        /// <param name="DestHeight"></param>
        /// <param name="DestWidth"></param>
        /// <returns></returns>
        public Image DrawThumbnail(Stream ImageStrm, int DestHeight, int DestWidth)
        {
            Image imgPhoto = Image.FromStream(ImageStrm);
            var outBmp = DrawThumbnail(imgPhoto, DestHeight, DestWidth);
            return outBmp;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="ImageObj"></param>
        /// <param name="DestHeight"></param>
        /// <param name="DestWidth"></param>
        /// <returns></returns>
        public Image DrawThumbnail(Image ImageObj, int DestHeight, int DestWidth)
        {
            System.Drawing.Imaging.ImageFormat thisFormat = ImageObj.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放           
            int sWidth = ImageObj.Width;
            int sHeight = ImageObj.Height;
            if (sHeight > DestHeight || sWidth > DestWidth)
            {
                if ((sWidth * DestHeight) > (sHeight * DestWidth))
                {
                    sW = DestWidth;
                    sH = (DestWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = DestHeight;
                    sW = (sWidth * DestHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(DestWidth, DestHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);

            // 设置画布的描绘质量         
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(ImageObj, new Rectangle((DestWidth - sW) / 2, (DestHeight - sH) / 2, sW, sH), 0, 0, ImageObj.Width, ImageObj.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量     
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            return outBmp;
        }
        public Bitmap GetImageThumb(Bitmap mg, Size newSize)
        {
            double ratio = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;
            int x = 0;
            int y = 0;

            Bitmap bp;

            if ((mg.Width / Convert.ToDouble(newSize.Width)) > (mg.Height /
            Convert.ToDouble(newSize.Height)))
                ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
            else
                ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
            myThumbHeight = Math.Ceiling(mg.Height / ratio);
            myThumbWidth = Math.Ceiling(mg.Width / ratio);

            Size thumbSize = new Size((int)newSize.Width, (int)newSize.Height);
            bp = new Bitmap(newSize.Width, newSize.Height);
            x = (newSize.Width - thumbSize.Width) / 2;
            y = (newSize.Height - thumbSize.Height);
            System.Drawing.Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
            g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);

            return bp;
        }
        /// <summary>
        /// 保存表单提交的文件
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="Text">文件文本</param>
        /// <returns>空时保存成功，反之错误信息</returns>
        public string SaveBase64ToFile(string FileName, string Text)
        {
            try
            {
                var strm = Base64ToStrm(Text);
                //将流转回Image，用于将PNG 式照片转为jpg，压缩体积以便保存。
                var imgae = System.Drawing.Image.FromStream(strm);
                var dir = Path.GetDirectoryName(FileName) + "\\";
                Directory.CreateDirectory(dir);
                imgae.Save(FileName, System.Drawing.Imaging.ImageFormat.Jpeg);//保存图片
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// 转换成流文件
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public Stream Base64ToStrm(string Text)
        {
            //获取base64字符串
            var imgBytes = System.Convert.FromBase64String(Text);
            //将base64字符串转换为字节数组
            var stream = new System.IO.MemoryStream(imgBytes);
            return stream;
        }

        /*
        * 
        * 使用说明：
        * 　建议先定义一个WaterImage实例
        * 　然后利用实例的属性，去匹配需要进行操作的参数
        * 　然后定义一个WaterImageManage实例
        * 　利用WaterImageManage实例进行DrawImage（），印图片水印
        * 　DrawWords（）印文字水印
        * 
        -*/

        /// <summary>
        /// 在图片上添加水印文字
        /// </summary>
        /// <param name="sourcePicture">源图片文件(文件名，不包括路径)</param>
        /// <param name="waterWords">需要添加到图片上的文字</param>
        /// <param name="alpha">透明度</param>
        /// <param name="position">位置</param>
        /// <param name="PicturePath">文件路径</param>
        /// <returns></returns>
        public static string DrawWords(string sourcePicture,
            string waterWords,
            float alpha,
            ImagePosition position,
            string PicturePath)
        {
            //
            // 判断参数是否有效
            //
            if (sourcePicture == string.Empty || waterWords == string.Empty || alpha == 0.0 || PicturePath == string.Empty)
            {
                return sourcePicture;
            }

            //
            // 源图片全路径
            //
            if (PicturePath.Substring(PicturePath.Length - 1, 1) != "/")
                PicturePath += "/";
            string sourcePictureName = PicturePath + sourcePicture;
            string fileExtension = System.IO.Path.GetExtension(sourcePictureName).ToLower();

            //
            // 判断文件是否存在,以及文件名是否正确
            //
            if (System.IO.File.Exists(sourcePictureName) == false || (
              fileExtension != ".gif" &&
              fileExtension != ".jpg" &&

              fileExtension != ".png"))
            {
                return sourcePicture;
            }

            //
            // 目标图片名称及全路径
            //
            string targetImage = sourcePictureName.Replace(System.IO.Path.GetExtension(sourcePictureName), "") + "_0703.jpg";

            //创建一个图片对象用来装载要被添加水印的图片
            Image imgPhoto = Image.FromFile(sourcePictureName);

            imgPhoto = DrawWords(imgPhoto, waterWords, alpha, position);

            //将grPhoto保存
            imgPhoto.Save(targetImage, ImageFormat.Jpeg);
            imgPhoto.Dispose();

            return targetImage.Replace(PicturePath, "");
        }
        /// <summary>
        /// 在图片上添加水印文字
        /// </summary>
        /// <param name="SrcImage">源图片</param>
        /// <param name="WaterWords">需要添加到图片上的文字</param>
        /// <param name="Alpha">透明度</param>
        /// <param name="Position">位置</param>
        /// <returns></returns>
        public static Image DrawWords(Image SrcImage, string WaterWords, float Alpha, ImagePosition Position)
        {
            //创建一个图片对象用来装载要被添加水印的图片
            Image imgPhoto = SrcImage;

            //获取图片的宽和高
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //
            //建立一个bitmap，和我们需要加水印的图片一样大小
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //SetResolution：设置此 Bitmap 的分辨率
            //这里直接将我们需要添加水印的图片的分辨率赋给了bitmap
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //Graphics：封装一个 GDI+ 绘图图面。
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //设置图形的品质
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //将我们要添加水印的图片按照原始大小描绘（复制）到图形中
            grPhoto.DrawImage(
             imgPhoto,                      //  要添加水印的图片
             new Rectangle(0, 0, phWidth, phHeight), // 根据要添加的水印图片的宽和高
             0,                           // X方向从0点开始描绘
             0,                           // Y方向

             phWidth,                      // X方向描绘长度
             phHeight,                      // Y方向描绘长度
             GraphicsUnit.Pixel);               // 描绘的单位，这里用的是像素

            //根据图片的大小我们来确定添加上去的文字的大小
            //在这里我们定义一个数组来确定
            int[] sizes = new int[] { 36, 30, 24, 18, 16, 14, 12, 10, 8, 6, 4 };

            //字体
            Font crFont = null;
            //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空
            SizeF crSize = new SizeF();

            //利用一个循环语句来选择我们要添加文字的型号
            //直到它的长度比图片的宽度小
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);

                //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。
                crSize = grPhoto.MeasureString(WaterWords, crFont);

                // ushort 关键字表示一种整数数据类型
                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            //截边5%的距离，定义文字显示(由于不同的图片显示的高和宽不同，所以按百分比截取)
            int yPixlesFromBottom = (int)(phHeight * .05);

            //定义在图片上文字的位置
            float wmHeight = crSize.Height;
            float wmWidth = crSize.Width;

            float xPosOfWm;

            float yPosOfWm;

            switch (Position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = phHeight / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = wmWidth / 2;
                    yPosOfWm = wmHeight / 2;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = wmHeight;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = wmWidth;

                    break;
                default:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            //封装文本布局信息（如对齐、文字方向和 Tab 停靠位），显示操作（如省略号插入和国家标准 (National) 数字替换）和 OpenType 功能。
            StringFormat StrFormat = new StringFormat();

            //定义需要印的文字居中对齐
            StrFormat.Alignment = StringAlignment.Center;
            StrFormat.LineAlignment = StringAlignment.Center;

            //SolidBrush:定义单色画笔。画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径。
            //这个画笔为描绘阴影的画笔，呈灰色
            int m_alpha = Convert.ToInt(256 * Alpha);
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(m_alpha, 0, 0, 0));

            //描绘文字信息，这个图层向右和向下偏移一个像素，表示阴影效果
            //DrawString 在指定矩形并且用指定的 Brush 和 Font 对象绘制指定的文本字符串。
            grPhoto.DrawString(WaterWords,                  //string of text
                          crFont,                     //font
                          semiTransBrush2,              //Brush
                          new PointF(xPosOfWm + 1, yPosOfWm + 1), //Position
                          StrFormat);

            //从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 Color 结构，这里设置透明度为153
            //这个画笔为描绘正式文字的笔刷，呈白色
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //第二次绘制这个图形，建立在第一次描绘的基础上
            grPhoto.DrawString(WaterWords,         //string of text
                          crFont,                  //font
                          semiTransBrush,              //Brush
                          new PointF(xPosOfWm, yPosOfWm), //Position
                          StrFormat);

            //imgPhoto是我们建立的用来装载最终图形的Image对象
            //bmPhoto是我们用来制作图形的容器，为Bitmap对象
            imgPhoto = bmPhoto;
            //释放资源，将定义的Graphics实例grPhoto释放，grPhoto功德圆满
            grPhoto.Dispose();


            return imgPhoto;
        }

        /// <summary>
        /// 根据Bom原始文件路径 获取文件名称
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static string BomSplitFileName(string urls = "")
        {
            if (urls.ToString().Length == 0)
            {
                return "";
            }
            else
            {
                var str = "";
                var arrStr = urls.Split('_');
                if (arrStr.Length == 1)
                {
                    str = "";
                }
                else if (arrStr.Length == 2)
                {
                    str = arrStr[1].ToString();
                }
                else
                {
                    for (int i = 0; i < arrStr.Length; i++)
                    {
                        if (i == 0)
                            continue;
                        str += arrStr[i].ToString() + "_";
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }
    }

    /// <summary>
    /// 装载水印图片的相关信息
    /// </summary>
    public class WaterImage
    {
        public WaterImage()
        {

        }

        private string m_sourcePicture;
        /// <summary>
        /// 源图片地址名字(带后缀)
        /// </summary>
        public string SourcePicture
        {
            get { return m_sourcePicture; }
            set { m_sourcePicture = value; }
        }

        private string m_waterImager;
        /// <summary>
        /// 水印图片名字(带后缀)
        /// </summary>
        public string WaterPicture
        {
            get { return m_waterImager; }
            set { m_waterImager = value; }
        }

        private float m_alpha;
        /// <summary>
        /// 水印图片文字的透明度
        /// </summary>
        public float Alpha
        {
            get { return m_alpha; }
            set { m_alpha = value; }
        }

        private ImagePosition m_postition;
        /// <summary>
        /// 水印图片或文字在图片中的位置
        /// </summary>
        public ImagePosition Position
        {
            get { return m_postition; }
            set { m_postition = value; }
        }

        private string m_words;
        /// <summary>
        /// 水印文字的内容
        /// </summary>
        public string Words
        {
            get { return m_words; }
            set { m_words = value; }
        }

    }
}
