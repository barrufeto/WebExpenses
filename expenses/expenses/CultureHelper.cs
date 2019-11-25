using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Threading;


namespace expenses
{
    
 /// <summary>
        /// Set the culture of the user for the rest of the application. The culture can be overridden by the user. 
        /// If it is overridden a cookie is set with the selected culture. For testing don't forget to remove the cookie for selecting the default browser language again.
        /// </summary>
        public static class CultureHelper
        {

            #region Constants

            /// <summary>
            /// The cookie name of the selected culture. By default there isn't any cookie but only the culture of the browser. 
            /// If the user selects a specific culture, the cookie is added.
            /// </summary>
            const string CookieName = "PreferredCulture";

            #endregion

            #region Fields

            /// <summary>
            /// The supported cultures of this application. If a new localization file is added to the application, the cultureinfo must be added as well.
            /// </summary>
            public static readonly CultureInfo[] SupportedCultures = new CultureInfo[]
            {
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("es-ES"),
                //CultureInfo.GetCultureInfo("de-DE"), 
                //CultureInfo.GetCultureInfo("fr-FR"), 
                //CultureInfo.GetCultureInfo("es-ES"), 
            };

            #endregion

            #region Public Methods

            public static void ApplyUserCulture(this HttpRequest request)
            {
                ApplyUserCulture(request.Headers, request.Cookies);
            }

            public static CultureInfo GetMatch(CultureInfo[] acceptedCultures, CultureInfo[] supportedCultures, Func<CultureInfo, CultureInfo, bool> predicate)
            {
                foreach (var acceptedCulture in acceptedCultures)
                {
                    var match = supportedCultures
                        .Where(supportedCulture => predicate(acceptedCulture, supportedCulture))
                        .FirstOrDefault();

                    if (match != null)
                    {
                        return match;
                    }
                }

                return null;
            }

            public static CultureInfo GetMatchingCulture(CultureInfo[] acceptedCultures, CultureInfo[] supportedCultures)
            {
                return
                    // first pass: exact matches as well as requested neutral matching supported region 
                    // supported: en-US, de-DE 
                    // requested: de, en-US;q=0.8 
                    // => de-DE! (de has precendence over en-US) 
                    GetMatch(acceptedCultures, supportedCultures, MatchesCompletely)
                    // second pass: look for requested neutral matching supported _neutral_ region 
                    // supported: en-US, de-DE 
                    // requested: de-AT, en-GB;q=0.8 
                    // => de-DE! (no exact match, but de-AT has better fit than en-GB) 
                    ?? GetMatch(acceptedCultures, supportedCultures, MatchesPartly);
            }

            public static void GetSwitchCultures(out CultureInfo currentCulture, out CultureInfo nextCulture)
            {
                currentCulture = Thread.CurrentThread.CurrentUICulture;
                var currentIndex = Array.IndexOf(SupportedCultures.Select(ci => ci.Name).ToArray(), currentCulture.Name);
                int nextIndex = (currentIndex + 1) % SupportedCultures.Length;
                nextCulture = SupportedCultures[nextIndex];
            }

            public static CultureInfo[] GetSwitchCultures(out CultureInfo currentCulture)
            {
                currentCulture = Thread.CurrentThread.CurrentUICulture;

                return SupportedCultures;
            }

            public static CultureInfo GetUserCulture(NameValueCollection headers)
            {
                var acceptedCultures = GetUserCultures(headers["Accept-Language"]);
                var culture = GetMatchingCulture(acceptedCultures, SupportedCultures);

                return culture;
            }

            public static CultureInfo[] GetUserCultures(string acceptLanguage)
            {
                // Accept-Language: fr-FR , en;q=0.8 , en-us;q=0.5 , de;q=0.3 
                if (string.IsNullOrWhiteSpace(acceptLanguage))
                    return new CultureInfo[] { };

                var cultures = acceptLanguage
                    .Split(',')
                    .Select(s => WeightedLanguage.Parse(s))
                    .OrderByDescending(w => w.Weight)
                     .Select(w => GetCultureInfo(w.Language))
                     .Where(ci => ci != null)
                     .ToArray();

                return cultures;
            }

            public static void SetPreferredCulture(this HttpResponseBase response, string cultureName)
            {
                SetPreferredCulture(response.Cookies, cultureName);
            }

            #endregion

            #region Private Methods

            private static void ApplyUserCulture(NameValueCollection headers, HttpCookieCollection cookies)
            {
                var culture = GetPreferredCulture(cookies)
                    ?? GetUserCulture(headers)
                    ?? SupportedCultures[0];

                var t = Thread.CurrentThread;
                t.CurrentCulture = culture;
                t.CurrentUICulture = culture;

                Debug.WriteLine("Culture: " + culture.Name);
            }

            private static CultureInfo GetCultureInfo(string language)
            {
                try
                {
                    return CultureInfo.GetCultureInfo(language);
                }
                catch (CultureNotFoundException)
                {
                    return null;
                }
            }

            private static CultureInfo GetPreferredCulture(HttpCookieCollection cookies)
            {
                var cookie = cookies[CookieName];
                if (cookie == null)
                    return null;

                var culture = GetCultureInfo((string)cookie.Value);
                if (culture == null)
                    return null;

                if (!SupportedCultures.Where(ci => ci.Name == culture.Name).Any())
                    return null;

                return culture;
            }

            private static bool MatchesCompletely(CultureInfo acceptedCulture, CultureInfo supportedCulture)
            {
                if (supportedCulture.Name == acceptedCulture.Name)
                {
                    return true;
                }

                // acceptedCulture could be neutral and supportedCulture specific, but this is still a match (de matches de-DE, de-AT, …) 
                if (acceptedCulture.IsNeutralCulture)
                {
                    if (supportedCulture.Parent.Name == acceptedCulture.Name)
                    {
                        return true;
                    }
                }

                return false;
            }

            private static bool MatchesPartly(CultureInfo acceptedCulture, CultureInfo supportedCulture)
            {
                supportedCulture = supportedCulture.Parent;
                if (!acceptedCulture.IsNeutralCulture)
                {
                    acceptedCulture = acceptedCulture.Parent;
                }

                if (supportedCulture.Name == acceptedCulture.Name)
                {
                    return true;
                }

                return false;
            }

            private static void SetPreferredCulture(HttpCookieCollection cookies, string cultureName)
            {
                var cookie = new HttpCookie(CookieName, cultureName)
                {
                    Expires = DateTime.Now.AddDays(30)
                };

                cookies.Set(cookie);

                Debug.WriteLine("SetPreferredCulture: " + cultureName);
            }

            #endregion

        }

        [DebuggerDisplay("Language = {Language} Weight = {Weight}")]
        internal class WeightedLanguage
        {
            public string Language { get; set; }

            public double Weight { get; set; }

            public static WeightedLanguage Parse(string weightedLanguageString)
            {
                // de 
                // en;q=0.8 
                var parts = weightedLanguageString.Split(';');
                var result = new WeightedLanguage { Language = parts[0].Trim(), Weight = 1.0 };

                if (parts.Length > 1)
                {
                    parts[1] = parts[1].Replace("q=", "").Trim();
                    double d;
                    if (double.TryParse(parts[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d))
                        result.Weight = d;
                }

                return result;
            }
        }
    
}