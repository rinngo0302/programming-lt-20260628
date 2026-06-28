// ゲームシーンからリザルトシーンへ最終順位データを渡すための静的な受け渡しクラス。
// SceneManager.LoadSceneはドメインリロードを伴わないため、静的フィールドで受け渡しできる。
public static class RaceResultData
{
    public struct RacerResult
    {
        public string Name;
        public int Rank;

        public RacerResult(string name, int rank)
        {
            Name = name;
            Rank = rank;
        }
    }

    public static RacerResult[] FinalResults { get; private set; }

    public static void SetFinalResults(RacerResult[] results)
    {
        FinalResults = results;
    }
}
