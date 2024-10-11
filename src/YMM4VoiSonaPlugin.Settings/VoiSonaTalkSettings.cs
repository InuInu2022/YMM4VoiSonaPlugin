using System.Windows.Controls;

namespace YMM4VoiSonaPlugin.Settings;

public class VoiSonaTalkSettings : UserControl
{
	public VoiSonaTalkSettings()
	{
		var stack = new StackPanel();


		var tbox = new TextBox { Text = "Hello, World!" };
		stack.Children.Add(tbox);


		Content = stack;
	}
}
