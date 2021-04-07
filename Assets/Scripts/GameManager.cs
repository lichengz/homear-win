using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HomeAR.Events;
using UnityEngine.UI;

namespace HomeAR.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE, LoadSceneMode.Additive);
        }

    }


}
