using System.Net.Http.Json;

namespace API_Feiertage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] bundesländer = { "Baden-Württemberg BW", "Brandenburg BB", "Hessen HE", "NRW (NW)"}
            HttpClient client = new HttpClient();
            Console.WriteLine("Jahr eingeben:");
            string jahr = Console.ReadLine();
            Console.WriteLine("Bundesland eingeben:");
            Console.WriteLine("Baden-Württemberg BW");
            Console.WriteLine("Brandenburg BB");
            Console.WriteLine("Baden-Württemberg BW");


            Rootobject root = client.GetFromJsonAsync<Rootobject>("https://get.api-feiertage.de?years=2024&states="+bundesland+").Result;
            foreach(Feiertage i in root.feiertage)
            {
                Console.WriteLine(i.fname + i.date);
            }
        }
    }
}
