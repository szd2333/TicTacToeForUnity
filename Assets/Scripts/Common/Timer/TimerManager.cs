using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class TimerManager : SingletonMonoBehaviour<TimerManager>
    {
        private List<Timer> _timerList = new List<Timer>();

        public static void AddTimer(Timer t)
        {
            if (Instance._timerList.Contains(t))
            {
                return;
            }
            Instance._timerList.Add(t);
        }

        public static void RemoveTimer(Timer t)
        {
            if (!Instance._timerList.Contains(t))
            {
                return;
            }
            Instance._timerList.Remove(t);
        }

        void Update()
        {
            for (int i = 0; i < _timerList.Count; i++)
            {
                Timer timer = _timerList[i];
                timer.Run();
                if (!timer.isActive && timer.isDestroy)
                {
                    _timerList.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }
            
        }
    }
}