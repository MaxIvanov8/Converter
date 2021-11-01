using System.Linq;
using System.Text.Json.Serialization;

namespace Converter
{
    public class Person
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }
        [JsonPropertyName("middlename")]
        public string MiddleName { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        private string _errorString;

        public Person(int id, string input)
        {
            Id = id;
            var index = input.IndexOf(' ');
            LastName = input[..index];
            input = input[(index + 1)..];
            index = input.IndexOfAny(new[] {' ', '\t'});
            FirstName = input[..index];
            input = input[(index + 1)..];
            MiddleName = string.Empty;
            if (input[0] >= 'А' && input[0] <= 'Я')
            {
                index = input.IndexOfAny(new[] {' ', '\t'});
                MiddleName = input[..index];
                input = input[(index + 1)..];
            }

            foreach (var character in input.Where(character => character is >= '0' and <= '9'))
                Phone += character;
            if (Phone[0] == '8')
                Phone = Phone.Remove(0, 1);
            if (Phone[0] != '7')
                Phone = Phone.Insert(0, "7");
            Validation();
        }

        private void Validation()
        {
            _errorString = string.Empty;
            if (LastName.Length is > 50 or < 2)
                _errorString += nameof(LastName);
            if(FirstName.Length is > 50 or < 2)
                _errorString += nameof(FirstName);
            if(MiddleName.Length is 1 or > 50)
                _errorString += nameof(MiddleName);
            if(Phone.Length is > 11 or < 10)
                _errorString += nameof(Phone);
        }
        [JsonIgnore]
        public string GetError => !string.IsNullOrEmpty(_errorString) ? $"{Id}: {_errorString}" : null;
    }
}
