﻿namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;


public class CourseSaveRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsOnline { get; set; }
    public IFormFile? File { get; set; }
}


public class CourseResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsOnline { get; set; }
    public string ImageUrl { get; set; }
}

public class CourseWithCommentResponse:CourseResponse
{
    public List<CommentResponse> Comments { get; set; } = [];
}