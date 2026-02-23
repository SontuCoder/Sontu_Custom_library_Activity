using Newtonsoft.Json;

namespace Sontu.Activities.Models
{
    public class IssuedBook
    {
        [JsonProperty("_id")]
        public string IssuedRecordId { get; set; }

        [JsonProperty("email")]
        public string StudentEmail { get; set; }

        [JsonProperty("book_id")]
        public string IssuedBookId { get; set; }

        [JsonProperty("issue_date")]
        public DateTime? IssuedDate { get; set; }

        [JsonProperty("return_date")]
        public DateTime? ReturnDate { get; set; }

        [JsonProperty("request_date")]
        public DateTime? RequestDate { get; set; }

        [JsonProperty("status")]
        public string IssuedStatus { get; set; }
    }

    public class Student
    {
        [JsonProperty("_id")]
        public string StudentId { get; set; }

        [JsonProperty("name")]
        public string StudentName { get; set; }

        [JsonProperty("email")]
        public string StudentEmail { get; set; }

        [JsonProperty("issued_books")]
        public List<IssuedBook> IssuedBooks { get; set; } = new ();
    }

    public class ListOfStudents
    {
        [JsonProperty("total_students")]
        public int TotalStudents { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("students")]
        public List<Student> Students { get; set; }
    }

}
