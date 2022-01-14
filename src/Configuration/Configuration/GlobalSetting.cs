using Calysto.Utility;

namespace RestaurantsReview.Configuration
{
	public class UnitTestSetting
	{
		public bool IsUnitTest => CalystoTools.IsUnitTest;

		/// <summary>
		/// Set IsUnitTest = true and initialize database connections for eRegistar
		/// </summary>
		public void PrepareUnitTestEnvironment(SiteNameEnum site = SiteNameEnum.RestaurantsReview)
		{
			SiteHelper.InitializeEnvironment(site);
		}
	}

	public class GlobalSetting
	{
		public static UnitTestSetting UnitTest { get; } = new UnitTestSetting();
	}
}
