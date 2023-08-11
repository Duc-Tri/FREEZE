using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    public enum GAME_MODE : ushort { NONE = 0, MENU, IN_GAME, GAME_OVER, PAUSE }

    public enum RESOURCE : ushort { NONE = 0, TREE, STONE, FLINT ,WOOD,RUSH}

    public enum INVENTORY_ITEM : ushort { NONE = 0, TOOL, RESOURCE }

    public enum TOOL : ushort { NONE = 0, CUTTER, AXE, FISHING_ROD, BOW, HAMMER, MACE, SPEAR }

}