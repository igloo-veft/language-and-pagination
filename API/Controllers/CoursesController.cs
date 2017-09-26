using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CoursesAPI.Models;
using CoursesAPI.Services.CoursesServices;
using CoursesAPI.Services.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/courses")]
    public class CoursesController : Controller
    {
        private readonly CoursesServiceProvider _service;

        public CoursesController(IUnitOfWork uow)
        {
            _service = new CoursesServiceProvider(uow);
        }

        [HttpGet]
        public IActionResult GetCoursesBySemester(string semester = null, string language = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // TODO: figure out the requested language (if any!)
            // and pass it to the service provider!

            // note: inspect the "accept-language" header
            // should we handle the list creation here or in services? probably better to construct in services and return
            // a CourseInstanceDTO model where Name is the CourseTemplate name in English if language is set to english
            var languageHeader = Request.Headers["Accept-language"].ToString();

            //Console.WriteLine(languageHeader.GetType());
            //Console.WriteLine(languageHeader);
            //Console.WriteLine(languageHeader.ToArray().GetValue(0));
            //SortedDictionary<String, int> dictionary = new SortedDictionary<String, int>();

            var languages = languageHeader.Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1)); // doesn't completely work since it doesn't give the topmost language the value of 1
            
            //Console.WriteLine(languages.ElementAt(0));
            //Console.WriteLine(languages.ElementAt(0).Value);
            //Console.WriteLine(languages.ElementAt(0).Quality);
            var qIS = 0.0;
            var hasEnglish = false;
            for(int i = 0; i < languages.Count(); i++)
            {
                if(languages.ElementAt(i).Value.ToLower() == "is")
                {
                    qIS = (double) languages.ElementAt(i).Quality;
                }
                if(languages.ElementAt(i).Value.ToLower().Contains("en") && languages.ElementAt(i).Quality > qIS)
                {
                    hasEnglish = true;
                }
            }

            //if (languageHeader.Contains("en") || languageHeader.Contains("en-us") || languageHeader.Contains("en-gb")) {
            if (hasEnglish) { // handles english
                return Ok(_service.GetCourseInstancesBySemester(semester, language="EN", pageNumber, pageSize));
            }
            else { // handles icelandic and default values
                return Ok(_service.GetCourseInstancesBySemester(semester, language, pageNumber, pageSize));
            }
            
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/teachers")]
        public IActionResult AddTeacher(int id, AddTeacherViewModel model)
        {
            var result = _service.AddTeacherToCourse(id, model);
            return Created("TODO", result);
        }
    }
}
