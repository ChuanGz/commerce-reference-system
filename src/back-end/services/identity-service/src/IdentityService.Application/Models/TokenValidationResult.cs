public class TokenValidationResult {
    public bool IsValid { get; set; }
    public string? UserId { get; set; }
    public IEnumerable<string> Permissions { get; set; } = [];
}
