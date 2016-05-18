using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace DevLunch.Data.Models
{
    public class Vote
    {
        [Key]
        public int ID { get; set; }

        public Lunch Lunch { get; set; }

        public Restaurant Restaurant { get; set; }

        public string UserName { get; set; }

        public VoteType VoteType { get; set; }

        public int Value { get; set; }
    }

    public enum VoteType
    {
        Upvote = 1,
        Downvote = 2
    }
}
