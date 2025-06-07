namespace YMM4VoiSonaPlugin;

public static class VoiSonaCastManager
{
	#pragma warning disable S1075 // URIs should not be hardcoded
	static readonly Dictionary<string, CastData> castData = new(StringComparer.Ordinal)
	{
		{"Tanaka San", new ("https://voisona.com/static/pdf/ja/tanaka-san_guidelines.pdf","Techno-speech, Inc", "") },
		{"Sato Sasara", new ("https://voisona.com/static/pdf/ja/sato-sasara_guidelines.pdf","CeVIO", "") },
		{"Suzuki Tsudumi", new ("https://voisona.com/static/pdf/ja/suzuki-tsudumi_guidelines.pdf","CeVIO", "") },
		{"Takahashi", new ("https://voisona.com/static/pdf/ja/takahashi_guidelines.pdf","CeVIO", "") },
		{"Futaba Minato", new ("https://voisona.com/static/pdf/ja/futaba-minato_guidelines.pdf","Gasoline Alley Inc.", "nc310893") },
		{"Tsurumaki Maki", new ("https://voisona.com/static/pdf/ja/tsurumaki-maki_guidelines.pdf","AHS", "") },
		{"Tsurumaki Maki English", new ("https://voisona.com/static/pdf/ja/tsurumaki-maki_guidelines.pdf","AHS", "") },
		{"Koharu Rikka", new ("https://voisona.com/static/pdf/ja/koharu-rikka_guidelines.pdf","TOKYO6 ENTERTAINMENT", "nc309924") },
		{"Natsuki Karin", new ("https://voisona.com/static/pdf/ja/natsuki-karin_guidelines.pdf","TOKYO6 ENTERTAINMENT", "nc309924") },
		{"Hanakuma Chifuyu", new ("https://voisona.com/static/pdf/ja/hanakuma-chifuyu_guidelines.pdf","TOKYO6 ENTERTAINMENT", "nc309924") },
		{"Tamaki", new ("https://voisona.com/static/pdf/ja/tamaki_guidelines.pdf","のりプロ", "") },
		{"Soyogi Soyogi", new ("https://voisona.com/static/pdf/ja/soyogi-soyogi_guidelines.pdf","そよぎフラクタル", "") },
		{"IA", new ("https://voisona.com/static/pdf/ja/ia_guidelines.pdf","1st PLACE", "") },
		{"OИE", new ("https://voisona.com/static/pdf/ja/one_guidelines.pdf","1st PLACE", "") },
		{"LeuR", new("https://voisona.com/static/pdf/ja/leur_guidelines.pdf","rurudot","") },
	};
	#pragma warning restore S1075 // URIs should not be hardcoded

	internal static string GetTermUrl(string castName)
	{
		return GetCastData(castName).TermUrl;
	}

	internal static string GetAuthor(string castName)
	{
		return GetCastData(castName).Author;
	}

	internal static string GetContentId(string castName)
	{
		return GetCastData(castName).ContentId;
	}

	internal static CastData GetCastData(string castName)
	{
		var result = castData.TryGetValue(castName, out var data);
		return !result || data is null
			? new CastData("","","")
			: data;
	}
}

/// <summary>
/// ボイスのデータ
/// </summary>
/// <param name="TermUrl">規約</param>
/// <param name="Author">製作者</param>
/// <param name="ContentId">ニコニコID</param>
internal record CastData(
	string TermUrl,
	string Author,
	string ContentId
);
