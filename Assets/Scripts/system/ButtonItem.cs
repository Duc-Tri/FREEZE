using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IHateWinter
{
    public class ButtonItem : MonoBehaviour
    {
        [SerializeField] private Image picture;
        [SerializeField] private TextMeshProUGUI tmp;

        public InventoryItem item;

        private void Awake()
        {
            //picture = GetComponentInChildren<Image>();
            //tmp = GetComponentInChildren<TextMeshProUGUI>();

            SetItem(null);
        }

        /*
        public void SetItemFromResource(IHarvestable ih)
        {
            //Debug.Log("SetItem: " + ih.Name);

            if (ih is AResource resource)
            {
                SetItem(GameManager.GameData.SearchItemByResourceType(resource.type));
            }
        }
        */

        public void SetItem(InventoryItem ii)
        {
            item = ii;
            if (item != null)
            {
                picture.sprite = item.UISprite;
                picture.color = Color.white;
                picture.enabled = true;
                tmp.text = item.name + (item.ItemType == ITEM_TYPE.RESOURCE ? "(" + item.InStack + ")" : "");
            }
            else
            {
                picture.enabled = false;
                picture.sprite = null;
                picture.color = Color.clear;
                tmp.text = string.Empty;
            }
        }

    }

}
