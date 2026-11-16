//Test-URLs:
//http://nowhere.org 
//https://www.startpage.com 
//https://www.govt.nz/ 
//https://www.htl-leonding.at/ 
//https://www.orkney.com/ 
//https://aka.ms/new-console-template
//https://tangodemayohotel.com-hotel.com/

//on one line
//http://nowhere.org https://www.startpage.com https://www.govt.nz/ https://www.htl-leonding.at/ https://www.orkney.com/ https://aka.ms/new-console-template https://tangodemayohotel.com-hotel.com/ 

using System.Diagnostics;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");
var progSw = new Stopwatch();
progSw.Start();

//Default URLs if none provided on command line
var defaultUrls = new[]
{
    "http://nowhere.org",
    "https://www.startpage.com",
    "https://www.govt.nz/",
    "https://www.htl-leonding.at/",
    "https://www.orkney.com/",
    "https://aka.ms/new-console-template",
    "https://tangodemayohotel.com-hotel.com/"
};

//Extract the URLS from the command line (or fallback to default test URLs)
var cmdUrls = args.Length > 0
    ? args
    : Environment.CommandLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

var urls = cmdUrls.Length > 0 ? cmdUrls : defaultUrls;

//a list for all the work to be done
var taskList = new List<Task<UrlResponseTimeImageCount>>();
using var client = new HttpClient();

async Task<UrlResponseTimeImageCount> ProcessUrlAsync(string url)
{
    Console.WriteLine($"Getting {url}...");
    var sw = Stopwatch.StartNew();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    sw.Stop();
    var imageCount = Regex.Matches(html, @"<img\b", RegexOptions.IgnoreCase).Count;
    return new UrlResponseTimeImageCount(url, sw.ElapsedMilliseconds, imageCount);
}

foreach (var url in urls)
{
    // TODO Process this URL concurrently: download its HTML with HttpClient.GetAsync,
    // measure the response time, count its <img> tags, and return a result record.
    taskList.Add(ProcessUrlAsync(url));
}

// TODO Provide a CancellationTokenSource and token so the spinner can be stopped
// after all URL-processing tasks have completed.
using var cts = new CancellationTokenSource();
var token = cts.Token;

// TODO Run a concurrent spinner task that prints a dot every 250 ms until its
// cancellation token reports that cancellation was requested.
var spinnerTask = Task.Run(async () =>
{
    while (!token.IsCancellationRequested)
    {
        Console.Write(".");
        try
        {
            await Task.Delay(250, token);
        }
        catch (OperationCanceledException)
        {
            break;
        }
    }
});

// TODO Await tasks in the task list; when they are ready,  collect every URL result, then cancel the spinner and
// stop the stopwatch that measures the program's total elapsed time.
var results = await Task.WhenAll(taskList);
cts.Cancel();
await spinnerTask;
progSw.Stop();


//already there ;)
Console.WriteLine();
Console.WriteLine($"Program took {progSw.ElapsedMilliseconds} ms to run.");
Console.WriteLine($"Sequentially, this would have taken {results.Sum(t => t.ResponseTimeMs)} ms.");
foreach(var result in results.OrderBy(result => result.ResponseTimeMs))
{
    Console.WriteLine(result);
}
