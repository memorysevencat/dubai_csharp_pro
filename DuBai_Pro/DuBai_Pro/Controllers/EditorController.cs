using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuBai_Pro.Controllers
{
    public class EditorController : Controller
    {
        // GET: Editor
        public ActionResult Index()
        {
            return View();
        }

       
        [ValidateInput(false)]
        public ActionResult Editor(FormCollection fc)
        {
            var con = fc["content"];
            using (DuBaiOfficeEntities en=new DuBaiOfficeEntities())
            {
                Dubai_Editor editor = new Dubai_Editor();
                en.Dubai_Editor.Add(editor);
                en.SaveChanges();
            }

            return View();
        }
    }
}