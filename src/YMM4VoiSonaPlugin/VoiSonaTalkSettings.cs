using YukkuriMovieMaker.Plugin;
using SonaBridge;
using SonaBridge.Core.Common;
using YMM4VoiSonaPlugin.ViewModel;
using YukkuriMovieMaker.Plugin.Voice;
using System.Collections.ObjectModel;
using Epoxy;
using YmmeUtil.Ymm4;

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
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0016")]
	public Dictionary<string, Dictionary<string, double>> SpeakersStyles
	{
		get { return _speakersStyles; }
		set { Set(ref _speakersStyles, value); }
	}

	ITalkAutoService? _service;
	bool _isCached;
	string[] _speakers = [];
	Dictionary<string, Dictionary<string, double>> _speakersStyles = new(StringComparer.Ordinal);

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

		var dic = new Dictionary<string, Dictionary<string, double>>(StringComparer.Ordinal);
		foreach (var item in Speakers)
		{
			if (item is null) continue;
			await _service.SetCastAsync(item)
				.ConfigureAwait(false);
			var styles = await _service
				.GetStylesAsync(item)
				.ConfigureAwait(false);
			dic.Add(item, styles.ToDictionary());

			await UIThread.InvokeAsync(()=>{
				TaskbarUtil.ShowProgress(index / total);
				return ValueTask.CompletedTask;
			}).ConfigureAwait(false);
			index++;
		}
		SpeakersStyles = dic;

		IsCached = true;

		await UIThread.InvokeAsync(()=>{
			TaskbarUtil.FinishIndeterminate();
			return ValueTask.CompletedTask;
		}).ConfigureAwait(false);
	}
}
