using Microsoft.AspNetCore.Mvc;
using Motora.Service.Discussion;
using static System.Net.Mime.MediaTypeNames;

namespace Motora.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly ReviewsService _reviewsService;
        private readonly EstimationService _estimationService;

        // Единый конструктор
        public DiscussionController(ReviewsService reviewsService, EstimationService estimationService)
        {
            _reviewsService = reviewsService ?? throw new ArgumentNullException(nameof(reviewsService));
            _estimationService = estimationService ?? throw new ArgumentNullException(nameof(_estimationService));
        }

        public IActionResult Reviews()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> reviews(string title, string text, string category, string estimation, bool hiddenPrivates)
        {
            var user_id = HttpContext.Session.GetInt32("ID");

            if (user_id == null)
            {
                TempData["ErrorMessage"] = "Вы должны быть авторизованы, чтобы оставить отзыв.";
                return RedirectToAction("login", "profile");
            }

            var result = await _reviewsService.ReviewsAsync(user_id, title, text, category, estimation, hiddenPrivates);

            if (result == null)
            {
                TempData["ErrorMessage"] = "Ошибка при сохранении отзыва. Возможно, вы уже оставили 5 отзывов.";
                return RedirectToAction("reviews");
            }

            TempData["SuccessMessage"] = "Отзыв успешно добавлена!";
            return RedirectToAction("allreviews");
        }

        public async Task<IActionResult> AllReviews()
        {
            var reviews = await _reviewsService.GetAllReviewsWithUsers();
            return View(reviews);
        }

        public IActionResult Estimation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> estimation(string hiddenEstimation, string hiddenRatingType, string hiddenCategory, string hiddenReason)
        {
            var user_id = HttpContext.Session.GetInt32("ID");

            if (user_id == null)
            {
                TempData["ErrorMessage"] = "Вы должны быть авторизованы, чтобы оставить оценку.";
                return RedirectToAction("login", "profile");
            }

            var result = await _estimationService.EstimationAsync(user_id, hiddenEstimation, hiddenRatingType, hiddenCategory, hiddenReason);

            if (result == null)
            {
                TempData["ErrorMessage"] = "Ошибка при сохранении оценки. Возможно, вы уже оставили 5 оценок.";
                return RedirectToAction("estimation");
            }

            TempData["SuccessMessage"] = "Оценка успешно добавлена!";
            return RedirectToAction("allestimation");
        }





        public async Task<IActionResult> AllEstimation()
        {
            var reviews = await _estimationService.GetAllEstimationWithUsers();
            return View(reviews);
        }
    }
}
