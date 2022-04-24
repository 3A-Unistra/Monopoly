/*
 * TokenCard.cs
 * UI display for chance/community cards.
 * 
 * Date created : 22/04/2022
 * Author       : Finn RAYMENT <rayment@etu.unistra.fr>
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Monopoly.UI
{

    [RequireComponent(typeof(CanvasGroup))]
    public class TokenCard : MonoBehaviour, IPointerDownHandler
    {

        public enum CardType
        {
            COMMUNITY, CHANCE
        }

        public Image image;
        public TMP_Text text;

        public GameObject chanceBackground;
        public GameObject communityBackground;

        public Sprite[] imageFaces;

        [Range(0.0f, 5.0f)]
        public float fadeTime = 0.5f;
        private float animate;

        private Coroutine routine;
        private bool shouldHide;

        private CanvasGroup canvasGroup;

        void Start()
        {
            routine = null;
            canvasGroup = GetComponent<CanvasGroup>();
            gameObject.SetActive(false);
        }

        public void ShowCard(CardType type, int idx, string message)
        {
            if (idx < 1 || idx > 32)
            {
                Debug.LogWarning(string.Format(
                    "Attempting to show invalid card #{0}!", idx));
                return;
            }
            gameObject.SetActive(true);
            canvasGroup.alpha = 0.0f;
            if (routine != null)
                StopCoroutine(routine);
            // TODO: set image to an index
            chanceBackground.SetActive(type == CardType.CHANCE);
            communityBackground.SetActive(type == CardType.COMMUNITY);
            text.text = message;
            shouldHide = false;
            if (imageFaces.Length >= idx)
                image.sprite = imageFaces[idx - 1];
            routine = StartCoroutine(EnumerateCard());
        }

        private IEnumerator EnumerateCard()
        {
            animate = 0.0f;
            while (canvasGroup.alpha < 1.0f)
            {
                animate += Time.deltaTime;
                // fade in
                canvasGroup.alpha =
                    Mathf.Lerp(0.0f, 1.0f, animate / fadeTime);
                if (Mathf.Abs(1.0f - canvasGroup.alpha) < 0.01f)
                    break;
                yield return null; // wait a frame
            }
            canvasGroup.alpha = 1.0f;
            yield return new WaitUntilForSeconds(() => shouldHide, 5);
            animate = 0.0f;
            while (canvasGroup.alpha > 0.0f)
            {
                animate += Time.deltaTime;
                // fade out
                canvasGroup.alpha =
                    Mathf.Lerp(1.0f, 0.0f, animate / fadeTime);
                if (canvasGroup.alpha < 0.01f)
                    break;
                yield return null; // wait a frame
            }
            canvasGroup.alpha = 0.0f;
            routine = null;
            gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Mouse was clicked on the card, so hide it.
            shouldHide = true;
        }

    }

}
