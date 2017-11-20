using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinConnect
{

   

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PeoplePage : ContentPage
    {
        public ObservableCollection<User> People { get; set; } = new ObservableCollection<User>();
        public User CurrentUser { get; set; } = new User();

        public PeoplePage(User currentUser)
        {
            CurrentUser =  currentUser;
            
            GetPeople();

            InitializeComponent();
            BindingContext = this;

            //CurrentUserDisplayName.Text = CurrentUser.DisplayName;
            //CurrentUserEmail.Text = CurrentUser.Mail;


        }

        public async void GetPeople()
        {            
            //Debug.WriteLine("GetAuthClient...");
            var graphClient = AuthenticationHelper.GetAuthenticatedClient();
                        
            try
            {
                //Debug.WriteLine("Attempting request...");
                //var me = await graphClient.Me.Request().GetAsync();

                /*  Version 1.2.1 of Microsoft.Graph
                 * The  Microsoft.Graph .NET Library doesn't have a method to list people
                 * The People list will return the people relevent to a person based on thier emails and other documents..a score is attached among other properties
                 * 
                 * To call this API with the assistance of the Microsoft.Graph API we will get the Me request URL for the signed in user and 
                 * obtain the people list for them by appending /people onto the URL and calling the API with it.
                 * The JSON object is handled dynamically since the returned JSON has three objects and we are only concerned with one of them--the list of people.
                 * 
                 * the latest version of Microsoft.Graph has a Me.People call built in.
                 */
                //string requestUrl = graphClient.Me.Request().RequestUrl;
                string requestUrl = graphClient.Users.Request().RequestUrl;
                                
                requestUrl = requestUrl +"('"+ CurrentUser.UserPrincipalName + "')/people";

                Debug.WriteLine(requestUrl);

                HttpRequestMessage hrm = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                              

                await graphClient.AuthenticationProvider.AuthenticateRequestAsync(hrm);

                var http = graphClient.HttpProvider;

                HttpResponseMessage response = await graphClient.HttpProvider.SendAsync(hrm);

                if (response.IsSuccessStatusCode)
                {
                    
                    var content = await response.Content.ReadAsStringAsync();
                    //var people = graphClient.HttpProvider.Serializer.DeserializeObject<People>(content);
                    
                    dynamic dyn = JsonConvert.DeserializeObject(content);

                    foreach (var obj in dyn.value)
                    {
                        People.Add(new User() { DisplayName = obj.displayName, Id = obj.id, Mail=obj.mail, UserPrincipalName=obj.userPrincipalName });
                    }
                    

                    //Debug.WriteLine(content);
                    
                }
                                
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);  //<sarcasm> some awesome, fine-grained error handling </sarcasm>
            }

            
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = (User)e.Item;
            
            Navigation.PushAsync(new PeoplePage(user));

        }
    }
}