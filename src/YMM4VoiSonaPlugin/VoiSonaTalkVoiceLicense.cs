
using YukkuriMovieMaker.Plugin.Voice;

namespace YMM4VoiSonaPlugin;

public record VoiSonaTalkVoiceLicense : IVoiceLicense
{
	//規約をどこに表示するか
	public VoiceLicenseDisplayLocation SummaryLocation
		=> VoiceLicenseDisplayLocation.CharacterEditor;
	//規約概要。なくてもよい
	public string? Summary { get; } = "ボイスライブラリの規約も確認してください。";
	public bool IsTermsAgreed { get; set; } = true;
	//ここにメッセージがあるとリンク文字列をクリックすると承認ダイアログ表示
	public string? Terms { get; }
	//TermsがnullならこちらのURLへ飛ぶようになる
	public string? TermsURL { get; }
		= "https://voisona.com/static/pdf/ja/terms.pdf";

	public VoiSonaTalkVoiceLicense(
		string? voiceTermsUrl = null
	)
	{
		if(voiceTermsUrl is not null)
		{
			TermsURL = voiceTermsUrl;
		}
	}

	public ValueTask<bool> ValidateLicenseAsync()
	{
		throw new NotSupportedException();
	}
}