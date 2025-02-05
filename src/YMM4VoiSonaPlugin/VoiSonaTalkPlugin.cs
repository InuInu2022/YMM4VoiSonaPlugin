using YmmeUtil.Common;
using YukkuriMovieMaker.Plugin;
using YukkuriMovieMaker.Plugin.Voice;
namespace YMM4VoiSonaPlugin;

[PluginDetails(AuthorName = "InuInu", ContentId = "nc375085")]
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
		=> "YMM4 VoiSona Talk プラグイン";

	public PluginDetailsAttribute Details => new()
	{
		//制作者
		AuthorName = "InuInu",
		//作品ID
		ContentId = "nc375085",
	};

	public VoiSonaTalkPlugin()
	{
		Console.WriteLine($"VoiSonaTalkPlugin: {AssemblyUtil.GetVersionString(typeof(VoiSonaTalkPlugin))}");
	}

	public async Task UpdateVoicesAsync()
	{
		await VoiSonaTalkSettings.Default
			.UpdateSpeakersAsync()
			.ConfigureAwait(false);
	}
}
