using System;

namespace ZEMS.Data.Models
{
    public class ZEMSApiClient : BaseEntity
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
