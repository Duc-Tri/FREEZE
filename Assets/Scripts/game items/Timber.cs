using UnityEngine;

namespace IHateWinter
{
    public class Timber : AResource
    {
        private void Awake()
        {
            Init(RESOURCE.TREE);
            tag = "Untagged"; // FOR THE MOMENT ...

        }

    }

}