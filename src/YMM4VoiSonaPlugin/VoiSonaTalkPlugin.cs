using YukkuriMovieMaker.Plugin.Voice;
namespace YMM4VoiSonaPlugin;

public sealed class VoiSonaTalkPlugin : IVoicePlugin
{
	public IEnumerable<IVoiceSpeaker> Voices { get; }
	public bool CanUpdateVoices { get; }
	public bool IsVoicesCached { get; }
	public string Name => "YMM4 VoiSona Talk プラグイン";

	public VoiSonaTalkPlugin()
	{
		Voices = [];
	}

	public Task UpdateVoicesAsync()
	{
		throw new NotImplementedException();
	}
}
