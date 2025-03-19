using Supabase.Gotrue;

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
                var existingUser = await _client.From<Reviews>()
                    .Filter("user_id", Supabase.Postgrest.Constants.Operator.Equals, user_id)
                    .Single();

                if (existingUser != null)
                {
                    return null;
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
    }
}