using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;

namespace RanksBot.Data
{
    public class Sheets
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "ranks";
        static readonly string SpreadsheetId = "1ulSzClXAEToaFdsnz2CtJg_AXyBkAMxm9ev2B9IwWKs";
        static readonly string Sheet = "Sheet1";
        static SheetsService Service = null!;

        public static IList<IList<object>> Connect(string rank)
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
            
            // WriteEntry("Not Ur Hero#4094");

            return ReadEntries(rank);
        }

        static IList<IList<object>> ReadEntries(string rank)
        {
            var range = $"{Sheet}!A1:C500";
            var request = Service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = request.Execute();
            var values = response.Values;
            
            return values ?? null;
        }

        static void WriteEntry(string value)
        {
            var range = $"{Sheet}!D1";
            var valueRange = new ValueRange();
            var objects = new List<object>(){ value };
            valueRange.Values = new List<IList<object>> { objects };

            var appendReq = Service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendReq.ValueInputOption =
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var response = appendReq.Execute();
            
            Console.WriteLine(response.Updates);
        }
    }
}
