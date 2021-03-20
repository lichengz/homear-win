using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeAR.Events;

namespace HomeAR.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] VoidEvent onGameStart;
        [SerializeField] IntEvent onNumberChanged;
        [SerializeField] int num = 0;
        // Start is called before the first frame update
        void Start()
        {
            onGameStart.Raise();
        }

        // Update is called once per frame
        void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     num++;
            //     onNumberChanged.Raise(num);
            // }
        }
    //     public void PrintNum(int testNum)
    //     {
    //         Debug.Log(testNum);
    //     }
    }


}
