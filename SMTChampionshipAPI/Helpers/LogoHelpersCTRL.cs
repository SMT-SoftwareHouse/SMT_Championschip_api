using Microsoft.Extensions.Configuration;

namespace SMTChampionshipAPI.Helpers
{
    public static class LogoHelpersCTRL
    {
     
        public static async Task<string> SaveLogoAsync(IConfiguration configuration,IFormFile logo)
        {
            if (logo == null) return "";

            var uploadsFolder = StorageHelperCTRL.GetStorageDirectoryPath(configuration);



            var ext = Path.GetExtension(logo.FileName);
            var fileName = $"team_{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await logo.CopyToAsync(stream);

            return $"{uploadsFolder}/{fileName}";
        }

    
    }
}
