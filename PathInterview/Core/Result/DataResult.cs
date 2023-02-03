using System.Collections.Generic;
using Newtonsoft.Json;

namespace PathInterview.Core.Result
{
    public class DataResult
    {
        [JsonProperty("isError")] public bool IsError => ErrorMessageList?.Count > 0;
        [JsonProperty("errorMessageList")] public List<string> ErrorMessageList { get; set; } = new List<string>();
        [JsonProperty("total")] public int Total { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
    }
}