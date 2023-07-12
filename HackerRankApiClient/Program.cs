using Pastel;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HackerRankApiClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Program();
        }

        public Program()
        {
            Console.WriteLine($"{"Hacker News Testing!!".Pastel(Color.LawnGreen)}" + Environment.NewLine);
            var tsk = Task.Run(() =>
            {
                bool runTillQuit = true;
                while (runTillQuit)
                {
                    Console.WriteLine(Prompt());
                    var input = Console.ReadLine();

                    switch (input.Trim().ToUpper())
                    {
                        case "1": GetTopStories(); break;
                        case "2": GetStory(); break;
                        case "3": runTillQuit = false; break;
                        default:
                            Console.WriteLine($"{"Invalid input. Try again".Pastel(Color.Red)}");
                            break;
                    }
                }
            });
            tsk.Wait();
        }

        public string Prompt()
        {
            return "Enter either of the following :- " + Environment.NewLine
                + "Enter " + " 1 ".Pastel(Color.Yellow).PastelBg(Color.Red) + " for top " + "X".Pastel(Color.Yellow).PastelBg(Color.Red) + " stories in descending order." + Environment.NewLine
                + "Enter " + " 2 ".Pastel(Color.Yellow).PastelBg(Color.Red) + " for a specific story." + Environment.NewLine
                + "Enter " + " 3 ".Pastel(Color.Yellow).PastelBg(Color.Red) + " To Quit the Application." + Environment.NewLine;
        }

        private void ActionHeader(string headerText)
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"=============   {headerText.ToUpper()}   =====================".Pastel(Color.Lime));
        }
        private void ActionFooter(string footerText)
        {
            Console.WriteLine("=============   {footerText.ToUpper()}   =====================".Pastel(Color.Lime));
            Console.WriteLine(Environment.NewLine);
        }

        public async Task GetTopStories()
        {
            using (HeaderFooter hf = new HeaderFooter("Get Top Stories"))
            {
                Console.Write($"Enter number n {"(0 < n < 200)".Pastel(Color.Yellow).PastelBg(Color.Maroon)} to get top n stories. n = ");
                var input = Console.ReadLine();
                int count;
                if (int.TryParse(input, out count))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var httpRespMsg = client.GetAsync("https://localhost:7268/api/HakerRank/GetTopStories/" + count)
                            .ContinueWith(h => h.Result.Content.ReadAsStringAsync());
                        while (!httpRespMsg.IsCompleted)
                        {
                            Console.Write(".");
                            Thread.Sleep(1000);
                        }
                        Console.Out.WriteLine(Environment.NewLine);
                        var context = httpRespMsg.Result.Result;
                        var json = JsonValue.Parse(context).ToJsonString(new JsonSerializerOptions() { WriteIndented = true });
                        Console.WriteLine(json.Pastel(Color.Green));
                    }
                }
                else
                {
                    Console.Out.WriteLine($"{input.Pastel(Color.Red)} is not a valid input!");
                }
            }
        }

        public async Task GetStory()
        {
            using (HeaderFooter hf = new HeaderFooter("Get Story"))
            {
                Console.Write($"Enter a story id for its details. id = ");
                var input = Console.ReadLine();
                int id;
                if (int.TryParse(input, out id))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var httpRespMsg = client.GetAsync("https://localhost:7268/api/HakerRank/GetStory/" + id)
                            .ContinueWith(h => h.Result.Content.ReadAsStringAsync());
                        while (!httpRespMsg.IsCompleted)
                        {
                            Console.Write(".");
                            Thread.Sleep(1000);
                        }
                        Console.Out.WriteLine(Environment.NewLine);
                        var context = httpRespMsg.Result.Result;
                        var json = JsonValue.Parse(context).ToJsonString(new JsonSerializerOptions() { WriteIndented = true });
                        Console.WriteLine(json.Pastel(Color.Green));
                    }
                }
                else
                {
                    Console.Out.WriteLine($"{input.Pastel(Color.Red)} is not a valid input!");
                }
            }
        }
    }

    internal class HeaderFooter : IDisposable
    {
        private string _startmsg, _endmsg = string.Empty;
        public HeaderFooter()
        {
            Start();
        }
        public HeaderFooter(string msg)
        {
            _startmsg = msg.ToUpper() + " - START ";
            _endmsg = msg.ToUpper() + " - END ";
            Start();
        }

        public void Start()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"=============   {_startmsg}    =====================".Pastel(Color.Lime));
        }
        public void Dispose()
        {
            Console.WriteLine($"=============   {_endmsg}    =====================".Pastel(Color.Lime));
            Console.WriteLine(Environment.NewLine);
        }
    }

}