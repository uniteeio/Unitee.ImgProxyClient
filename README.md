![Nuget](https://img.shields.io/nuget/v/Unitee.ImgProxyClient)

# Unitee.ImgProxy

Url generation for ImgProxy

---

https://www.nuget.org/packages/Unitee.ImgProxyClient/

## Quick start

1) Install the package (`dotnet add package Unitee.ImgProxyClient`)
2) Configure your AppSettings:

```json
{
    ...,

    "ImgProxy": {
        "Key": "mysecretkey", // optional
        "Salt": "mysecretsalt", // optional
        "BaseUrl": "https://my-instance-of-img-proxy-or-cdn.com"
    }
}
```

More info on how to generate salt and key: https://docs.imgproxy.net/configuration?id=url-signature
Be sure you have correclty configured `IMGPROXY_KEY` and `IMGPROXY_SALT` on the server with the same values.  
If `Key` or `Salt` is missing, we will generate **insecure** urls.

Remember, you can sign the url even if `IMGPROXY_KEY` or `IMGPROXY_SALT` has been provided. So that, you can first migrate the code, then the server.


3) Register the service:

```cs
builder.Services.AddImgProxyClient(builder.Configuration, "max_bytes:1000000" /* optional */);
```

You can add global options that will be used for all requests.

4) Use it from the controller:

```cs
public class MyController : Controller
{
    private readonly ImgProxyService _imgProxyService;
    private readonly MyUserManager _myUserManager;

    public MyController(ImgProxyService imgProxyService, MyUserManager myUserManager)
    {
        _imgProxyService = imgProxyService;
        _myUserManager = myUserManager
    }

    public MyAction()
    {
        return View(from user in _myUserManager.GetAll()
                    select new UserViewModel
                    {
                        Avatar = _imgProxyService.GetUrl(user.Avatar, new ImgProcessingOptions
                        {
                            Width = 32,
                            Height = 32,
                        }
                };
        )
    }
}
```

**OR directly from cshtml (prefered)**

5) In `_ViewImports.cshtml`, add `@using Unitee.ImgProxyClient`

6) Use it like:

```razor
@inject ImgProxyService _img;

@foreach (var user in Model)
{
   <img src="@_img.GetUrl(user.Avatar, new ImgProcessingOptions { Format = "wepb" })" />
}
```

## Available options

List of the options implemented in the `ImageProxyOptions.cs`:

[./Unitee.ImgProxyClient/ImgProcessingOptions.cs](./Unitee.ImgProxyClient/ImgProcessingOptions.cs)

## Extra options

Use options for each request in the project

 ```cs
 builder.Services.AddImgProxyClient(builder.Configuration, "max_bytes:10000");
 ```
 
 Use the `Extra` field to add options that are not (yet) implemented in the `ImgProcessingOptions`:
 
 ```cs
 _imgProxyService.GetUrl(user.Avatar, new ImgProcessingOptions
 {
     Width = 32,
     Height = 32,
     Extra = "max_bytes:10000",
 }
 ```
