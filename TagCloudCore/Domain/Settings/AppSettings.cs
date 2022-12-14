﻿using TagCloudCore.Interfaces.Settings;

namespace TagCloudCore.Domain.Settings;

public class AppSettings : IImagePathSettings, IWordsPathSettings
{
    public string ImagePath { get; set; } = null!;
    public string WordsPath { get; set; } = null!;
    public ImageSettings ImageSettings { get; init; } = null!;
}