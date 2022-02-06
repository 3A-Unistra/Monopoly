using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

using Monopoly.Util;

namespace Monopoly.Test
{

    public class JSONTest : MonoBehaviour
    {
        void Start()
        {
            string json = @"{
                'a': '123',
                'b': '456',
                'c': '789'
            }";
            Dictionary<string, string> jsonDic =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Debug.Log("a: " + jsonDic["a"]);
            Debug.Log("b: " + jsonDic["b"]);
            Debug.Log("c: " + jsonDic["c"]);

            StringLocaliser.LoadStrings("Locales/french",
                                        "french", "Français");
            StringLocaliser.SetLanguage("french");
            Debug.Log(string.Format("test: {0}",
                      StringLocaliser.GetString("test")));
        }

        void Update()
        {

        }
    }

}
