using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class Loader
{
    private static HttpClient httpClient = new HttpClient();

    public static Action<bool> FileLoaded;

    public static async Task GetFile(int index)
    {
        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
        "https://www.sevsu.ru/univers/shedule/");

        HttpResponseMessage response;

        try
        {
            response = await httpClient.SendAsync(request);
        }
        catch
        {
            FileLoaded?.Invoke(false);
            return;
        }

        string source = await response.Content.ReadAsStringAsync();

        Regex regex = new Regex(@"univers/shedule/download\W*php\?file=\S*""");

        string m = regex.Matches(source)[index].Value;

        await DownloadFile(m);

        response.Dispose();
    }

    private static async Task DownloadFile(string str)
    {
        try
        {
            using (var stream = await httpClient.GetStreamAsync("https://sevsu.ru/" + str))
            {
                using (var fileStream = new FileStream(GameManager.Path, FileMode.OpenOrCreate, 
                    FileAccess.Write, FileShare.None, 81920, true))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }
        catch
        {
            FileLoaded?.Invoke(false);
            return;
        }

        FileLoaded?.Invoke(true);
    }
}
