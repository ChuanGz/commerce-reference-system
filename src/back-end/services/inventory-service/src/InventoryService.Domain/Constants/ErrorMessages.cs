namespace InventoryService.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string InsufficientInventory = "Insufficient inventory to reserve";
        public const string InsufficientReservedQuantity =
            "Insufficient reserved quantity to release";
        public const string ValidationFailed = "Validation failed";
        public const string UnhandledException = "An error occurred while processing your request";
        public const string IdMismatch = "ID mismatch";
    }
}
