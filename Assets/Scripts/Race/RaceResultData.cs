// ゲームシーンからリザルトシーンへ最終順位データを渡すための静的な受け渡しクラス。
// SceneManager.LoadSceneはドメインリロードを伴わないため、静的フィールドで受け渡しできる。
public static class RaceResultData
{
    public struct RacerResult
    {
        public string Name;
        public int Rank;
        public bool IsPlayer;

        public RacerResult(string name, int rank, bool isPlayer)
        {
            Name = name;
            Rank = rank;
            IsPlayer = isPlayer;
        }
    }

    public static RacerResult[] FinalResults { get; private set; }

    public static void SetFinalResults(RacerResult[] results)
    {
        FinalResults = results;
    }
}
