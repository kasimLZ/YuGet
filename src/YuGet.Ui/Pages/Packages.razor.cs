using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YuGet.Database.Models;
using YuGet.Ui.Services;

namespace YuGet.Ui.Pages
{
	public class PackagesComponent : LayoutComponentBase
	{
		[CascadingParameter(Name = "layoutService")]
		public LayoutStatusService layoutService { get; set; }

		protected IList<Package> PackageList { get; private set; } = new List<Package>();

		protected int PageSize = 20;

		protected int PageIndex = 1;

		protected int TotalCount = 1247621;

		protected override void OnInitialized()
		{
			layoutService.SearchBarVisible = true;
			layoutService.LayoutStatusChange();
		}

		protected void PageIndexChange (int pageIndex)
		{
		
		}

	}
}
