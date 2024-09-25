using System.ComponentModel.DataAnnotations;

namespace UrlShorter.Models
{
    public class UrlEntry
    {
        [Key]
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
