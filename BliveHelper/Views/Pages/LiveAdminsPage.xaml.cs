using BliveHelper.Utils;
using BliveHelper.Utils.Blive;
using BliveHelper.Utils.Structs;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

namespace BliveHelper.Views.Pages;

/// <summary>
/// LiveAdminsPage.xaml 的交互逻辑
/// </summary>
public partial class LiveAdminsPage : ObservableUserControl
{
    public int MaxAdminsCount
    {
        get;
        set => SetProperty(ref field, value);
    }

    public BliveAdminInfo SelectedAdmin
    {
        get;
        set => SetProperty(ref field, value);
    }

    public bool AddAdminEnabled
    {
        get;
        set => SetProperty(ref field, value);
    }

    public string AddAdminContent
    {
        get;
        set => SetProperty(ref field, value);
    }

    public ObservableCollection<BliveAdminInfo> Admins { get; } = [];

    public ICommand AddAdminCommand => new RelayCommand(AddAdmin);
    public ICommand RemoveAdminCommand => new RelayCommand(RemoveAdmin);

    public string AdminInfoText => $"当前房管数: {Admins.Count}/{MaxAdminsCount}";

    public LiveAdminsPage() : base()
    {
        InitializeComponent();
        Loaded += LiveAdminsPage_Loaded;
        Admins.CollectionChanged += Admins_CollectionChanged;
    }

    private async void LiveAdminsPage_Loaded(object sender, RoutedEventArgs e)
    {
        var res = await ENV.BliveAPI.GetLiveAdmins(ENV.BliveInfo.RoomId);
        MaxAdminsCount = res.MaxRoomAnchorsNumber;
        Admins.Clear();
        Admins.AddRange(res.Admins);
    }

    private void Admins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        NotifyPropertyChanged(nameof(AdminInfoText));
    }

    private async void RemoveAdmin()
    {
        if (SelectedAdmin != null)
        {
            var result = MessageBox.Show($"确定要移除管理员 {SelectedAdmin.UserName} 吗？", "确认移除", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result is MessageBoxResult.Yes)
            {
                var removeResult = await ENV.BliveAPI.RemoveLiveAdmin(SelectedAdmin.UserId);
                if (removeResult)
                {
                    Admins.Remove(SelectedAdmin);
                    SelectedAdmin = null;
                }
                else
                {
                    MessageBox.Show("移除管理员失败，请稍后再试。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    private async void AddAdmin()
    {
        AddAdminEnabled = false;
        if (!string.IsNullOrEmpty(AddAdminContent))
        {
            var result = await ENV.BliveAPI.AddLiveAdmin(AddAdminContent);
            if (result.Success)
            {
                var newAdmin = new BliveAdminInfo
                {
                    UserId = result.UserId,
                    UserName = result.UserName,
                    CreationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                Admins.Add(newAdmin);
                AddAdminContent = string.Empty;
            }
        }
        AddAdminEnabled = true;
    }
}
