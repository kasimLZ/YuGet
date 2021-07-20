using MatBlazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YuGet.Database.Models;

namespace YuGet.Ui.Pages
{
	public class PackagesComponent : LayoutComponentBase
	{
		protected IList<Package> PackageList { get; private set; } = new List<Package>();

		protected int PageSize = 20;

		protected void OnPage(MatPaginatorPageEvent e)
		{
			PackageList.Add(new Package());
		}
	}
}
