using IMDBApi_Assignment4.Services.Interface;
using Supabase;

namespace IMDBApi_Assignment4.Services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly Client _supabaseClient;
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;

        public SupabaseService(IConfiguration configuration)
        {
            _configuration = configuration;

            var supabaseUrl = _configuration["Supabase:Url"];
            var supabaseKey = _configuration["Supabase:Key"];
            _bucketName = _configuration["Supabase:BucketName"];

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            _supabaseClient = new Client(supabaseUrl, supabaseKey, options);
        }

        public async Task<string> UploadMoviePosterAsync(int movieId, IFormFile posterImage)
        {
            if (posterImage == null || posterImage.Length == 0)
            {
                throw new ArgumentException("No file uploaded");
            }

            var fileExtension = Path.GetExtension(posterImage.FileName).ToLower();

            if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
            {
                throw new ArgumentException("Only jpg, jpeg, and png files are allowed");
            }

            var fileName = $"{movieId}_{Guid.NewGuid()}{fileExtension}";

            using var memoryStream = new MemoryStream();
            await posterImage.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var uploadOptions = new Supabase.Storage.FileOptions
            {
                CacheControl = "3600",
                Upsert = true
            };

            var result = await _supabaseClient.Storage
                .From(_bucketName)
                .Upload(memoryStream.ToArray(), fileName, uploadOptions);

            var publicUrl = _supabaseClient.Storage
                .From(_bucketName)
                .GetPublicUrl(fileName);

            return publicUrl;
        }

        public async Task<bool> DeleteMoviePosterAsync(string imagePath)
        {
            try
            {
                var uri = new Uri(imagePath);
                var fileName = Path.GetFileName(uri.LocalPath);

                await _supabaseClient.Storage
                    .From(_bucketName)
                    .Remove(new List<string> { fileName });

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting file: {ex.Message}", ex);
            }
        }
    }
}