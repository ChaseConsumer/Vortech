using Newtonsoft.Json;

namespace ResellerBot; 


public class User
{
    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Success;

    [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
    public string Username;

    [JsonProperty("subscriptions", NullValueHandling = NullValueHandling.Ignore)]
    public List<Subscription> Subscriptions;

    [JsonProperty("uservars", NullValueHandling = NullValueHandling.Ignore)]
    public List<object> Uservars;

    [JsonProperty("ip", NullValueHandling = NullValueHandling.Ignore)]
    public string Ip;

    [JsonProperty("hwid", NullValueHandling = NullValueHandling.Ignore)]
    public string Hwid;

    [JsonProperty("createdate", NullValueHandling = NullValueHandling.Ignore)]
    public long? Createdate;

    [JsonProperty("lastlogin", NullValueHandling = NullValueHandling.Ignore)]
    public long Lastlogin;

    [JsonProperty("cooldown", NullValueHandling = NullValueHandling.Ignore)]
    public object Cooldown;

    [JsonProperty("password", NullValueHandling = NullValueHandling.Ignore)]
    public object Password;

    [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
    public string Token;

    [JsonProperty("banned", NullValueHandling = NullValueHandling.Ignore)]
    public object Banned;
}

public class License
{
    [JsonProperty("success")]
    public bool Success;

    [JsonProperty("duration")]
    public string Duration;

    [JsonProperty("hwid")]
    public string Hwid;

    [JsonProperty("note")]
    public string Note;

    [JsonProperty("status")]
    public string Status;

    [JsonProperty("level")]
    public string Level;

    [JsonProperty("createdby")]
    public string Createdby;

    [JsonProperty("usedby")]
    public string Usedby;

    [JsonProperty("usedon")]
    public string Usedon;

    [JsonProperty("creationdate")]
    public string Creationdate;
}

public class Subscription
{
    [JsonProperty("subscription")]
    public string Name;

    [JsonProperty("key")]
    public object Key;

    [JsonProperty("expiry")]
    public string Expiry;
}


public class AllLicense
{
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public int Id;

    [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
    public string Key;

    [JsonProperty("note", NullValueHandling = NullValueHandling.Ignore)]
    public string Note;

    [JsonProperty("expires", NullValueHandling = NullValueHandling.Ignore)]
    public string Expires;

    [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
    public string Status;

    [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
    public string Level;

    [JsonProperty("genby", NullValueHandling = NullValueHandling.Ignore)]
    public string Genby;

    [JsonProperty("gendate", NullValueHandling = NullValueHandling.Ignore)]
    public string Gendate;

    [JsonProperty("usedon", NullValueHandling = NullValueHandling.Ignore)]
    public object Usedon;

    [JsonProperty("usedby", NullValueHandling = NullValueHandling.Ignore)]
    public object Usedby;

    [JsonProperty("app", NullValueHandling = NullValueHandling.Ignore)]
    public string App;

    [JsonProperty("banned", NullValueHandling = NullValueHandling.Ignore)]
    public object Banned;
}

public class FetchAllKeys
{
    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    public bool Success;

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string Message;

    [JsonProperty("keys", NullValueHandling = NullValueHandling.Ignore)]
    public List<AllLicense> Keys;
}