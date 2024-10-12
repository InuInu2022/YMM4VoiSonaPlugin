using System.Diagnostics;
using System.Reflection;

using Epoxy;

using YmmeUtil.Common;

namespace YMM4VoiSonaPlugin.ViewModel;

[ViewModel]
public class TalkSettingViewModel
{
	public string? PluginVersion { get; } = "x.x.x";
	public string UpdateMessage { get; set; } = "Update checkボタンを押してください";

	public bool HasUpdate { get; set; }
	public bool IsDownloadable { get; set; }

	public Command UpdateCheck { get; set; }
	public Command Download { get; set; }
	public Command OpenGithub { get; set; }

	public TalkSettingViewModel()
	{
		PluginVersion = //AssemblyUtil.GetVersionString(typeof(VoiSonaTalkPlugin));

		typeof(TalkSettingViewModel).Assembly
			.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute))
			.Cast<AssemblyInformationalVersionAttribute>()
			.FirstOrDefault()?
			.InformationalVersion ?? "unknown";

		var executingAssembly = Assembly
			.GetExecutingAssembly()
			.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute))
			.Cast<AssemblyInformationalVersionAttribute>()
			.FirstOrDefault()?
			.InformationalVersion;
        Console.WriteLine($"exe ver: {executingAssembly}");

		UpdateCheck = Command.Factory.Create(async () =>
		{
			var checker = UpdateChecker
				.Build("InuInu2022", "YMM4VoiSonaPlugin");
			HasUpdate = await checker
				.IsAvailableAsync(typeof(TalkSettingViewModel))
				.ConfigureAwait(true);
			UpdateMessage = GetUpdateMessage();
		});

		Download = Command.Factory.Create(async () =>
		{
			var checker = UpdateChecker
				.Build("InuInu2022", "YMM4VoiSonaPlugin");
			var result = await checker.GetDownloadUrlAsync(
				"YMM4VoiSonaPlugin.ymme",
				"https://github.com/InuInu2022/YMM4VoiSonaPlugin/releases");
			await OpenUrlAsync(result)
				.ConfigureAwait(true);
		});


		OpenGithub = Command.Factory.Create(async () =>
			await OpenUrlAsync("https://github.com/InuInu2022/YMM4VoiSonaPlugin")
				.ConfigureAwait(true)
		);
	}

	static async Task<Process> OpenUrlAsync(string openUrl)
	{
		return await Task.Run(()
			=> Process.Start(new ProcessStartInfo()
			{
				FileName = openUrl,
				UseShellExecute = true,
			}) ?? new()
		);
	}

	string GetUpdateMessage(){
		return HasUpdate ? "プラグインの更新があります" : "プラグインは最新です";
	}
}
