using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace IHateWinter
{

    // To call from Gamemanager
    public class TemperatureSystem
    {
        private static TemperatureSystem instance;

        private TemperatureTime[] temperatureTimes;
        private int currentIndex;
        private float currentTime, currentTemperature, lastTime, startTemp, endTemp, timeSpan;

        public static TemperatureSystem Instance
        {
            get
            {
                if (instance == null) instance = new TemperatureSystem();

                return instance;
            }
        }

        public TemperatureSystem() { }

        public void Update(float deltaTime)
        {
            currentTime += deltaTime;
            if (currentTime >= temperatureTimes[currentIndex].time)
            {
                lastTime = temperatureTimes[currentIndex].time;
                currentTemperature = startTemp = endTemp;

                if (currentIndex < temperatureTimes.Length - 1)
                {
                    timeSpan = temperatureTimes[currentIndex + 1].time - lastTime;
                    currentIndex++;
                }

                endTemp = temperatureTimes[currentIndex].temperature;

                //Debug.Log($"■■■ currentTime:{currentTime:0.00} currentTemp:{currentTemperature:0.00} ■ (startTemp{startTemp} -> endTemp{endTemp})");
            }
            else if (currentIndex < temperatureTimes.Length)
            {
                currentTemperature = Mathf.Lerp(startTemp, endTemp, (currentTime - lastTime) / timeSpan);
                //Debug.Log($"currentTime:{currentTime:0.00} currentTemp:{currentTemperature:0.00} ■ (startTemp{startTemp} -> endTemp{endTemp})");
            }
        }

        public void Init(TemperatureTime[] tt)
        {
            temperatureTimes = tt;

            //  (temperatureTimes[0].time > 0) ALWAYS THE CASE
            for (int i = 1; i < tt.Length; i++)
                Assert.IsTrue(tt[i].time > 0 && tt[i].time > tt[i - 1].time);

            currentTime = 0;
            currentIndex = 0;
            currentTemperature = startTemp = endTemp = temperatureTimes[0].temperature;
            timeSpan = temperatureTimes[0].time;
        }

    }

    [Serializable]
    public struct TemperatureTime
    {
        public float time; // in seconds
        public float temperature;
    }

}
