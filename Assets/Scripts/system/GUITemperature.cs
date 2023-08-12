using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace IHateWinter
{
    public class GUITemperature : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerTemp;
        [SerializeField] private TextMeshProUGUI environmentTemp;
        [SerializeField] private Image background;

        public static GUITemperature Instance;
        public static readonly Color WARMING_COLOR = new Color(0.9f, 0.35f, 0.07f);
        public static readonly Color COLD_COLOR = new Color(0.07f, 0.52f, 0.9f);
        public static readonly Color NEUTRAL_COLOR = Color.white;

        private void Awake()
        {
            background=GetComponentInChildren<Image>();
            Instance = this;
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
            background.color =
                (Fire2_5.NUMBER_FIRES_WARMING_PLAYER > 0) ? WARMING_COLOR :
                (t < 20) ? COLD_COLOR :
                NEUTRAL_COLOR;
        }

        internal void PlayerInsideFireWarm()
        {
        }

        internal void PlayerOutsideFireWarm()
        {
        }

    }

}
