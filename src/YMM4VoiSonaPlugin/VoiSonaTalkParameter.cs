using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Plugin.Voice;

using YMM4VoiSonaPlugin.View;
using System.Diagnostics;

namespace YMM4VoiSonaPlugin;

public partial class VoiSonaTalkParameter : VoiceParameterBase
{
	double _speed = 1.0;
	double _volume;
	double _pitch;
	double _alpha;
	double _intonation = 1.0;
	double _husky;

	ImmutableList<VoiSonaTalkStyleParameter> _styles = [];
	string _voice = "";

	ImmutableList<string>? _preset;

	public string Voice
	{
		get => _voice;
		set => Set(ref _voice, value);
	}

	#region synthesis_options
	/*
	bool _isDoSynth;

	[Display(GroupName = "合成オプション", Name = "パラメータ変更再合成", Description = "パラメータ変更時に再合成をするかどうか。")]
	[ToggleSlider]
	public bool IsDoSynth {
		get => _isDoSynth;
		set => Set(ref _isDoSynth, value);
	}
	*/

	#endregion


	[Display(Name = "プリセット", Description = "プリセットを選択")]
	[VoiSonaTalkPresetDisplay]
	public ImmutableList<string>? Preset {get => _preset; set => Set(ref _preset, value); }

	public int PresetIndex { get; set; } = -1;

	[Display(Name = nameof(Speed), Description = "話速を調整")]
	[TextBoxSlider("F2", "", 0.2, 5, Delay = -1)]
	[Range(0.2, 5)]
	[DefaultValue(1.0)]
	public double Speed
	{
		get => _speed;
		set => Set(ref _speed, value);
	}

	[Display(Name = nameof(Volume), Description = "ボリュームを調整")]
	[TextBoxSlider("F2", "", -8, 8, Delay = -1)]
	[Range(-8, 8)]
	[DefaultValue(0.0)]
	public double Volume
	{
		get => _volume;
		set => Set(ref _volume, value);
	}

	[Display(Name = nameof(Pitch), Description = "ピッチの高さを調整")]
	[TextBoxSlider("F2", "", -600, 600, Delay = -1)]
	[Range(-600, 600)]
	[DefaultValue(0.0)]
	public double Pitch
	{
		get => _pitch;
		set => Set(ref _pitch, value);
	}

	[Display(Name = nameof(Alpha), Description = "声質の詳細を調整")]
	[TextBoxSlider("F2", "", -1, 1, Delay = -1)]
	[Range(-1, 1)]
	[DefaultValue(0.0)]
	public double Alpha
	{
		get => _alpha;
		set => Set(ref _alpha, value);
	}

	[Display(Name = "Into.", Description = "抑揚を調整")]
	[TextBoxSlider("F2", "", 0.0, 2, Delay = -1)]
	[Range(0, 2)]
	[DefaultValue(1.0)]
	public double Intonation
	{
		get => _intonation;
		set => Set(ref _intonation, value);
	}

	[Display(Name = "Hus.", Description = "声質の擦れ具合を調整")]
	[TextBoxSlider("F2", "", -20, 20, Delay = -1)]
	[Range(-20, 20)]
	[DefaultValue(0.0)]
	public double Husky
	{
		get => _husky;
		set => Set(ref _husky, value);
	}

	[Display(AutoGenerateField = true)]
	[JsonProperty]
	public ImmutableList<VoiSonaTalkStyleParameter> ItemsCollection
	{
		get => _styles;
		set
		{
			UnsubscribeFromItems(_styles);
			try
			{
				Set(ref _styles, value);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
			}

			SubscribeToItems(_styles);
		}
	}

	 // 個々のアイテムの PropertyChanged イベントに登録
    void SubscribeToItems(ImmutableList<VoiSonaTalkStyleParameter> newItems)
    {
        foreach (var item in newItems)
        {
            item.PropertyChanged += Item_PropertyChanged;
        }
    }

	// 個々のアイテムの PropertyChanged イベントの登録解除
	void UnsubscribeFromItems(ImmutableList<VoiSonaTalkStyleParameter> oldItems)
    {
        foreach (var item in oldItems)
        {
            item.PropertyChanged -= Item_PropertyChanged;
        }
    }

	void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		OnPropertyChanged($"{nameof(ItemsCollection)}.{e.PropertyName}");
	}
}
