namespace BookApi.Exceptions
{
    public class CannotChangeYourOwnRoleException : Exception
    {
        public CannotChangeYourOwnRoleException(string message) 
            : base(message) { }
    }
}
