namespace E_Commerce.Application.Common
{
    public record Error(string Code, string Description, ErrorType ErrorType = ErrorType.Failure)
    {
        public static Error Failure(string code = "General.Failure", string description = "General.Failure has occurred") => new(code, description, ErrorType.Failure);
        public static Error Validation(string code = "General.Validation", string description = "Validation.Failure has occurred") => new(code, description, ErrorType.Validation);
        public static Error NotFound(string code = "General.NotFound", string description = "NotFound.Failure has occurred") => new(code, description, ErrorType.NotFound);
        public static Error Conflict(string code = "General.Conflict", string description = "NotFound.Conflict has occurred") => new(code, description, ErrorType.Conflict);
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "NotFound.Unauthorized has occurred") => new(code, description, ErrorType.Unauthorized);
        public static Error Forbidden(string code = "General.Forbidden", string description = "NotFound.Forbidden has occurred") => new(code, description, ErrorType.Forbidden);
        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "NotFound.InvalidCredentials has occurred") => new(code, description, ErrorType.InvalidCredentials);
    }

    public enum ErrorType 
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        InvalidCredentials = 6,
    }
}