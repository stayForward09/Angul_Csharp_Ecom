namespace StackApi.Models;

public class PartImages
{
    public Guid PiId { get; set; }
    public Guid Pid { get; set; }
    public string PiFilename { get; set; }
    public bool PiIsTD { get; set; }
    public DateTime ModifiedOn { get; set; }
    public Part parts { get; set; }
}