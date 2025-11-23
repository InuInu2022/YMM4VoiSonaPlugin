using System.Windows;
using System.Windows.Controls;

namespace YMM4VoiSonaPlugin.Behaviors;

public static class PasswordBoxHelper
{
	public static readonly DependencyProperty BoundPasswordProperty =
		DependencyProperty.RegisterAttached(
			"BoundPassword",
			typeof(string),
			typeof(PasswordBoxHelper),
			new FrameworkPropertyMetadata(
				string.Empty,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				OnBoundPasswordChanged
			)
		);

	public static readonly DependencyProperty BindPasswordProperty =
		DependencyProperty.RegisterAttached(
			"BindPassword",
			typeof(bool),
			typeof(PasswordBoxHelper),
			new PropertyMetadata(false, OnBindPasswordChanged)
		);

	private static readonly DependencyProperty UpdatingPasswordProperty =
		DependencyProperty.RegisterAttached(
			"UpdatingPassword",
			typeof(bool),
			typeof(PasswordBoxHelper),
			new PropertyMetadata(false)
		);

	public static void SetBindPassword(DependencyObject dp, bool value) =>
		dp.SetValue(BindPasswordProperty, value);

	public static bool GetBindPassword(DependencyObject dp) =>
		(bool)dp.GetValue(BindPasswordProperty);

	public static string GetBoundPassword(DependencyObject dp) =>
		(string)dp.GetValue(BoundPasswordProperty);

	public static void SetBoundPassword(DependencyObject dp, string value) =>
		dp.SetValue(BoundPasswordProperty, value);

	private static bool GetUpdatingPassword(DependencyObject dp) =>
		(bool)dp.GetValue(UpdatingPasswordProperty);

	private static void SetUpdatingPassword(DependencyObject dp, bool value) =>
		dp.SetValue(UpdatingPasswordProperty, value);

	private static void OnBoundPasswordChanged(
		DependencyObject d,
		DependencyPropertyChangedEventArgs e
	)
	{
		if (d is PasswordBox passwordBox)
		{
			passwordBox.PasswordChanged -= HandlePasswordChanged;
			if (!GetUpdatingPassword(passwordBox))
			{
				passwordBox.Password = e.NewValue?.ToString() ?? string.Empty;
			}
			passwordBox.PasswordChanged += HandlePasswordChanged;
		}
	}

	private static void OnBindPasswordChanged(
		DependencyObject d,
		DependencyPropertyChangedEventArgs e
	)
	{
		if (d is PasswordBox passwordBox)
		{
			var wasBound = (bool)e.OldValue;
			var needBind = (bool)e.NewValue;

			if (wasBound)
			{
				passwordBox.PasswordChanged -= HandlePasswordChanged;
			}
			if (needBind)
			{
				passwordBox.PasswordChanged += HandlePasswordChanged;
				passwordBox.Password = GetBoundPassword(passwordBox) ?? string.Empty;
			}
		}
	}

	private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
	{
		var passwordBox = (PasswordBox)sender;
		SetUpdatingPassword(passwordBox, true);
		SetBoundPassword(passwordBox, passwordBox.Password);
		SetUpdatingPassword(passwordBox, false);
	}
}
