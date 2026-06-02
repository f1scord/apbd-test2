namespace ApbdTesta.Exceptions;

public class BadRequestException(string message = "Bad request") : Exception(message);
