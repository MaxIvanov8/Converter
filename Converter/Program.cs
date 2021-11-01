using System.IO;

namespace Converter
{
    internal class Program
    {
        private static void Main()
        {
            var temp = File.ReadAllLines("data.txt");
            var personList = new Persons();
            personList.SetData(temp);
            File.WriteAllText("result.txt", personList.ToJson());
        }
    }
}
