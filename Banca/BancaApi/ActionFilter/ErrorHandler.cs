namespace BancaApi.ActionFilter
{
    public class ErrorHandler
    {
        public Error Error { get; set; }

        public ErrorHandler(string code, string message, string target, Exception? exception = null)
        {
            this.Error = new Error
            {
                Code = code,
                Message = message,
                Target = target,
                InnerError = PopulateInnerError(exception)
            };
        }

        public ErrorHandler(string code, string message, string target, List<Detail> details)
        {
            this.Error = new Error
            {
                Code = code,
                Details = details,
                Message = message,
                Target = target
            };
        }

        private InnerErrorDetail? PopulateInnerError(Exception? exception)
        {
            if (exception?.InnerException == null) return null;

            var innerException = new InnerErrorDetail
            {
                Message = exception.InnerException.Message,
                Target = exception.InnerException?.TargetSite?.Name,
                InnerError = PopulateInnerError(exception.InnerException)
            };

            return innerException;
        }
    }

    public class Error
    {
        public string Code { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Target { get; set; } = null!;
        public List<Detail>? Details { get; set; }
        public InnerErrorDetail? InnerError { get; set; }
    }

    public class Detail
    {
        public string? Code { get; set; }
        public string? Target { get; set; }
        public string? Message { get; set; }
    }

    public class InnerErrorDetail
    {
        public string? Message { get; set; }
        public string? Target { get; set; }
        public InnerErrorDetail? InnerError { get; set; }
    }
}

