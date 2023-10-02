namespace BackendAPI.Dtos;

public record RoomDto(int Id, string Name, double Grade);
public record CreateRoomDto(string Name, double Grade);