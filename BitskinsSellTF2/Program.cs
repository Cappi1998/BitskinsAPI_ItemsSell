using OtpSharp;
using System;
using Albireo.Base32;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using BitskinsAPI_ItemsSell.Models;
using System.Linq;
using System.Threading;

namespace BitskinsAPI_ItemsSell
{
    class Program
    {
        public static string Secret = "";
        public static string ApiKey = "";
        public static string AppID = "";

        public static string ItemNameToSell = "";
        public static decimal PricetoSell = 1000;
        public static int qtn = 1;

        public static bool Run = true;
        public static bool RunBeep = true;
        static void Main(string[] args)
        {

            #region ConsoleInputs

            StartPOINT:
            Console.Title = "Bitskins Sell Itens";

            try
            {
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter Secret 2FA from Bitskins:");
                Console.ForegroundColor = ConsoleColor.White;
                Secret = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter Api Key from Bitskins:");
                Console.ForegroundColor = ConsoleColor.White;
                ApiKey = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter AppID From Steam Inventory:");
                Console.ForegroundColor = ConsoleColor.White;
                AppID = Console.ReadLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter market_hash_name From Item to Sell:");
                Console.ForegroundColor = ConsoleColor.White;
                ItemNameToSell = Console.ReadLine();

                PRICEAGAIN:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter Price to Sell Item:");
                Console.ForegroundColor = ConsoleColor.White;
                PricetoSell = Convert.ToDecimal(Console.ReadLine());


                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("The specified price is"); Console.ForegroundColor = ConsoleColor.Green; Console.Write($" ${PricetoSell} "); Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine("is this value correct? (y/n)?");
                Console.ForegroundColor = ConsoleColor.White;
                string checkvalue = Console.ReadLine();
                if (checkvalue != "y")
                {
                    Console.Clear();
                    goto PRICEAGAIN;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Quantity of items for sale simultaneously:");
                Console.ForegroundColor = ConsoleColor.White;
                qtn = Convert.ToInt32(Console.ReadLine());

                Console.Clear();
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                goto StartPOINT;
            }
            #endregion


            GetWalletValue();

            TryAgain:
            try
            {

                while (Run)
                {
                    InventoryResponse.Root inventory = LoadSteamInventory();

                    foreach (var item in inventory.data.steam_inventory.items)
                    {
                        if (item.market_hash_name == ItemNameToSell)
                        {
                            SellItens(item.asset_ids);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                goto TryAgain;
            }

            Console.ReadLine();
        }

        public static string Get2FAToken()
        {
            var totpgen = new Totp(Base32.Decode(Secret));
            var code = totpgen.ComputeTotp();
            Console.Write("Generate New 2FA Code:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{code}");
            Console.ForegroundColor = ConsoleColor.White;
            return code;
        }

        public static void SellItens(List<string> AssetsID)
        {
            if(AssetsID.Count > qtn)
            {
                AssetsID = AssetsID.Take(qtn).ToList();
            }

            List<string> PriceList = new List<string>();
            while(PriceList.Count < AssetsID.Count)
            {
                PriceList.Add(PricetoSell.ToString().Replace(",","."));
            }

            string ITEM_IDS = String.Join(",", AssetsID.ToArray());
            string PRICES = String.Join(",", PriceList.ToArray());

            var RequestURL = $"https://bitskins.com/api/v1/list_item_for_sale/?api_key={ApiKey}&item_ids={ITEM_IDS}&prices={PRICES}&code={Get2FAToken()}&app_id={AppID}";
            var rest = new RequestBuilder(RequestURL).POST().Execute();


            SellItensResponse.Root response = JsonConvert.DeserializeObject<SellItensResponse.Root>(rest.Content);

            if(response.status == "success")
            {
                RunBeep = true;
                var t = new Thread(() => Beep());
                t.Start();

                Console.WriteLine($"Ad successfully created!, Bot Name: {response.data.bot_info.name_prefix} TradeToken: {response.data.trade_tokens[0]}");
                Console.WriteLine("Press any key to resume the process ....");
                Console.ReadKey();
                RunBeep = false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(response.data.error_message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static InventoryResponse.Root LoadSteamInventory()
        {
            var rest = new RequestBuilder($"https://bitskins.com/api/v1/get_my_inventory/?api_key={ApiKey}&code={Get2FAToken()}&app_id={AppID}").GET().Execute();
            InventoryResponse.Root inventory = JsonConvert.DeserializeObject<InventoryResponse.Root>(rest.Content);

            Console.Write("Total Steam Iventory Itens:");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{inventory.data.steam_inventory.total_items}");
            Console.ForegroundColor = ConsoleColor.White;

            return inventory;
        }

        public static void GetWalletValue()
        {
            var rest = new RequestBuilder($"https://bitskins.com/api/v1/get_account_balance/?api_key={ApiKey}&code={Get2FAToken()}").GET().Execute();
            WalletResponse response = JsonConvert.DeserializeObject<WalletResponse>(rest.Content);

            if(response.status == "success")
            {
                string wallet = response.data.available_balance.ToString("N2");
                Console.Write($"Bitskins Wallet:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"${wallet}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void Beep()
        {
            while (RunBeep)
            {
                Console.Beep();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
