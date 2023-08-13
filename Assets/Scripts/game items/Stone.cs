using UnityEngine;

namespace IHateWinter
{
    public class Stone : AResource
    {
        private void Awake()
        {
            Init(RESOURCE.STONE);
            tag = "Untagged"; // FOR THE MOMENT ...

        }

    }

}