using YukkuriMovieMaker.Commons;
using System.Windows;
using YMM4VoiSonaPlugin.ViewModel;

namespace YMM4VoiSonaPlugin.View;

[AttributeUsage(AttributeTargets.Property)]
public sealed class VoiSonaTalkPresetDisplayAttribute : PropertyEditorAttribute2
{
	public override void SetBindings(FrameworkElement control, ItemProperty[] itemProperties)
	{
		if (control is not TalkPresetsView editor) return;

		editor.ItemProperties = itemProperties;
		if(itemProperties?[0].PropertyOwner is VoiSonaTalkParameter vsParam)
		{
			int? oldIndex = vsParam.PresetIndex;

			editor.DataContext = new TalkPresetsViewModel(vsParam.Preset, oldIndex);
		}
		if(editor.DataContext is not TalkPresetsViewModel vm) return;
		vm.ItemProperties = itemProperties;
	}

	public override FrameworkElement Create()
	{
		return new TalkPresetsView();
	}

	public override void ClearBindings(FrameworkElement control)
	{
		if (control is not TalkPresetsView editor) return;
        editor.ItemProperties = null;
		if(editor.DataContext is not TalkPresetsViewModel vm) return;
		vm.Dispose();
		editor.DataContext = null;
	}
}
