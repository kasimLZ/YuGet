using System.ComponentModel.DataAnnotations;

namespace YuGet.Storage.Azure
{
    public class AzureTableOptions
    {
        [Required]
        public string ConnectionString { get; set; }
    }
}
