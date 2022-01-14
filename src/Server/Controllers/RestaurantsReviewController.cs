using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantsReview.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Calysto.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RestaurantsReview.Server.Authentication;

namespace RestaurantsReview.Server.Controllers
{
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	[ApiController]
	[Route("api")]
	public class RestaurantsReviewController : ControllerBase
	{
		private readonly dbRestaurantsReviewDataContext _dbcontext;
		private readonly ILogger<RestaurantsReviewController> _logger;

		public RestaurantsReviewController(ILogger<RestaurantsReviewController> logger, dbRestaurantsReviewDataContext dbcontext)
		{
			_logger = logger;
			_dbcontext = dbcontext;
		}

		private bool TryGetCurrentMemberId(out int memberId)
		{
			memberId = -1;
			return (this.User?.Identity?.IsAuthenticated == true && int.TryParse(this.User.FindFirst(nameof(tblMember.MemberID))?.Value, out memberId));
		}

		#region register / login member

		public class MessageItem
		{
			public bool IsError { get; set; }
			public string Message { get; set; }
		}

		[AllowAnonymous]
		[Route(nameof(RegisterUser))]
		[HttpPost]
		public MessageItem RegisterUser(string firstName, string lastName, string email, string username, string password)
		{
			List<string> errors = new List<string>();

			if (!(firstName?.Length >= 3))
				errors.Add("First name length should be at least 3 chars.");

			if (!(lastName?.Length >= 3))
				errors.Add("Last name length should be at least 3 chars.");

			if (!(username?.Length >= 3))
				errors.Add("Username length should be at least 3 chars.");

			if (!(password?.Length >= 3))
				errors.Add("Password length should be at least 3 chars.");

			if (!(email?.Contains("@") == true))
				errors.Add("E-mail address is not in valid format.");

			if (errors.Any())
			{
				return new MessageItem() { IsError = true, Message = string.Join("\r\n", errors) };
			}

			var existingUser = _dbcontext.tblMember.Where(o => o.Username == username).FirstOrDefault();
			if (existingUser != null)
			{
				return new MessageItem() { IsError = true, Message = "Username already registered." };
			}

			tblMember member = new tblMember()
			{
				CreationDate = DateTime.Now,
				Email = email,
				Username = username,
				Password = password,
				FirstName = firstName,
				LastName = lastName
			};

			member.tblMemberRoleList.Add(new tblMemberRole()
			{
				RoleID = 1 // user
			});

			_dbcontext.tblMember.Add(member);
			_dbcontext.SaveChanges();

			return new MessageItem() { IsError = false, Message = "Registration successful." };
		}

		public class LoginUserResult : MessageItem
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}

		[AllowAnonymous]
		[Route(nameof(LoginUser))]
		[HttpPost]
		public LoginUserResult LoginUser(string username, string password)
		{
			var existingUser = string.IsNullOrWhiteSpace(username) ? null : _dbcontext.tblMember.Where(o => o.Username == username).FirstOrDefault();
			if (existingUser != null && existingUser.Password == password)
			{
				existingUser.LastLoginDate = DateTime.Now;
				existingUser.LoginsCount++;
				_dbcontext.SaveChanges();

				return new LoginUserResult()
				{
					Message = "Login successful.",
					FirstName = existingUser.FirstName,
					LastName = existingUser.LastName
				};
			}
			else
			{
				return new LoginUserResult()
				{
					IsError = true,
					Message = "Invalid account data!"
				};
			}
		}

		[CalystoAuthorize]
		[Route(nameof(GetCurrentUserPermissions))]
		[HttpPost]
		public IEnumerable<tblRole> GetCurrentUserPermissions()
		{
			return _dbcontext.tblMember.Where(o => o.Username == this.User.Identity.Name)
				.SelectMany(o => o.tblMemberRoleList)
				.Select(o => o.tblRole);
		}

		#endregion

		#region roles / groups administration

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRole))]
		[Route(nameof(GetRoles))]
		[HttpGet]
		public IEnumerable<tblRole> GetRoles()
		{
			return _dbcontext.tblRole;
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRole))]
		[Route(nameof(GetRoleAsync))]
		[HttpGet]
		public tblRole GetRoleAsync(int roleId)
		{
			return _dbcontext.tblRole.Where(o => o.RoleID == roleId).FirstOrDefault();
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRole))]
		[Route(nameof(SaveRoleAsync))]
		[HttpGet]
		public bool SaveRoleAsync(string roleJson)
		{
			tblRole role = System.Text.Json.JsonSerializer.Deserialize<tblRole>(roleJson);
			if (role.RoleID > 0)
				_dbcontext.tblRole.Update(role);
			else
				_dbcontext.tblRole.Add(role);

			_dbcontext.SaveChanges();

			return true;
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRole))]
		[Route(nameof(DeleteRoleAsync))]
		[HttpGet]
		public bool DeleteRoleAsync(int roleId)
		{
			_dbcontext.tblRole.Where(o => o.RoleID == roleId).DeleteDirect();
			return true;
		}

		#endregion

		#region members administration

		public class RoleItem
		{
			public int RoleID { get; set; }
			public string RoleName { get; set; }
			public bool IsAssigned { get; set; }
		}

		public class MembersResult
		{
			public int MemberID { get; set; }
			public string Email { get; set; }
			public string Username { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
			public List<RoleItem> Roles { get; set; }
			public int LoginsCount { get; set; }
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditMember))]
		[Route(nameof(GetMembersWithRoles))]
		[HttpGet]
		public IEnumerable<MembersResult> GetMembersWithRoles(bool sortDesc = false, int skip = 0, int take = 10)
		{
			var q1 = _dbcontext.tblMember.Include(p => p.tblMemberRoleList).ThenInclude(p => p.tblRole);

			var q2 = sortDesc ? q1.OrderByDescending(p => p.MemberID) : q1.OrderBy(p => p.MemberID);

			return q2.Skip(skip).Take(take).ToList().Select(o => new MembersResult()
			{
				MemberID = o.MemberID,
				Email = o.Email,
				Username = o.Username,
				FirstName = o.FirstName,
				LastName = o.LastName,
				LoginsCount = o.LoginsCount,
				Roles = o.tblMemberRoleList.Select(k => new RoleItem()
				{
					RoleID = k.RoleID,
					RoleName = k.tblRole.RoleName,
					IsAssigned = true
				}).ToList()
			});
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditMember))]
		[Route(nameof(GetMemberDetails))]
		[HttpGet]
		public MembersResult GetMemberDetails(int memberId)
		{
			var rolesAll = _dbcontext.tblRole.Select(o => new RoleItem()
			{
				RoleID = o.RoleID,
				RoleName = o.RoleName
			}).ToList();

			return _dbcontext.tblMember.Include(p => p.tblMemberRoleList).Where(o => o.MemberID == memberId)
				.ToList()
				.Select(o => new MembersResult()
				{
					MemberID = o.MemberID,
					Email = o.Email,
					Username = o.Username,
					FirstName = o.FirstName,
					LastName = o.LastName,
					LoginsCount = o.LoginsCount,
					Roles = rolesAll.Select(r =>
					{
						// mark roles assigned to member
						r.IsAssigned = o.tblMemberRoleList.Where(m => m.RoleID == r.RoleID).Any();
						return r;
					}).ToList()
				}).First();
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditMember))]
		[Route(nameof(DeleteMember))]
		[HttpGet]
		public bool DeleteMember(int memberId)
		{
			_dbcontext.tblMember.Where(o => o.MemberID == memberId).DeleteDirect();
			return true;
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditMember))]
		[Route(nameof(SaveMemberRoles))]
		[HttpGet]
		public bool SaveMemberRoles(int memberId, string assignedRolesCsv)
		{
			int[] assignedRoles = (assignedRolesCsv ?? "").Split(',').Where(o => !string.IsNullOrWhiteSpace(o)).Select(o => Convert.ToInt32(o)).ToArray();

			// remove which were assigned previously
			_dbcontext.tblMemberRole.Where(o => o.MemberID == memberId && !assignedRoles.Contains(o.RoleID)).DeleteDirect();

			// current
			var currentRoles = _dbcontext.tblMemberRole.Where(o => o.MemberID == memberId).Select(o => o.RoleID).ToList();

			// roles to be added
			var rolesToAdd = assignedRoles.Where(roleId => !currentRoles.Contains(roleId)).ToList();
			_dbcontext.AddRange(rolesToAdd.Select(roleId => new tblMemberRole()
			{
				MemberID = memberId,
				RoleID = roleId
			}));
			_dbcontext.SaveChanges();

			return true;
		}

		#endregion

		#region restaurants administration

		/// <summary>
		/// Only the owner can create his new restaurant.
		/// </summary>
		/// <param name="restaurantName"></param>
		/// <param name="restaurantAddress"></param>
		/// <param name="restaurantDescription"></param>
		/// <returns></returns>
		[CalystoAuthorize(Permission = nameof(tblRole.CanCreateRestaurant))]
		[Route(nameof(CreateNewRestaurant))]
		[HttpGet]
		public int CreateNewRestaurant(string restaurantName, string restaurantAddress, string restaurantDescription)
		{
			if (this.TryGetCurrentMemberId(out int memberId))
			{
				tblRestaurant restaurant = new tblRestaurant()
				{
					OwnerMemberID = memberId,
					CreationDate = DateTime.Now,
					Name = restaurantName,
					Address = restaurantAddress,
					Description = restaurantDescription
				};
				_dbcontext.tblRestaurant.Add(restaurant);
				_dbcontext.SaveChanges();

				return restaurant.RestaurantID;
			}
			else
			{
				// it should never come here
				throw new UnauthorizedAccessException();
			}
		}

		/// <summary>
		/// Admin can save all restaurants. Owner can save his restaurants only.
		/// </summary>
		/// <param name="restaurantId"></param>
		/// <param name="restaurantName"></param>
		/// <param name="restaurantAddress"></param>
		/// <param name="restaurantDescription"></param>
		/// <returns></returns>
		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRestaurant))]
		[Route(nameof(SaveRestaurant))]
		[HttpGet]
		public bool SaveRestaurant(int restaurantId, string restaurantName, string restaurantAddress, string restaurantDescription)
		{
			tblRestaurant res2 = _dbcontext.tblRestaurant.Where(o => o.RestaurantID == restaurantId).First();

			bool mayUpdate = false;

			if (this.User.IsInRole(RoleEnum.Owner.ToString()) 
				&& this.TryGetCurrentMemberId(out int memberId) 
				&& res2.OwnerMemberID == memberId)
			{
				// owner may update his own restaurant only
				mayUpdate = true;
			}
			else if (this.User.IsInRole(RoleEnum.Admin.ToString()))
			{
				// admin may update any restaurant
				mayUpdate = true;
			}

			if (mayUpdate)
			{
				res2.Name = restaurantName;
				res2.Address = restaurantAddress;
				res2.Description = restaurantDescription;
				_dbcontext.SaveChanges();
			}

			return mayUpdate;
		}

		/// <summary>
		/// Admin can delete all restaurants. Owner can delete his restaurants only.
		/// </summary>
		/// <param name="restaurantId"></param>
		/// <returns></returns>
		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRestaurant))]
		[Route(nameof(DeleteRestaurant))]
		[HttpGet]
		public bool DeleteRestaurant(int restaurantId)
		{
			tblRestaurant res2 = _dbcontext.tblRestaurant.Where(o => o.RestaurantID == restaurantId).First();

			bool mayDelete = false;

			if (this.User.IsInRole(RoleEnum.Owner.ToString())
				&& this.TryGetCurrentMemberId(out int memberId)
				&& res2.OwnerMemberID == memberId)
			{
				// owner may delete his own restaurant only
				mayDelete = true;
			}
			else if (this.User.IsInRole(RoleEnum.Admin.ToString()))
			{
				// admin may delete any restaurant
				mayDelete = true;
			}

			if (mayDelete)
			{
				_dbcontext.tblRestaurant.Remove(res2);
				_dbcontext.SaveChanges();
			}

			return mayDelete;
		}

		public class RestaurantExtendedResult
		{
			public int RestaurantID { get; set; }
			public string Name { get; set; }
			public string Address { get; set; }
			public string Description { get; set; }
			public decimal? AverageRating { get; set; }
			public int OwnerMemberID { get; set; }
			public string OwnerFullName { get; set; }
			public int ReviewsCount { get; set; }
			public int NewUnrepliedReviews { get; set; }
		}

		internal class RestaurantsQuerySetting
		{
			public int Skip = 0;
			public int Take = 100;
			public int? FilteredByMemberID;
			public bool IsRestaurantOwner;
			public int? FilteredByRestaurantID;
			public bool OrderByAverageRatingDesc;
		}

		private void ApplyFilterDependingOnRole(RestaurantsQuerySetting sett)
		{
			if (this.TryGetCurrentMemberId(out int memberId))
			{
				// owner will get his restaurants only
				// admin will get all restaurants
				if (sett.FilteredByMemberID == null)
				{
					var roles = _dbcontext.tblMemberRole.Where(o => o.MemberID == memberId)
						.Select(o => o.tblRole)
						.ToList();

					if (roles.Where(o => o.CanListAllRestaurants).Any())
					{
						// ok, may list all restaurants
					}
					else if (roles.Where(o => o.CanListOwnRestaurants).Any())
					{
						// may list own restaurants only
						sett.FilteredByMemberID = memberId;
						sett.IsRestaurantOwner = true;
					}
					else
					{
						// may not list any restaurant
						sett.FilteredByMemberID = -1;
					}
				}
			}
			else
			{
				// it should never come here
				throw new UnauthorizedAccessException();
			}
		}

		internal IEnumerable<RestaurantExtendedResult> GetRestaurantsWorker(RestaurantsQuerySetting sett)
		{
			this.ApplyFilterDependingOnRole(sett);

			var q1 = (IQueryable<tblRestaurant>)_dbcontext.tblRestaurant;
			if (sett.FilteredByMemberID > 0)
			{
				q1 = q1.Where(o => o.OwnerMemberID == sett.FilteredByMemberID);
			}
			if(sett.FilteredByRestaurantID > 0)
			{
				q1 = q1.Where(o => o.RestaurantID == sett.FilteredByRestaurantID);
			}

			return q1.Select(restaurant => new
			{
				Restaurant = restaurant,
				ReviewsCount = restaurant.tblReviewList.Count(),
				RatingSum = restaurant.tblReviewList.Sum(review => review.RatingStars),
				ReviewRepliesCount = restaurant.tblReviewList.Where(r => r.tblReviewReply != null).Count()
			})
			.Select(item => new RestaurantExtendedResult()
			{
				RestaurantID = item.Restaurant.RestaurantID,
				Name = item.Restaurant.Name,
				Address = item.Restaurant.Address,
				Description = item.Restaurant.Description,
				ReviewsCount= item.ReviewsCount,
				NewUnrepliedReviews = item.ReviewsCount - item.ReviewRepliesCount,
				AverageRating = item.ReviewsCount > 0 ? item.RatingSum / item.ReviewsCount : null,
				OwnerMemberID = item.Restaurant.OwnerMemberID,
				OwnerFullName = item.Restaurant.tblMember.FirstName + " " + item.Restaurant.tblMember.LastName,
			})
			.OrderByDescending(o=> sett.IsRestaurantOwner && o.NewUnrepliedReviews > 0 ? o.NewUnrepliedReviews : 0)
			.ThenByDescending(o => sett.OrderByAverageRatingDesc ? o.AverageRating : o.RestaurantID)
			.Skip(sett.Skip)
			.Take(sett.Take);
		}

		/// <summary>
		/// Admin can get all restaurants. Owner can get his restaurants only.
		/// </summary>
		/// <param name="restaurantId"></param>
		/// <returns></returns>
		[CalystoAuthorize(Permission = nameof(tblRole.CanListOwnRestaurants))]
		[CalystoAuthorize(Permission = nameof(tblRole.CanListAllRestaurants))]
		[Route(nameof(GetSingleRestaurant))]
		[HttpGet]
		public RestaurantExtendedResult GetSingleRestaurant(int restaurantId)
		{
			RestaurantsQuerySetting sett = new RestaurantsQuerySetting()
			{
				Skip = 0,
				Take = 1,
				FilteredByRestaurantID = restaurantId,
			};

			return this.GetRestaurantsWorker(sett).First();
		}

		/// <summary>
		/// Admin can get all restaurants. Owner can get his restaurants only.
		/// </summary>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <returns></returns>
		[CalystoAuthorize(Permission = nameof(tblRole.CanListOwnRestaurants))]
		[CalystoAuthorize(Permission = nameof(tblRole.CanListAllRestaurants))]
		[Route(nameof(GetRestaurants))]
		[HttpGet]
		public IEnumerable<RestaurantExtendedResult> GetRestaurants(int skip = 0, int take = 100)
		{
			return this.GetRestaurantsWorker(new RestaurantsQuerySetting()
			{
				Skip = skip,
				Take = take,
				OrderByAverageRatingDesc = true
			});
		}

		#endregion

		#region restaurant reviewing

		[CalystoAuthorize(Permission = nameof(tblRole.CanWriteReview))]
		[Route(nameof(AddRestaurantReview))]
		[HttpGet]
		public bool AddRestaurantReview(int restaurantId, decimal? rating, string comment)
		{
			if (rating < 0 || !rating.HasValue)
				rating = 0;

			if (rating > 5)
				rating = 5;

			if (this.TryGetCurrentMemberId(out int memberId))
			{
				tblReview review = new tblReview()
				{
					Comment = comment,
					CreationDate = DateTime.Now,
					RatingStars = rating,
					RestaurantID = restaurantId,
					ReviewerMemberID = memberId,
				};
				_dbcontext.tblReview.Add(review);
				_dbcontext.SaveChanges();
				return true;
			}
			else
			{
				// it should never come here
				throw new UnauthorizedAccessException();
			}
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRestaurantReview))]
		[Route(nameof(UpdateRestaurantReview))]
		[HttpGet]
		public bool UpdateRestaurantReview(int reviewId, decimal? rating, string comment)
		{
			if (rating < 0 || !rating.HasValue)
				rating = 0;

			if (rating > 5)
				rating = 5;

			tblReview review = _dbcontext.tblReview.Where(o => o.ReviewID == reviewId).First();
			review.RatingStars = rating;
			review.Comment = comment;
			_dbcontext.SaveChanges();
			return true;
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRestaurantReview))]
		[Route(nameof(DeleteRestaurantReview))]
		[HttpGet]
		public bool DeleteRestaurantReview(int reviewId)
		{
			_dbcontext.tblReview.Where(o => o.ReviewID == reviewId).DeleteDirect();
			return true;
		}

		public class ReviewerMember
		{
			public int MemberID { get; set; }
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}

		public class ReviewReplay
		{
			public ReviewerMember Replyer { get; set; }
			public DateTime CreationDate { get; set; }
			public string ReplyText { get; set; }
		}

		public class ReviewExtended
		{
			public int RestaurantID { get; set; }
			public int ReviewID { get; set; }
			public DateTime CreationDate { get; set; }
			public decimal? RatingStars { get; set; }
			public string Comment { get; set; }
			public ReviewerMember Reviever { get; set; }
			public ReviewReplay Replay { get; set; }
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanReadReviews))]
		[Route(nameof(GetRestaurantReviews))]
		[HttpGet]
		public IEnumerable<ReviewExtended> GetRestaurantReviews(int restaurantId)
		{
			return _dbcontext.tblReview.Where(o => o.RestaurantID == restaurantId)
				.Select(review => new
				{
					Review = review,
					ReviewerMember = review.tblMember,
					ReviewReplay = _dbcontext.tblReviewReply.Where(k=>k.ReviewID == review.ReviewID).Include(r=>r.tblMember).FirstOrDefault()
				}).Select(item => new ReviewExtended()
				{
					RestaurantID = item.Review.RestaurantID,
					ReviewID = item.Review.ReviewID,
					CreationDate = item.Review.CreationDate,
					RatingStars = item.Review.RatingStars,
					Comment = item.Review.Comment,
					Reviever = new ReviewerMember()
					{
						MemberID = item.ReviewerMember.MemberID,
						FirstName = item.ReviewerMember.FirstName,
						LastName = item.ReviewerMember.LastName
					},
					Replay = item.ReviewReplay == null ? null : new ReviewReplay()
					{
						Replyer = new ReviewerMember()
						{
							MemberID = item.ReviewReplay.tblMember.MemberID,
							FirstName = item.ReviewReplay.tblMember.FirstName,
							LastName = item.ReviewReplay.tblMember.LastName
						},
						CreationDate = item.ReviewReplay.CreationDate,
						ReplyText = item.ReviewReplay.ReplyText
					}
				}).OrderByDescending(o=>o.ReviewID);
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanReplayToRestaurantReview))]
		[Route(nameof(AddReviewReply))]
		[HttpGet]
		public bool AddReviewReply(int reviewId, string replyText)
		{
			if (this.TryGetCurrentMemberId(out int memberId))
			{
				// may reply to his restaurant reviews only
				tblRestaurant restaurant = _dbcontext.tblReview.Where(r => r.ReviewID == reviewId).Select(r => r.tblRestaurant).FirstOrDefault();
				if (restaurant.OwnerMemberID == memberId)
				{
					tblReviewReply rr = new tblReviewReply()
					{
						ReviewID = reviewId,
						CreationDate = DateTime.Now,
						ReplyedMemberID = memberId,
						ReplyText = replyText
					};
					_dbcontext.tblReviewReply.Add(rr);
					_dbcontext.SaveChanges();
					return true;
				}
			}
			// it should never come here
			throw new UnauthorizedAccessException();
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditReplayToRestaurantReview))]
		[Route(nameof(UpdateReviewReply))]
		[HttpGet]
		public bool UpdateReviewReply(int reviewId, string replyText)
		{
			tblReviewReply rr = _dbcontext.tblReviewReply.Where(o => o.ReviewID == reviewId).FirstOrDefault();
			rr.ReplyText = replyText;
			_dbcontext.SaveChanges();
			return true;
		}

		[CalystoAuthorize(Permission = nameof(tblRole.CanEditRestaurantReview))]
		[Route(nameof(DeleteReviewReply))]
		[HttpGet]
		public bool DeleteReviewReply(int reviewId)
		{
			_dbcontext.tblReviewReply.Where(o => o.ReviewID == reviewId).DeleteDirect();
			return true;
		}

		#endregion

	}
}
