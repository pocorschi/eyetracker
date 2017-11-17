using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Tobii.Interaction;

namespace brdeye
{

    class ApiSendData
    {
        public string ScreenArea { get; set; }
    }

    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static async Task RunAsync(string screen)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                // HTTP POST
                var datatobeSent = new ApiSendData()
                {
                    ScreenArea = screen
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("eye", datatobeSent);

                if (response.IsSuccessStatusCode)
                {
                   
                }
            }
        }

        static void ScreenInteract(string screen)
        {
            Console.WriteLine("You are looking at the screen " + screen);
            RunAsync(screen);
        }

        static void Main(string[] args)
        {
            var host = new Host();

            var screenBoundsState = host.States.GetScreenBoundsAsync().Result;
            var screenBounds = screenBoundsState.IsValid
                ? screenBoundsState.Value
                : new Rectangle(0d, 0d, 1000d, 1000d);
            var width = screenBoundsState.Value.Width;
            var height = screenBoundsState.Value.Height;
            Console.WriteLine("screen width: " + width);
            Console.WriteLine("screen height: " + height);
            var virtualWindowsAgent = host.InitializeVirtualWindowsAgent();

            var rect_1_1 = new Rectangle
            {
                X = 0,
                Y = 0,
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };

            var rect_1_2 = new Rectangle
            {
                X = 1 * Convert.ToInt32(width / 4),
                Y = 0,
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };

            var rect_1_3 = new Rectangle
            {
                X = 2 * Convert.ToInt32(width / 4),
                Y = 0,
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };

            var rect_1_4 = new Rectangle
            {
                X = 3 * Convert.ToInt32(width / 4),
                Y = 0,
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };

            var rect_2_1 = new Rectangle
            {
                X = 0,
                Y = Convert.ToInt32(height / 2),
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };

            var rect_2_2 = new Rectangle
            {
                X = 1 * Convert.ToInt32(width / 4),
                Y = Convert.ToInt32(height / 2),
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };

            var rect_2_3 = new Rectangle
            {
                X = 2 * Convert.ToInt32(width / 4),
                Y = Convert.ToInt32(height / 2),
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };

            var rect_2_4 = new Rectangle
            {
                X = 3 * Convert.ToInt32(width / 4),
                Y = Convert.ToInt32(height / 2),
                Height = Convert.ToInt32(height / 2),
                Width = Convert.ToInt32(width / 4)
            };


            var window_1_1 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_1_1", rect_1_1).Result;
            var window_1_2 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_1_2", rect_1_2).Result;
            var window_1_3 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_1_3", rect_1_3).Result;
            var window_1_4 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_1_4", rect_1_4).Result;

            var window_2_1 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_2_1", rect_2_1).Result;
            var window_2_2 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_2_2", rect_2_2).Result;
            var window_2_3 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_2_3", rect_2_3).Result;
            var window_2_4 = virtualWindowsAgent.CreateFreeFloatingVirtualWindowAsync("window_2_4", rect_2_4).Result;

            var interactor_1_1 = host.InitializeVirtualInteractorAgent(window_1_1.Id);
            var interactor_1_2 = host.InitializeVirtualInteractorAgent(window_1_2.Id);
            var interactor_1_3 = host.InitializeVirtualInteractorAgent(window_1_3.Id);
            var interactor_1_4 = host.InitializeVirtualInteractorAgent(window_1_4.Id);

            var interactor_2_1 = host.InitializeVirtualInteractorAgent(window_2_1.Id);
            var interactor_2_2 = host.InitializeVirtualInteractorAgent(window_2_2.Id);
            var interactor_2_3 = host.InitializeVirtualInteractorAgent(window_2_3.Id);
            var interactor_2_4 = host.InitializeVirtualInteractorAgent(window_2_4.Id);

            interactor_1_1
                .AddInteractorFor(rect_1_1)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("1_1"));

            interactor_1_2
                .AddInteractorFor(rect_1_2)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("1_2"));

            interactor_1_3
                .AddInteractorFor(rect_1_3)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("1_3"));

            interactor_1_4
                .AddInteractorFor(rect_1_4)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("1_4"));

            interactor_2_1
                .AddInteractorFor(rect_2_1)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("2_1"));

            interactor_2_2
                .AddInteractorFor(rect_2_2)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("2_2"));

            interactor_2_3
                .AddInteractorFor(rect_2_3)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("2_3"));

            interactor_2_4
                .AddInteractorFor(rect_2_4)
                .WithGazeAware()
                .HasGaze(() => ScreenInteract("2_4"));

            Console.ReadKey(true);

            host.DisableConnection();
        }
    }
}
