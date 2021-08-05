using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YuGet.Database.Abstractions.Infrastructure;

namespace YuGet.Database.Models
{
    [Index("Moniker")]
	public class TargetFramework : EntitySet
    {
        [MaxLength(256)]
        public string Moniker { get; set; }

        [ForeignKey("Package")]
        public Guid PackageId { get; set; }

        public virtual Package Package { get; set; }
    }
}
