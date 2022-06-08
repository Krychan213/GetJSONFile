using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace GetJSONFile
{

    public class GetAddress // Pobiera adres od uzytkownika
    {
        public List<string> GetURL()
        {
            List<string> url = new List<string>();
            Console.WriteLine("Podaj adresy URL, oddzielone między sobą średnikiem (;)");
            string str = Console.ReadLine() + ";";

            while (!string.IsNullOrEmpty(str))
            {

                int lastpos = str.IndexOf(";");

                url.Add(str.Substring(0, lastpos));
                str = str.Remove(0, lastpos + 1);

            }
            return url;
        }
    }
    public class Loc //pobiera lokalizacje docelową
    {
        public string GetLocation() 
        {
            Console.WriteLine("Podaj lokalizacje docelową i nazwę pliku");
            string loc = Console.ReadLine();
            return loc;
        }
    }
    public class Check // Sprawdza czy adres istnieje w sieci
    {
        public bool CheckURL(List<string> url) 
        {

            foreach (string str in url)
            {
                try
                {
                    HttpWebRequest request = HttpWebRequest.Create(str) as HttpWebRequest;
                    request.Timeout = 5000;
                    request.Method = "HEAD";

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        int statusCode = (int)response.StatusCode;
                        if (statusCode >= 100 && statusCode < 400) //Good requests
                        {
                            continue;
                        }
                        else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                        {
                            Console.WriteLine(String.Format("Serwer wyrzucił błąd. Podane adres URL jest nieprawidłowy: {0}", url));
                            return false;
                        }
                    }
                }
                catch { Console.WriteLine("Wystapił nieoczekiwany błąd"); return false; }
            }
            return true;
        }
    }

    public class Extract // Pobiera dane
    {
        public string GetData(string str)
        {
             
                using WebClient wc = new WebClient();
                string json = wc.DownloadString(str);
                return json;
            
        }
    }
    public class Save // Zapisuje dane
    {
        public void SaveData(string json, string loc, string name)
        {
            File.WriteAllText(@loc+"/"+name, json);
        }
    }
    public class FileName // Pobiera nazwę pliku
    {
        public string ExtractName(string url)
        {
            int firstIndex = url.LastIndexOf(@"/");

            string str = url.Substring(firstIndex+1);
            return str;
        }
    }

    public class CheckType // Sprawdza czy podany adres jest prawidłowy (prowadzi do pliku json)
    {
        public bool CheckJSON(List<string> url)
        {
            foreach (string str in url){
                int firstIndex = str.LastIndexOf(".");

                string check = str.Substring(firstIndex + 1);

                if (check == "json") continue;
                else
                {
                    Console.WriteLine("Conajmniej jeden z podanych adresów URL nie jest plikiem JSON");
                    return false;
                }
                
            }
            return true;
           
        }
    }

    public class DirCheck // Sprawdza czy podany adres docelowy istnieje
    {
        public bool CheckDir(string loc)
        {
            return Directory.Exists(loc);
        }
    }

    class Program
    {
        static void Main(string[] args) 
        {
            // Tworzenie obiektów
            var address = new GetAddress();
            var location = new Loc();
            var chk = new Check();
            var get = new Extract();
            var save = new Save();
            var filename = new FileName();
            var checktype = new CheckType();
            var dirCheck = new DirCheck();

            List<string> url = new List<string>(); // Deklaracja listy
            url = address.GetURL(); // Pobiera adresy URL
            if (chk.CheckURL(url)&&checktype.CheckJSON(url)){ // Sprawdza czy adresy istnieją w sieci i są prawidłowe
                string loc = location.GetLocation(); // Pobiera lokalizacje docelową
                if (dirCheck.CheckDir(loc)) // Sprawdza czy lokalizacja istnieje
                {
                    foreach (string str in url) 
                    {
                        string json = get.GetData(str); // Pobiera dane JSON z adresu URL
                        string name = filename.ExtractName(str); // Pobiera nazwę pliku z adresu URL
                        save.SaveData(json, loc, name); // Zapisuje dane jako plik JSON
                    }
                }else Console.WriteLine("Podano nieistniejący folder");
            }else Console.WriteLine("Jeden z adresów URL jest nieprawidłowy lub nie istnieje");
            Console.WriteLine("OK");
        }
    }
}
