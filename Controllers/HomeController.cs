using CV_builder.Models;
using CV_builder.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CV_builder.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static CVStorage _storage = new CVStorage();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = _storage.GetCvList();
            return View(model);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CVModel cv)
        {
            if (ModelState.IsValid)
            {
                _storage.AddCV(cv);
                var check = _storage.GetCvList();
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
            var cv = _storage.GetCvList().FirstOrDefault(c => c.Id == id);
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
                    // Update other fields as needed
                }
                return RedirectToAction(nameof(Index));
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
        //[ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cv = _storage.GetCvList().FirstOrDefault(c => c.Id == id);
            if (cv != null)
            {
                _storage.GetCvList().Remove(cv);
            }
            return RedirectToAction(nameof(Index));
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
