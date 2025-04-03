using Supabase.Gotrue;

namespace Motora.Service.Discussion
{
    public class EstimationService
    {
        private readonly Supabase.Client _client;

        public EstimationService(SupabaseClientService clientService)
        {
            _client = clientService.Client;
        }

        public async Task<Estimation?> EstimationAsync(int? user_id, string estimation, string type, string category, string reason)
        {
            try
            {
                // Проверяем, сколько оценок у пользователя
                // Проверяем, сколько оценок у пользователя
                var userEstimationCount = await _client.From<Estimation>()
                    .Filter("user_id", Supabase.Postgrest.Constants.Operator.Equals, user_id)
                    .Count(Supabase.Postgrest.Constants.CountType.Exact);

                // Если уже есть 5 или больше, не даем оставить новую
                if (userEstimationCount >= 5)
                {
                    throw new Exception("Вы уже оставили 5 оценок. Новые оценки недоступны.");
                }

                // Создание новой оценки
                var newEstimation = new Estimation
                {
                    user_id = user_id,
                    estimation = estimation,
                    type = type,
                    category = category,
                    reason = reason
                };

                var response = await _client.From<Estimation>().Insert(newEstimation);

                return response.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении оценки: " + ex.Message);
                return null;
            }
        }


        public async Task<List<Estimation>> GetAllEstimationWithUsers()
        {
            var reviews = await _client.From<Estimation>().Get();

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