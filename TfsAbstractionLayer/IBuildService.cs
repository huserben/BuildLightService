using System;
using System.Collections.Generic;
using System.Text;

namespace TfsAbstractionLayer
{
   public interface IBuildService
   {
      int GetIdForBuildDefinitionName(string buildDefinitionName);

      void QueueBuild(int buildDefinitionId, string requestedBy);

      void QueueBuild(int buildDefinitionId);

      void QueueBuild(string buildDefinitionName);

      void QueueBuild(string buildDefinitionName, string requestedBy);

      bool IsBuildRunning(string buildDefinitionName);
   }
}
