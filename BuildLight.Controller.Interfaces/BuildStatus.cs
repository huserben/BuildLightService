namespace BuildLight.Controller.Interfaces
{
   public enum BuildStatus
   {
      /// <summary>
      /// Build was successful
      /// </summary>
      Successful,

      /// <summary>
      /// Build ran, but partially failed (e.g. has warning etc.)
      /// </summary>
      PartiallySuccessful,

      /// <summary>
      /// Build failed.
      /// </summary>
      Failed,

      /// <summary>
      /// Unclear status, for example build is currently running.
      /// </summary>
      Inconclusive
   }
}
