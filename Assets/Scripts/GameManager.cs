using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HomeAR.Events;

namespace HomeAR.Managers
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene("GameScene");
        }

    }


}
