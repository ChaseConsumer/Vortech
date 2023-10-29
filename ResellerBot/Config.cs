using Newtonsoft.Json;

namespace ResellerBot; 



public class Config {
    public string Token { get; set; }
    public string Webhook { get; set; }
    public List<Reseller> Resellers { get; set; } = new();
    public List<ulong> Administrators { get; set; } = new();
    public Dictionary<ulong, List<Product>> PanelOwners { get; set; } = new();
    public Dictionary<Product, string> DownloadLinks { get; set; } = new();
    public Dictionary<Product, string> Sellerkey { get; set; } = new();
}

public class Infos
{
    public static Dictionary<Product, string> SellerKeys = new()
    {
        { Product.FortnitePrivate, "" },
        { Product.FortnitePublic, "6779d2969dfbf1736b04a5d31fa3332b" },
        { Product.PermSpoofer, "c6b117beda72a6921c75efe041ceecda" },
    };

}


public enum Product {
    FortnitePrivate,
    FortnitePublic,
    PermSpoofer,
}
public class Reseller {
    public string Name { get; set; }
    public ulong Id { get; set; }
    public int Balance { get; set; }
    public int KeysGenerated { get; set; }
}

public enum KeyExpiry {
    Day,
    Week,
    Month,
    Lifetime
}

public static class ConfigHelper {
    public static Config? Get() {
        if (!File.Exists("config.json")) return null;
        var json = File.ReadAllText("config.json");
        if (string.IsNullOrEmpty(json)) return null;
        return JsonConvert.DeserializeObject<Config>(json);
    }

    public static void Save(this Config config) {
        var json = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText("config.json", json);    
    }
}