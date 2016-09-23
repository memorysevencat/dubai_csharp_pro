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

        
        public ActionResult Editor()
        {
               return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Editor( string title,string abstr,FormCollection fc)
        {
            var con = fc["content"];
            
            using (DuBaiOfficeEntities en=new DuBaiOfficeEntities())
            {
                Dubai_Editor editor = new Dubai_Editor() { Title=title,Description=abstr,Article=con,ReleaseTime=DateTime.Now };
                en.Dubai_Editor.Add(editor);
                en.SaveChanges();
            }

            return View();
        }


        public ActionResult GetAllEditor()
        {
            using (DuBaiOfficeEntities en = new DuBaiOfficeEntities())
            {
                var editor = (from ed in en.Dubai_Editor select ed).ToList();

                return View("GetAllEditor", editor);
            }
         
        }


        public ActionResult Delete(int id)
        {
            using (DuBaiOfficeEntities en = new DuBaiOfficeEntities())
            {
              
                var editor = en.Dubai_Editor.Find(id);
                en.Dubai_Editor.Remove(editor);
                en.SaveChanges();
                return View("GetAllEditor", editor);
            }
        }





    }
}