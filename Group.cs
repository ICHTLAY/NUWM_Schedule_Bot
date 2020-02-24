using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NUWM_Schedule_Bot
{
    public static class Group
    {
        ///Template
        public static async Task GetJSON()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            WebRequest request;
            WebResponse response;
            Stream stream;
            StreamReader reader;
            string query = "";
            string faculty = "";
            string url = $"http://desk.nuwm.edu.ua/cgi-bin/timetable.cgi?n=701&lev=142&faculty={faculty}&query={query}";

            request = WebRequest.Create(url);
            request.Method = "GET";

            response = await request.GetResponseAsync();
            using (stream = response.GetResponseStream())
            {
                using (reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251")))
                {
                    string text = reader.ReadToEnd();
                    Console.WriteLine(text);
                    Groups groups = JsonConvert.DeserializeObject<Groups>(text);
                }
            }
        }
        public static async Task GetAllGroups()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            WebRequest request;
            WebResponse response;
            Stream stream;
            StreamReader reader;
            string url;

            string[] alphabet = { "а", "б", "в", "г", "д", "е", "ж", "з", "і", "к", "л", "м", "о", "п", "р", "т", "у", "ф", "ц" };
            int[] faculty_id = { 4, 7, 8, 9, 11, 13, 1017, 1018, 1019, 1022, 1023, 1024, 1060, 1064, 1066, 1068, 1069, 1070 };


            for (int i = 0; i < faculty_id.Length; i++)
            {
                string groupdata = "";
                foreach (string s in alphabet)
                {
                    url = $"http://desk.nuwm.edu.ua/cgi-bin/timetable.cgi?n=701&lev=142&faculty={faculty_id[i].ToString()}&query={s}";
                    //url = $"http://desk.nuwm.edu.ua/cgi-bin/timetable.cgi?n=701&lev=142&faculty={"4"}&query={"б"}";
                    request = WebRequest.Create(url);
                    request.Method = "GET";

                    response = await request.GetResponseAsync();
                    using (stream = response.GetResponseStream())
                    {
                        using (reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251")))
                        {
                            string text = reader.ReadToEnd();
                            try
                            {
                                Groups g = JsonConvert.DeserializeObject<Groups>(text);
                                Console.WriteLine(g.suggestions[0]);
                                foreach (string str in g.suggestions)
                                {
                                    groupdata += $"{str}, ";
                                }

                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                    await Task.Delay(200);
                }
                File.WriteAllText($@"F:\faculty{faculty_id[i].ToString()}.txt", groupdata);
            }
        }
        public static void SortGroups()
        {
            List<string> groups = new List<string>();

            int[] faculty_id = { 4, 7, 8, 9, 11, 13, 1019, 1022, 1023, 1024, 1060, 1064, 1066, 1068, 1069, 1070 };
            for (int i = 0; i < faculty_id.Length; i++)
            {
                groups.Add(File.ReadAllText($@"F:\groups\faculty{faculty_id[i].ToString()}.txt"));
            }
            List<string[]> groupsArray = new List<string[]>();

            for (int i = 0; i < faculty_id.Length; i++)
            {
                string[] arr = groups[i].Split(',');
                groupsArray.Add(arr);
                groupsArray[i] = arr.Distinct().ToArray();
                File.WriteAllText($@"F:\groups\sorted_faculty{faculty_id[i].ToString()}.txt", ReturnStringFromArray(groupsArray[i]));
            }
            foreach (string[] strarr in groupsArray)
            {
                foreach (string str in strarr)
                {
                    Console.Write($"{str},");
                }
                Console.WriteLine();
            }
        }
        public static string ReturnStringFromArray(string[] array)
        {
            string output = "";
            foreach (string str in array)
            {
                output += $"{str},";
            }
            return output;
        }
        public static string[] ReturnArrayFromString(string str)
        {
           return str.Split(',');
        }
    }
}
