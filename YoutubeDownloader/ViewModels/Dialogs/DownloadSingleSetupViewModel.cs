﻿using Tyrrrz.Extensions;
using YoutubeDownloader.Services;
using YoutubeDownloader.ViewModels.Components;
using YoutubeDownloader.ViewModels.Framework;
using YoutubeExplode.Models;

namespace YoutubeDownloader.ViewModels.Dialogs
{
    public class DownloadSingleSetupViewModel : DownloadSetupViewModelBase<DownloadViewModel>
    {
        private readonly DialogManager _dialogManager;

        public Video Video { get; set; }

        public DownloadSingleSetupViewModel(IViewModelFactory viewModelFactory, SettingsService settingsService,
            DownloadService downloadService, DialogManager dialogManager)
            : base(viewModelFactory, settingsService, downloadService)
        {
            _dialogManager = dialogManager;
        }

        public bool CanConfirm => Video != null;

        public void Confirm()
        {
            // Prompt user for output file path
            var filter = $"{SelectedFormat.ToUpperInvariant()} file|*.{SelectedFormat}";
            var defaultFileName = GetDefaultFileName(Video, SelectedFormat);
            var filePath = _dialogManager.PromptSaveFilePath(filter, defaultFileName);

            // If canceled - return
            if (filePath.IsBlank())
                return;

            // Enqueue download
            var download = EnqueueDownload(Video, filePath, SelectedFormat);

            // Close dialog with result
            Close(download);
        }
    }
}