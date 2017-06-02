using System;
using BuildLight.Controller.Interfaces;
using TfsRestServices;

namespace BuildLight.Source.Tfs
{
   public class TfsBuildSource : IBuildSource
   {
      private readonly TfsServiceRepository tfsRepository;

      public TfsBuildSource(string tfsUrl, string project, string userName, string password)
      {
         tfsRepository = new TfsServiceRepository(tfsUrl, project, userName, password);
      }

      public BuildStatus GetBuildStatus(string buildName)
      {
         if (tfsRepository.BuildService.IsBuildRunning(buildName))
         {
            return BuildStatus.Inconclusive;
         }

         var buildStatus = tfsRepository.BuildService.GetStatusOfLastBuild(buildName);

         switch (buildStatus)
         {
            case "succeeded":
               return BuildStatus.Successful;
            case "partiallySucceeded":
               return BuildStatus.PartiallySuccessful;
            case "failed":
               return BuildStatus.Failed;
            default:
               return BuildStatus.Inconclusive;
         }
      }
   }
}
