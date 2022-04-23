/*
 * UIPlayerInfo.cs
 * UI handler for player information.
 * 
 * Date created : 12/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monopoly.Classes;
using Monopoly.Runtime;

namespace Monopoly.UI
{

    public class UIPlayerInfo : MonoBehaviour
    {

        public GameObject PlayerFieldPrefab;

        public GameObject FieldObject;
        [HideInInspector]
        public List<PlayerField> FieldList;

        public void AddPlayer(Player player, bool me)
        {
            GameObject field =
                Instantiate(PlayerFieldPrefab, FieldObject.transform);
            PlayerField fieldScript = field.GetComponent<PlayerField>();
            fieldScript.SetUser(player, me);
            FieldList.Add(fieldScript);
        }

        public void SetActive(Player player)
        {
            foreach (PlayerField p in FieldList)
                p.SetActive(p.HandlesPlayer(player));
        }

        public void SetMoney(Player player, int amount)
        {
            foreach (PlayerField p in FieldList)
            {
                if (p.HandlesPlayer(player))
                {
                    p.SetMoney(amount, true);
                    return;
                }
            }
        }

    }

}
