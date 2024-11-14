using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace API_Feiertage
{
    internal class Program
    {
        public static List<(string bundesland, string kuerzel)> bundesländer = new List<(string bundesland, string kuerzel)>{ ("Baden-Württemberg","BW"), ("Brandenburg","BB"), ("Hessen", "HE"), ("NRW", "NW"),("Sachsen","SN"),("Thüringen","TH"),("Bayern","BY"),("Bremen","HB"),("Mecklenburg-Vorpommern","MV"),("Rheinland-Pfalz","RP"),("Sachsen-Anhalt","ST"),("Berlin","BE"),("Hamburg","HH"),("Niedersachsen","NI"), ("Saarland","SL"),("Schleswig-Holstein","SH")};

        static void Main(string[] args)
        {
            string bundeslandKuerzel = BundeslandWaehlen();
            Console.WriteLine("Jahr eingeben:");
            string jahr = Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"Feiertage für {bundesländer.Where(x => x.kuerzel == bundeslandKuerzel.ToUpper()).Select(x => x.bundesland).ToArray()[0]} {jahr}:"); //Direktes Ansprechen des Tuple leider nicht möglich, Index 0 vom Tuple-Array = Bundesland (ausgeschrieben)
            GetData(bundeslandKuerzel, jahr);
        }

        /// <summary>
        /// Bundesland und Jahr an API übergeben und zurückgegebene JSON-Datei ausgeben
        /// </summary>
        /// <param name="bundeslandKuerzel">kleingeschriebenes Kürzel des Bundeslandes</param>
        /// <param name="jahr">Jahr im Stringformat</param>
        public static void GetData(string bundesland, string jahr)
        {
            HttpClient client = new HttpClient();
            Rootobject root = client.GetFromJsonAsync<Rootobject>("https://get.api-feiertage.de?years=" + jahr + "&states=" + bundesland).Result;

            if (root != null)
            {
                if (root.feiertage != null)                                 //Gültige Eingabe => Feiertage ausgeben
                {
                    foreach (Feiertage i in root.feiertage)
                    {
                        Console.WriteLine(i.fname + i.date);
                    }
                }
                else                                                        //Fehlermeldung innerhalb der JSON-Datei bei ungültiger Jahreszahl
                {
                    Console.WriteLine(root.additional_note);
                }
            }
            else
            {
                Console.WriteLine("JSON-Datei konnte nicht geladen werden");
            }
        }

        /// <summary>
        /// Prüft ob es sich bei der Kürzeleingabe um ein gültiges Bundesland handelt
        /// </summary>
        /// <returns>Das Kürzel des gewählten Bundeslandes in Kleinbuchstaben</returns>
        public static string BundeslandWaehlen()
        {
            string bundeslandKuerzel;

            Console.WriteLine("Bundesland eingeben:");              //Bundesländer und Kürzel ausgeben
            foreach ((string, string) tuple in bundesländer)
            {
                Console.WriteLine(tuple.Item1 + " - " + tuple.Item2);
            }
            Console.WriteLine();
            
            while (true)             //Loopen bis gültige Eingabe vorliegt
            {
                try
                {
                    bundeslandKuerzel = Console.ReadLine().ToLower();

                    if (bundesländer.Where(x => x.kuerzel.ToLower() == bundeslandKuerzel).Count() != 1)        //Ungültige Eingabe
                    {
                        Console.Clear();
                        foreach ((string, string) tuple in bundesländer)
                        {
                            Console.WriteLine(tuple.Item1 + " - " + tuple.Item2);
                        }
                        throw new Exception("Ungültiges Kürzel " + bundeslandKuerzel + ". Bitte erneut eingeben"); //Eingabemaske durch throw erneut aufrufen
                    }
                    return bundeslandKuerzel;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }   
            //neu
           /* while (true) // Loopen bis gültige Eingabe vorliegt
            {
                Console.Clear();
                foreach ((string, string) tuple in bundesländer)
                {
                    Console.WriteLine($"{tuple.Item1} - {tuple.Item2}");
                }
                bundeslandKuerzel = Console.ReadLine().ToLower();
                try
                {
                    if (!bundesländer.Any(x => x.kuerzel.ToLower() == bundeslandKuerzel)) // Ungültige Eingabe
                    {
                        throw new Exception($"Ungültiges Kürzel {bundeslandKuerzel}. Bitte erneut eingeben");
                    }
                    return bundeslandKuerzel;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }*/
            //ende neu 
            return null;  
        }
    }
}
