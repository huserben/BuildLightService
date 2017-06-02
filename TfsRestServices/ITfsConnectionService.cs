using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TfsRestServices
{
    internal interface ITfsConnectionService
    {
      HttpClient CreateConnection();

      string CreateRequestString(string request);
    }
}
