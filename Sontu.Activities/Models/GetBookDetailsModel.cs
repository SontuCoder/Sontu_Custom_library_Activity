using Newtonsoft.Json;

namespace Sontu.Activities.Models
{
    public class GetBookDetails
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("edition")]
        public int Edition { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("available")]
        public int Available { get; set; }

        [JsonProperty("category")]
        public List<string> Category { get; set; }

        [JsonProperty("added_at")]
        public DateTime AddedAt { get; set; }

    }

    public class GetBookResponse
    {
        [JsonProperty("data")]
        public GetBookDetails Data { get; set; }
    }

    public class FiltersData
    {
        [JsonProperty("category")]
        public List<string> Categories { get; set; }

        [JsonProperty("edition")]
        public int? Edition { get; set; }

        [JsonProperty("book_name")]
        public string BookName { get; set; }

        [JsonProperty("book_author")]
        public string BookAuthor { get; set; }
    }


    public class GetBooks
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("filters")]
        public FiltersData Filters { get; set; }

        [JsonProperty("nextCursor")]
        public string NextCursor { get; set; }

        [JsonProperty("hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty("total_books")]
        public int TotalBooks { get; set; }

        [JsonProperty("data")]
        public List<GetBookDetails> Data { get; set; }
    }
}
