using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace AuthEndpoint
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            Console.WriteLine("Sending email.");
            Console.WriteLine("To: " + message.Destination);
            Console.WriteLine("Subject: " + message.Subject);
            Console.WriteLine("Body " + message.Body);
            return Task.FromResult(0);
        }
    }
}
