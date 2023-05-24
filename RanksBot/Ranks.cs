namespace RanksBot;

static class Ranks
{
    public static readonly Dictionary<string, int> ranks = new Dictionary<string, int>()
    {
        { "Recruit", 60 },
        { "Corporal", 180 },
        { "Novice", 730 },
    };
} // { RankName : TimeToPromotionInDays }