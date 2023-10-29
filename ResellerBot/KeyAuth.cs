using Newtonsoft.Json;
using ResellerBot.Modules;

namespace ResellerBot; 

public class KeyAuth {
    private static string BaseUrl = "https://keyauth.win/api/seller/?sellerkey=";
    private static HttpClient _client = new();
    
    public static string? GenerateLicense(string sellerKey, KeyExpiry keyExpiry, string keymask = "VORTECH-******-******", int level = 1, string owner = "SellerAPI",string userid = "none") {
        int expiry = 0;
        
        switch (keyExpiry) {
            case KeyExpiry.Day:
                expiry = 1;
                break;
            case KeyExpiry.Week:
                expiry = 7;
                break;
            case KeyExpiry.Month:
                expiry = 30;
                break;
            case KeyExpiry.Lifetime:
                expiry = 4000;
                break;
        }
        
        var request = _client.GetAsync(BaseUrl + sellerKey + $"&type=add&format=text&expiry={expiry}&mask={keymask}&level={level}&amount=1&owner={owner}&character=2&note={userid}").Result;
        if (!request.IsSuccessStatusCode) return null;
        var response = request.Content.ReadAsStringAsync().Result;
        return response;
    }
    
    public static bool ResetHwid(string sellerkey, string license) {
        var request = _client.GetAsync(BaseUrl + sellerkey + $"&type=resetuser&user={license}").Result;
        return request.IsSuccessStatusCode;
    }
    
    public static bool DeleteLicense(string sellerkey, string license, int userToo = 1) {
        var request = _client.GetAsync(BaseUrl + sellerkey + $"&type=del&key={license}&userToo={userToo}").Result;
        return request.IsSuccessStatusCode;
    }
    
    public static FetchAllKeys? FetchAllKeys(string sellerkey) {
        var request = _client.GetAsync(BaseUrl + sellerkey + $"&type=fetchallkeys&format=json").Result;
        if (!request.IsSuccessStatusCode) return null;
        var response = request.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<FetchAllKeys>(response);
    }

    public static bool KeyExists(string sellerKey, string license) {
        var request = _client.GetAsync(BaseUrl + sellerKey + $"&type=verify&key={license}").Result;
        return request.IsSuccessStatusCode;
    }
}