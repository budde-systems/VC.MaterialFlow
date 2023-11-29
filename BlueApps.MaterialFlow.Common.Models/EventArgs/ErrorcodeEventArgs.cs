namespace BlueApps.MaterialFlow.Common.Models.EventArgs
{
    public class ErrorcodeEventArgs : System.EventArgs
    {
        public ICollection<short>? Errorcodes { get; set; }
    }
}
