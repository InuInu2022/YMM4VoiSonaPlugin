using YukkuriMovieMaker.Plugin;
using SonaBridge;
using SonaBridge.Core.Common;

namespace YMM4VoiSonaPlugin;

/// <summary>
/// 設定クラス
/// Speaker一覧をキャッシュする
/// </summary>
public partial class VoiSonaTalkSettings : SettingsBase<VoiSonaTalkSettings>
{
	public override SettingsCategory Category
		=> SettingsCategory.Voice;
	public override string Name => "VoiSona Talk ボイス設定";
	public override bool HasSettingView => false;
	public override object? SettingView => throw new NotSupportedException();

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

	ITalkAutoService? _service;
	bool _isCached;
	string[] _speakers = [];

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
		IsCached = true;

		Speakers = await _service
			.GetAvailableCastsAsync()
			.ConfigureAwait(false);
	}
}
