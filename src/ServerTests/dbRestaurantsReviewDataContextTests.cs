using Microsoft.EntityFrameworkCore;
using RestaurantsReview.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantsReview.ServerTests
{
	public partial class dbRestaurantsReviewDataContextTests : dbRestaurantsReviewDataContext
	{
		public dbRestaurantsReviewDataContextTests() 
			: base(c=>c.UseSqlServer("Server=.\\mssql2019;Database=dbRestaurantsReview;Trusted_Connection=True;MultipleActiveResultSets=true"))
		{

		}
	}
}
