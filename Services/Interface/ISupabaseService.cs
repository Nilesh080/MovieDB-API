namespace IMDBApi_Assignment4.Services.Interface
{
    public interface ISupabaseService
    {
        Task<string> UploadMoviePosterAsync(int movieId, IFormFile posterImage);
        Task<bool> DeleteMoviePosterAsync(string imagePath);
    }
}