//using Raven.Client;
//using Raven.Client.Embedded;
//using System;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Controllers;

//namespace AuthEndpoint
//{
//    public abstract class RavenDBController : ApiController
//    {
//        private static readonly Lazy<IDocumentStore> LazyDocStore = new Lazy<IDocumentStore>(() =>
//                {
//                    var docStore = new EmbeddableDocumentStore()
//                    {
//                        UseEmbeddedHttpServer = true,
//                        DataDirectory = "~\\App_Data\\Ravendb",
//                        DefaultDatabase = "UserDataStore"
//                    };
//                    docStore.Initialize();
//                    return docStore;
//                });

//        public IDocumentStore DocStore { get; private set; }
//        public IAsyncDocumentSession AsyncSession { get; set; }

//        public async override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
//        {
//            using (var AsyncSession = DocStore.OpenAsyncSession())
//            {
//                AsyncSession.Advanced.UseOptimisticConcurrency = true;
//                var result = await base.ExecuteAsync(controllerContext, cancellationToken);
//                await AsyncSession.SaveChangesAsync();
//                return result;
//            }
//        }
//    }
//}