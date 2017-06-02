using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TfsAbstractionLayer
{
    internal interface ITfsConnectionService
    {
      HttpClient CreateConnection();

      string CreateRequestString(string request);
    }
}
