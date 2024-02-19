namespace DotNetAPI.Models
{
    public partial class JobPost
    {
        public int JobId{set; get;}
        public int UserId{set; get;}
        public string? Title{set; get;}
        public string? DESCRIPTION{set; get;}
        public string? Requirements{set; get;}
        public DateTime PostedAt{set; get;}
        public DateTime UpdateAt{set; get;}
    }
}