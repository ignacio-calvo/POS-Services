namespace POS.Stores.Business.CustomExceptions
{
    public class UniqueConstraintViolationException : Exception
    {
        public string FieldName { get; }

        public UniqueConstraintViolationException(string fieldName, string message) : base(message)
        {
            FieldName = fieldName;
        }

        public UniqueConstraintViolationException(string fieldName, string message, Exception innerException) : base(message, innerException)
        {
            FieldName = fieldName;
        }
    }
}
