namespace IdentityService.API.Middlewares;

public class ValidationErrorResponse
{
    public string Message { get; set; } = "Validation failed";
    public List<FieldError> Errors { get; set; } = [];
}

public class FieldError
{
    public required string Field { get; set; }
    public required string Error { get; set; }
}
