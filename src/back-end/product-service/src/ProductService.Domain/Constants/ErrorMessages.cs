namespace ProductService.Domain.Constants;

public static class ErrorMessages
{
    public const string ValidationFailed = "Validation failed";
    public const string UnhandledException = "An error occurred while processing your request";
    public const string IdMismatch = "ID mismatch";
    public const string ProductNotFound = "Product not found";
    public const string DuplicateProductSku = "Product with this SKU already exists";
}
