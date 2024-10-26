using System.ComponentModel.DataAnnotations;

using YukkuriMovieMaker.Controls;
using YukkuriMovieMaker.Plugin.Voice;
using YukkuriMovieMaker.UndoRedo;

namespace YMM4VoiSonaPlugin;

public partial class VoiSonaTalkPronounce : UndoRedoable, IVoicePronounce
{
	public VoiSonaTalkPronounce()
	{
	}

	public void BeginEdit()
	{

	}

	public ValueTask EndEditAsync()
	{
		return ValueTask.CompletedTask;
	}

}
