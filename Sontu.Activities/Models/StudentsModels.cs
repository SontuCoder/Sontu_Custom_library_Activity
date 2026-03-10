using System;
using Newtonsoft.Json;

namespace Sontu.Activities.Models
{

    public class StudentRequest
    {
        [JsonProperty("_id")]
        public string RequestId { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("book_id")]
        public string BookId { get; set; }
        [JsonProperty("issue_date")]
        public DateTime? IssuedDate { get; set; }
        [JsonProperty("return_date")]
        public DateTime? ReturnDate { get; set; }
        [JsonProperty("request_date")]
        public DateTime? RequestDate { get; set; }
        [JsonProperty("status")]
        public string BookStatus { get; set; }
        [JsonProperty("previous_status")]
        public string PreviousStatus { get; set; }
        [JsonProperty("book_name")]
        public string BookName { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("edition")]
        public int Edition { get; set; }
    }

    public class ListOfStudentRequests
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public List<StudentRequest> ListOfRequests { get; set; }
    }


    // ================== Common Models =================


}

