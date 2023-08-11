using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IHateWinter
{
    public class ButtonPlay : MonoBehaviour
    {

        public void OnPlay()
        {
            SceneManager.LoadScene("I-HATE-WINTER");
        }
    }
     
}
