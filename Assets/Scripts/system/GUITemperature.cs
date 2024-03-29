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
        [SerializeField] private Image backgroundSnowflake;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image barPlayerTemp; // vertical, filled

        public static GUITemperature Instance;
        public static readonly Color WARMING_COLOR = new Color(0.9f, 0.35f, 0.07f);
        public static readonly Color COLD_COLOR = new Color(0.07f, 0.52f, 0.9f);
        public static readonly Color NEUTRAL_COLOR = Color.white;

        private void OnEnable()
        {
            Instance = this;
        }

        private void Awake()
        {
            backgroundSnowflake = GetComponentInChildren<Image>();
            playerTemp.text = environmentTemp.text = string.Empty;
        }

        public void UpdateEnvironmentTemp(float t)
        {
            environmentTemp.text = $"{t:0.0}�c";
        }

        public void UpdatePlayerTemp(float t)
        {
            playerTemp.text = $"{t:0.0}�c";

            //if (background != null)
            backgroundSnowflake.color =
                (Fire.NUMBER_FIRES_WARMING_PLAYER > 0) ? WARMING_COLOR :
                (t < Player.COOLING_TEMPERATURE) ? COLD_COLOR :
                NEUTRAL_COLOR;

            barPlayerTemp.fillAmount = (t - Player.LOWEST_BODY_TEMPERATURE_BEARABLE) / (37f - Player.LOWEST_BODY_TEMPERATURE_BEARABLE);
            barPlayerTemp.color = gradient.Evaluate(barPlayerTemp.fillAmount);
        }

        /*
        internal void PlayerInsideFireWarm()
        {
        }

        internal void PlayerOutsideFireWarm()
        {
        }
        */

    }

}
