﻿using System.Windows;

namespace VideoJam.Windows
{
	/// <summary>
	/// Логика взаимодействия для AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
			
			// Вставьте ниже код, необходимый для создания объекта.
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}