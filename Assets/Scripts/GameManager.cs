using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HomeAR.Events;

namespace HomeAR.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] VoidEvent onGameStart;
        // Start is called before the first frame update
        void Start()
        {
            onGameStart.Raise();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}
