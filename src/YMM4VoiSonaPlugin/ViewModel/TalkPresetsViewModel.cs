using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

using Epoxy;

using SonaBridge;
using SonaBridge.Core.Common;

using YmmeUtil.Ymm4;

using YukkuriMovieMaker.Commons;

namespace YMM4VoiSonaPlugin.ViewModel;

[ViewModel]
public class TalkPresetsViewModel: IPropertyEditorControl, IDisposable
{
	public ObservableCollection<string> Presets { get; set; } = [];
	public int PresetIndex { get; set; }
	public Command ReloadPresets { get; set; }

	[IgnoreInject]
	public IEditorInfo? EditorInfo { get; set; }
	public ItemProperty[]? ItemProperties { get; set; }

	public event EventHandler? BeginEdit;
    public event EventHandler? EndEdit;

	static readonly ITalkAutoService _service =
		new TalkServiceProvider()
			.GetService<ITalkAutoService>();
	bool _isPresetLoad;
	bool _disposedValue;

	public TalkPresetsViewModel(
		IList<string>? presets,
		int? presetIndex = null
	)
	{
		if(presets is not null)
		{
			Presets = [..presets];
		}
		if(presetIndex is null || Presets.Count > presetIndex)
		{
			PresetIndex = presetIndex ?? -1;
		}
		ReloadPresets = Command.Factory.Create(async ()=>{
			var voice = EditorInfo?.Voice?.Speaker?.SpeakerName;
			if(voice is null) return;

			// (re)load presets
			TaskbarUtil.StartIndeterminate();
			var presets = await _service
				.GetPresetsAsync(voice)
				.ConfigureAwait(true);
			TaskbarUtil.FinishIndeterminate();
			Presets = [.. presets];
			WindowUtil.FocusBack();
			PresetIndex = -1;
		});
	}

	[PropertyChanged(nameof(PresetIndex))]
	[SuppressMessage("","IDE0051")]
	private async ValueTask PresetIndexChangedAsync(int index)
	{
		if (index < 0) return;
		if (Presets[index] is not string preset) return;
		if (EditorInfo?.Voice?.Speaker?.SpeakerName is not string voice) return;

		_isPresetLoad = true;

		TaskbarUtil.StartIndeterminate();
		BeginEdit?.Invoke(this, EventArgs.Empty);
		await _service.SetPresetsAsync(voice, preset)
			.ConfigureAwait(true);

		//PresetIndex = -1;   //reset combo

		if(ItemProperties?[0].PropertyOwner is VoiSonaTalkParameter vsParam)
		{
			var globalParams = await _service.GetGlobalParamsAsync()
				.ConfigureAwait(true);
			var styles = await _service.GetStylesAsync(voice)
				.ConfigureAwait(true);
			var items = styles
				.Select(s => new VoiSonaTalkStyleParameter() {
					DisplayName = s.Key,
					Value = s.Value,
					Description = $"Style: {s.Key}",
				});
			vsParam.PresetIndex = index;
			vsParam.Alpha = globalParams["Alpha"];
			vsParam.Husky = globalParams["Hus."];
			vsParam.Pitch = globalParams["Pitch"];
			vsParam.Speed = globalParams["Speed"];
			vsParam.Intonation = globalParams["Into."];
			vsParam.Volume = globalParams["Volume"];
			vsParam.ItemsCollection = [.. items];
		}
		EndEdit?.Invoke(this, EventArgs.Empty);
		TaskbarUtil.FinishIndeterminate();

		WindowUtil.FocusBack();
		_isPresetLoad = false;
	}

	[PropertyChanged(nameof(ItemProperties))]
	[SuppressMessage("","IDE0051")]
	private ValueTask ItemPropertiesChangedAsync(ItemProperty[] value)
	{
		if(value is null or []){ return default; }

		if(ItemProperties?[0].PropertyOwner is VoiSonaTalkParameter vsParam)
		{
			vsParam.PropertyChanged += ResetPresetSelectionEvent;
		}
		return default;
	}

	void ResetPresetSelectionEvent(object? sender, PropertyChangedEventArgs e)
	{
		var isTargetReset = e.PropertyName switch
		{
			"Speed" or "Volume" or "Pitch" or "Alpha" or "Into." or "Hus." => true,
			"ItemsCollection.Value" => true,
			_ => false,
		};
		if (!isTargetReset) return;
		if (_isPresetLoad) return;

		// reset combo
		PresetIndex = -1;
		if(ItemProperties?[0].PropertyOwner is VoiSonaTalkParameter vsParam)
		{
			vsParam.PresetIndex = -1;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				if(ItemProperties?[0].PropertyOwner is VoiSonaTalkParameter vsParam)
				{
					vsParam.PropertyChanged -= ResetPresetSelectionEvent;
				}
				_service.Dispose();
			}

			_disposedValue = true;
		}
	}

	// 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
	// ~TalkPresetsViewModel()
	// {
	//     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
	//     Dispose(disposing: false);
	// }

	public void Dispose()
	{
		// このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}