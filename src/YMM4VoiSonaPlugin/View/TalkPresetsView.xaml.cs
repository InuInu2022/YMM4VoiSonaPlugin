using System.Windows;
using System.Windows.Controls;

using YMM4VoiSonaPlugin.ViewModel;

using YukkuriMovieMaker.Commons;

namespace YMM4VoiSonaPlugin.View;

/// <summary>
/// Interaction logic for TalkPresetsView.xaml
/// </summary>
public partial class TalkPresetsView : UserControl, IPropertyEditorControl2
{
	public event EventHandler? BeginEdit;
	public event EventHandler? EndEdit;
	public ItemProperty[]? ItemProperties { get; set; }

    public TalkPresetsView()
    {
        InitializeComponent();
    	DataContextChanged += TalkPresetsView_DataContextChanged;
    }

	public void SetEditorInfo(IEditorInfo info)
	{
		if (DataContext is not TalkPresetsViewModel vm) return;
		vm.EditorInfo = info;
	}

	private void TalkPresetsView_DataContextChanged(
		object sender, DependencyPropertyChangedEventArgs e)
	{
		if (e.OldValue is TalkPresetsView oldVm)
		{
			oldVm.BeginEdit -= TalkPresetsView_BeginEdit;
			oldVm.EndEdit -= TalkPresetsView_EndEdit;
		}
		if (e.NewValue is TalkPresetsView newVm)
		{
			newVm.BeginEdit += TalkPresetsView_BeginEdit;
			newVm.EndEdit += TalkPresetsView_EndEdit;
		}
	}

	private void TalkPresetsView_BeginEdit(object? sender, EventArgs e)
	{
		BeginEdit?.Invoke(this, e);
	}

	private void TalkPresetsView_EndEdit(object? sender, EventArgs e)
	{
		//var vm = DataContext as TalkPresetsViewModel;

		EndEdit?.Invoke(this, e);
	}

}
