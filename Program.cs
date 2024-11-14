using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace API_Feiertage
{
    internal class Program
    {
        static (string bundesland, string kuerzel)[] bundesländer = { ("Baden-Württemberg","BW"), ("Brandenburg","BB"), ("Hessen", "HE"), ("NRW", "NW"),("Sachsen","SN"),("Thüringen","TH"),("Bayern","BY"),("Bremen","HB"),("Mecklenburg-Vorpommern","MV"),("Rheinland-Pfalz","RP"),("Sachsen-Anhalt","ST"),("Berlin","BE")
            , ("Hamburg","HH"),("Niedersachsen","NI"), ("Saarland","SL"),("Schleswig-Holstein","SH")};
        static void Main(string[] args)
        {
            string bundesland = string.Empty;
            string jahr = string.Empty;


            bundesland = blWaehlen();
            Console.WriteLine("Jahr eingeben:");
            jahr = Console.ReadLine();
            Console.Clear();
            GetData(bundesland, jahr);




      
         
        }

        public static void GetData(string bundesland, string jahr)
        {
            HttpClient client = new HttpClient();

            Console.WriteLine($"Feiertage für {bundesländer.Where(x => x.kuerzel == bundesland.ToUpper()).Select(x => x.bundesland).ToArray()[0]} {jahr}:"); //Direktes Ansprechen des Tuple leider nicht möglich, Index 0 vom Tuple-Array = Bundesland (ausgeschrieben)

            Rootobject root = client.GetFromJsonAsync<Rootobject>("https://get.api-feiertage.de?years=" + jahr + "&states=" + bundesland).Result;
            if (root != null)
            {
                if (root.feiertage != null)
                {
                    foreach (Feiertage i in root.feiertage)
                    {
                        Console.WriteLine(i.fname + i.date);
                    }
                }
                else
                {
                    Console.WriteLine(root.additional_note);
                }
            }
        }
        public static string blWaehlen()
        {
            string bundesland;
            bool gültig = false;

            Console.WriteLine("Bundesland eingeben:");
            foreach ((string, string) tuple in bundesländer)
            {
                Console.WriteLine(tuple.Item1 + " - " + tuple.Item2);
            }
            Console.WriteLine();

            while (!gültig)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    bundesland = Console.ReadLine().ToLower();
                    Console.ResetColor();
                    if (bundesländer.Where(x => x.kuerzel.ToLower() == bundesland).Count() != 1)
                    {
                        Console.Clear();
                        foreach ((string, string) tuple in bundesländer)
                        {
                            Console.WriteLine(tuple.Item1 + " - " + tuple.Item2);
                        }
                        throw new Exception("Ungültiges Kürzel " + bundesland + ". Bitte erneut eingeben");
                    }
                    return bundesland;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
            
        }
    }
}
