![Nuget](https://img.shields.io/nuget/v/TestDotnetImgProxy)

# dotnet-img-proxy
URL generation, img-proxy compatible

---

https://www.nuget.org/packages/DotnetImgProxy/

## How to use

1) Install the package
2) Configure an environment variable: `IMG_PROXY_BASE_URL` without the trailing slash, defaulted to https://img.unitee.io.
3) Register the service:

```cs
  services.AddDotnetImgProxy(Configuration);
```
4) Use it from the controller:

```cs
public class MyController : Controller
{
    private readonly ImageProxyService _imgProxyService;
    private readonly MyUserManager _myUserManager;

    public MyController(ImageProxyService imgProxyService, MyUserManager myUserManager)
    {
        _imgProxyService = imgProxyService;
        _myUserManager = myUserManager
    }

    public MyAction()
    {
        return View(from user in _myUserManager
                    select new UserViewModel
                    {
                        Avatar = _imgProxyService.GetUrl(user.avatar, new ImageProxyOptions
                        {
                            Width = 32,
                            Height = 32,
                        }
                };
    }
}
```

**OR directly from cshtml (prefered)**

5) In `_ViewImports.cshtml`, add `@using DotnetImgProxy`

6) Use it like:

```razor
@inject ImageProxyService _img;


@foreach (var user in Model)
{
   <img src="@_img.GetUrl(user.Avatar)" />
}
```


## Extra options

Use options for each request in the project

 ```cs
 services.AddDotnetImgProxy(Configuration, "max_bytes:10000");
 ```
 
 Use the extra file to add options that are not (yet) implemented in the `ImageProxyOptions`:
 
 ```cs
 _imgProxyService.GetUrl(user.avatar, new ImageProxyOptions
 {
     Width = 32,
     Height = 32,
     Extra = "max_bytes:10000",
 }
 ```
 
