
using System.ComponentModel.DataAnnotations.Schema;

namespace DevCreedMoviesApi.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 
        public Byte Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string shit {  get; set; }
        public int mynumber { get; set; }     
        public string Name2 { get; set; }
        public string Name3 { get; set; }
    }


}
