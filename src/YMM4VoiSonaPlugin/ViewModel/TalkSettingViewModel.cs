using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Shell;

using Epoxy;

using SonaBridge;
using SonaBridge.Core.Common;

using YmmeUtil.Common;
using YmmeUtil.Ymm4;

namespace YMM4VoiSonaPlugin.ViewModel;

[ViewModel]
public class TalkSettingViewModel
{
	public string? PluginVersion { get; }
	public string UpdateMessage { get; set; } = "Update checkボタンを押してください";

	public bool IsPreloading { get;set;}
	public bool IsPreloadButtonEnabled { get; set; } = true;
	public bool HasUpdate { get; set; }
	public bool IsUpdateCheckEnabled { get; set; } = true;
	public bool IsDownloadable { get; set; }

	public Command PreloadVoice { get; set; }
	public Command UpdateCheck { get; set; }
	public Command Download { get; set; }
	public Command OpenGithub { get; set; }

	private readonly UpdateChecker checker;

	public TalkSettingViewModel()
	{
		PluginVersion = AssemblyUtil.GetVersionString(typeof(VoiSonaTalkPlugin));

		checker = UpdateChecker
			.Build("InuInu2022", "YMM4VoiSonaPlugin");

		PreloadVoice = Command.Factory.Create(PreloadAsync);

		UpdateCheck = Command.Factory.Create(async () =>
		{
			IsUpdateCheckEnabled = false;
			TaskbarUtil.StartIndeterminate();

			HasUpdate = await checker
				.IsAvailableAsync(typeof(TalkSettingViewModel))
				.ConfigureAwait(true);
			IsDownloadable = HasUpdate;
			UpdateMessage = await GetUpdateMessageAsync().ConfigureAwait(true);

			IsUpdateCheckEnabled = true;
			TaskbarUtil.FinishIndeterminate();
		});

		Download = Command.Factory.Create(async () =>
		{
			try
			{
				var result = await checker.GetDownloadUrlAsync(
					"YMM4VoiSonaPlugin.ymme",
					"https://github.com/InuInu2022/YMM4VoiSonaPlugin/releases")
					.ConfigureAwait(false);
				await OpenUrlAsync(result)
					.ConfigureAwait(false);
			}
			catch (Exception e)
			{
				await Console.Error.WriteLineAsync(e.Message).ConfigureAwait(false);
			}
		});


		OpenGithub = Command.Factory.Create(async () =>
			await OpenUrlAsync("https://github.com/InuInu2022/YMM4VoiSonaPlugin")
				.ConfigureAwait(false)
		);
	}

	async ValueTask PreloadAsync()
	{
		IsPreloading = true;
		IsPreloadButtonEnabled = false;

		TaskbarUtil.StartIndeterminate();

		var provider = new TalkServiceProvider();
		using var service = provider.GetService<ITalkAutoService>();
		await service.StartAsync().ConfigureAwait(true);

		IsPreloading = false;
		IsPreloadButtonEnabled = true;
		TaskbarUtil.FinishIndeterminate();
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
