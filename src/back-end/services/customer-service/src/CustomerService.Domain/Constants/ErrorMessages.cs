namespace CustomerService.Domain.Constants {
    public static class ErrorMessages {
        public const string ValidationFailed = "Validation failed";
        public const string UnhandledException = "An error occurred while processing your request";
        public const string IdMismatch = "ID mismatch";
        public const string CustomerNotFound = "Customer not found";
        public const string DuplicateCustomerEmail = "Customer with this email already exists";
    }
}
