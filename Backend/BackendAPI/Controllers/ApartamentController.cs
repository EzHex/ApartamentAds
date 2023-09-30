namespace BackendAPI.Controllers;

public class ApartamentController
{
    private readonly ApartamentAdsDbContext _context;

    public ApartamentController(ApartamentAdsDbContext context)
    {
        this._context = context;
    }
}