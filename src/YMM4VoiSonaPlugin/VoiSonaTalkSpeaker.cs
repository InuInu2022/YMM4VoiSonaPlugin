
using SonaBridge;
using SonaBridge.Core.Common;

using YukkuriMovieMaker.Plugin.Voice;

namespace YMM4VoiSonaPlugin;

public class VoiSonaTalkSpeaker : IVoiceSpeaker
{
	public string EngineName => "VoiSona Talk";
	public string SpeakerName { get; }
	public string API => "YMM4VoiSonaTalk";
	public string ID { get; }
	public bool IsVoiceDataCachingRequired => true;
	//TODO: support english voice
	public SupportedTextFormat Format => SupportedTextFormat.Text;
	public IVoiceLicense? License
		=> new VoiSonaTalkVoiceLicense(
			//TODO: add term url by voice
		);
	//TODO: cast data json
	public IVoiceResource? Resource => null;

	//TODO: ボイス毎の制作者指定
	public string? SpeakerAuthor { get; }
	//TODO: ボイス毎のニコニコID指定（あれば）
	public string? SpeakerContentId { get; }
	public string? EngineAuthor { get; } = "Techno-Speech, Inc.";
	public string? EngineContentId { get; }

	static readonly SemaphoreSlim Semaphore = new(1);
	readonly ITalkAutoService _service;

	public VoiSonaTalkSpeaker(string voiceName)
	{
		SpeakerName = $"{voiceName}";
		ID = $"YMM4VoiSona{voiceName}";

		var provider = new TalkServiceProvider();
		_service = provider.GetService<ITalkAutoService>();
	}

	public async Task<string> ConvertKanjiToYomiAsync(string text, IVoiceParameter voiceParameter)
	{
		//TODO: get yomi from vs talk
		return await Task.FromResult(text)
			.ConfigureAwait(false);
	}

	public async Task<IVoicePronounce?> CreateVoiceAsync(
		string text,
		IVoicePronounce? pronounce,
		IVoiceParameter? parameter,
		string filePath
	)
	{
		await Semaphore.WaitAsync().ConfigureAwait(false);

		try
		{
			Console.WriteLine($"from ymm4 path: {filePath}");
			var sw = System.Diagnostics.Stopwatch.StartNew();
			await _service
				.SetCastAsync(SpeakerName)
				.ConfigureAwait(false);
			sw.Stop();
			Console.WriteLine($"set cast time: {sw.Elapsed.TotalSeconds}");
			sw.Restart();
			var result = await _service
				.OutputWaveToFileAsync(text, filePath)
				.ConfigureAwait(false);
			Console.WriteLine($"output time: {sw.Elapsed.TotalSeconds}");
			if(!result){
				await Console.Error
					.WriteLineAsync($"ERROR! {nameof(CreateVoiceAsync)} : {text}")
					.ConfigureAwait(false);
			}
		}
		catch(Exception ex)
		{
			await Console.Error
				.WriteLineAsync($"ERROR! {ex.Message}")
				.ConfigureAwait(false);
		}
		finally
		{
			Semaphore.Release();
		}
		//TODO: get pronounce from vs talk
		return null;
	}

	public IVoiceParameter CreateVoiceParameter()
	{
		return new VoiSonaTalkParameter();
	}

	public bool IsMatch(string api, string id)
	{
		return string.Equals(api, API, StringComparison.Ordinal)
			&& string.Equals(id, ID, StringComparison.Ordinal);
	}

	public IVoiceParameter MigrateParameter(IVoiceParameter currentParameter)
	{
		return currentParameter is not VoiSonaTalkParameter
			? CreateVoiceParameter()
			: currentParameter;
	}
}
