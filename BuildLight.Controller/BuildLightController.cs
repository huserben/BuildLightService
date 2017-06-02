using System.Threading;
using BuildLight.Controller.Interfaces;

namespace BuildLight.Controller
{
   public class BuildLightController : IBuildLightController
   {
      private readonly IBuildLamp buildLamp;
      private readonly IBuildSource buildSource;

      private readonly Timer buildTimer;
      private readonly string buildName;

      public BuildLightController(IBuildLamp buildLamp, IBuildSource buildSource, string buildName)
      {
         this.buildLamp = buildLamp;
         this.buildSource = buildSource;
         this.buildName = buildName;

         buildTimer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
      }

      public void Start(int updateIntervall)
      {
         buildTimer.Change(0, updateIntervall);
      }

      public void Stop()
      {
         buildTimer.Change(Timeout.Infinite, Timeout.Infinite);
      }

      private void OnTimerElapsed(object stateInfo)
      {
         var buildStatus = buildSource.GetBuildStatus(buildName);
         buildLamp.UpdateStatus(buildName, buildStatus);
      }
   }
}
