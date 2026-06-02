namespace ApbdTest.Exceptions;

public class BadRequestException(string message = "Bad request") : Exception(message);
