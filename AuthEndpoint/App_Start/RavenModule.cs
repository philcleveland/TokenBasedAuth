using Ninject.Activation;
using Ninject.Modules;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthEndpoint
{
    public class RavenModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>()
                .ToMethod(InitDocStore)
                .InSingletonScope();
        }

        private IDocumentStore InitDocStore(IContext context)
        {
            var docStore = new DocumentStore()
            {
                Url = "http://win-0dual5d7944:8080/",
                DefaultDatabase = "brewday",
            };

            docStore.Initialize();
            //Raven.Database.Server.NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8082);
            return docStore;
        }
    }
}