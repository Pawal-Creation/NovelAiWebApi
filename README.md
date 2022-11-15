# NovelAi WebApi
Web api for novelai image generation.

## Apis
| Url | Method |
|-|-|
| /NovelAi | POST |

## Config
## /NovelAi
| Name | Json Path | Description |
|-|-|-|
| Authorization | NovelAi.Authorization | Your novelai authorization token |
| LimitedSize | NovelAi.LimitedSize | The max size(px) of image |
| EnableImageToImage | NovelAi.EnableImageToImage | Enable image to image generation |
| Url | NovelAi.Url | NovelAi Url |

## Parameters
## /NovelAi
| Name | Type | Description |
|-|-|-|
| Tags | String | The image tags |
| Height | UInt32 | The height of image |
| Width | UInt32 | The width of image |
| Image | String | The base64 string of source image(image2image) |