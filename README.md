TokenBasedAuth
==============

#ASP.NET Owin OAuth Bearer Token WebApi

Show how to create seperate WebAPIs which use OAuth bearer tokens for authentication.

This example uses ASP.NET Identity and a miinimal AccountController to register users. And exposes a token endpoint to generate
a temporary bearer token.

There is also a protected resource server which can be accessed using the bearer token from the AuthEndpoint.


**To register a user:**
```curl
curl -X POST -H "Content-Type: application/json" -H "Cache-Control: no-cache" -H "Postman-Token: 2633c6c1-e0c7-93d1-37dd-9ef4f60755f8" -d '{
  "userName": "user",
  "password": "MyPass",
  "confirmPassword": "MyPass"
}' http://localhost:51723/api/account/register
```

**To generate a token:**
```curl
curl -X POST -H "Cache-Control: no-cache" -H "Postman-Token: 1e78f63c-f93a-c9a1-527e-6bf63936c590" -H "Content-Type: application/x-www-form-urlencoded" -d 'grant_type=password&username=user&password=MyPass' http://localhost:51723/token
```

**To access the protected resource:**
```curl
curl -X GET -H "Accept: application/json" -H "Content-Type: application/json" -H "Authorization: Bearer FPkStqtb9ziRTvONSA1HaoTtLonfn-P00fgCINYnupT4fFOc8ClV7-qxmnUw1ZeNhvtOpnR0aXp6muHs4H4zl9h0j7jzzS3GjWdzaza6E70I210jPzJ9do5jf68tTnITC7p_GrvlouoPsyby8oskMz-rqcVNZgiyFZp6xc5heo6bGsY6uiQmOyBow8s4Y-4R8f70mFYjqzR9lHzcBZ87btYCEWwBss2y-4McTd8fKgk" -H 
"Cache-Control: no-cache" -H "Postman-Token: 73eaf691-c442-efc1-95d9-3912c79a6743" http://localhost:56282/api/protected/data
```

The following blog posts were instrumental in getting this working:
* http://bitoftech.net/2014/06/01/token-based-authentication-asp-net-web-api-2-owin-asp-net-identity/
* http://bitoftech.net/2014/09/24/decouple-owin-authorization-server-resource-server-oauth-2-0-web-api/
* http://bitoftech.net/2014/07/16/enable-oauth-refresh-tokens-angularjs-app-using-asp-net-web-api-2-owin/
* http://www.asp.net/web-api/overview/security/individual-accounts-in-web-api
* https://github.com/MikeWasson/LocalAccountsApp
