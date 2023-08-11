using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    // An object which can be put into Inventory
    public interface IHarvestable
    {
        public string Name { get; }
    }
}