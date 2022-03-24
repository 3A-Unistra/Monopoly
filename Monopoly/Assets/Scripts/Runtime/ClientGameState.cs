/*
 * ClientGameState.cs
 * Client game state handler.
 * 
 * Date created : 15/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Monopoly.UI;
using Monopoly.Util;

namespace Monopoly.Runtime
{

    public class ClientGameState : MonoBehaviour
    {

        public static ClientGameState current;
        public CardDisplay cardDisplay;
        public List<Dictionary<string, int>> squareData;

        private bool loadedLanguage = false;

        private void LoadLanguage(string language)
        {
            if (!loadedLanguage)
            {
                // init the language module
                // NOTE: PUT LANGUAGE MODULES HERE TO LOAD PLEASE
                // TODO: Error handle the init
                StringLocaliser.LoadStrings("Locales/english", "english",
                                            "English");
                StringLocaliser.LoadStrings("Locales/french", "french",
                                            "Français");
                loadedLanguage = true;
            }
            StringLocaliser.SetLanguage(language);
        }

        private void LoadGameData()
        {
            squareData =
                JsonLoader.LoadJsonAsset<List<Dictionary<string, int>>>
                ("Data/squares");
        }

        void Awake()
        {
            if (current != null)
            {
                Debug.LogError("Cannot create two concurrent gamestates!");
                Destroy(this);
            }
            current = this;
            // TODO: Use a persistent file to load the user preference from.
            LoadLanguage("french");
            LoadGameData();
            Debug.Log("Initialised gamestate.");
        }

        void OnDestroy()
        {
            if (current == this)
            {
                current = null;
                Debug.Log("Successfully destroyed gamestate.");
            }
        }

        private void DisplayCardPreview()
        {
            Ray cubeRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] cubeHits = Physics.RaycastAll(cubeRay);
            bool rendered = false;
            foreach (RaycastHit ray in cubeHits)
            {
                GameObject obj = ray.collider.gameObject;
                SquareCollider collider = obj.GetComponent<SquareCollider>();
                if (collider != null)
                {
                    Debug.Log("found " + collider.name);
                    cardDisplay.Render(collider.squareIndex);
                    rendered = true;
                    break;
                }
            }
            if (!rendered)
                cardDisplay.Render(-1); // hide card renderer
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                DisplayCardPreview();
        }

        public Dictionary<string, int> GetSquareDataIndex(int idx)
        {
            return squareData.First<Dictionary<string, int>>(
                (x) => x.ContainsKey("id") && x["id"] == idx
            );
        }

    }

}
