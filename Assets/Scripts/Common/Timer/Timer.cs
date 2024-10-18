using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace TTT
{

    public class Timer
    {
        public delegate void IntervalAct(Object args); 
        //总时间和当前持续时间
        private float curtime = 0;
        private float totalTime = 0;

        //激活
        public bool isActive;
        //计时结束是否销毁
        public bool isDestroy;
        //是否暂停
        public bool isPause;

        //间隔事件和间隔事件——Dot
        private float intervalTime = 0;
        private float curInterval = 0;
        private IntervalAct onInterval;
        private Object args;

        //进入事件
        public Action onEnter;
        private bool isOnEnter = false;
        //持续事件
        public Action onStay;
        //退出事件
        public Action onEnd;

        public Timer(float totalTime, bool isDestroy = true, bool isPause = false)
        {
            curtime = 0;
            this.totalTime = totalTime;
            isActive = true;
            this.isDestroy = isDestroy;
            this.isPause = isPause;
            TimerManager.AddTimer(this);
        }

        public void Run()
        {
            //暂停计时
            if (isPause || !isActive)
                return;

            if (onEnter != null)
            {
                if (!isOnEnter)
                {
                    isOnEnter = true;
                    onEnter();
                }
            }

            //持续事件
            if (onStay != null)
                onStay();

            curtime += Time.deltaTime;

            //间隔事件
            if (onInterval != null)
            {
                curInterval += Time.deltaTime;
                if (curInterval > intervalTime)
                {
                    onInterval(args);
                    curInterval = 0;
                }
            }

            //计时结束
            if (curtime > totalTime)
            {
                curtime = 0;
                isActive = false;
                if (onEnd != null)
                {
                    onEnd();
                }
            }
        }

        //设置间隔事件
        public void SetInterval(float interval, IntervalAct intervalFunc, Object args = null)
        {
            this.intervalTime = interval;
            onInterval = intervalFunc;
            curInterval = 0;
            this.args = args;
        }

        //重置计时器
        public void Reset()
        {
            curtime = 0;
            isActive = true;
            isPause = false;
            curInterval = 0;
            isOnEnter = false;
        }

        public void Stop()
        {
            isActive = false;
        }

        //获取剩余时间
        public float GetRemainTime()
        {
            return totalTime - curtime;
        }
    }
}