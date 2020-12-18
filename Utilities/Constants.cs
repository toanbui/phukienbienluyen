using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Constants
    {
        public const string CookieCart = "CookieCart";
        public const string LiveChatGuest = "GuestInfo";
        public enum RecordStatus
        {
            Draft = 1,
            Pending = 2,
            Published = 3,
            UnPublished = 4,
            Locked = 5,
        }
    }
}
