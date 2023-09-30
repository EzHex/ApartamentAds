namespace BackendAPI.Controllers;

public class RoomController
{
    private readonly ApartamentAdsDbContext _context;

    public RoomController(ApartamentAdsDbContext context)
    {
        this._context = context;
    }
    
    
}