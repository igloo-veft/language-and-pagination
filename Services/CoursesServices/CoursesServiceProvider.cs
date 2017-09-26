using System.Collections.Generic;
using System.Linq;
using System;
using CoursesAPI.Models;
using CoursesAPI.Services.DataAccess;
using CoursesAPI.Services.Exceptions;
using CoursesAPI.Services.Models.Entities;

namespace CoursesAPI.Services.CoursesServices
{
    public class CoursesServiceProvider
    {
        private readonly IUnitOfWork _uow;

        private readonly IRepository<CourseInstance> _courseInstances;
        private readonly IRepository<TeacherRegistration> _teacherRegistrations;
        private readonly IRepository<CourseTemplate> _courseTemplates;
        private readonly IRepository<Person> _persons;

        public CoursesServiceProvider(IUnitOfWork uow)
        {
            _uow = uow;

            _courseInstances = _uow.GetRepository<CourseInstance>();
            _courseTemplates = _uow.GetRepository<CourseTemplate>();
            _teacherRegistrations = _uow.GetRepository<TeacherRegistration>();
            _persons = _uow.GetRepository<Person>();
        }

        /// <summary>
        /// You should implement this function, such that all tests will pass.
        /// </summary>
        /// <param name="courseInstanceID">The ID of the course instance which the teacher will be registered to.</param>
        /// <param name="model">The data which indicates which person should be added as a teacher, and in what role.</param>
        /// <returns>Should return basic information about the person.</returns>
        public PersonDTO AddTeacherToCourse(int courseInstanceID, AddTeacherViewModel model)
        {
            // TODO: implement this logic!
            return null;
        }

        /// <summary>
        /// You should write tests for this function. You will also need to
        /// modify it, such that it will correctly return the name of the main
        /// teacher of each course.
        /// </summary>
        /// <param name="semester"></param>
        /// <returns></returns>
        public Envelope<CourseInstanceDTO> GetCourseInstancesBySemester(string semester, string language, int pageNumber, int pageSize)
        {
            if (string.IsNullOrEmpty(semester))
            {
                semester = "20153";
            }

            List<CourseInstanceDTO> courses;

            //if (string.IsNullOrEmpty(language) || language == "IS") {
            if (language == "EN") {
                Console.WriteLine("English");
                var coursesTmp = (from c in _courseInstances.All()
                                    join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
                                    where c.SemesterID == semester
                                    select new CourseInstanceDTO
                                    {
                                        Name = ct.Name_EN,
                                        TemplateID = ct.CourseID,
                                        CourseInstanceID = c.ID,
                                        MainTeacher = "" // Hint: it should not always return an empty string!
                                    }).ToList();
                courses = coursesTmp;
            }
            else { // IS is default choice
                Console.WriteLine("Not english");
                var coursesTmp = (from c in _courseInstances.All()
                                    join ct in _courseTemplates.All() on c.CourseID equals ct.CourseID
                                    where c.SemesterID == semester
                                    select new CourseInstanceDTO
                                    {
                                        Name = ct.Name,
                                        TemplateID = ct.CourseID,
                                        CourseInstanceID = c.ID,
                                        MainTeacher = "" // Hint: it should not always return an empty string!
                                    }).ToList();

                courses = coursesTmp;
            }

            var paging = new Paging {
                PageCount = (int)Math.Ceiling(courses.Count / (double)pageSize),
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalNumberOfItems = courses.Count
            };

            return new Envelope<CourseInstanceDTO> {
                Items = courses.Skip((pageNumber-1)*pageSize).Take(pageSize), // should not be necessary to do ToList() because this is already a list
                Paging = paging
            };
        }
    }
}
