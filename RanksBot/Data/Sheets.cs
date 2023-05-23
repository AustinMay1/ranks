using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;

namespace RanksBot.Data
{
    public class Sheets
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "ranks";
        static readonly string SpreadsheetId = "1ulSzClXAEToaFdsnz2CtJg_AXyBkAMxm9ev2B9IwWKs";
        static readonly string Sheet = "Sheet1";
        static SheetsService Service = null!;

        public static void Connect(string rank)
        {
            GoogleCredential credential;
            using (var stream = new FileStream("ranks.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            ReadEntries(rank);
        }

        static void ReadEntries(string rank)
        {
            var range = $"{Sheet}!A1:C500";
            var request = Service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;
            var today = DateTime.Now;
            TimeSpan diff;
            

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    
                    diff = today - DateTime.Parse((string) row[2]);
                    
                    if ((string) row[1] == rank)
                    {
                        if (diff.TotalDays <= Ranks.ranks[rank])
                        {
                            Console.WriteLine($"User: {row[0]}     |      Rank: {row[1]}     |     Total Days: {(int) diff.TotalDays}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No data :P");
            }
        }
    }
}
