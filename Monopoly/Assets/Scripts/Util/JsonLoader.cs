/*
 * JsonLoader.cs
 * Generic Json loading utility.
 * 
 * Date created : 8/03/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

public static class JsonLoader
{

    public static T LoadJsonAsset<T>(string filename)
    {
        try
        {
            TextAsset contents = Resources.Load<TextAsset>(filename);
            if (contents == null)
            {
                Debug.LogError(string.Format(
                    "Failed to load JSON asset '{0}'.",
                    filename));
                return default;
            }
            return JsonConvert.DeserializeObject<T>(contents.text);
        }
        catch (JsonException e)
        {
            Debug.LogError(string.Format(
                "Failed to deserialise JSON asset '{0}'",
                filename));
            Debug.LogException(e);
            return default;
        }
    }

}
