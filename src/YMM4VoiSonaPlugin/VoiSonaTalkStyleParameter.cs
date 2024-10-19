using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using YMM4VoiSonaPlugin.View;

using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Plugin.Voice;

namespace YMM4VoiSonaPlugin;

public partial class VoiSonaTalkStyleParameter : VoiceParameterBase
{
	double _value;
	string _displayName = string.Empty;
	string _description = string.Empty;

	public string DisplayName { get => _displayName; init => Set(ref _displayName, value); }
	public string Description { get => _description; init => Set(ref _description, value); }

	[VoiSonaStyleDisplay]
	[TextBoxSlider("F2", "", -1, 2, Delay = -1)]
	[Range(0, 1)]
	[DefaultValue(0.0)]
	public double Value { get => _value; set => Set(ref _value, value);}
}