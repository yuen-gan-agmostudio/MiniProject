﻿using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Services
{
    public static class CourseServiceManager
    {
        public static List<CourseModel> GetCourseList(FilterModel filterModel)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var query = db.Courses.AsQueryable();

                var skip = (filterModel.Page - 1) * (filterModel.Take);
                var courses = query.Skip(skip).Take(filterModel.Take).ToList();
                return courses;
            }
        }
        public static CourseModel? GetCourse(int id)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var course = db.Courses.Where(x => x.Id == id).FirstOrDefault();
                return course;
            }
        }
        public static void CreateCourse(CourseModel model)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var newCourse = new CourseModel()
                {
                    Name = model.Name,
                    Description = model.Description
                };
                db.Courses.Add(newCourse);
                db.SaveChanges();
            }
        }
        public static CourseModel? UpdateCourse(CourseModel model)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var course = db.Courses.Where(x => x.Id == model.Id).FirstOrDefault();
                if(course != null)
                {
                    course.Name = string.IsNullOrEmpty(model.Name) ? course.Name : model.Name;
                    course.Description = model.Description;
                    db.SaveChanges();
                }
                return course;
            }
        }
        public static void DeleteCourse(int id)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var course = db.Courses.Where(x => x.Id == id).FirstOrDefault();
                if (course != null)
                {
                    db.Courses.Remove(course);
                    db.SaveChanges();
                }
            }
        }
        public static bool EnrollCourse(UserCourseModel model)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var course = db.Courses.Where(x => x.Id == model.CourseId).FirstOrDefault();
                if (course == null) return false;

                var userCourse = db.UserCourses.Where(x => x.UserId == model.UserId && x.CourseId == model.CourseId).FirstOrDefault();
                if (userCourse != null) return false;

                db.UserCourses.Add(new UserCourseModel()
                {
                    UserId = model.UserId,
                    CourseId = model.CourseId
                });
                db.SaveChanges();
                return true;
            }
        }
        public static bool WithdrawCourse(UserCourseModel model)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var course = db.Courses.Where(x => x.Id == model.CourseId).FirstOrDefault();
                if (course == null) return false;

                var userCourse = db.UserCourses.Where(x => x.UserId == model.UserId && x.CourseId == model.CourseId).FirstOrDefault();
                if(userCourse == null) return false;

                db.UserCourses.Remove(userCourse);
                db.SaveChanges();
                return true;
            }
        }
        public static List<UserCourseModel> GetCourseUserList(FilterModel filterModel)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var query = db.UserCourses
                    .Include(x => x.User)
                    .Include(x => x.Course)
                    .Where(x => (!filterModel.CourseId.HasValue || x.CourseId == filterModel.CourseId)
                    && (string.IsNullOrEmpty(filterModel.UserId) || x.UserId == filterModel.UserId))
                    .AsQueryable();

                var skip = (filterModel.Page - 1) * (filterModel.Take);
                var courseUsers = query.Skip(skip).Take(filterModel.Take).ToList();

                return courseUsers;
            }
        }
        public static bool ApproveCourseEnrollment(UserCourseModel model)
        {
            using (var db = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
            {
                var course = db.Courses.Where(x => x.Id == model.CourseId).FirstOrDefault();
                if (course == null) return false;

                var userCourse = db.UserCourses.Where(x => x.UserId == model.UserId && x.CourseId == model.CourseId).FirstOrDefault();
                if (userCourse == null) return false;

                userCourse.IsApproved = model.IsApproved;
                userCourse.ApprovedBy = model.IsApproved ? model.ApprovedBy : null;
                userCourse.IsRejected = !model.IsApproved;
                userCourse.RejectedBy = !model.IsApproved ? model.ApprovedBy : null;
                
                db.SaveChanges();
                return true;
            }
        }
    }
}
