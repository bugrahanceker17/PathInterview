using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace PathInterview.Core.Result
{
    public static class DataResultHelper
    {
        public static IActionResult HttpResponse(this DataResult dataResult)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;

            return new ObjectResult(dataResult)
            {
                StatusCode = (int?)statusCode
            };
        }
    }
}