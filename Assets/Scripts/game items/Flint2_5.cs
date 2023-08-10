
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace IHateWinter
{
    // 
    public class Flint2_5 : AResource, IInventoryItem
    {
        string IInventoryItem.Name { get => name; }

        private void Awake()
        {
            Init(RESOURCE.FLINT);
        }

    }

}