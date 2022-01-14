using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsReview.Configuration
{
	public class SiteSetting
	{
		public SitePlatformEnum Platform { get; internal set; }
		public string DatabaseConnectionString { get; internal set; }
		public SiteNameEnum SiteName { get; }

		public SiteSetting(SiteNameEnum siteName)
		{
			this.SiteName = siteName;
		}
	}
}
