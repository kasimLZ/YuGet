using System.ComponentModel.DataAnnotations;

namespace YuGet.Storage.Aliyun
{
    internal class AliyunStorageOptions
    {
        [Required]
        public string AccessKey { get; set; }

        [Required]
        public string AccessKeySecret { get; set; }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string Bucket { get; set; }

        public string Prefix { get; set; }
    }
}
