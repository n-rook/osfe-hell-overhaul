using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Hell_Overhaul
{

    class DelayTracker
    {
        private static readonly float LOG_EVERY_N = 1000;

        class SingleEnemyDelayTracker
        {
            float totalOld = 0.0f;
            float totalNew = 0.0f;
            int count = 0;

            internal void Increment(float latestOld, float latestNew)
            {
                totalOld += latestOld;
                totalNew += latestNew;
                count++;
            }

            internal float averageOld()
            {
                if (count == 0)
                {
                    return 0.0f;
                }
                return totalOld / count;
            }

            internal float averageNew()
            {
                if (count == 0)
                {
                    return 0.0f;
                }
                return totalNew / count;
            }
        }

        private Dictionary<string, SingleEnemyDelayTracker> delayByName = new Dictionary<string, SingleEnemyDelayTracker>();
        private int log_count = 0;

        public void trackDelay(Enemy enemy, float original, float newValue)
        {
            string name = enemy.name;

            SingleEnemyDelayTracker tracker;
            if (delayByName.ContainsKey(name))
            {
                tracker = delayByName[name];
            }
            else
            {
                tracker = new SingleEnemyDelayTracker();
                delayByName[name] = tracker;
            }

            tracker.Increment(original, newValue);


        }

        private void maybeLog(string name, SingleEnemyDelayTracker tracker, float oldValue, float newValue)
        {
            log_count++;
            if (log_count > LOG_EVERY_N)
            {
                log_count = 0;

                Debug.Log($"Delay for {name} changed from {oldValue} to {newValue} (avg: {tracker.averageOld()} -> {tracker.averageNew()}");
            }
        }
    }
}
