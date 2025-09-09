namespace LockerManagementSystem.Dtos;

public record AddLockerDto(string Number, int PlaceCount, string Type);
public record AssignDto(string Number, int? Place, int UserId);
public record RemoveAssignedDto(string Number, int Place);
//распределение N пользователей
public record DistributeDto(int Count);