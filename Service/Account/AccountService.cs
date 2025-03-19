using Supabase.Gotrue;

public class AccountService
{
    private readonly Supabase.Client _client;

    public AccountService(SupabaseClientService clientService)
    {
        _client = clientService.Client;
    }

    //Авторизация
    public async Task<User?> LoginAsync(string email, string password)
    {
        try
        {
            var user = await _client.From<User>()
                .Filter("email", Supabase.Postgrest.Constants.Operator.Equals, email)
                .Single();

            if (user == null || user.password != password)
            {
                return null; // Неверный email или пароль
            }

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LoginAsync: {ex.Message}");
            return null;
        }
    }


    // Регистрация
    public async Task<User?> RegisterAsync(string email, string password, string your_name)
    {
        try
        {
            var existingUser = await _client.From<User>()
                .Filter("email", Supabase.Postgrest.Constants.Operator.Equals, email)
                .Single();

            if (existingUser != null)
            {
                return null;
            }

            var newUser = new User
            {
                your_name = your_name,
                email = email,
                password = password,
            };

            var response = await _client.From<User>().Insert(newUser);

            return response.Models.FirstOrDefault();
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
