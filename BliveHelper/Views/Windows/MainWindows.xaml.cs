using BliveHelper.Utils;
using BliveHelper.Utils.Blive;
using BliveHelper.Utils.Obs;
using BliveHelper.Utils.QRCoder;
using BliveHelper.Utils.Structs;
using BliveHelper.Views.Components;
using BliveHelper.Views.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BliveHelper.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : BaseWindow
{
    private ObsWebSocketAPI WebSocket => ENV.WebSocket;
    public BliveInfo Info => ENV.BliveInfo;

    // 扫码
    private string QrCodeUrl { get; set; } = string.Empty;

    public bool ScanQR
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool ShowCloseScanQR
    {
        get;
        set => SetProperty(ref field, value);
    }

    public BitmapImage QrCodeImage
    {
        get;
        set => SetProperty(ref field, value);
    }

    public string QrCodeMessage
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;

    public string DanmuMessage
    {
        get;
        set => SetProperty(ref field, value);
    } = string.Empty;

    // 控件属性
    public bool DanmuEnable
    {
        get;
        set => SetProperty(ref field, value);
    }

    // 选择标签页
    public TabItemModel SelectedPage
    {
        get;
        set => SetProperty(ref field, value);
    }

    // 显示注销按钮=
    public bool ShowSignOutButton
    {
        get;
        set => SetProperty(ref field, value);
    } = true;

    public ObservableCollection<TabItemModel> Pages { get; } = [];

    // 命令
    public ICommand SignOutCommand => new RelayCommand(SignOut);
    public ICommand SendDanmuCommand => new RelayCommand(SendDanmu);
    public ICommand OpenUserPageCommand => new RelayCommand(OpenUserPage);
    public ICommand CopyUserIdCommand => new RelayCommand(CopyUserId);
    public ICommand OpenLivePageCommand => new RelayCommand(OpenLivePage);
    public ICommand CopyLiveRoomdIdCommand => new RelayCommand(CopyLiveRoomdId);
    public ICommand CloseScanQRCommand => new RelayCommand(() => ScanQR = false);
    public ICommand CloseCommand => new RelayCommand(Close);

    public string WebSocketConnectText => WebSocket.IsOpen ? "已连接" : "已断开";
    public string WebSocketVersionText => WebSocket.IsOpen ? $"[OBS版本: {WebSocket.ObsStudioVerison}, 插件版本: {WebSocket.ObsPluginVersion}]" : string.Empty;
    public string WebSocketStateText => $"{WebSocketConnectText} {WebSocketVersionText}";
    public string UserName => string.IsNullOrEmpty(Info.UserName) ? "未登录" : Info.UserName;
    public string RoomIdText => Info.RoomId > 0 ? Info.RoomId.ToString() : "未登录";

    public MainWindow() : base()
    {
        InitializeComponent();
        // 绑定事件
        Loaded += MainWindow_Loaded;
        ENV.BliveInfo.PropertyChanged += BliveInfo_PropertyChanged;
        ENV.WebSocket.OnStateChanged += WebSocket_OnStateChanged;
        // 添加标签页
        Pages.Add(new TabItemModel("基本信息", new LiveSettingsPage()));
        Pages.Add(new TabItemModel("封面设置", new LiveCoverSettingsPage()));
        Pages.Add(new TabItemModel("背景设置", new LiveBackgroundsPage()));
        Pages.Add(new TabItemModel("用户封禁", new LiveBlockUsersPage()));
        Pages.Add(new TabItemModel("房管设置", new LiveAdminsPage()));
        Pages.Add(new TabItemModel("OBS插件", new ObsSettingsPage()));
        SelectedPage = Pages.First();
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // 如果没有 Cookies 则显示二维码扫码登录
        if (ENV.Config.Cookies.Count == 0)
        {
            RefreshLoginQR();
        }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        // 取消关闭事件, 隐藏窗口
        e.Cancel = true;
        Hide();
    }

    private void OnQRImageMouseDown(object sender, MouseButtonEventArgs e)
    {
        // 刷新二维码
        RefreshLoginQR();
    }

    private void BliveInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Info.IsStart):
                NotifyPropertyChanged(nameof(RoomIdText));
                break;
            case nameof(Info.RoomId):
                NotifyPropertyChanged(nameof(RoomIdText));
                break;
            case nameof(Info.UserName):
                NotifyPropertyChanged(nameof(UserName));
                break;
        }
    }

    private void WebSocket_OnStateChanged(object sender, bool value)
    {
        NotifyPropertyChanged(nameof(WebSocketStateText));
    }

    private void SignOut()
    {
        var result = MessageBox.Show("确定要退出登录?", "注销", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        if (result is MessageBoxResult.OK)
        {
            ENV.Config.Cookies = [];
            ENV.BliveAPI.Cookies = [];
            Info.IsStart = false;
            ScanQR = true;
            ShowSignOutButton = false;
            Info.UserName = string.Empty;
            Info.RoomId = default;
            Info.Title = string.Empty;
            Info.SelectedArea = string.Empty;
            Info.SelectedGame = string.Empty;
            // 生成二维码
            RefreshLoginQR();
        }
    }

    private async void SendDanmu()
    {
        DanmuEnable = false;
        if (Info.RoomId > 0 && !string.IsNullOrEmpty(DanmuMessage) && await ENV.BliveAPI.SendDanmu(Info.RoomId, DanmuMessage))
        {
            DanmuMessage = string.Empty;
        }
        DanmuEnable = true;
    }

    private void OpenUserPage()
    {
        Process.Start(new ProcessStartInfo { FileName = $"https://space.bilibili.com/{Info.UserId}", UseShellExecute = true });
    }

    private void CopyUserId()
    {
        Clipboard.SetDataObject(Info.UserId.ToString());
    }

    private void OpenLivePage()
    {
        Process.Start(new ProcessStartInfo { FileName = $"https://live.bilibili.com/{Info.RoomId}", UseShellExecute = true });
    }

    private void CopyLiveRoomdId()
    {
        Clipboard.SetDataObject(Info.RoomId.ToString());
    }

    public void ShowQrCode(string url, string message = "", bool showClose = false)
    {
        // 显示图形二维码
        ScanQR = true;
        QrCodeUrl = url;
        ShowCloseScanQR = showClose;
        QrCodeMessage = message;

        // 将网页地址转为二维码
        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
        var qrCodeBytes = bitmapByteQRCode.GetGraphic(20);

        using var ms = new MemoryStream(qrCodeBytes);
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = ms;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        QrCodeImage = bitmapImage;
    }

    public async void RefreshLoginQR()
    {
        // 获取二维码
        var qrResponse = await ENV.BliveAPI.GetLoginQRCode();
        if (qrResponse != null)
        {
            ShowQrCode(qrResponse.Url);
            ShowSignOutButton = false;
            // 循环检查二维码扫描状态
            while (qrResponse.Url == QrCodeUrl)
            {
                var state = await ENV.BliveAPI.PollLoginQRCode(qrResponse.QRCodeKey);
                if (state != null)
                {
                    if (state.Code == 0)
                    {
                        ScanQR = false;
                        ShowSignOutButton = true;
                        ENV.Config.Cookies = ENV.BliveAPI.Cookies;
                        break;
                    }
                    else if (state.Code == 86038)
                    {
                        QrCodeMessage = "二维码失效, 重新生成中";
                        RefreshLoginQR();
                        break;
                    }
                    else if (state.Code == 86090)
                    {
                        QrCodeMessage = "二维码已扫描，等待确认";
                    }
                    else
                    {
                        QrCodeMessage = "等待扫描中";
                    }
                }
                await Task.Delay(1000);
            }
        }
    }

    private void OnToolBarMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton is MouseButtonState.Pressed && !IsDescendantOfButton(e.OriginalSource as DependencyObject))
        {
            DragMove();
        }
    }

    private static bool IsDescendantOfButton(DependencyObject source)
    {
        while (source != null)
        {
            if (source is Button) return true;
            source = VisualTreeHelper.GetParent(source);
        }
        return false;
    }
}