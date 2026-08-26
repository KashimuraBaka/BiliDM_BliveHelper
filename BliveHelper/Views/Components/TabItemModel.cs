namespace BliveHelper.Views.Components;

public class TabItemModel(string header, object content)
{
    public string Header { get; set; } = header;
    public object Content { get; set; } = content;
}
