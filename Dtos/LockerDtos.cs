namespace LockerManagementSystem.Dtos;
// запросы
public record AddLockerDto(string Number, int PlaceCount, string Type);
public record AssignDto(string Number, int? Place, int UserId);
public record RemoveAssignedDto(string Number, int Place);
//распределение N пользователей
public record DistributeDto(int Count);

// ответы
public record LockerInfoResponse(
    string Number,
    IEnumerable<LockerPlaceInfo> Places
);

public record LockerPlaceInfo(
    int Place,
    int? UserId,
    string? FullName,
    string? Group,
    string? BarCode
);