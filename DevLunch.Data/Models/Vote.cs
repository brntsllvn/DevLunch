using System.ComponentModel.DataAnnotations;

namespace DevLunch.Data.Models
{
    public class Vote
    {
        [Key]
        public int ID { get; set; }

        public Lunch Lunch { get; set; }

        public Restaurant Restaurant { get; set; }

        public int Value { get; set; }
    }
}
