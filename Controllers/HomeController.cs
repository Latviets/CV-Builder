using CV_builder.Models;
using CV_builder.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CV_builder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CVStorage _storage;

        public HomeController(ILogger<HomeController> logger, CVStorage storage)
        {
            _logger = logger;
            _storage = storage;
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
    }
}
