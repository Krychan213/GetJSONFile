using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace GetJSONFile.UnitTest
{
    class GetAddressTests
    {
        [Test]
        public void Get_WhenCalledAndTyped_ShouldReturnSame() //Sprawdza czy po wpisaniu adresu URL zostaje on prawidłowo wprowadzony do listy
        {
            var Address = new GetAddress();
            List<string> url = new List<string>();
            List<string> res = new List<string>();

            var stringReader = new StringReader("https://raw.githubusercontent.com/dotnet/templating/main/global.json");
            Console.SetIn(stringReader);
            url = Address.GetURL();
            res.Add("https://raw.githubusercontent.com/dotnet/templating/main/global.json");

            Assert.AreEqual(res, url);
        }
        [Test]
        public void Get_WhenCalledAndTypedMultiple_ShouldReturnSame() // Sprawdza czy po wpisaniu wielu adresów są one poprawnie wprowadzone
        {
            var Address = new GetAddress();
            List<string> url = new List<string>();
            List<string> res = new List<string>();

            var stringReader = new StringReader("https://www.wp.pl;https://store.steampowered.com;https://www.youtube.com");
            Console.SetIn(stringReader);
            url = Address.GetURL();
            res.Add("https://www.wp.pl");
            res.Add("https://store.steampowered.com");
            res.Add("https://www.youtube.com");

            Assert.AreEqual(res, url);
        }
    }
    class LocTests
    {
        [Test]
        public void Get_WhenCalledAndTyped_ShouldReturnSame() // Sprawdza czy podana lokalizacja jest poprawnie wprowadzona
        {
            var Loc = new Loc();
            

            var stringReader = new StringReader("C:\a");
            Console.SetIn(stringReader);
            string act= "C:\a";
            string res = Loc.GetLocation();

            Assert.AreEqual(res, act);
        }
    }
    class CheckTests
    {
        [Test]
        public void Check_WhenCalled_ShouldReturnTrue() // Sprawdza czy metoda prawidłowo weryfikuje podany adres za istniejący
        {
            var Check = new Check();
            List<string> url = new List<string>();

            url.Add("https://www.wp.pl");
            bool res = Check.CheckURL(url);


            Assert.IsTrue(res);
        }
        [Test]
        public void Check_WhenCalled_ShouldReturnFalse() // Sprawdza czy metoda prawidłowo weryfikuje podany adres za nieistniejący
        {
            var Check = new Check();
            List<string> url = new List<string>();

            url.Add("http://abcd");
            bool res = Check.CheckURL(url);


            Assert.IsFalse(res);
        }
    }
    class ExtractTests
    {
        [Test]
        public void Check_WhenCalled_ShouldReturnSameString() // Sprawdza czy pobrane dane są identyczne 
        {
            var extract = new Extract();
            string actual = "{\n  \"tools\": {\n    \"dotnet\": \"7.0.100-preview.4.22178.9\",\n    \"runtimes\": {\n      \"dotnet\": [\n        \"$(VSRedistCommonNetCoreSharedFrameworkx6470PackageVersion)\"\n      ]\n    }\n  },\n  \"msbuild-sdks\": {\n    \"Microsoft.DotNet.Arcade.Sdk\": \"7.0.0-beta.22301.2\"\n  }\n}\n";
            string res = extract.GetData("https://raw.githubusercontent.com/dotnet/templating/main/global.json");

            Assert.AreEqual(res, actual);
        }
    }
    class SaveTests
    {
        [Test]
        public void Check_WhenCalled_ShouldCreate() // Sprawdza czy poprawnie został utworzony plik
        {
            var save = new Save();
            string json;
            using (StreamReader r = new StreamReader("C:/Users/kryst/source/repos/GetJSONFile/GetJSONFile.UnitTest/tsconfig1.json"))
            {
                json = r.ReadToEnd();
            }
            string name = "test.json";
            string loc = "C:/a";
            save.SaveData(json, loc, name);
            Assert.IsTrue(System.IO.File.Exists("C:/a/test.json"));
        }
    }
    class FileNameTests
    {
        [Test]
        public void Check_WhenCalled_ShouldReturnRightName() // Sprawdza czy nazwa pliku została poprawnie pobrana
        {
            var filename = new FileName();

            string url = "https://raw.githubusercontent.com/dotnet/templating/main/global.json";
            string res = filename.ExtractName(url);
            string actual = "global.json";

            Assert.AreEqual(actual, res);
        }
    }
    class CheckTypeTests
    {
        [Test]
        public void Check_WhenCalled_ShouldReturnTrue() // Sprawdza czy metoda poprawnie identyfikuje pliki JSON
        {
            var checktype = new CheckType();
            List<string> url = new List<string>();

            url.Add("https://raw.githubusercontent.com/dotnet/templating/main/global.json");
            bool res = checktype.CheckJSON(url);

            Assert.AreEqual(true, res);
        }
        [Test]
        public void Check_WhenCalled_ShouldReturnFalse() // Sprawdza czy metoda poprawnie nierozpoznała w podanym adresie pliku JSON
        {
            var checktype = new CheckType();
            List<string> url = new List<string>();

            url.Add("https://www.youtube.com");
            bool res = checktype.CheckJSON(url);

            Assert.AreEqual(false, res);
        }
    }
    class DirCheckTests
    {
        [Test]
        public void Check_Input_ShouldBeTrue() // Sprawdza czy podana ścieżka istnieje
        {
            var dircheck = new DirCheck();

            string loc = @"C:\";
            bool res = dircheck.CheckDir(loc);

            Assert.IsTrue(res);
        }
        [Test]
        public void Check_Input_ShouldBeFalse()// Sprawdza czy podana ścieżka nieistnieje
        {
            var dircheck = new DirCheck();

            string loc = @"C:\b";
            bool res = dircheck.CheckDir(loc);

            Assert.IsFalse(res);
        }
    }
}
