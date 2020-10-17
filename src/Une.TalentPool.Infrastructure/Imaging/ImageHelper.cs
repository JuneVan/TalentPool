using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Une.TalentPool.Infrastructure.Imaging
{
    public class ImageHelper
    {
        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary> 
        public static void Square(Stream fromFile, string fileSaveUrl, int width, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(fileSaveUrl);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromStream(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= width && initImage.Height <= width)
            {
                initImage.Save(fileSaveUrl, ImageFormat.Jpeg);
            }
            else
            {
                //原始图片的宽、高
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型
                if (initWidth != initHeight)
                {
                    //截图对象
                    using (Image pickedImage = new Bitmap(initHeight, initHeight))
                    {
                        using (Graphics pickedG = Graphics.FromImage(pickedImage))
                        {

                            //宽大于高的横图
                            if (initWidth > initHeight)
                            {

                                //设置质量
                                pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                //定位
                                Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                                Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                                //画图
                                pickedG.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);
                                //重置宽
                                initWidth = initHeight;
                            }
                            //高大于宽的竖图
                            else
                            {
                                //设置质量
                                pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                //定位
                                Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                                Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                                //画图
                                pickedG.DrawImage(initImage, toR, fromR, GraphicsUnit.Pixel);
                                //重置高
                                initHeight = initWidth;
                            }

                            //将截图对象赋给原图
                            initImage = (Image)pickedImage.Clone();

                        }
                    }
                }

                //缩略图对象
                Image resultImage = new Bitmap(width, width);
                Graphics resultG = Graphics.FromImage(resultImage);
                //设置质量
                resultG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                resultG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //用指定背景色清空画布
                resultG.Clear(Color.White);
                //绘制缩略图
                resultG.DrawImage(initImage, new Rectangle(0, 0, width, width), new Rectangle(0, 0, initWidth, initHeight), GraphicsUnit.Pixel);

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                //保存缩略图
                resultImage.Save(fileSaveUrl, ici, ep);

                //释放关键质量控制所用资源
                ep.Dispose();

                //释放缩略图资源
                resultG.Dispose();
                resultImage.Dispose();

                //释放原始图片资源
                initImage.Dispose();
            }
        }
    }
}
