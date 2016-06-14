using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEngine
{
    public class Constants
    {
        public struct Enums
        {
            public enum ContactDetailType { Agency = 1 }
        }

        public struct StoredProcedures
        {
            public const string SaveAgency = "pSaveAgency";

            public const string DeleteAgency = "pDeleteAgency";

            public const string LoadAgencies = "pLoadAgencies";

            public const string LoadHistoryItems = "pLoadHistoryItems";

            public const string SaveHistoryItems = "pSaveHistoryItems";

            public const string DeleteContactDetail = "pDeleteContactDetail";

            public const string SaveContactDetail = "pSaveContactDetail";

            public const string LoadContactDetail = "pLoadContactDetail";

        }
    }
}
