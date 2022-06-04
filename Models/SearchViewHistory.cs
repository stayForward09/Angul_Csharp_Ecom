namespace StackApi.Models;

public class SearchViewHistory
{
    public Guid SvId { get; set; }
    public string searchterm { get; set; }
    public string visitedPrd { get; set; }
    public string UsID { get; set; }
    public DateTime SerachedOn { get; set; }
}