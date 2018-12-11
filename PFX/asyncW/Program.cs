using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace asyncW
{
    class Program
    {
      static  private async void ReadFromWeb()
        {
            var web = new WebClient();
            var text =
                  await web.DownloadStringTaskAsync("https://msdn.microsoft.com/ru-ru/");
            Console.WriteLine(text.Length);
        }
    
    static void Main(string[] args)
        {

            ReadFromWeb();
            Thread.Sleep(3000);
        }

    }
}

