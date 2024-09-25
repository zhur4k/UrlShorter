namespace UrlShorter.Dto
{
    public class UrlEntryTableDto
    {
        public int Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public int ClickCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
