using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IHateWinter
{
    public enum GAME_STATE : ushort { NONE = 0, MENU, IN_GAME, GAME_OVER, PAUSE }

    public enum RESOURCE : ushort { NONE = 0, TREE, STONE, FLINT, WOOD, RUSH, FISH }

    public enum ITEM_TYPE : ushort { NONE = 0, TOOL, RESOURCE }

    public enum TOOL : ushort { NONE = 0, CUTTER, AXE, FISHING_ROD, BOW, HAMMER, MACE, SPEAR }

    public enum CRAFT_TEMP : ushort { NONE = 0, FIRE_CAMP, FISHING_ROD }

    public static class Commons
    {
        public static bool NearEnoughXZ(Vector3 pos, Vector3 target, float distance) => Mathf.Abs(target.z - pos.z) <= distance && Mathf.Abs(target.x - pos.x) <= distance;

        public static bool NearEnoughXYZ(Vector3 pos, Vector3 target, float distance) =>
            Mathf.Abs(target.z - pos.z) <= distance &&
            Mathf.Abs(target.y - pos.y) <= distance &&
            Mathf.Abs(target.x - pos.x) <= distance;
    }

}