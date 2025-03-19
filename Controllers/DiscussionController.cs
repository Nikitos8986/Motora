using Microsoft.AspNetCore.Mvc;
using Motora.Service.Discussion;

namespace Motora.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly ReviewsService _reviewsService;

        // Единый конструктор
        public DiscussionController(ReviewsService reviewsService)
        {
            _reviewsService = reviewsService ?? throw new ArgumentNullException(nameof(reviewsService));
        }

        public IActionResult Reviews()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> reviews(string title, string text, string category, string estimation, bool privates)
        {
            var user_id = HttpContext.Session.GetInt32("ID");

            var user = await _reviewsService.ReviewsAsync(user_id, title, text, category, estimation, privates);

            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь с таким email уже существует.");
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult AllReviews()
        {
            return View();
        }

        public IActionResult Estimation()
        {
            return View();
        }

        public IActionResult AllEstimation()
        {
            return View();
        }
    }
}
