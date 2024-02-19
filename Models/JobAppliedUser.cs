

namespace DotNetAPI
{
    public  partial class JobAppliedUser
    {
        public int JobId { get; set; }

        public string? Title { get; set; }
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Gender { get; set; }

        public bool Role { get; set; }

    }
}