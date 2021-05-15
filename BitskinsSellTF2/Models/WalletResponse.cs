using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitskinsAPI_ItemsSell.Models
{
    // WalletResponse myDeserializedClass = JsonConvert.DeserializeObject<WalletResponse>(myJsonResponse); 
    public class Data
    {
        public decimal available_balance { get; set; }
        public decimal pending_withdrawals { get; set; }
        public decimal withdrawable_balance { get; set; }
        public decimal couponable_balance { get; set; }
    }

    public class WalletResponse
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
}
