using System.Text;
using System.Text.Json;

namespace NovelAi;

public class NovelAiOption
{
    private static string _fixedInput = "{{masterpiece}}, best quality";

    private static string _defaultModel = "nai-diffusion";

    private static UInt16 _defaultScale = 11;

    private static string _defaultSampler = "k_euler_ancestral";

    private static UInt16 _defaultSteps = 28;

    private static UInt16 _stepsWithImage = 50;

    private static Random _randomSeed = new Random(DateTime.UtcNow.GetHashCode());

    private static UInt16 _defaultNSamples = 1;

    private static UInt16 _defaultUcPreset = 0;

    private static UInt16 _defaultWidth = 512;

    private static UInt16 _defaultHeight = 768;

    private static bool _defaultQualityToggle = true;

    private static double _defaultStrength = 0.7;

    private static double _defaultNoise = 0.2;

    private static string _defaultUc = "lowres, bad anatomy, bad hands, text, error, missing fingers, extra digit, fewer digits, cropped, worst quality, low quality, normal quality, jpeg artifacts, signature, watermark, username, blurry";

    private string _input;

    private string _model;

    private UInt16 _scale;

    private string _sampler;

    private UInt16 _steps;

    private UInt32 _seed;

    private UInt16 _NSamples;

    private UInt16 _ucPreset;

    private UInt16 _width;

    private UInt16 _height;

    private bool _qualityToggle;

    private string? _image;

    private double? _strength;

    private double? _noise;

    private string _uc;

    public string Input { get => _input; }

    public string Model { get => _model; set => _model = value; }
    public ushort Scale { get => _scale; set => _scale = value; }
    public string Sampler { get => _sampler; set => _sampler = value; }
    public ushort Steps { get => _steps; set => _steps = value; }
    public uint Seed { get => _seed; set => _seed = value; }
    public ushort NSamples { get => _NSamples; set => _NSamples = value; }
    public ushort Height { get => _height; set => _height = value; }
    public ushort Width { get => _width; set => _width = value; }
    public bool QualityToggle { get => _qualityToggle; set => _qualityToggle = value; }

    public string? Image
    {
        get => _image;
        set
        {
            _image = value;
            _strength = _defaultStrength;
            _noise = _defaultNoise;
            _steps = _stepsWithImage;
        }
    }
    public double? Strength
    {
        get => _strength;
        set
        {
            if (value > 1 || value < 0.1)
            {
                value = _defaultStrength;
            }
            _strength = value;
        }
    }
    public double? Noise
    {
        get => _noise;
        set
        {
            if(value > 1 || value < 0.1)
            {
                value = _defaultNoise;
            }
            _noise = value;
        }
    }



    public string Uc { get => _uc; }
    public ushort UcPreset { get => _ucPreset; set => _ucPreset = value; }

    public NovelAiOption(ICollection<string> tags, ICollection<string>? ucTags = null)
    {
        _image = null;
        _strength = null;
        _noise = null;
        StringBuilder builder = new StringBuilder();
        builder.Append(_fixedInput);
        bool halfSplited = false;
        if (tags.Count != 0)
        {
            foreach (var tag in tags)
            {
                if (tag != "" && tag != "," && tag != ", " && tag != " ")
                {
                    if (!halfSplited)
                    {
                        builder.Append(", ");
                    }
                    else
                    {
                        builder.Append(' ');
                    }
                    builder.Append(tag);
                    halfSplited = tag.EndsWith(",");
                }
            }
        }
        _input = builder.ToString();
        _model = _defaultModel;
        _width = _defaultWidth;
        _height = _defaultHeight;
        _scale = _defaultScale;
        _sampler = _defaultSampler;
        _steps = _defaultSteps;
        _seed = Convert.ToUInt32(_randomSeed.NextInt64(0, Int32.MaxValue));
        _NSamples = _defaultNSamples;
        _ucPreset = _defaultUcPreset;
        _qualityToggle = _defaultQualityToggle;
        if (ucTags != null && ucTags.Count != 0)
        {
            builder.Clear();
            foreach (var tag in ucTags)
            {
                builder.Append(tag);
                builder.Append(", ");
            }
            _uc = builder.ToString();
            //remove ", "
            _uc.Remove(_uc.Count() - 2, 2);
        }
        else
        {
            _uc = _defaultUc;
        }
    }

    public string BuildOption()
    {
        if (_image != null)
        {
            var parameterObject = new { width = Width, height = Height, scale = Scale, sampler = Sampler, steps = Steps, seed = Seed, n_samples = NSamples, ucPreset = UcPreset, qualityToggle = QualityToggle,strength = Strength,noise = Noise, image = Image, uc = Uc };
            var tmp = new { input = Input, model = Model, parameters = parameterObject };
            return JsonSerializer.Serialize(tmp);
        }
        else
        {
            var parameterObject = new { width = Width, height = Height, scale = Scale, sampler = Sampler, steps = Steps, seed = Seed, n_samples = NSamples, ucPreset = UcPreset, qualityToggle = QualityToggle, uc = Uc };
            var tmp = new { input = Input, model = Model, parameters = parameterObject };
            return JsonSerializer.Serialize(tmp);
        }
    }
}