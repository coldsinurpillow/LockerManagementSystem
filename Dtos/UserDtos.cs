namespace LockerManagementSystem.Dtos;

// ----- Запросы -----
public record CreateUserDto(
    string FirstName,
    string MiddleName,
    string LastName,
    string Group,
    string BarCode,
    string Iin
);

public record UpdateUserDto(
    string FirstName,
    string MiddleName,
    string LastName,
    string Group,
    string BarCode,
    string Iin
);

// ----- Ответы -----
public record UserResponseDto(
    int Id,
    string FullName,
    string Group,
    string BarCode,
    string? LockerNumber,
    int? LockerPlace
);
