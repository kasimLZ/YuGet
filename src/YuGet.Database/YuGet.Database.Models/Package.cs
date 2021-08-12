using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
	[Index("Id")]
    [Index("Id", "NormalizedVersionString", IsUnique = true)]
    public class Package : EntitySet
	{
        [Required]
        public string Key { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string Summary { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string RepositoryType { get; set; }

        [MaxLength(20)]
        public string Language { get; set; }

        [MaxLength(44)]
        public string MinClientVersion { get; set; }

        [Column("Version"), MaxLength(64), Required]
        public string NormalizedVersionString { get; set; }

        [Column("OriginalVersion"), MaxLength(64)]
        public string OriginalVersionString { get; set; }

        [MaxLength(1000)]
        public string ReleaseNotes { get; set; }

        public List<string> Authors { get; set; }

        [MaxLength(1000)]
        public Uri IconUrl { get; set; }

        [MaxLength(1000)]
        public Uri LicenseUrl { get; set; }

        [MaxLength(1000)]
        public Uri ProjectUrl { get; set; }

        [MaxLength(1000)]
        public Uri RepositoryUrl { get; set; }

        public virtual ICollection<PackageTag> Tags { get; set; }

        public long Downloads { get; set; }

        public bool HasReadme { get; set; }

        public bool HasEmbeddedIcon { get; set; }

        public bool IsPrerelease { get; set; }

        public bool Listed { get; set; }

        public DateTime Published { get; set; }

        public bool RequireLicenseAcceptance { get; set; }

        public SemVerLevel SemVerLevel { get; set; }

        
        /// <summary>
        /// Used for optimistic concurrency.
        /// </summary>
        public byte[] RowVersion { get; set; }

        public virtual ICollection<PackageDependency> Dependencies { get; set; }
        public virtual ICollection<PackageType> PackageTypes { get; set; }
        public virtual ICollection<TargetFramework> TargetFrameworks { get; set; }

        [NotMapped]
        public NuGetVersion Version
        {
            get
            {
                // Favor the original version string as it contains more information.
                // Packages uploaded with older versions of BaGet may not have the original version string.
                return NuGetVersion.Parse(OriginalVersionString ?? NormalizedVersionString);
            }
            set
            {
                NormalizedVersionString = value.ToNormalizedString().ToLowerInvariant();
                OriginalVersionString = value.OriginalVersion;
            }
        }

        [NotMapped]
        public string IconUrlString => IconUrl?.AbsoluteUri ?? string.Empty;

        [NotMapped]
        public string LicenseUrlString => LicenseUrl?.AbsoluteUri ?? string.Empty;

        [NotMapped]
        public string ProjectUrlString => ProjectUrl?.AbsoluteUri ?? string.Empty;

        [NotMapped]
        public string RepositoryUrlString => RepositoryUrl?.AbsoluteUri ?? string.Empty;
    }
}
