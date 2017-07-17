using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Azure.Mobile.Analytics;
using Zermelo.API.Exceptions;

namespace Zermelo.App.UWP.Helpers
{
    class ExceptionHelper
    {
        public static void HandleException(Exception ex, string location, Action<string> ShowError,
            string zermelo400Text = "De opgevraagde gegevens zijn niet gevonden in Zermelo.",
            string zermelo404Text = "De opgevraagde gegevens zijn niet gevonden in Zermelo.",
            string zermelo403Text = "Je hebt niet voldoende rechten om deze informatie te bekijken.",
            string zermeloDefaultText = "Er is iets fout gegaan. Zermelo geeft de volgende foutmelding: {0} {1}",
            string defaultText = "Er is iets fout gegaan. De ontwikkelaar wordt op de hoogte gesteld.")
        {
            if (Debugger.IsAttached)
                Debugger.Break();

            switch (ex)
            {
                case ZermeloHttpException zex:
                    if (zex.StatusCode == 400)
                        ShowError(zermelo400Text);
                    else if (zex.StatusCode == 404)
                        ShowError(zermelo404Text);
                    else if (zex.StatusCode == 403)
                        ShowError(zermelo403Text);
                    else
                        ShowError(string.Format(zermeloDefaultText, zex.StatusCode, zex.Status));

                    Analytics.TrackEvent("ZermeloHttpException", new Dictionary<string, string>
                    {
                        { "Location", location },
                        { "Message", zex.Message.MaxLength(64) },
                        { "StatusCode", zex.StatusCode.ToString() },
                        { "ResponseContent", zex.ResponseContent.MaxLength(64) }
                    });
                    break;

                case System.Net.Http.HttpRequestException hre:
                    Analytics.TrackEvent("HttpRequestException", new Dictionary<string, string>
                    {
                        { "Location", location },
                        { "Message", hre.Message.MaxLength(64) },
                        { "Source", hre.Source },
                        { "StackTrace", ex.StackTrace.MaxLength(64) }
                    });
                    break;

                default:
                    ShowError(defaultText);

                    Analytics.TrackEvent("UnknownException", new Dictionary<string, string>
                    {
                        { "Location", location },
                        { "Message", ex.Message.MaxLength(64) },
                        { "Source", ex.Source },
                        { "StackTrace", ex.StackTrace.MaxLength(64) }
                    });
                    break;
            }
        }
    }
}
