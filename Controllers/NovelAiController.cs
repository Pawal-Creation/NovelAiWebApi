using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings;

using NovelAi;
using NovelAi.Controllers.Forms;

namespace NovelAi.Controllers;

[ApiController]
[Route("[controller]")]
public class NovelAiController : ControllerBase
{
    private readonly ILogger<NovelAiController> _logger;

    private readonly NovelAiRpc _rpc;

    public NovelAiController(ILogger<NovelAiController> logger, NovelAiRpc rpc)
    {
        _logger = logger;
        _rpc = rpc;
    }

    [HttpPost(Name = "NovelAi")]
    public async Task<IActionResult> Post([FromForm]NovelAiForm form)
    {
        _logger.LogInformation("Generate image {}", form);
        if(form.Tags.EndsWith(","))
        {
            form.Tags = form.Tags.Remove(form.Tags.Length - 1);
        }
        NovelAiOption option = new NovelAiOption(form.Tags.Split(','));
        if(form.Height != null)
        {
            option.Height = form.Height.Value;
        }
        if(form.Width != null)
        {
            option.Width = form.Width.Value;
        }
        if(form.Image != null)
        {
            option.Image = form.Image;
        }
        try
        {
            var image = await _rpc.Invoke(option);
            return File(image, "image/png");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _logger.LogError("Fail to generate image {}",form);
            return BadRequest(e.Message);
        }
    }
}
