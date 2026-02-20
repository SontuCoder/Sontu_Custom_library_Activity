using System;
using Newtonsoft.Json;

namespace Sontu.Activities.Models
{
    public class AuthUserResponse
    {
        // Fixed / Required
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        // Optional
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("Ph_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("organization")]
        public string Organization { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }
    }
}
