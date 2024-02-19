namespace DotNetAPI.Dto
{
    public partial class JobToEditDto
    {
        public int JobId{set; get;}
        public string? Title{set; get;}
        public string? DESCRIPTION{set; get;}
        public string? Requirements{set; get;}
    }
}