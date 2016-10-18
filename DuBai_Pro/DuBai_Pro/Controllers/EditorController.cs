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
                return Content("<script>alert('删除成功！');window.location='" + Url.Content("~/Editor/GetAllEditor") + "'</script>");
            }
        }


        public ActionResult Details(int id)
        {
            using (DuBaiOfficeEntities en = new DuBaiOfficeEntities())
            {
                var detail = from ed in en.Dubai_Editor where ed.EditorID == id select ed;

                Dubai_Editor deta = detail.FirstOrDefault();

                if (deta != null)
                {
                    return View("Details", deta);
                }
                else
                {
                    return RedirectToAction("GetAllEditor");
                }
                
                

            }
           

        }

        public ActionResult Change(int id)
        {
            using (DuBaiOfficeEntities en = new DuBaiOfficeEntities())
            {
                var ch = (from c in en.Dubai_Editor where c.EditorID == id select c).ToList();

                Dubai_Editor deta = ch.FirstOrDefault();

                ViewData["course"] = deta;

                return View("Change", deta);
            }

            
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Change(Dubai_Editor editor)
        {
            using (DuBaiOfficeEntities en = new DuBaiOfficeEntities())
            {
                var bj = en.Dubai_Editor.Find(editor.EditorID);

                bj.Article = editor.Article;
                bj.ArticleType = editor.ArticleType;
                bj.Description = editor.Description;
                bj.ReleaseTime = editor.ReleaseTime;
                bj.Title = editor.Title;
                bj.sort = editor.sort;

                en.Entry<Dubai_Editor>(bj).State = System.Data.Entity.EntityState.Modified;
                int intCount = en.SaveChanges();
                                 if (intCount > 0)
                                     {
                                        return RedirectToAction("Details", new { id = editor.EditorID });
                                     }
                                else
                 {
                                       return Content("修改失败");
                                    }


               
            }


        }

    }
}