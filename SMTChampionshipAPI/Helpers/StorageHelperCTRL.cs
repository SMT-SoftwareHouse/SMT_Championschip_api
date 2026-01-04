namespace SMTChampionshipAPI.Helpers
{
    public static class StorageHelperCTRL
    {
        public static string GetStorageDirectoryPath(IConfiguration configuration)
        {
            return configuration["StorageDirectoryPath"];
        }
    }
}
