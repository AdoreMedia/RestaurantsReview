using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantsReview.Database;
using RestaurantsReview.Server.Controllers;
using RestaurantsReview.ServerTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsReview.Server.Controllers.Tests
{
	[TestClass()]
	public class RestaurantsReviewControllerTests
	{
		[TestMethod()]
		public void GetRolesTest()
		{
			using (var context = new dbRestaurantsReviewDataContextTests())
			{
				RestaurantsReviewController controller = new RestaurantsReviewController(null, context);
				var list1 = controller.GetRoles().ToList();
				Assert.AreEqual(3, list1.Count);
			}
		}

		[TestMethod()]
		public void GetMembersWithRolesTest()
		{
			using (var context = new dbRestaurantsReviewDataContextTests())
			{
				RestaurantsReviewController controller = new RestaurantsReviewController(null, context);
				var list1 = controller.GetMembersWithRoles(true).ToList();
				Assert.IsTrue(list1.Count > 0);
			}
		}

		[TestMethod]
		public void GetRestaurantsForReviewTest()
		{
			using (var context = new dbRestaurantsReviewDataContextTests())
			{
				RestaurantsReviewController controller = new RestaurantsReviewController(null, context);

				var list1 = controller.GetRestaurantsWorker(new RestaurantsReviewController.RestaurantsQuerySetting()).ToList();
				Assert.IsTrue(list1.Count > 0);
			}
		}
	}
}