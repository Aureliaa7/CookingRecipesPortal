namespace CookingRecipesPortal_DAL.DomainModels
{
    public class FollowerFollowee
    {
        public Guid Id { get; set; }

        public Guid FollowerId { get; set; }

        public User Follower { get; set; }

        public Guid FolloweeId { get; set; }

        public User Followee { get; set; }
    }
}
