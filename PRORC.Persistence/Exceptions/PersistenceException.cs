namespace PRORC.Persistence.Exceptions
{
    // Excepción personalizada para encapsular errores dentro de persistence
    public class PersistenceException : Exception
    {
        // Constructor que recibe un mensaje y la excepción que causó el error
        public PersistenceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
