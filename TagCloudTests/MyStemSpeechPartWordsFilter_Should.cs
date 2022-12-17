﻿using MyStemWrapper;
using MyStemWrapper.Domain;
using TagCloudCoreExtensions.WordsFilters;
using TagCloudCoreExtensions.WordsFilters.Settings;

namespace TagCloudTests;

[TestFixture]
public class MyStemSpeechPartWordsFilter_Should
{
    private const string MyStemExecutablePath = "../../../../MyStemBin/mystem.exe";
    private MyStemSpeechPartWordsFilter _speechPartFilter = null!;
    private ISpeechPartWordsFilterSettings _settings = null!;

    [SetUp]
    public void SetUp()
    {
        _settings = new FiltersSettings
        {
            ExcludedSpeechParts = new HashSet<SpeechPart>(),
            ExcludeUndefined = false
        };
        _speechPartFilter = new MyStemSpeechPartWordsFilter(
            _settings,
            new MyStemWordsGrammarInfoParser(MyStemExecutablePath)
        );
    }

    [Test]
    public void DontSkip_IfCorrect()
    {
        _speechPartFilter.FilterWords(new[] {"слово"}).GetValueOrThrow()
            .Should().ContainSingle()
            .Which.Should().Be("слово");
    }

    [Test]
    public void SkipWord_IfSpeechPartExcluded()
    {
        _settings.ExcludedSpeechParts.Add(SpeechPart.Noun);
        _speechPartFilter.FilterWords(new[] {"слово"}).GetValueOrThrow()
            .Should().BeEmpty();
    }

    [Test]
    public void SkipWordWithUndefinedPartSpeech_IfOptionEnabled()
    {
        _settings.ExcludeUndefined = true;
        _speechPartFilter.FilterWords(new[] {"абв"}).GetValueOrThrow()
            .Should().BeEmpty();
    }

    [Test]
    public void SkipOnlyIncorrectWords_Successfully()
    {
        _settings.ExcludedSpeechParts.Add(SpeechPart.Noun);
        _settings.ExcludeUndefined = true;
        _speechPartFilter.FilterWords(new[] {"слово", "абв", "длинный"}).GetValueOrThrow()
            .Should().ContainSingle()
            .Which.Should().Be("длинный");
    }
}