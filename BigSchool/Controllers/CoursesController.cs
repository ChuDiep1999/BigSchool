using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            // get list Category
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.listCategory = context.Category.ToList();
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objcourse)
        {
            BigSchoolContext context = new BigSchoolContext();
            ModelState.Remove("LecturedId");
            if (!ModelState.IsValid)
            {
                objcourse.listCategory = context.Category.ToList();
                return View("Create", objcourse);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objcourse.LecturedId = user.Id;
            context.Course.Add(objcourse);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        BigSchoolContext context = new BigSchoolContext();
        // Không xét valid LectureId vì bằng user đăng nhập
       
    }
}
