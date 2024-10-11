using YmmeUtil.Common;

using YukkuriMovieMaker.Plugin;
using YukkuriMovieMaker.Plugin.Voice;
namespace YMM4VoiSonaPlugin;

public sealed class VoiSonaTalkPlugin : IVoicePlugin
{
	public IEnumerable<IVoiceSpeaker> Voices
		=> VoiSonaTalkSettings
			.Default
			.Speakers
			.Select(v => new VoiSonaTalkSpeaker(v));
	public bool CanUpdateVoices { get; } = true;
	public bool IsVoicesCached => VoiSonaTalkSettings.Default.IsCached;
	public string Name
		=> $"YMM4 VoiSona Talk プラグイン";

	public PluginDetailsAttribute Details => new()
	{
		AuthorName = "InuInu",
		ContentId = "",
	};

	public VoiSonaTalkPlugin()
	{
		Console.WriteLine($"VoiSonaTalkPlugin: {AssemblyUtil.GetVersionString(typeof(VoiSonaTalkPlugin))}");
	}

	public Task UpdateVoicesAsync()
		=> VoiSonaTalkSettings.Default.UpdateSpeakersAsync();
}
