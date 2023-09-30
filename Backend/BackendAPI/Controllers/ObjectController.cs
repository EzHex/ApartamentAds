namespace BackendAPI.Controllers;

public class ObjectController
{
    private readonly ApartamentAdsDbContext _context;

    public ObjectController(ApartamentAdsDbContext context)
    {
        this._context = context;
    }
}