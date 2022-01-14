using Calysto.Common.Extensions;
using Calysto.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantsReview.Configuration
{
	public class SiteHelper
	{
		#region current site name resolver

		// CURRENT_SITE_NAME has to be set inside C:\Windows\System32\inetsrv\config\applicationHost.config
		// under app pool name add environmentVariables:
		/*
		  <add name="ArizonaWebCore AppPool" autoStart="true" managedRuntimeVersion="" startMode="AlwaysRunning">
                <processModel identityType="LocalSystem" />
				<environmentVariables>
					 <add name="CURRENT_SITE_NAME" value="POSDigitalnaPlatformaHR" />
				  </environmentVariables>
            </add>
		*/

		const string CURRENT_SITE_NAME = nameof(CURRENT_SITE_NAME);
		const string APP_POOL_ID = nameof(APP_POOL_ID);

		private static IEnumerable<string> GetAppPoolNames()
		{
			yield return _siteCurrentName?.ToString()?.ToNullIfEmpty(true);
			yield return Environment.GetEnvironmentVariable(APP_POOL_ID)?.ToNullIfEmpty(true); // IIS Application Pool name
			yield return Environment.GetEnvironmentVariable(CURRENT_SITE_NAME)?.ToNullIfEmpty(true); // environmentVariables from applicationHost.config
		}

		private static SiteNameEnum? ParseAppPoolName(string appPoolName)
		{
			int index = appPoolName?.IndexOf("_Debug") ?? -1;
			if(index > 0)
			{
				appPoolName = appPoolName.Remove(index);
			}
			
			if (!string.IsNullOrWhiteSpace(appPoolName) && Enum.TryParse(appPoolName.Trim(), out SiteNameEnum site))
				return site;
			else
				return default;
		}

		private static SiteNameEnum ResolveCurrentSiteName()
		{
			SiteNameEnum? siteName = GetAppPoolNames().Select(o => ParseAppPoolName(o)).Where(o => o.HasValue).FirstOrDefault();

			if (siteName.HasValue)
			{
				return siteName.Value;
			}
			else if (CalystoTools.IsDebugDefined || CalystoTools.IsLocalMachine)
			{
				return SiteNameEnum.RestaurantsReview;
			}
			
			throw new NotSupportedException("Can not resolve SiteName from non-hosted app");
		}

		/// <summary>
		/// current site name is unique per app instance
		/// </summary>
		private static SiteNameEnum? _siteCurrentName;

		/// <summary>
		/// Resolved SiteName from current IIS Site name.
		/// </summary>
		public static SiteNameEnum CurrentSiteName => (_siteCurrentName ?? (_siteCurrentName = ResolveCurrentSiteName())).Value;

		#endregion

		#region SiteSetting resolver

		static Dictionary<SiteNameEnum, SiteSetting> _allSites;

		private static SiteSetting GetSiteSettingImpl(Dictionary<SiteNameEnum, SiteSetting> dicSource, SiteNameEnum siteName)
		{
			if (dicSource.TryGetValue(siteName, out SiteSetting item))
				return item;
			else
				throw new KeyNotFoundException(siteName + "");
		}

		public static SiteSetting GetSiteSetting(SiteNameEnum siteName) => GetSiteSettingImpl(_allSites, siteName);

		public static SiteSetting CurrentSiteSetting => GetSiteSettingImpl(_allSites, SiteHelper.CurrentSiteName);

		#endregion

		#region predefined settings

		static SiteHelper()
		{
			// SiteSetting based on site name
			_allSites = ConfigurationsCollection.GetPredefinedSettings().ToDictionary(o => o.SiteName);
		}

		#endregion

		#region InitializeEnvironment

		/// <summary>
		/// Set specific site to be used for the whole app domain
		/// </summary>
		/// <param name="site"></param>
		public static void InitializeEnvironment(SiteNameEnum site)
		{
			_siteCurrentName = site;

			Environment.SetEnvironmentVariable(CURRENT_SITE_NAME, site.ToString());
		}

		#endregion

		#region using site name methods

		/// <summary>
		/// Samo za UnitTest, not thread safe.
		/// </summary>
		/// <param name="site"></param>
		/// <param name="action"></param>
		public static void UsingSiteName(SiteNameEnum site, Action action)
		{
			if (!GlobalSetting.UnitTest.IsUnitTest)
				throw new InvalidOperationException("can not be used without GlobalSetting.UnitTest.IsUnitTest == true");

			SiteNameEnum? _current = _siteCurrentName;
			_siteCurrentName = site; // use new site
			try
			{
				action.Invoke();
				_siteCurrentName = _current; // restore previous
			}
			catch
			{
				_siteCurrentName = _current; // restore previous;
				throw;
			}
		}

		#endregion

	}
}
