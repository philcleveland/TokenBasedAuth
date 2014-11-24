using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthEndpoint.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        readonly IAuthRepository _authRepo;
        public SimpleAuthorizationServerProvider(IAuthRepository authRepo)
        {
            if (authRepo == null) throw new ArgumentNullException("authRepo");
            _authRepo = authRepo;
        }

        //http://leastprivilege.com/2013/11/13/embedding-a-simple-usernamepassword-authorization-server-in-web-api-v2/
        //client in the OAuth2 sense – which is the piece of software making the request [silicon-based lifeform] – 
        //not the human aka user of that client [carbon-based lifeform].
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // OAuth2 supports the notion of client authentication
            // this is not used here
            context.Validated();
        }   

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //allow CORS on the token middleware provider we need to add the header “Access-Control-Allow-Origin” to Owin context, 
            //if you forget this, generating the token will fail when you try to call it from your browser
            //instead of * this should be changed to only allow domains you wish to authenticate from
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //using(var authRepo = new AuthRepository())
            {
                var user = await _authRepo.FindUser(context.UserName, context.Password);
                if (user == null)
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                else
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    //http://leastprivilege.com/2013/11/13/embedding-a-simple-usernamepassword-authorization-server-in-web-api-v2/
                    //the user id (the ‘sub’ claim) 
                    //Don’t go crazy here! The more claims you add, the bigger the token gets – 
                    //and the token has to be submitted on every request.
                    identity.AddClaim(new Claim("sub", context.UserName));
                    identity.AddClaim(new Claim("role", "user"));

                    context.Validated(identity);
                }
            }
        }
    }
}
