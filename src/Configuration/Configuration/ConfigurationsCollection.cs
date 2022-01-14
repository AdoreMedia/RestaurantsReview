using Calysto.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsReview.Configuration
{
	class ConfigurationsCollection
	{
		private static string GetServerName()
		{
			return CalystoTools.IsLocalMachine ? "MSSQL2019" : "MSSQL2019";
		}

		private static string CreateConnString(string database)
		{
			return $@"Data Source=.\{GetServerName()};Initial Catalog={database};Integrated Security=True;Connect Timeout=30;";
		}

		static IEnumerable<SiteSetting> GetAllConfigurations()
		{
			yield return new SiteSetting(SiteNameEnum.RestaurantsReview)
			{
				Platform = SitePlatformEnum.LocalDevelopment,
				DatabaseConnectionString = CreateConnString("dbRestaurantsReview"),
			};
		}

#if DEBUG
		const bool _isDebug = true;
#else
		const bool _isDebug = false;
#endif

		public static IEnumerable<SiteSetting> GetPredefinedSettings()
		{
			foreach (SiteSetting sett in GetAllConfigurations())
			{
				if (_isDebug)
				{
					sett.Platform = SitePlatformEnum.LocalDevelopment;
					yield return sett;
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					sett.Platform = SitePlatformEnum.LinuxProduction;
					yield return sett;
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					sett.Platform = SitePlatformEnum.WindowsProduction;
					yield return sett;
				}
				else
				{
					throw new NotSupportedException($"{nameof(SiteSetting)} not found");
				}
			}
		}
	}
}
