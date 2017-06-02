using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TfsRestServices
{
   public class TfsConnectionService : ITfsConnectionService
   {
      private readonly string tfsUrl;
      private readonly string project;
      private readonly string userName;
      private readonly string password;

      public TfsConnectionService(string tfsUrl, string project)
      {
         this.tfsUrl = tfsUrl;
         this.project = project;
      }

      public TfsConnectionService(string tfsUrl, string project, string userName, string password) : this(tfsUrl, project)
      {
         this.userName = userName;
         this.password = password;
      }

      public HttpClient CreateConnection()
      {
         if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
         {
            var clientHandler = new HttpClientHandler
            {
               UseDefaultCredentials = true
            };

            return new HttpClient(clientHandler);
         }

         var client = new HttpClient();
         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
          Convert.ToBase64String(
              Encoding.ASCII.GetBytes($"{userName}:{password}")));
         return client;
      }

      public string CreateRequestString(string request)
      {
         return $"{tfsUrl}/{project}/_apis{request}";
      }
   }
}
