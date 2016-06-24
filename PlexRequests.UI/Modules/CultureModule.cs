﻿#region Copyright
// /************************************************************************
//    Copyright (c) 2016 Jamie Rees
//    File: CultureModule.cs
//    Created By: Jamie Rees
//   
//    Permission is hereby granted, free of charge, to any person obtaining
//    a copy of this software and associated documentation files (the
//    "Software"), to deal in the Software without restriction, including
//    without limitation the rights to use, copy, modify, merge, publish,
//    distribute, sublicense, and/or sell copies of the Software, and to
//    permit persons to whom the Software is furnished to do so, subject to
//    the following conditions:
//   
//    The above copyright notice and this permission notice shall be
//    included in all copies or substantial portions of the Software.
//   
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  ************************************************************************/
#endregion
using System;

using Nancy;
using Nancy.Extensions;
using Nancy.Responses;

using PlexRequests.Core;
using PlexRequests.Core.SettingModels;
using PlexRequests.UI.Helpers;

namespace PlexRequests.UI.Modules
{
    public class CultureModule : BaseModule
    {
        private const string CookieName = "_culture";
        public CultureModule(ISettingsService<PlexRequestSettings> pr) : base("culture",pr)
        {
            Get["/"] = x => SetCulture();
        }

        public RedirectResponse SetCulture()
        {
            var culture = (string)Request.Query["l"];
            var returnUrl = (string)Request.Query["u"];

            // Validate
            culture = CultureHelper.GetImplementedCulture(culture);

            var outCookie = string.Empty;
            if (Cookies.TryGetValue(CookieName, out outCookie))
            {
                Cookies[CookieName] = culture;
            }
            else
            {
                Cookies.Add(CookieName, culture);
            }
            var cookie = Cookies["_culture"];
            var response = Context.GetRedirect(returnUrl);

            response.WithCookie(CookieName, cookie ?? culture, DateTime.Now.AddYears(1));

            return response;
        }
    }
}