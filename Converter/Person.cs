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

        private static readonly char[] DelimiterChars = { ' ', '\t' };

        private static bool IsPhone(string field)
        {
            return field[0]>='0' && field[0]<='9' || field[0]=='+' || field[0] == '(';
        }

        public Person(int id, string input)
        {
            Id = id;
            LastName = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            Phone = string.Empty;
            if (!string.IsNullOrEmpty(input))
            {
                var fields = input.Split(DelimiterChars,4);
                var length = fields.Length;
                if (IsPhone(fields.Last()))
                {
                    foreach (var character in fields.Last().Where(character => character is >= '0' and <= '9'))
                        Phone += character;
                    if (Phone[0] == '8')
                        Phone = Phone.Remove(0, 1);
                    if (Phone[0] != '7')
                        Phone = Phone.Insert(0, "7");
                    length -= 1;
                }

                switch (length)
                {
                    case 0:
                        break;
                    case 1:
                        LastName = fields[0];
                        break;
                    case 2:
                        LastName = fields[0];
                        FirstName = fields[1];
                        break;
                    case 3:
                        LastName = fields[0];
                        FirstName = fields[1];
                        MiddleName = fields[2];
                        break;
                }
            }
            Validation();
        }

        private void Validation()
        {
            _errorString = string.Empty;
            if (LastName is{Length: > 50 or < 2})
                _errorString += $"{nameof(LastName)} ";
            if(FirstName is{Length: > 50 or < 2})
                _errorString += $"{nameof(FirstName)} ";
            if(MiddleName is {Length: 1 or > 50})
                _errorString += $"{nameof(MiddleName)} ";
            if(Phone is {Length:> 11 or < 10})
                _errorString += $"{nameof(Phone)} ";
        }
        [JsonIgnore]
        public string GetError => !string.IsNullOrEmpty(_errorString) ? $"{Id}: {_errorString}" : null;
    }
}
