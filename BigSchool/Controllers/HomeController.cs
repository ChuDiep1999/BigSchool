﻿using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            BigSchoolContext context = new BigSchoolContext();
            var upcomingCourse = context.Course.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            var userID = User.Identity.GetUserId();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturedId);
                i.Name = user.Name;
                if (userID != null)
                {
                    i.isLogin = true;
                    Attendance find = context.Attendance.FirstOrDefault(p =>
p.CourseId == i.Id && p.Attendee == userID);
                    if (find == null)
                        i.isShowGoing = true;
                    Following findFollow = context.Following.FirstOrDefault(p =>
p.FollowerId == userID && p.FolloweeId == i.LecturedId);
                    if (findFollow == null)
                        i.isShowFollow = true;
                }
            }
            return View(upcomingCourse);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}