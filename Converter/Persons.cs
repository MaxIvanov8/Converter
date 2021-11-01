using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Converter
{
    public class Persons
    {
        [JsonPropertyName("contacts")]
        public List<Person> List { get; set; }
        [JsonPropertyName("count")]
        public int Count => List.Count;
        private readonly List<string> _errorList;
        [JsonPropertyName("errors")]
        public string ErrorString => _errorList.Aggregate(string.Empty, (current, error) => current + $"{error}, ");

        public Persons()
        {
            List = new List<Person>();
            _errorList = new List<string>();
        }

        public void SetData(string[] lines)
        {
            for (var i = 0; i < lines.Length; ++i)
            {
                var person = new Person(i + 1, lines[i]);
                List.Add(person);
                var error = person.GetError;
                if (error != null)
                    _errorList.Add(error);
            }
        }
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
    }
}
