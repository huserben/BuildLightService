using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TfsRestServices
{
   internal class BuildService : IBuildService
   {
      private readonly ITfsConnectionService tfsConnectionService;

      public BuildService(ITfsConnectionService tfsConnectionService)
      {
         this.tfsConnectionService = tfsConnectionService;
      }

      public int GetIdForBuildDefinitionName(string buildDefinitionName)
      {
         var responseMessage = string.Empty;

         using (var response = tfsConnectionService.CreateConnection()
            .GetAsync(tfsConnectionService.CreateRequestString($"/build/definitions?api-version=2.0&name={buildDefinitionName}"))
            .Result)
         {
            response.EnsureSuccessStatusCode();
            responseMessage = response.Content.ReadAsStringAsync().Result;
         }

         dynamic buildDefinition = JsonConvert.DeserializeObject(responseMessage);
         var firstBuildDefinition = buildDefinition.value[0];
         return firstBuildDefinition.id;
      }

      public string GetStatusOfLastBuild(string buildName)
      {
         var definitionID = GetIdForBuildDefinitionName(buildName);

         var responseMessage = string.Empty;
         using (var response = tfsConnectionService.CreateConnection()
            .GetAsync(tfsConnectionService.CreateRequestString($"/build/builds?api-version=2.0&definitions={definitionID}&statusFilter=completed"))
            .Result)
         {
            response.EnsureSuccessStatusCode();
            responseMessage = response.Content.ReadAsStringAsync().Result;
         }

         dynamic runningBuilds = JsonConvert.DeserializeObject(responseMessage);

         if (runningBuilds.count > 0)
         {
            var latestBuild = runningBuilds.value[0];
            return latestBuild.result;
         }

         return string.Empty;
      }

      public bool IsBuildRunning(string buildDefinitionName)
      {
         var definitionID = GetIdForBuildDefinitionName(buildDefinitionName);

         var responseMessage = string.Empty;

         using (var response = tfsConnectionService.CreateConnection()
            .GetAsync(tfsConnectionService.CreateRequestString($"/build/builds?api-version=2.0&definitions={definitionID}&statusFilter=inProgress,notStarted"))
            .Result)
         {
            response.EnsureSuccessStatusCode();
            responseMessage = response.Content.ReadAsStringAsync().Result;
         }

         dynamic runningBuilds = JsonConvert.DeserializeObject(responseMessage);
         return runningBuilds.count != 0;
      }

      public void QueueBuild(int buildDefinitionId, string requestedBy)
      {
         var contentStringBuilder = new StringBuilder("{"); // = "{}";
         contentStringBuilder.AppendLine("\"definition\": { \"id\": " + buildDefinitionId + "}");

         if (!string.IsNullOrEmpty(requestedBy))
         {
            contentStringBuilder.AppendLine("\"requestedFor\": { \"id\": " + requestedBy + "}");
         }

         contentStringBuilder.AppendLine("}");
         var content = new StringContent(contentStringBuilder.ToString());

         content.Headers.Clear();
         content.Headers.Add("Content-Type", "application/json");
         using (var response = tfsConnectionService.CreateConnection()
            .PostAsync(tfsConnectionService.CreateRequestString($"/build/builds?api-version=2.0"), content)
            .Result)
         {
           response.EnsureSuccessStatusCode();
         }
      }

      public void QueueBuild(int buildDefinitionId)
      {
         QueueBuild(buildDefinitionId, string.Empty);
      }

      public void QueueBuild(string buildDefinitionName, string requestedBy)
      {
         var definitionId = GetIdForBuildDefinitionName(buildDefinitionName);
         QueueBuild(definitionId, requestedBy);
      }

      public void QueueBuild(string buildDefinitionName)
      {
         QueueBuild(buildDefinitionName, string.Empty);
      }
   }
}
