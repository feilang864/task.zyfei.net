using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.UI.PC.WebHelper
{
    public class ImageCodeHelper
    {
        public HttpContextBase Context { get; set; }
        public ImageCodeHelper(HttpContextBase context)
        {
            Context = context;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0) + 5, 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                //Twist Image
                image = TwistImage(image, true, 1, 1);

                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// Generate random identifying code
        /// </summary>
        /// <param name="VcodeNum">length of the identifying code</param>
        /// <returns>identifying code</returns>
        public static string CreateValidateCode(int VcodeNum)
        {
            string Vchar = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
            int len = Vchar.Length; // the whole collection length
            int codeLen = VcodeNum;
            int i = 0; ;
            StringBuilder idenCode = new StringBuilder();
            System.Random random = new Random();
            int next = -1;
            string picked = string.Empty;
            do
            {
                next = random.Next(0, len - 1);
                picked = Vchar.Substring(next, 1);
                if (idenCode.ToString().IndexOf(picked) == -1)
                {
                    i++;
                    idenCode.Append(picked);
                }
            } while (i < codeLen);
            return idenCode.ToString();
        }

        /// <summary>
        /// sinusoid twist
        /// </summary>
        /// <param name="srcBmp"></param>
        /// <param name="bXDir">whether to twist</param>
        /// <param name="dMultValue">Mult Value usually < 3 </param>
        /// <param name="dPhase">the start phase value, usually [0 - 2*Math.PI]</param>
        /// <returns></returns>
        private static Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {

            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? Double.Parse(destBmp.Height.ToString()) : Double.Parse(destBmp.Width.ToString());
            for (int i = 0; i <= destBmp.Width - 1; i++)
            {
                for (int j = 0; j <= destBmp.Height - 1; j++)
                {
                    double dx = 0;
                    dx = bXDir ? Math.PI * 2 * Double.Parse(j.ToString()) / dBaseAxisLen : Math.PI * 2 * Double.Parse(i.ToString()) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // get the color of the current point
                    int nOldX = 0;
                    int nOldY = 0;
                    nOldX = bXDir ? i + Convert.ToInt32(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + Convert.ToInt32(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }


        #region ImageCode

        /// <summary>
        /// Check if the input image code is same with the value in the session
        /// </summary>
        /// <param name="inputCode"></param>
        /// <returns></returns>
        private ImageCodeError CheckImageCode(string inputCode)
        {
            object imageCode = Context.Session[SessionKey.SESSION_IMAGE_CODE];

            if (imageCode == null)
            {
                return ImageCodeError.Expired;
            }
            else if (imageCode.ToString().Trim() != inputCode.ToUpper())
            {
                return ImageCodeError.Wrong;
            }
            else
            {
                return ImageCodeError.NoError;
            }

        }

        internal ImageCodeModel CheckResult()
        {
            ImageCodeModel model = new ImageCodeModel();

            model.InputImageCode = Context.Request.Form["InputImageCode"];
            model.ImageCodeError = CheckImageCode(model.InputImageCode);

            return model;
        }

        internal void ClearImageCode()
        {
            Context.Session[SessionKey.SESSION_IMAGE_CODE] = null;
        }

        #endregion
    }
}