using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitskinsAPI_ItemsSell.Models
{
    public class InventoryResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Tags
        {
            public string quality { get; set; }
            public string type { get; set; }
        }

        public class RecentSalesInfo
        {
            public string hours { get; set; }
            public string average_price { get; set; }
        }

        public class Item
        {
            public string app_id { get; set; }
            public string context_id { get; set; }
            public int number_of_items { get; set; }
            public List<string> item_ids { get; set; }
            public List<string> asset_ids { get; set; }
            public string class_id { get; set; }
            public string instance_id { get; set; }
            public string market_hash_name { get; set; }
            public string suggested_price { get; set; }
            public string item_type { get; set; }
            public object item_class { get; set; }
            public object item_rarity { get; set; }
            public object item_weapon { get; set; }
            public string item_quality { get; set; }
            public object item_itemset { get; set; }
            public string image { get; set; }
            public bool inspectable { get; set; }
            public string inspect_link { get; set; }
            public object phase { get; set; }
            public Tags tags { get; set; }
            public bool has_buy_orders { get; set; }
            public RecentSalesInfo recent_sales_info { get; set; }
            public object stickers { get; set; }
            public List<List<object>> fraud_warnings { get; set; }
            public bool is_listing_allowed { get; set; }
            public List<string> prices { get; set; }
            public List<bool> is_featured { get; set; }
            public List<object> float_values { get; set; }
            public List<int> created_at { get; set; }
            public List<int> updated_at { get; set; }
            public List<int> withdrawable_at { get; set; }
        }

        public class SteamInventory
        {
            public string status { get; set; }
            public string fresh_or_cached { get; set; }
            public int total_items { get; set; }
            public List<Item> items { get; set; }
        }

        public class BitskinsInventory
        {
            public string status { get; set; }
            public int total_items { get; set; }
            public string total_price { get; set; }
            public List<Item> items { get; set; }
            public int page { get; set; }
            public int items_per_page { get; set; }
        }

        public class PendingWithdrawalFromBitskins
        {
            public string status { get; set; }
            public int total_items { get; set; }
            public List<object> items { get; set; }
        }

        public class Data
        {
            public string app_id { get; set; }
            public string context_id { get; set; }
            public SteamInventory steam_inventory { get; set; }
            public BitskinsInventory bitskins_inventory { get; set; }
            public PendingWithdrawalFromBitskins pending_withdrawal_from_bitskins { get; set; }
        }

        public class Root
        {
            public string status { get; set; }
            public Data data { get; set; }
        }

    }
}
