using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Analytics;
using Zermelo.API.Exceptions;

namespace Zermelo.App.UWP.Helpers
{
    class ExceptionHelper
    {
        public static void HandleException(Exception ex, string location, Action<string> ShowError)
        {
            switch (ex)
            {
                case ZermeloHttpException zex:
                    if (zex.StatusCode == 404)
                        ShowError("De opgevraagde gegevens zijn niet gevonden in Zermelo.");
                    else if (zex.StatusCode == 403)
                        ShowError("Je hebt niet voldoende rechten om deze informatie te bekijken.");
                    else
                        ShowError($"Er is iets fout gegaan. Zermelo geeft de volgende foutmelding: {zex.StatusCode} {zex.Status}");

                    Analytics.TrackEvent("ZermeloHttpException", new Dictionary<string, string>
                    {
                        { "Location", location },
                        { "Message", zex.Message },
                        { "StatusCode", zex.StatusCode.ToString() },
                        { "ResponseContent", zex.ResponseContent }
                    });
                    break;

                default:
                    ShowError("Er is iets fout gegaan. De ontwikkelaar wordt op de hoogte gesteld.");

                    Analytics.TrackEvent("UnknownException", new Dictionary<string, string>
                    {
                        { "Location", location },
                        { "Message", ex.Message },
                        { "Source", ex.Source },
                        { "StackTrace", ex.StackTrace }
                    });
                    break;
            }
        }
    }
}
