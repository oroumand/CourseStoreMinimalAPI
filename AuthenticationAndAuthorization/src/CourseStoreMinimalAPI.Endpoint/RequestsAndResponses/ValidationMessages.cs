namespace CourseStoreMinimalAPI.Endpoint.RequestsAndResponses;

public static class ValidationMessages
{
    public static string REQUIRED = "برای {PropertyName} مقدار لازم است.";
    public static string MAX_LENGHT = "برای {PropertyName} حداکثر 50 کاراکتر مجاز است.";
    public static string MIN_LENGHT = "برای {PropertyName} حداقل 2 کاراکتر مجاز است.";
    public static string REPEATED_VALUE = "برای {PropertyName} قبلا مقدار ثبت شده و مقدار تکراری قابل قبول نیست.";
}


public static class PropertyName
{
    public static string FirstName = "نام";
    public static string LastName = "فامیلی";
    public static string Name = "نام";
}