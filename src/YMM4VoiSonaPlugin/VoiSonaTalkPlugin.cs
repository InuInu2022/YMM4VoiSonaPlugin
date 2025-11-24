using YmmeUtil.Common;

using YukkuriMovieMaker.Commons;
using YukkuriMovieMaker.Plugin;
using YukkuriMovieMaker.Plugin.Voice;
namespace YMM4VoiSonaPlugin;

[PluginDetails(AuthorName = "InuInu", ContentId = "nc375085")]
public sealed class VoiSonaTalkPlugin : IVoicePlugin, IDisposable
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

	readonly DisposeCollector _disposer = new();
	bool _disposedValue;

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

	private void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_disposer.Dispose();
			}

			// TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
			// TODO: 大きなフィールドを null に設定します
			_disposedValue = true;
		}
	}

	// // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
	// ~VoiSonaTalkPlugin()
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
