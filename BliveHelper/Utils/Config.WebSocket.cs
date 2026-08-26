using BliveHelper.Utils.Structs;
using Newtonsoft.Json;

namespace BliveHelper.Utils;

public class WebSocketSetting : ObservableObject
{
    [JsonProperty("server_url")]
    public string ServerUrl
    {
        get;
        set => SetProperty(ref field, value);
    } = "localhost:4455";

    [JsonProperty("server_key")]
    public string ServerKey
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;

    [JsonProperty("auto_stream")]
    public bool AutoStream
    {
        get;
        set => SetProperty(ref field, value);
    }
}
