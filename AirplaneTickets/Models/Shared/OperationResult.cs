namespace AirplaneTickets.Models.Shared
{
    public class OperationResult
    {
        public bool Succeeded { get; }
        public string[] Errors { get; }

        private OperationResult(bool succeeded, string[] errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public static OperationResult Success()
        {
            return new OperationResult(true, Array.Empty<string>());
        }

        public static OperationResult Failure(params string[] errors)
        {
            return new OperationResult(false, errors);
        }
    }
}
