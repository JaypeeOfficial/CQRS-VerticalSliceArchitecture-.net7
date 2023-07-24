using CQRSample.Common;

namespace CQRSample.Domain
{
    public class UserRole : BaseEntity
    {
        public string RoleCode  { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateAdded { get; set; }  = DateTime.Now;

    }
}
