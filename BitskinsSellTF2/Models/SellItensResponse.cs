using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitskinsAPI_ItemsSell.Models
{
    class SellItensResponse
    {

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Item
        {
            public string app_id { get; set; }
            public string context_id { get; set; }
            public string item_id { get; set; }
            public string asset_id { get; set; }
            public string class_id { get; set; }
            public string instance_id { get; set; }
        }

        public class BotInfo
        {
            public string uid { get; set; }
            public string name_prefix { get; set; }
            public int joined_steam_at { get; set; }
        }

        public class Data
        {
            public string error_message { get; set; }
            public List<Item> items { get; set; }
            public List<string> trade_tokens { get; set; }
            public BotInfo bot_info { get; set; }
        }

        public class Root
        {
            public string status { get; set; }
            public Data data { get; set; }
        }
    }
}

