using AuthEndpoint.Controllers;
using AuthEndpoint.Models;
using Raven.Client;
using Raven.Client.Embedded;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace AuthEndpoint
{
    /// <summary>
    /// This class will only be instantiated once and last the lifetime of the application.
    /// </summary>
    public class CompositionRoot : IHttpControllerActivator
    {
        
        RavenDBUserStore<UserModel> _userStore;

        public CompositionRoot()
        {
            //create singletons here
            var documentStore = new EmbeddableDocumentStore()
            {
                //ConnectionStringName = "RavenDB",
                UseEmbeddedHttpServer = true,
                DataDirectory="~\\App_Data\\Ravendb",
                DefaultDatabase="UserDataStore"
            };
            documentStore.Initialize();
            Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8082);

            _userStore = new RavenDBUserStore<UserModel>(documentStore);
        }

        /// <summary>
        /// This method creates the appropriate controller for the request. This method is called once per request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="controllerDescriptor"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            //create per request instances here

            if(controllerType == typeof(AccountController))
            {
                return new AccountController(_userStore);
            }

            return Activator.CreateInstance(controllerType) as IHttpController;

            //throw new InvalidOperationException(string.Format("Unknown controller type requested. {0}", 
            //                                                    controllerType.ToString()));
        }
    }
}