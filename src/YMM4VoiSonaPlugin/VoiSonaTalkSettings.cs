using YukkuriMovieMaker.Plugin;
using SonaBridge;
using SonaBridge.Core.Common;
using YMM4VoiSonaPlugin.ViewModel;
using YukkuriMovieMaker.Plugin.Voice;
using System.Collections.ObjectModel;
using Epoxy;
using YmmeUtil.Ymm4;
using System.Diagnostics.CodeAnalysis;

namespace YMM4VoiSonaPlugin;

/// <summary>
/// 設定クラス
/// Speaker一覧をキャッシュする
/// </summary>
public partial class VoiSonaTalkSettings : SettingsBase<VoiSonaTalkSettings>
{
	public override SettingsCategory Category
		=> SettingsCategory.Voice;
	public override string Name => "YMM4 VoiSona Talk";
	public override bool HasSettingView => true;
	public override object? SettingView
	{
		get
		{
			var view = new YMM4VoiSonaPlugin.View.TalkSettingsView
			{
				DataContext = new TalkSettingViewModel()
			};
			return view;
		}
	}

	public bool IsCached
	{
		get { return _isCached; }
		set { Set(ref _isCached, value); }
	}
	public string[] Speakers
	{
		get { return _speakers; }
		set { Set(ref _speakers, value); }
	}
	[SuppressMessage("Design", "MA0016")]
	public Dictionary<string, Dictionary<string, double>> SpeakersStyles
	{
		get { return _speakersStyles; }
		set { Set(ref _speakersStyles, value); }
	}

	[SuppressMessage("Design", "MA0016")]
	public Dictionary<string, IList<string>> SpeakersPresets
	{
		get { return _speakersPresets; }
		set { Set(ref _speakersPresets, value); }
	}

	ITalkAutoService? _service;
	bool _isCached;
	string[] _speakers = [];
	Dictionary<string, Dictionary<string, double>> _speakersStyles = new(StringComparer.Ordinal);
	Dictionary<string, IList<string>> _speakersPresets = new(StringComparer.Ordinal);

	public override void Initialize()
	{
		var provider = new TalkServiceProvider();
		_service = provider.GetService<ITalkAutoService>();
	}

	/// <summary>
	/// 話者一覧を更新する
	/// </summary>
	public async Task UpdateSpeakersAsync()
	{
		if (_service is null) return;

		await UIThread.InvokeAsync(()=>{
			TaskbarUtil.StartIndeterminate();
			return ValueTask.CompletedTask;
		}).ConfigureAwait(false);

		Speakers = await _service
			.GetAvailableCastsAsync()
			.ConfigureAwait(false);

		await UIThread.InvokeAsync(()=>{
			TaskbarUtil.FinishIndeterminate();
			TaskbarUtil.ShowNormal();
			return ValueTask.CompletedTask;
		}).ConfigureAwait(false);

		double total = Speakers.Length;
		var index = 1;

		var styleDic = new Dictionary<string, Dictionary<string, double>>(StringComparer.Ordinal);
		var presetDic = new Dictionary<string, IList<string>>(StringComparer.Ordinal);
		foreach (var item in Speakers)
		{
			if (item is null) continue;
			await _service.SetCastAsync(item)
				.ConfigureAwait(false);

			//styles
			var styles = await _service
				.GetStylesAsync(item)
				.ConfigureAwait(false);
			styleDic.Add(item, styles.ToDictionary());

			//presets
			var presets = await _service
				.GetPresetsAsync(item)
				.ConfigureAwait(false);
			presetDic.Add(item, [..presets]);

			await UIThread.InvokeAsync(()=>{
				TaskbarUtil.ShowProgress(index / total);
				return ValueTask.CompletedTask;
			}).ConfigureAwait(false);
			index++;
		}
		SpeakersStyles = styleDic;
		SpeakersPresets = presetDic;

		IsCached = true;

		await UIThread.InvokeAsync(()=>{
			TaskbarUtil.FinishIndeterminate();
			WindowUtil.FocusBack();
			return ValueTask.CompletedTask;
		}).ConfigureAwait(false);
	}
}
