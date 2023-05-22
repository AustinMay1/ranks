using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;

namespace RanksBot
{
    public class Sheets
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "ranks";
        static readonly string SpreadsheetId = "1ulSzClXAEToaFdsnz2CtJg_AXyBkAMxm9ev2B9IwWKs";
        static readonly string Sheet = "Sheet1";
        static SheetsService Service = null!;

        public static void Connect()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("ranks.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            Service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            ReadEntries();
        }

        static void ReadEntries()
        {
            var range = $"{Sheet}!A1:B10";
            var request = Service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;

            if(values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    Console.WriteLine("{0}           |         {1}", row[0], row[1]);
                }
            }
            else
            {
                Console.WriteLine("No data :P");
            }
        }
    }
}
