using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuBai_Pro.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        //登录
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //提交登录页
        public ActionResult Login(string username, string userpwd, string securityCode, string returnUrl)
        {
            //查询用户名密码是否相同
            int ji;
            using (DuBaiOfficeEntities entities = new DuBaiOfficeEntities())
            {
                ji = (from nn in entities.Dubai_USER where nn.UserName == username && nn.UserPwd == userpwd select nn).Count();
            }
            if (ModelState.IsValid)
            {
                //验证码验证
                if (String.Compare(TempData["SecurityCode"].ToString(), securityCode, true) != 0)
                {
                    //验证消息
                    ModelState.AddModelError("SecurityCode", "验证码不正确！");
                    return View("Login");
                }

                if (ji > 0)   //登录成功
                {
                    //用户名传值
                    TempData["username"] = username;

                    if (!string.IsNullOrEmpty(username))//记住用户名和密码
                    {
                        HttpCookie nameCookie = new HttpCookie("UserName", username);
                        Response.Cookies.Add(nameCookie);

                        HttpCookie passwordCookie = new HttpCookie("UserPwd", userpwd);
                        Response.Cookies.Add(passwordCookie);
                    }


                    Session["CurrentUser"] = username;//保持用户状态
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        //返回首页
                        return Redirect("~/");
                    }
                    else
                    {
                        return Redirect(Server.UrlDecode(returnUrl));
                    }
                }
                else
                {
                    ModelState.AddModelError("user", "用户名或密码不正确");
                    return View("Login");

                }
            }
            else
            {
                return View("Login");
            }

        }



        #region  生成验证码图片
        //[OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = false)]
        public ActionResult SecurityCode()
        {

            string oldcode = TempData["SecurityCode"] as string;
            string code = CreateRandomCode(5);
            TempData["SecurityCode"] = code;
            return File(CreateValidateGraphic(code), "image/Jpeg");
        }

        /// <summary>
        /// 生成随机的字符串
        /// </summary>
        private string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 16.0), 27);
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
                Font font = new Font("Arial", 13, (FontStyle.Bold | FontStyle.Italic));
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
        #endregion



    }
}