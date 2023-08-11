using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IHateWinter
{
    public class GUITemperature : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerTemp;
        [SerializeField] private TextMeshProUGUI environmentTemp;

        private void Awake()
        {
            playerTemp.text = environmentTemp.text = string.Empty;

            TemperatureSystem.OnTemperatureChange -= UpdateEnvironmentTemp;
            TemperatureSystem.OnTemperatureChange += UpdateEnvironmentTemp;

            Player2_5.OnBodyTemperatureChange -= UpdatePlayerTemp;
            Player2_5.OnBodyTemperatureChange += UpdatePlayerTemp;
        }

        public void UpdateEnvironmentTemp(float t)
        {
            environmentTemp.text = $"{t:0.0}°c";
        }

        public void UpdatePlayerTemp(float t)
        {
            playerTemp.text = $"{t:0.0}°c";
        }

    }

}
