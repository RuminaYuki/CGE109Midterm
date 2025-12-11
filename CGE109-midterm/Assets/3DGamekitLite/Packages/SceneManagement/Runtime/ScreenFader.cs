using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gamekit3D
{
    public class ScreenFader : MonoBehaviour
    {
        public enum FadeType
        {
            Black, Loading, GameOver, PauseGame,
        }

        public static ScreenFader Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<ScreenFader>();

                if (s_Instance != null)
                    return s_Instance;

                Create();

                return s_Instance;
            }
        }

        public static bool IsFading
        {
            get { return Instance.m_IsFading; }
        }

        protected static ScreenFader s_Instance;

        public static void Create()
        {
            ScreenFader controllerPrefab = Resources.Load<ScreenFader>("ScreenFader");
            s_Instance = Instantiate(controllerPrefab);
        }


        public CanvasGroup faderCanvasGroup;
        public CanvasGroup loadingCanvasGroup;
        public CanvasGroup gameOverCanvasGroup;
        public CanvasGroup PauseGameCanvasGroup;
        public float fadeDuration = 1f;

        protected bool m_IsFading;

        const int k_MaxSortingLayer = 32767;

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        // ==========================================================
        //  ✔ Fade แบบ unscaled time (ทำงานแม้ timeScale = 0)
        // ==========================================================
        protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
        {
            m_IsFading = true;
            canvasGroup.blocksRaycasts = true;

            float startAlpha = canvasGroup.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.unscaledDeltaTime;      // <<< แก้ตรงนี้
                float t = time / fadeDuration;

                canvasGroup.alpha = Mathf.Lerp(startAlpha, finalAlpha, t);

                yield return null;
            }

            canvasGroup.alpha = finalAlpha;
            m_IsFading = false;
        }

        public static void SetAlpha(float alpha)
        {
            Instance.faderCanvasGroup.alpha = alpha;
        }

        public static IEnumerator FadeSceneIn()
        {
            CanvasGroup canvasGroup;

            if (Instance.faderCanvasGroup.alpha > 0.1f)
                canvasGroup = Instance.faderCanvasGroup;
            else if (Instance.gameOverCanvasGroup.alpha > 0.1f)
                canvasGroup = Instance.gameOverCanvasGroup;
            else if (Instance.PauseGameCanvasGroup.alpha > 0.1f)
                canvasGroup = Instance.PauseGameCanvasGroup;
            else
                canvasGroup = Instance.loadingCanvasGroup;

            yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));

            canvasGroup.gameObject.SetActive(false);
        }

        public static IEnumerator FadeSceneOut(FadeType fadeType = FadeType.Black)
        {
            CanvasGroup canvasGroup;

            switch (fadeType)
            {
                case FadeType.Black:
                    canvasGroup = Instance.faderCanvasGroup;
                    break;
                case FadeType.GameOver:
                    canvasGroup = Instance.gameOverCanvasGroup;
                    break;
                case FadeType.PauseGame:
                    canvasGroup = Instance.PauseGameCanvasGroup;
                    break;
                default:
                    canvasGroup = Instance.loadingCanvasGroup;
                    break;
            }

            canvasGroup.gameObject.SetActive(true);

            yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
        }
    }
}
