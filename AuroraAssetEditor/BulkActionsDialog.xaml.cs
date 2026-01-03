// 
//  BulkActionsDialog.xaml.cs
//  AuroraAssetEditor
// 
//  Created by Cehbab on 23/08/2025
//  Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraAssetEditor {
	using System;
	using System.ComponentModel;
    using System.Windows;
	using System.Windows.Controls;
	using AuroraAssetEditor.Classes;

	public partial class BulkActionsDialog
    {
		private enum OnlineAssetSources
		{
			XboxUnityOption = 0,
			ArchiveOption = 1
		}
		private XboxLocale[] _locales;

        public BulkActionsDialog(Window owner) {
            InitializeComponent();
            Icon = App.WpfIcon;
            Owner = owner;

			SourceBox.SelectedIndex = (int)OnlineAssetSources.XboxUnityOption;

			#region Xbox.com Locale worker

			var bw = new BackgroundWorker();
            bw.DoWork += LocaleWorkerDoWork;
            bw.RunWorkerCompleted += (sender, args) => {
										LocaleBox.ItemsSource = _locales;
										var index = 0;
										for (var i = 0; i < _locales.Length; i++)
										{
											if (!_locales[i].Locale.Equals("en-us", StringComparison.InvariantCultureIgnoreCase))
												continue;
											index = i;
											break;
										}
										LocaleBox.SelectedIndex = index;
									};
            bw.RunWorkerAsync();

			#endregion
		}

		private void LocaleWorkerDoWork(object sender, DoWorkEventArgs doWorkEventArgs) { _locales = XboxAssetDownloader.GetLocales(); }

		public XboxLocale Locale { get { return LocaleBox.SelectedItem as XboxLocale; } }

		public bool ReplaceExisting { get { return ReplaceExistingChk.IsChecked ?? false; } }

        public bool CoverArtOnly { get { return CoverArtOnlyChk.IsChecked ?? false; } }

		public bool UnitySource {  get { return SourceBox.SelectedIndex == (int)OnlineAssetSources.XboxUnityOption; } }

		private void CoverArtOnlyChk_Checked(Object sender, RoutedEventArgs e)
		{
			LocaleGrid.Visibility = Visibility.Hidden;
		}

		private void CoverArtOnlyChk_Unchecked(Object sender, RoutedEventArgs e)
		{
			LocaleGrid.Visibility = Visibility.Visible;
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e) { DialogResult = true; }

	}
}
