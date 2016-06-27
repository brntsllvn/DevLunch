namespace DevLunch.ViewModels
{
    public class VoteViewModel
    {
        public int? OldLunchRestaurantId { get; set; }
        public int? OldLunchRestaurantVoteTotal { get; set; }
        public int NewLunchRestaurantId { get; set; }
        public int NewLunchRestaurantVoteTotal { get; set; }
    }
}