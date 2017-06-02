using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TfsAbstractionLayer
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

      public TfsConnectionService(string userName, string password, string tfsUrl, string project) : this(tfsUrl, project)
      {
         this.userName = userName;
         this.password = password;
      }

      public HttpClient CreateConnection()
      {
         var clientHandler = new HttpClientHandler();

         if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
         {
            clientHandler.Credentials = new NetworkCredential(userName, password);
         }
         else
         {
            clientHandler.UseDefaultCredentials = true;
         }

         var client = new HttpClient(clientHandler);
         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
         //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
         // Convert.ToBase64String(
         //     Encoding.ASCII.GetBytes(
         //         string.Format("{0}:{1}", "benjamin.m.huser@gmail.com", "***"))));


         return client;
      }

      public string CreateRequestString(string request)
      {
         return $"{tfsUrl}/{project}/_apis/";
      }
   }
}
