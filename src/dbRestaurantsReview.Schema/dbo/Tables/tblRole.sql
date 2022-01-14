CREATE TABLE [dbo].[tblRole] (
    [RoleID]                          INT          IDENTITY (1, 1) NOT NULL,
    [RoleName]                        VARCHAR (50) NOT NULL,
    [CanReadReviews]                  BIT          CONSTRAINT [DF_tblRole_CanReadReviews] DEFAULT ((0)) NOT NULL,
    [CanWriteReview]                  BIT          NOT NULL,
    [CanCreateRestaurant]             BIT          NOT NULL,
    [CanEditRestaurant]               BIT          NOT NULL,
    [CanReplayToRestaurantReview]     BIT          NOT NULL,
    [CanEditMember]                   BIT          NOT NULL,
    [CanEditRestaurantReview]         BIT          NOT NULL,
    [CanEditReplayToRestaurantReview] BIT          NOT NULL,
    [CanEditRole]                     BIT          CONSTRAINT [DF_tblRole_CanEditRole] DEFAULT ((0)) NOT NULL,
    [CanListOwnRestaurants]           BIT          CONSTRAINT [DF_tblRole_CanListOwnRestaurants] DEFAULT ((0)) NOT NULL,
    [CanListAllRestaurants]           BIT          CONSTRAINT [DF_tblRole_CanListAllRestaurants] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_tblRole] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);



