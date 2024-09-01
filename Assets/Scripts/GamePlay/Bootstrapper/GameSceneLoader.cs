using UnityEngine.SceneManagement;

namespace MobaPrototype.Scope
{
    public class GameSceneLoader : IPostAsyncStart
    {
        private int gameplaySceneIndex = 1;

        public void PostAsyncStart()
        {
            SceneManager.LoadScene(gameplaySceneIndex, LoadSceneMode.Additive);
        }
    }
}