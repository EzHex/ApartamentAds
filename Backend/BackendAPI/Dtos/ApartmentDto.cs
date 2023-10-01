namespace BackendAPI.Dtos;

public record ApartmentDto(int Id, string Address, int Floor, int Number, double Area, double Rating);
public record CreateApartmentDto(string Address, int Floor, int Number, double Area, double Rating);
public record UpdateApartmentDto(double Rating);
