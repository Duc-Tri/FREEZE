using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IHateWinter
{
    public class IntroButtons : MonoBehaviour
    {

        public void OnPlay()
        {
            SceneManager.LoadScene("I-HATE-PENGUINS");
        }

        public void OnQuit()
        {
            Application.Quit();
        }

    }
}
