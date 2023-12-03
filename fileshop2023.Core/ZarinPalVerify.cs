using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileshop2023.Core
{
    public class ZarinPalVerify
    {
        public Root VertifyZarinPal(string authority, int amount, string merchantId)
        {
            amount = amount * 10;
            var client = new RestClient($"https://api.zarinpal.com/pg/v4/payment/verify.json?merchant_id={merchantId}&amount={amount}&authority={authority}");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);
            return myDeserializedClass;
        }
        public class Data
        {
            public int code { get; set; }
            public string message { get; set; }
            public string card_hash { get; set; }
            public string card_pan { get; set; }
            public long ref_id { get; set; }
            public string fee_type { get; set; }
            public int fee { get; set; }
        }
        public class Root
        {
            public Data data { get; set; }
            public List<object> errors { get; set; }
        }
    }
}
