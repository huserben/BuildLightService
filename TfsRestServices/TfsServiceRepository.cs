namespace TfsRestServices
{
    public class TfsServiceRepository
    {
      private readonly string tfsUrl;
      private readonly string userName;
      private readonly string password;
      private readonly string project;

      public TfsServiceRepository(string tfsUrl, string project)
      {
         this.tfsUrl = tfsUrl;
         this.project = project;

         InitializeServices();
      }

      public TfsServiceRepository(string tfsUrl, string project, string userName, string password)
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
