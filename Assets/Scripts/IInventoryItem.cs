using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    // An object which can be put into Inventory
    public interface IInventoryItem
    {
        public string Name { get; }
    }
}