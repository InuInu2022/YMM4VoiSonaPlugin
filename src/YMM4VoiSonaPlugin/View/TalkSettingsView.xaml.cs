using System.Windows.Controls;

using YmmeUtil.Common;

namespace YMM4VoiSonaPlugin.View;

/// <summary>
/// Interaction logic for TalkSettingsView.xaml
/// </summary>
public partial class TalkSettingsView : UserControl
{
    public TalkSettingsView()
    {
        InitializeComponent();

		var verstr = AssemblyUtil.GetVersionString(typeof(TalkSettingsView));
        Console.WriteLine($"ver: {verstr}");
    }
}
