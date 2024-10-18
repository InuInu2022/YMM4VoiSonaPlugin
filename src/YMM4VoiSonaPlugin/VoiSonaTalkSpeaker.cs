
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;

using SonaBridge;
using SonaBridge.Core.Common;

using YMM4VoiSonaPlugin.ViewModel;

using YukkuriMovieMaker.Plugin.Voice;

namespace YMM4VoiSonaPlugin;

public class VoiSonaTalkSpeaker : IVoiceSpeaker
{
	public string EngineName => "VoiSona Talk";
	public string SpeakerName { get; }
	public string API => "YMM4VoiSonaTalk";
	public string ID { get; }
	public bool IsVoiceDataCachingRequired => true;
	public SupportedTextFormat Format => SupportedTextFormat.Text;
	public IVoiceLicense? License { get; }
	//TODO: cast data json
	public IVoiceResource? Resource => null;
	public string? SpeakerAuthor { get; }
	public string? SpeakerContentId { get; }
	public string? EngineAuthor { get; } = "Techno-Speech, Inc.";
	public string? EngineContentId { get; }

	static readonly SemaphoreSlim Semaphore = new(1);
	static readonly ITalkAutoService _service = new TalkServiceProvider()
		.GetService<ITalkAutoService>();

	readonly string _voiceName;
	ReadOnlyDictionary<string, double> _styles;
	bool isInitialized;

	public VoiSonaTalkSpeaker(string voiceName)
	{
		SpeakerName = $"{voiceName}";
		ID = $"YMM4VoiSona{voiceName}";
		var castData = VoiSonaCastManager.GetCastData(voiceName);
		SpeakerAuthor = castData.Author;
		SpeakerContentId = castData.ContentId;
		License = new VoiSonaTalkVoiceLicense(
			castData.TermUrl
		);
		_voiceName = voiceName;
		_styles = new(new Dictionary<string, double>(StringComparer.Ordinal));
	}

	public async ValueTask InitAsync()
	{
		if (isInitialized) return;
		_styles = await _service.GetStylesAsync(_voiceName)
			.ConfigureAwait(false);
		isInitialized = true;
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
			if(parameter is VoiSonaTalkParameter vstParam)
			{
				await _service.SetGlobalParamsAsync(
					new Dictionary<string,double>(StringComparer.Ordinal)
					{
						{nameof(vstParam.Speed), vstParam.Speed},
						{nameof(vstParam.Volume), vstParam.Volume},
						{nameof(vstParam.Pitch), vstParam.Pitch},
						{nameof(vstParam.Alpha), vstParam.Alpha},
						{"Into.", vstParam.Intonation},
						{"Hus.", vstParam.Husky},
					}
				).ConfigureAwait(false);

				await _service.SetStylesAsync(
					SpeakerName,
					vstParam.ItemsCollection
						.ToDictionary(
							x => x.DisplayName,
							x => x.Value,
							StringComparer.Ordinal)
				).ConfigureAwait(false);
			}
			sw.Stop();
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
			throw;
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
		if(!VoiSonaTalkSettings.Default.SpeakersStyles.TryGetValue(_voiceName, out var saved)){
			return new VoiSonaTalkParameter();
		}
		_styles = saved.AsReadOnly();
		return new VoiSonaTalkParameter
		{
			Voice = _voiceName,
			ItemsCollection = _styles
				.Select(v => new VoiSonaTalkStyleParameter(){
					DisplayName=v.Key,
					Value=v.Value,
					Description=$"Style: {v.Key}",
				})
				.ToImmutableList(),
		};
	}

	public bool IsMatch(string api, string id)
	{
		return string.Equals(api, API, StringComparison.Ordinal)
			&& string.Equals(id, ID, StringComparison.Ordinal);
	}

	public IVoiceParameter MigrateParameter(IVoiceParameter currentParameter)
	{
		if(currentParameter is not VoiSonaTalkParameter vsParam){
			return CreateVoiceParameter();
		}

		//声質切替で固有Styleが切り替わらないので強制再読み込みを掛ける
		var isSame = string.Equals(vsParam.Voice, _voiceName, StringComparison.Ordinal);
		return isSame ? currentParameter : CreateVoiceParameter();
	}
}
