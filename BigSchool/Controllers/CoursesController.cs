using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendance.Where(p=>p.Attendee==currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach(Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.LecturedName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(objCourse.LecturedId).Name;
                courses.Add(objCourse);

            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            BigSchoolContext con = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var courses = con.Course.Where(c => c.LecturedId == currentUser.Id && c.DateTime > DateTime.Now).ToList();

            foreach (Course i in courses)
            {
                i.LecturedName = currentUser.Name;
            }
            return View(courses);
        }
        public ActionResult Edit(int? id)
        {
            Course course = context.Course.Find(id);
            course.listCategory = context.Category.ToList();
            if (id == null)
            {
                return HttpNotFound();
            }
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course objcourse)
        {
            ModelState.Remove("LecturedId");
            if (!ModelState.IsValid)
            {
                objcourse.listCategory = context.Category.ToList();
                return View("Edit", objcourse);
            }
            
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objcourse.LecturedId = user.Id;

            context.Course.AddOrUpdate(objcourse);
            context.SaveChanges();

            return RedirectToAction("Mine", "Courses");
        }
        public ActionResult Delete(int? id)
        {
            Course course = context.Course.Find(id);
            course.listCategory = context.Category.ToList();
            return View (course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            Course course = context.Course.Find(id);
            Attendance attendance = context.Attendance.Find(id, currentUser.Id);
            context.Attendance.Remove(attendance);
            context.SaveChanges();
            context.Course.Remove(course);
            context.SaveChanges();
            return RedirectToAction("Mine", "Courses");
        }
    }
}
