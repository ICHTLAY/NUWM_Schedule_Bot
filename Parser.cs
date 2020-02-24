using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NUWM_Schedule_Bot
{
    public static class Parser
    {
        public static async Task<HtmlNodeCollection> GetTable(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode parent_node = doc.DocumentNode.SelectSingleNode("//div[@class='jumbotron pf16']/div[@class='container']");
            if (parent_node!=null)
            {
                return parent_node.SelectNodes(".//h4|.//td");
            }
            else
            {
                return doc.DocumentNode.SelectNodes(".//p|.//div[@class='alert alert-warning']|.//div[@class='alert alert-info']");
            }
            
        }
        public static async Task<string> GetData(HtmlNodeCollection dataNode)
        {
            string[] dayOfWeek = { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
            string result="";
            List<string> data = new List<string>();
            Dictionary<string, int> days = new Dictionary<string, int>();

            foreach (var i in dataNode)
            {
                data.Add(i.InnerText);
            }
            data.RemoveAt(1);

            for(int i = 0; i<data.Count;i++)
            {
                
                if (i == 1)
                    data.Insert(i, " ");
                for(int j = 0; j<dayOfWeek.Length;j++)
                {
                    if (data[i].Contains(dayOfWeek[j]))
                    {
                        days.Add(dayOfWeek[j], i);
                        data[i] = $"<b>{data[i]}</b>";
                    }
                    
                }
            }

            foreach (var i in data)
            {
                result += $"{i}\n";
            }
            return result;
        }
        public static async Task<string> GetTableData(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode parent_node = doc.DocumentNode.SelectSingleNode("//div[@class='jumbotron pf16']");

            HtmlNode node = parent_node.SelectSingleNode("./p");
            if (node != null) return "Перевірте введені дані і спробуйте ще раз";

            node = parent_node.SelectSingleNode("./div[@class='alert alert-warning']");
            if (node != null) return "Знайдено більше одної назв груп, що входять до Вашого пошуку. Уточніть назву групи і спробуйте ще раз";

            node = parent_node.SelectSingleNode("./div[@class='alert alert-info']");
            if (node != null) return "За вашим запитом записів не знайдено. Змініть або вкажіть більш точні дані для формування розкладу.";
            //-------------//
            var nodes = parent_node.SelectNodes(".//div[@class='container']//h4|.//div[@class='container']//td");

            string[] dayOfWeek = { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
            string result = "";
            List<string> data = new List<string>();
            Dictionary<int, string> days = new Dictionary<int, string>();

            foreach (var i in nodes)
            {
                data.Add(i.InnerText);
            }
            data.RemoveAt(1);

            //for (int i = 0; i < data.Count; i++)
            //{

            //    //if (i == 1)
            //    //    data.Insert(i, " ");
            //    for (int j = 0; j < dayOfWeek.Length; j++)
            //    {
            //        if (data[i].Contains(dayOfWeek[j]))
            //        {
            //            days.Add(i, dayOfWeek[j]);
            //            data[i] = $"<b>{data[i]}</b>";
            //        }
            //    }
            //}

            List<List<string>> daysData = new List<List<string>> {new List<string>() };
            
            int count = 0;

            for(int i = 0; i<data.Count;i++)
            {
                for (int j=0;j<dayOfWeek.Length;j++)
                {
                    if (data[i].Contains(dayOfWeek[j]))
                    {
                        count++;
                        daysData.Add(new List<string>());
                    }
                }
                daysData[count].Add(data[i]);
            }

            for(int i =0;i<daysData.Count;i++)
            {
                for(int j=0;j<daysData[i].Count;j++)
                {
                    if (j % 3 == 1) daysData[i][j] = daysData[i][j].Insert(1, " пара");
                    if (j % 3 == 2) daysData[i][j] = daysData[i][j].Insert(5, " - ");
                }
                daysData[i].Add("<b>---------------------------------------------------------------------------------------------------</b>");
                daysData[i][0] = $"<b>{daysData[i][0]}</b>";
            }

            //foreach (var i in data)
            //{
            //    result += $"{i}\n";
            //}

            foreach(List<string> l in daysData)
            {
                foreach(string s in l)
                {
                    result+= $"{s}\n";
                }
            }
            return result;
        }
        public static async Task<string> GetSchedule(string groupName)
        {
            string html = await Request.PostRequestAsync(groupName, "24.02.2020", "28.02.2020");
            string data = await Parser.GetTableData(html);
            return data;
        }
    }
}
