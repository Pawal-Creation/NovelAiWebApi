using System;
using System.Diagnostics.CodeAnalysis;

namespace NovelAi;

public class NovelAiApiOption
{
    [AllowNull]
    public string Authorization { get; set; }
    public uint LimitedSize { get; set; } = UInt32.MaxValue;
    public bool EnableImageToImage { get; set; } = false;
    public string Url { get; set; } = "https://api.novelai.net/ai/generate-image";
}