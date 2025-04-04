﻿using Supabase.Gotrue;

namespace Motora.Service.Discussion
{
    public class ReviewsService
    {
        private readonly Supabase.Client _client;

        public ReviewsService(SupabaseClientService clientService)
        {
            _client = clientService.Client;
        }

        public async Task<Reviews?> ReviewsAsync(int? user_id, string title, string text, string category,string estimation, bool privates)
        {
            try
            {
                var userEstimationCount = await _client.From<Reviews>()
                    .Filter("user_id", Supabase.Postgrest.Constants.Operator.Equals, user_id)
                    .Count(Supabase.Postgrest.Constants.CountType.Exact);

                // Если уже есть 5 или больше, не даем оставить новую
                if (userEstimationCount >= 5)
                {
                    throw new Exception("Вы уже оставили 5 отзывов. Новые отзывы недоступны, вы можете удалить старые в профиле.");
                }

                var reviews = new Reviews
                {
                    user_id = user_id,
                    title = title,
                    text = text,
                    category = category,
                    estimation = estimation,
                    privates = privates
                };

                var response = await _client.From<Reviews>().Insert(reviews);

                return response.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Reviews>> GetAllReviewsWithUsers()
        {
            var reviews = await _client.From<Reviews>().Get();

            foreach (var review in reviews.Models)
            {
                review.User = (await _client.From<User>()
                    .Where(u => u.id == review.user_id)
                    .Get()).Models.FirstOrDefault();
            }

            return reviews.Models;
        }


    }
}