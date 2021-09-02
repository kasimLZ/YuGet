using System;

namespace YuGet.UI.Services
{
	public class LayoutStatusService
	{
		private readonly Action StatusChange;

		public LayoutStatusService(Action StatusChange)
		{
			this.StatusChange = StatusChange;
		}

		public bool SearchBarVisible { get; set;}

		public void LayoutStatusChange() => StatusChange();
	}
}
