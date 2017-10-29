using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SwissArmyKnife
{
    public class FirstSceneTracker : SingletonPersistent<FirstSceneTracker>
    {

        public Scene _firstScene;
        public string _firstSceneName;

        public override void AwakeSingleton()
        {
            _firstScene = SceneManager.GetActiveScene();
            _firstSceneName = _firstScene.name;
        }

        private bool InternalIsFirstScene()
        {
            return SceneManager.GetActiveScene() == _firstScene;
        }

        public static bool IsFirstScene()
        {
            return Instance == null ? false : Instance.InternalIsFirstScene();
        }
        public static bool IsNotFirstScene()
        {
            return Instance == null ? false : !Instance.InternalIsFirstScene();
        }
    }
}