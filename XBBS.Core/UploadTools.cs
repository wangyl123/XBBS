using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
namespace XBBS.Core
{
    public class UploadTools
    {
        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="autoCreate">不存在是否自动创建</param>
        /// <returns></returns>
        public static bool CheckDirExists(string path, bool autoCreate)
        {
            path = Path.GetFullPath(path);
            bool checkResult = Directory.Exists(path);
            if (!checkResult && autoCreate)
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (Exception ex) { throw ex; }
            }

            return checkResult;
        }

        /// <summary>
        /// 检查头像是否存在
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="webRootPath">网站根目录</param>
        /// <returns></returns>
        public static bool CheckAvatarExists(string uid, string webRootPath)
        {
            var dirpath = GetUploadPath(uid, webRootPath);
            bool check = true;
            if (!File.Exists(dirpath + uid + "-Large..jpg"))
            {
                check = false;
            }
            if (!File.Exists(dirpath + uid + "-Medium..jpg"))
            {
                check = false;
            }
            if (!File.Exists(dirpath + uid + "-Small..jpg"))
            {
                check = false;
            }
            return check;
        }

        /// <summary>
        /// 获得头像地址
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="webRootPath"></param>
        /// <returns></returns>
        public static AvatorModel GetAvatarPath(string uid)
        {
            StringBuilder dirpath = new StringBuilder();
            dirpath.Append("/uploads/avatar/");
            dirpath.Append(uid.Substring(0, 1) + "/");
            dirpath.Append(uid.Length >= 2 ? uid.Substring(0, 2) : uid.Substring(0, 1) + "/");
            return new AvatorModel()
            {
                Large = dirpath.ToString() + uid + "-Large..jpg",
                Medium = dirpath.ToString() + uid + "-Medium..jpg",
                Small = dirpath.ToString() + uid + "-Small..jpg"
            };
        }

        /// <summary>
        /// 获得上传目录的路径 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string GetUploadPath(string uid, string webRootPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(webRootPath.EndsWith("/") ? webRootPath : (webRootPath + "/") + "uploads/avatar/");
            if (sb[sb.Length - 1].ToString() != "/")
            {
                sb.Append("/");
            }
            sb.Append(uid.Substring(0, 1) + "/");
            sb.Append(uid.Length >= 2 ? uid.Substring(0, 2) : uid.Substring(0, 1) + "/");
            CheckDirExists(Path.GetFullPath(sb.ToString()), true);
            return sb.ToString();
        }

        /// <summary>
        /// 获得缩略图的保存地址
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static AvatorModel GetAvatorSavePath(string uid, string fileExtension, string WebRootPath)
        {
            string dirPath = GetUploadPath(uid, WebRootPath);
            return new AvatorModel()
            {
                Large = dirPath + uid + "-Large." + fileExtension,
                Medium = dirPath + uid + "-Medium." + fileExtension,
                Small = dirPath + uid + "-Small." + fileExtension,
                Source = dirPath + uid + fileExtension
            };
        }





        /// <summary>
        /// 生成头像缩略图
        /// </summary>
        /// <param name="avatarSavePath"></param>
        public static void MakeAvatarThumbnal(Core.AvatorModel avatarSavePath)
        {
            Core.UploadTools.MakeThumbnail(avatarSavePath.Source, avatarSavePath.Large, 100, 100, "Cut", "jpg");
            Core.UploadTools.MakeThumbnail(avatarSavePath.Source, avatarSavePath.Medium, 48, 48, "Cut", "jpg");
            Core.UploadTools.MakeThumbnail(avatarSavePath.Source, avatarSavePath.Small, 24, 24, "Cut", "jpg");
        }



        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>　　
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode, string imageType)
        {
            Image originalImage = Image.FromFile(originalImagePath);
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）　　　　　　　　
                    break;
                case "W"://指定宽，高按比例　　　　　　　　　　
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）　　　　　　　　
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
              new Rectangle(x, y, ow, oh),
              GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                switch (imageType.ToLower())
                {
                    case "gif":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "jpg":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "bmp":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "png":
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }















    }
    /// <summary>
    /// 上传文件类型
    /// </summary>
    public enum UploadType
    {
    }
    /// <summary>
    /// 缩略图保存地址类
    /// </summary>
    public class AvatorModel
    {
        public string Large { get; set; }

        public string Medium { get; set; }

        public string Small { get; set; }

        public string Source { get; set; }
    }
}
