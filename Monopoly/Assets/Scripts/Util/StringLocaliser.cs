/*
 * StringLocaliser.cs
 * JSON string deserialiser class for key-value string pairs used for locales.
 * 
 * Date created : 06/02/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Monopoly.Util
{

    /**
     * <summary>
     *     JSON string deserialiser and key-value handler for localisation
     *     files.
     * </summary>
     */
    public static class StringLocaliser
    {

        /**
         * <summary>
         *     Dictionary-id pair class for localisations.
         * </summary>
         */
        private class StringLanguage
        {
            /**
             * <summary>
             *     Dictionary that contains key-value pairs for localised
             *     strings. 
             * </summary>
             */
            public Dictionary<string, string> StringSet
            {
                get;
                private set;
            }
            /**
             * <summary>
             *     Friendly name of the language for display purposes.
             * </summary>
             */
            public readonly string FriendlyName;
            /**
             * <summary>
             *     Name of the language that the string set refers to.
             * </summary>
             */
            public readonly string Name;

            public StringLanguage(string name, string friendly,
                                  Dictionary<string, string> set)
            {
                this.Name = name;
                this.FriendlyName = friendly;
                this.StringSet = set;
            }
        }

        private static readonly List<StringLanguage> languages;
        private static StringLanguage languageSelector;

        static StringLocaliser()
        {
            languages = new List<StringLanguage>();
            languageSelector = null;
        }

        /**
         * <summary>
         *     Load a set of language strings from a given file so that they may
         *     be selected and used at runtime.
         * </summary>
         * <param name="filename">
         *     File path to the JSON string asset. Note that the path is
         *     relative to the Unity Resources folder and the path must
         *     <b>not</b> contain the file extension.
         * </param>
         * <param name="id">
         *     Internal name for the file language.
         * </param>
         * <param name="friendly">
         *     Friendly name for the file language. Used for display.
         * </param>
         */
        public static void LoadStrings(string filename, string id,
                                       string friendly)
        {
            if (filename == null)
            {
                Debug.LogError("Attempted to load null language asset.");
                return;
            }
            if (id == null)
            {
                Debug.LogError(string.Format(
                    "Attempted to load language asset '{0}' with null id.",
                    filename));
                return;
            }
            try
            {
                TextAsset contents = Resources.Load<TextAsset>(filename);
                if (contents == null)
                {
                    Debug.LogError(string.Format(
                        "Failed to load JSON asset '{0}'.",
                        filename));
                    return;
                }
                Dictionary<string, string> stringSet =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>
                        (contents.text);

                StringLanguage language =
                    new StringLanguage(id, friendly, stringSet);
                languages.Add(language);
            }
            catch (JsonException e)
            {
                Debug.LogError(string.Format(
                    "Failed to deserialise JSON asset '{0}'",
                    filename));
                Debug.LogException(e);
            }
        }

        /**
         * <summary>
         *     Sets the current language to be used by the localiser.
         *     Any strings obtained from <see cref="SetLanguage(string)"/>
         *     after invoking this method will pull string values from the given
         *     file loaded via. <see cref="LoadStrings(string, string)"/> and
         *     specified by the <paramref name="id"/> parameter.
         * </summary>
         * <param name="id">
         *     Internal name for the file language.
         *     This value should correspond to the one given in
         *     <see cref="LoadStrings(string, string)"/>.
         * </param>
         */
        public static void SetLanguage(string id)
        {
            if (id == null)
            {
                Debug.LogError("Attempted to set language to null.");
                return;
            }
            StringLanguage language = languages.Find(x => x.Name.Equals(id));
            if (language == null)
            {
                Debug.LogError(
                    string.Format("Attempted to set non-existing language {0}.",
                                  id));
            }
            else
            {
                languageSelector = language;
                Debug.Log(string.Format("Language was set to {0}.", id));
            }
        }

        /**
         * <summary>
         *     Returns the friendly name of the currently selected language.
         *     If <see cref="SetLanguage(string)"/> has not yet been invoked,
         *     then the string <c>"null"</c> will be returned.
         * </summary>
         * <returns>
         *     The friendly name of the currently selected language.
         * </returns>
         */
        public static string GetLanguageName()
        {
            if (languageSelector == null)
                return "null";
            return languageSelector.FriendlyName;
        }

        /**
         *  <summary>
         *      Return a string array of the internal id strings for each
         *      language that has been loaded with
         *      <see cref="LoadStrings(string, string, string)"/>.
         *      If no languages have yet been loaded, then <c>null</c> is
         *      returned.
         *  </summary>
         *  <returns>
         *      A string array consisting of all internal id names for loaded
         *      languages.
         *  </returns>
         */
        public static string[] GetLanguageList()
        {
            if (StringLocaliser.languages.Count == 0)
                return null;
            string[] languages = new string[StringLocaliser.languages.Count];
            int i = 0;
            foreach (StringLanguage l in StringLocaliser.languages)
                languages[i++] = l.Name;
            return languages;
        }

        /**
         * <summary>
         *     <para>
         *         Returns a string value from the set of strings loaded via.
         *         <see cref="LoadStrings(string, string)"/> and denoted by the
         *         <paramref name="key"/> parameter.
         *     </para>
         *     <para>
         *         <b>Note</b>: If a <c>null</c> key is specified, no
         *         language was ever set by invoking
         *         <see cref="SetLanguage(string)"/> or no string that matches
         *         the given key can be found, then the string
         *         <c>"null"</c> will be returned.
         *     </para>
         * </summary>
         * <param name="key">
         *     The key that corresponds to a given string in the currently
         *     loaded file.
         * </param>
         * <returns>
         *     The language string value that corresponds to the given key.
         * </returns>
         */
        public static string GetString(string key)
        {
            if (languageSelector == null ||
                !languageSelector.StringSet.ContainsKey(key))
                return "null";
            return languageSelector.StringSet[key];
        }
    }

}
