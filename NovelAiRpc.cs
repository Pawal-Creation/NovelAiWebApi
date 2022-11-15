using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.Net.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NovelAi;

public class NovelAiRpc
{
    private readonly IHttpClientFactory _factory;

    private readonly ILogger<NovelAiRpc> _logger;

    private readonly NovelAiRpcOption _option;

    public NovelAiRpc(ILogger<NovelAiRpc> logger, IHttpClientFactory factory, IOptions<NovelAiRpcOption> option)
    {
        _logger = logger;
        _factory = factory;
        _option = option.Value;
    }

    public async Task<byte[]> Invoke(NovelAiOption option)
    {
        if (option.Height * option.Width > _option.LimitedSize)
        {
            throw new ApplicationException($"Size must smaller than {_option.LimitedSize} px");
        }
        if (option.Image != null && !_option.EnableImageToImage)
        {
            throw new ApplicationException("Image to image was disabled");
        }
        var client = _factory.CreateClient();
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,_option.Url);
        request.Headers.Add("authorization", _option.Authorization);
        var json = option.BuildOption();
        request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        _logger.LogInformation("Genrate request is {}", request.ToString());
        HttpResponseMessage response = await client.SendAsync(request,HttpCompletionOption.ResponseContentRead);
        _logger.LogInformation("Generate reponse status code is {}", response.StatusCode);
        if (response.StatusCode != HttpStatusCode.Created)
        {
            string errorMsg = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Invoke rpc failure:{errorMsg}");
        }
        _logger.LogInformation("Now reading response");
        string data = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response length is {}",data.Length);
        if(data.Length < 128)
        {
            _logger.LogError($"Incorrect response {data}");
            throw new ApplicationException("Invoke rpc failure:response is incorrect");
        }
        data = data.Substring(data.IndexOf("data:") + 5);
        var image = Convert.FromBase64String(data);
        _logger.LogInformation("Generate completed length is {}",image.Length);
        return image;
    }
}