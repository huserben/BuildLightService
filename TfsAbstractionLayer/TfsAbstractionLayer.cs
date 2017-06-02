using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TfsAbstractionLayer
{
   public class TfsAbstractionLayer
   {
      private readonly string tfsUrl;
      private readonly string userName;
      private readonly string password;
      private readonly string project;

      public TfsAbstractionLayer(string tfsUrl, string project)
      {
         this.tfsUrl = tfsUrl;
         this.project = project;

         InitializeServices();
      }

      public TfsAbstractionLayer(string tfsUrl, string project, string userName, string password)
      {
         this.tfsUrl = tfsUrl;
         this.project = project;
         this.userName = userName;
         this.password = password;

         InitializeServices();
      }

      public IBuildService BuildService
      {
         get;
         private set;
      }

      private void InitializeServices()
      {
         TfsConnectionService tfsConnectionService;

         if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
         {
            tfsConnectionService = new TfsConnectionService(tfsUrl, project, userName, password);
         }
         else
         {
            tfsConnectionService = new TfsConnectionService(tfsUrl, project);
         }

         BuildService = new BuildService(tfsConnectionService);
      }
   }
}
