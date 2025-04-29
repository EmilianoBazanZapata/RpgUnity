using System.Collections.Generic;
using Game.InventoryAndObjects.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI.Scripts
{
    public class CraftList: MonoBehaviour , IPointerDownHandler
    {
        [SerializeField] private Transform craftSlotParent;
        [SerializeField] private GameObject craftSlotPrefab;

        [SerializeField] private List<ItemDataEquipment> craftEquipment;


        void Start()
        {
            transform.parent.GetChild(0).GetComponent<CraftList>().SetupCraftList();
            SetupDefaultCraftWindow();
        }

        public void SetupCraftList()
        {
            for (int i = craftSlotParent.childCount - 1; i >= 0; i--)
            {
                Transform child = craftSlotParent.GetChild(i);
                if (child.gameObject != craftSlotPrefab)  // <<--- ¡Evita destruir el prefab!
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < craftEquipment.Count; i++)
            {
                GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
                newSlot.SetActive(true);
                newSlot.GetComponent<CraftSlot>().SetupCraftSlot(craftEquipment[i]);
            }
        }



        public void OnPointerDown(PointerEventData eventData)
        {
            SetupCraftList();
        }

        public void SetupDefaultCraftWindow()
        {
            if (craftEquipment[0] != null)
                GetComponentInParent<UserInterface>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}