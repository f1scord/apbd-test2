namespace GroupD.Exceptions;

public class NotFoundException(string message = "Not found") : Exception(message);
