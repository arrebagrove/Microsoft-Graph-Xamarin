//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace XamarinConnect
{


    /*
    * https://developer.microsoft.com/en-us/graph/quick-start
    * 
    * https://developer.microsoft.com/en-us/graph/docs/concepts/xamarin
    * 
    * https://apps.dev.microsoft.com/#/appList
    * 
    * https://github.com/Azure-Samples/active-directory-xamarin-native-v2
    * 
    * https://developer.microsoft.com/en-us/graph/docs/concepts/permissions_reference
    * 
    * 
    */


    public class App : Application
    {
        public static PublicClientApplication IdentityClientApp;
        public static string ClientID = "d6d83e2c-0cc8-4008-bf09-28bdbe80394e"; //"b6ddfab8-9da6-406d-bc41-068b938477e1";
        public static string RedirectUri = "msal" + ClientID + "://auth";
        public static string[] Scopes = { "User.Read","People.Read.All", "Mail.Send", "Files.ReadWrite" };
        public static string Username = string.Empty;
        public static string UserEmail = string.Empty;

        public static UIParent UiParent;
        public App()
        {
            IdentityClientApp = new PublicClientApplication(ClientID);
            MainPage = new NavigationPage(new XamarinConnect.MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
