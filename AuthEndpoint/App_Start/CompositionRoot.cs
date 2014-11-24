using AuthEndpoint.Controllers;
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
        public CompositionRoot()
        {
            //create singletons here

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
                return new AccountController();
            }

            throw new InvalidOperationException(string.Format("Unknown controller type requested. {0}", 
                                                                controllerType.ToString()));
        }
    }
}