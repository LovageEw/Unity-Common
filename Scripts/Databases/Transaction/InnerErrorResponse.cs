namespace Databases.Transaction
{
    public class InnerErrorResponse
    {
        public string errorType;
        public string errorMessage;

        public bool IsErrorState()
        {
            return !string.IsNullOrEmpty(errorType) && !string.IsNullOrEmpty(errorMessage);
        }

        public override string ToString()
        {
            return $"errorType : {errorType} \nerrorMessage : {errorMessage}";
        }
    }
}