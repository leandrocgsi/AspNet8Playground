using System.Text.Json.Serialization;

namespace APIAspNetCore5.Data.VO
{
    public class PersonVO
    {
        [JsonPropertyName("code")]
        public long? Id { get; set; }

        [JsonPropertyName("name")]
        public string FirstName { get; set; }


        [JsonPropertyName("last_name")]
        public string LastName { get; set; }


        [JsonPropertyName("your_address")]
        [JsonIgnore]
        public string Address { get; set; }


        [JsonPropertyName("sex")]
        public string Gender { get; set; }
    }
}
//https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/