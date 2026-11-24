using System.Collections.Generic;
using UnityEngine;

namespace BattleMobileIndia.Gameplay
{
    public class LootAndInventory : MonoBehaviour
    {
        public int maxSlots = 12;
        public List<string> items = new List<string>();

        public bool AddItem(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName) || items.Count >= maxSlots)
            {
                return false;
            }

            items.Add(itemName);
            return true;
        }

        public bool RemoveItem(string itemName)
        {
            return items.Remove(itemName);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LootPickup"))
            {
                string itemLabel = other.gameObject.name.Replace("(Clone)", "").Trim();
                if (AddItem(itemLabel))
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}

