using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUWM_Schedule_Bot.Properties;

namespace NUWM_Schedule_Bot
{
    public class Request
    {
        public static async Task<string> PostRequestAsync(string group, string startDate, string endDate)
        {
            string faculty = GetFaculty(group);
            
            string teacher = "";
            //string group = "КІ-51";
            //string startDate = "01.01.2020";
            //string endDate = "07.02.2020";
            string html;

            string urlRequest = "http://desk.nuwm.edu.ua/cgi-bin/timetable.cgi?n=700";
            string postData = $"faculty={faculty}&teacher={teacher}&group={group}&sdate={startDate}&edate={endDate}&n=700";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            WebRequest request = WebRequest.Create(urlRequest);
            request.Method = "POST";
            byte[] byteArray = System.Text.Encoding.GetEncoding("windows-1251").GetBytes(postData);
            request.ContentLength = byteArray.Length;

            using (Stream dataStream = request.GetRequestStream())
            {
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
            }
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
                html = reader.ReadToEnd();
            }
            response.Close();
            return html;
        }
        public static string GetFaculty(string group)
        {
            int[] faculty_id = { 4, 7, 8, 9, 11, 13, 1019, 1022, 1023, 1024, 1060, 1064, 1066, 1068, 1069, 1070 };
            List<string> groups = new List<string>();
            for(int i=0;i<faculty_id.Length;i++)
            {
                groups.Add(Resources.ResourceManager.GetString($"faculty{faculty_id[i]}"));
                if (groups[i].Contains(group))
                {
                    return faculty_id[i].ToString();
                }
                else continue;
            }
            return "0";
        }
    }
}
