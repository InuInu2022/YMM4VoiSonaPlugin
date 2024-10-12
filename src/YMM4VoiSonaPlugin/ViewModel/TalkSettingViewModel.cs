using System.Diagnostics;
using System.Reflection;

using Epoxy;

using YmmeUtil.Common;

namespace YMM4VoiSonaPlugin.ViewModel;

[ViewModel]
public class TalkSettingViewModel
{
	public string? PluginVersion { get; }
	public string UpdateMessage { get; set; } = "Update checkボタンを押してください";

	public bool HasUpdate { get; set; }
	public bool IsDownloadable { get; set; }

	public Command UpdateCheck { get; set; }
	public Command Download { get; set; }
	public Command OpenGithub { get; set; }

	private readonly UpdateChecker checker;

	public TalkSettingViewModel()
	{
		PluginVersion = AssemblyUtil.GetVersionString(typeof(VoiSonaTalkPlugin));

		checker = UpdateChecker
			.Build("InuInu2022", "YMM4VoiSonaPlugin");

		UpdateCheck = Command.Factory.Create(async () =>
		{
			HasUpdate = await checker
				.IsAvailableAsync(typeof(TalkSettingViewModel))
				.ConfigureAwait(true);
			IsDownloadable = HasUpdate;
			UpdateMessage = await GetUpdateMessageAsync().ConfigureAwait(true);
		});

		Download = Command.Factory.Create(async () =>
		{
			try{
				var result = await checker.GetDownloadUrlAsync(
					"YMM4VoiSonaPlugin.ymme",
					"https://github.com/InuInu2022/YMM4VoiSonaPlugin/releases")
					.ConfigureAwait(false);
				await OpenUrlAsync(result)
					.ConfigureAwait(false);
			}catch(Exception e)
			{
				await Console.Error.WriteLineAsync(e.Message).ConfigureAwait(false);
			}
		});


		OpenGithub = Command.Factory.Create(async () =>
			await OpenUrlAsync("https://github.com/InuInu2022/YMM4VoiSonaPlugin")
				.ConfigureAwait(false)
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
		).ConfigureAwait(false);
	}

	async ValueTask<string> GetUpdateMessageAsync(){
		return HasUpdate
			? $"プラグインの更新があります {await checker.GetRepositoryVersionAsync().ConfigureAwait(false)}"
			: "プラグインは最新です";
	}
}
