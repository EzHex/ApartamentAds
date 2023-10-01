namespace BackendAPI.Controllers;

public class RoomController
{
    private readonly ApartmentAdsDbContext _context;

    public RoomController(ApartmentAdsDbContext context)
    {
        this._context = context;
    }
    
    
}