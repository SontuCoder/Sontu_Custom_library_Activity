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
        public int? TotalStudents { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("students")]
        public List<Student> Students { get; set; }
    }

    public class Books
    {

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

        [JsonProperty("book_name")]
        public string BookName { get; set; }

        [JsonProperty("author")]
        public string BookAuthor { get; set; }

        [JsonProperty("edition")]
        public int BookEdition { get; set; }

        [JsonProperty("category")]
        public List<string> Category { get; set; }
    }
    public class StudentDetails
    {
        [JsonProperty("stu_email")]
        public string StudentEmail { get; set; }

        [JsonProperty("stu_name")]
        public string? StudentName { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("books")]
        public List<Books>? BooksList { get; set; }
    }

    public class  StudentResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public StudentDetails Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class BookDetails
    {

        [JsonProperty("_id")]
        public string BookId { get; set; }

        [JsonProperty("author")]
        public string BookAuthor { get; set; }

        [JsonProperty("description")]
        public string BookDescription { get; set; }

        [JsonProperty("title")]
        public string BookTitle { get; set; }

        [JsonProperty("edition")]
        public int BookEdition { get; set; }

        [JsonProperty("quantity")]
        public int BookQuantity { get; set; }

        [JsonProperty("available")]
        public int BookAvailable { get; set; }

        [JsonProperty("category")]
        public List<string> Category { get; set; }

        [JsonProperty("added_at")]
        public DateTime BookAddedAt{ get; set; }
    }

    public class AddBookResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("book")]
        public BookDetails NewBook { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("book_id")]
        public string ExistingBookId { get; set; }

        [JsonProperty("book_qty")]
        public int? ExistingBookQuentity { get; set; }

        [JsonProperty("book_avi")]
        public int? ExistingBookAvi { get; set; }
    }

    public class DeleteBookResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("book_id")]
        public string ExistingBookId { get; set; }

        [JsonProperty("book_qty")]
        public int? ExistingBookQuentity { get; set; }

        [JsonProperty("book_avi")]
        public int? ExistingBookAvi { get; set; }
    }

    public class Reqbook
    {
        [JsonProperty("id")]
        public string IssueId { get; set; }

        [JsonProperty("student_email")]
        public string StudentEmail { get; set; }

        [JsonProperty("book_title")]
        public string BookTitle { get; set; }

        [JsonProperty("book_author")]
        public string BookAuthor { get; set; }

        [JsonProperty("issue_date")]
        public DateTime? IssuedDate { get; set; }

        [JsonProperty("return_date")]
        public DateTime? ReturnDate { get; set; }

        [JsonProperty("request_date")]
        public DateTime? RequestedDate { get; set; }

        [JsonProperty("status")]
        public string IssueStatus { get; set; }

    }

    public class ListOfRequestedBook
    {
        [JsonProperty("total_requests")]
        public int? TotalRequests { get; set; }

        [JsonProperty("requests")]
        public List<Reqbook>? Requests { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class IssuedBookDetails
    {
        [JsonProperty("id")]
        public string IssueId { get; set; }
        [JsonProperty("student_email")]
        public string StudentEmail { get; set; }
        [JsonProperty("book_title")]
        public string BookTitle { get; set; }
        [JsonProperty("book_id")]
        public string BookId { get; set; }
        [JsonProperty("book_author")]
        public string BookAuthor { get; set; }
        [JsonProperty("issue_date")]
        public DateTime? IssuedDate { get; set; }
        [JsonProperty("return_date")]
        public DateTime? RequestedDate { get; set; }
        [JsonProperty("status")]
        public string IssueStatus { get; set; }
    }

    public class ListOfIssuedBooks
    {
        [JsonProperty("total_issued_books")]
        public int? TotalIssuedBooks { get; set; }
        [JsonProperty("issued_books")]
        public List<IssuedBookDetails>? IssuedBooks { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }

    }

    public enum ApprovalAction
    {
        Approved,
        Rejected
    }

    public class Admin
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("provider")]
        public string Provider { get; set; }
        [JsonProperty("creaed_at")]
        public DateTime? CreatedAt { get; set; }
    }

    public class AllAdmins
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("admins")]
        public List<Admin>? Admins { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }


    //========================= Common ==========================================
    public class APIResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class ApiErrorResponse
    {
        public string detail { get; set; }
    }

}
