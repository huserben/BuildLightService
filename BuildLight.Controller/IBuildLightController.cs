namespace BuildLight.Controller
{
   internal interface IBuildLightController
   {
      /// <summary>
      /// Starts the build light controller and updates the status in the given intervall.
      /// </summary>
      /// <param name="updateIntervall">The update intervall in milliseconds.</param>
      void Start(int updateIntervall);

      void Stop();
   }
}