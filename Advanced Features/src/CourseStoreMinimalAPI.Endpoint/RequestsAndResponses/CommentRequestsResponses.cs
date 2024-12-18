namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;


public class CommentRequest
{
    public string CommentBody { get; set; }
    public DateTime CommentDate { get; set; }
    public int CourseId { get; set; }
}
public class CommentResponse
{
    public int Id { get; set; }
    public string CommentBody { get; set; }
    public DateTime CommentDate { get; set; }
    public int CourseId { get; set; }
}