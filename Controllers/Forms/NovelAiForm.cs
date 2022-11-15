using System;
using System.Diagnostics.CodeAnalysis;

namespace NovelAi.Controllers.Forms;

public class NovelAiForm
{
    [AllowNull]
    public string Tags { get; set; }

    public UInt16? Width { get; set; } = null;
    public UInt16? Height { get; set; } = null;

    public string? Image { get; set; } = null;

    public override string ToString()
    {
        return $"Tags:{Tags} Width:{Width} Height:{Height} Image:{Image}";
    }
}