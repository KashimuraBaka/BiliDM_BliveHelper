using BliveHelper.Utils.Structs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BliveHelper.Utils;

public class Config : ObservableObject
{
    [JsonIgnore]
    private bool Loaded { get; set; }

    public bool PluginEnabled
    {
        get;
        set => SetProperty(ref field, value);
    }

    public Dictionary<string, string> Cookies
    {
        get;
        set => SetProperty(ref field, value);
    }

    public WebSocketSetting WebSocket
    {
        get;
        set => SetProperty(ref field, value);
    } = new();

    public Config()
    {
        // 如果参数属性发生变动
        PropertyChanged += OnPropertyChanged;
        WebSocket.PropertyChanged += OnPropertyChanged;
    }

    protected async void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (Loaded && !string.IsNullOrEmpty(ENV.ConfigFileName))
        {
            await SaveAsync();
        }
    }

    public async Task LoadAsync()
    {
        if (File.Exists(ENV.ConfigFileName))
        {
            using var fs = new FileStream(ENV.ConfigFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(fs, Encoding.UTF8);
            var configString = await reader.ReadToEndAsync();
            var config = JsonConvert.DeserializeObject<Config>(configString);
            if (config != null)
            {
                PluginEnabled = config.PluginEnabled;
                Cookies = config.Cookies;
                WebSocket.ServerUrl = config.WebSocket.ServerUrl;
                WebSocket.ServerKey = config.WebSocket.ServerKey;
                WebSocket.AutoStream = config.WebSocket.AutoStream;
            }
        }
        Loaded = true;
    }

    public async Task SaveAsync()
    {
        using var fs = new FileStream(ENV.ConfigFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        using var sw = new StreamWriter(fs, Encoding.UTF8);
        fs.SetLength(0);
        await sw.WriteAsync(JsonConvert.SerializeObject(this, Formatting.Indented));
    }
}
