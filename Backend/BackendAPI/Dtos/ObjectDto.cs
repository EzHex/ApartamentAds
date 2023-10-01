namespace BackendAPI.Dtos;

public record UpdateObjectDto(string Description, string Image, double Grade);
public record ObjectDto(int Id, string Name, string Description, string Image, double Grade);
public record CreateObjectDto(string Name, string Description, string Image, double Grade);