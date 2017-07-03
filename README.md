# Schoolrooster voor Zermelo

*Note: this is a third party initiative, it's not affiliated with Zermelo Software B.V. in any way.*

*Note: even though the default and only app language is Dutch, all the code, documentation, comments, this file, all issues etc. are in English. Reasons for this are my personal preference for thinking in English while coding, keeping in line with the [official documentation](https://zermelo.atlassian.net/wiki/spaces/DEV) by Zermelo Software B.V., and enabling non-Dutch speakers to understand everything happening here.*

![Schoolrooster voor Zermelo](logo.png)

*Schoolrooster voor Zermelo* is an app that allows students and teachers to check their schedules on all their Windows 10 devices. The app works with all schools and organizations that use the Zermelo portal technology to do their scheduling. The app is built on the Universal Windows Platform.

The backend for connecting to the Zermelo REST API that's used in this app is also available on GitHub: [Zermelo.API](https://github.com/arthurrump/Zermelo.API)

<table style="width: auto">
<thead>
<tr>
  <th>Build status</th>
  <th></th>
</tr>
</thead>
<tbody>
<tr>
  <td>master</td>
  <td><img src="https://build.mobile.azure.com/v0.1/apps/757e7976-b352-4f99-be93-62e61c4e66ca/branches/master/badge" alt="Build status"></td>
</tr>
</tbody>
</table>

## How to contribute

All contributions are welcome! You can make a pull-request and write some code yourself, but please first open an issue detailing the things you want to change. It would be sad if you spend a lot of time building something that's completely beyond the scope of this project, or in a way that's completely different than the rest of the app. If you don't know how to write code, you can open issues too, if you think there's something that should be improved!

## Compiling

The project won't just compile after cloning the project, because there are some API keys needed that, for security reasons, I won't check in into git. To compile the project you'll need to add a `Secrets.cs` file in the `Zermelo.App.UWP` folder, which contains a static class `Secrets` with static properties for every secret:

```csharp
namespace Zermelo.App.UWP
{
    static class Secrets
    {
        public static string Secret => "api-key";
    }
}
```

The secrets that are currently needed are:

* `HockeyAppAppId` (`string`): the App ID for HockeyApp analytics.
* `MobileCenterAppSecret` (`string`): the App Secret for Visual Studio Mobile Center analytics.

---
Licensed under the MIT License (see LICENSE file)