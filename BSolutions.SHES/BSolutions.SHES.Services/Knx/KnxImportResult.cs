namespace BSolutions.SHES.Services.Knx
{
    public sealed class KnxImportResult
    {
        public bool Successful { get; set; } = true;

        public string ErrorMessage { get; set; }

        public KnxImportData Data { get; set; } = new KnxImportData();
    }
}
