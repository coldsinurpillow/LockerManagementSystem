namespace LockerManagementSystem.Dtos;

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
// DTO для ответа с данными пользователя(GET)
public record UserResponseDto(
    int Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string Group,
    string BarCode,
    string Iin,
    int? ActiveLockerPlace, //№ места в шкафчике
    string? ActiveLocker    //№ шкафчика
);

