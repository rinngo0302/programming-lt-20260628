using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadTitle()
    {
        SceneManager.LoadScene(SceneNames.Title);
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene(SceneNames.Game);
    }

    public static void LoadResult()
    {
        SceneManager.LoadScene(SceneNames.Result);
    }
}
