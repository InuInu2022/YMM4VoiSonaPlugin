
using System.ComponentModel.DataAnnotations;

using YukkuriMovieMaker.ItemEditor;

namespace YMM4VoiSonaPlugin.View;

[AttributeUsage(AttributeTargets.Property)]
public sealed class VoiSonaStyleDisplayAttribute : CustomDisplayAttributeBase
{
	public override string? GetDescription(object instance)
	{
		if (instance is not VoiSonaTalkStyleParameter styleParam) return null;
		return GetParamProp(styleParam, nameof(VoiSonaTalkStyleParameter.Description));
	}

	static string? GetParamProp(
		VoiSonaTalkStyleParameter styleParam,
		string propName)
	{
		var temp = styleParam
			.GetType()
			.GetProperty(propName)?
			.GetValue(styleParam);
		return temp is string propValue ? propValue : null;
	}

	public override string? GetGroupName(object instance) => null;

	public override string? GetName(object instance)
	{
		if (instance is not VoiSonaTalkStyleParameter styleParam) return null;

		return GetParamProp(styleParam, nameof(VoiSonaTalkStyleParameter.DisplayName));
	}

	public override bool? GetAutoGenerateField(object instance) => false;

	public override bool? GetAutoGenerateFilter(object instance) => true;

	public override int? GetOrder(object instance) => 0;
}
