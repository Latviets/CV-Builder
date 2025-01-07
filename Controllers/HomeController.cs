using CV_builder.Models;
using CV_builder.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CV_builder.Services;

namespace CV_builder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CVStorage _storage;
        private DocumentService _documentService;

        public HomeController(ILogger<HomeController> logger, CVStorage storage, DocumentService documentService)
        {
            _logger = logger;
            _storage = storage;
            _documentService = documentService;
        }

        public IActionResult Index()
        {
            var model = _storage.GetCvList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CVModel
            {
                Education = new List<Education> { new Education() },
                WorkExperience = new List<WorkExperience> { new WorkExperience() }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CVModel cv)
        {
            if (ModelState.IsValid)
            {
                _storage.AddCV(cv);
                return RedirectToAction("Index");
            }

            return View(cv);
        }
        public IActionResult Details(int id)
        {
            var cv = _storage.GetCvList().FirstOrDefault(c => c.Id == id);
            if (cv == null)
            {
                return NotFound();
            }

            return View(cv);
        }

        public IActionResult Edit(int id)
        {
            var list = _storage.GetCvList();
            var cv = _storage.GetCvList().FirstOrDefault(cv => cv.Id == id);
            if (cv == null)
            {
                return NotFound();
            }
                     
            if (cv.Education == null)
            {
                cv.Education = new List<Education>();
            }
            if (cv.WorkExperience == null)
            {
                cv.WorkExperience = new List<WorkExperience>();
            }
            
            return View(cv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CVModel cv)
        {
            if (id != cv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCv = _storage.GetCvList().FirstOrDefault(c => c.Id == id);
                if (existingCv != null)
                {
                    existingCv.Name = cv.Name;
                    existingCv.Surname = cv.Surname;
                    existingCv.Phone = cv.Phone;
                    existingCv.Email = cv.Email;
                    existingCv.Address = cv.Address;
                    existingCv.Education = cv.Education;
                    existingCv.WorkExperience = cv.WorkExperience;
                    _storage.SaveCVs();
                }
                
                return RedirectToAction("Index");
            }
            return View(cv);
        }

        public IActionResult Delete(int id)
        {
            var cv = _storage.GetCvList().FirstOrDefault(c => c.Id == id);
            if (cv == null)
            {
                return NotFound();
            }
            return View(cv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cv = _storage.GetCvList().FirstOrDefault(c => c.Id == id);
            if (cv != null)
            {
                _storage.GetCvList().Remove(cv);
                _storage.SaveCVs();
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Info()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Download(int id)
        {
            var cv = _storage.GetCvList().FirstOrDefault(c => c.Id == id);
            if (cv == null)
            {
                return NotFound();
            }

            var docBytes = _documentService.GenerateWordDocument(cv);
            string fileName = $"CV_{cv.Name}_{cv.Surname}.docx";
            
            return File(docBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);
        }
    }
}
