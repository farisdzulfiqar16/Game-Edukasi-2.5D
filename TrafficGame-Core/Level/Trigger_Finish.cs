using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;

public class TriggerFinish : MonoBehaviour
{
    private bool triggered = false;

    [Header("Debug")]
    [SerializeField] private bool enableDebug = true;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Players")) return;

        triggered = true;

        if (enableDebug)
            Debug.Log($"[TriggerFinish] TRIGGER TERPANGGIL oleh: {other.name}");

        if (LevelManager.Instance != null)
        {
            StartCoroutine(GoNextLevel());
        }
        else if (enableDebug)
        {
            Debug.LogWarning("[TriggerFinish] LevelManager.Instance NULL, level tidak bisa lanjut!");
        }
    }

    private IEnumerator GoNextLevel()
    {
        yield return new WaitForSeconds(0.2f);

        if (LevelManager.Instance != null)
            LevelManager.Instance.NextLevel();
        else if (enableDebug)
            Debug.LogWarning("[TriggerFinish] LevelManager.Instance masih null saat GoNextLevel(), level tidak bisa lanjut.");
    }

    // =========================
    // PUBLIC RESET TRIGGER
    // =========================
    public void ResetTrigger()
    {
        triggered = false;
        gameObject.SetActive(false);
    }
}



//using System.Collections;
//using UnityEngine;

//public class TriggerFinish : MonoBehaviour
//{
//    private bool triggered = false;

//    [Header("Debug")]
//    [SerializeField] private bool enableDebug = true;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (triggered) return;

//        if (!other.CompareTag("Players")) return;

//        triggered = true;

//        if (enableDebug)
//            Debug.Log($"[TriggerFinish] TRIGGER TERPANGGIL oleh: {other.name}");

//        if (LevelManager.Instance != null)
//        {
//            StartCoroutine(GoNextLevel());
//        }
//        else if (enableDebug)
//        {
//            Debug.LogWarning("[TriggerFinish] LevelManager.Instance NULL, level tidak bisa lanjut!");
//        }
//    }

//    private IEnumerator GoNextLevel()
//    {
//        yield return new WaitForSeconds(0.2f);

//        if (LevelManager.Instance != null)
//            LevelManager.Instance.NextLevel();
//        else if (enableDebug)
//            Debug.LogWarning("[TriggerFinish] LevelManager.Instance masih null saat GoNextLevel(), level tidak bisa lanjut.");
//    }

//    // =========================
//    // PUBLIC (bisa dipakai reset level)
//    // =========================
//    public void ResetTrigger()
//    {
//        triggered = false;
//        gameObject.SetActive(false);
//    }
//}









//using System.Collections;
//using UnityEngine;

//public class TriggerFinish : MonoBehaviour
//{
//    private bool triggered = false;

//    [Header("Debug")]
//    [SerializeField] private bool enableDebug = true; // bisa matikan debug log kalau mau

//    private void OnTriggerEnter(Collider other)
//    {
//        if (triggered) return;

//        // Cek tag player
//        if (!other.CompareTag("Players")) return;

//        triggered = true;

//        if (enableDebug)
//            Debug.Log($"[TriggerFinish] TRIGGER TERPANGGIL oleh: {other.name}");

//        // Pastikan LevelManager ada
//        if (LevelManager.Instance == null)
//        {
//            if (enableDebug)
//                Debug.LogWarning("[TriggerFinish] LevelManager.Instance NULL, level tidak bisa lanjut!");
//            return;
//        }

//        if (enableDebug)
//            Debug.Log("[TriggerFinish] LevelManager ADA, pindah level...");

//        StartCoroutine(GoNextLevel());
//    }

//    private IEnumerator GoNextLevel()
//    {
//        yield return new WaitForSeconds(0.2f);

//        if (LevelManager.Instance != null)
//            LevelManager.Instance.NextLevel();
//        else if (enableDebug)
//            Debug.LogWarning("[TriggerFinish] LevelManager.Instance masih null saat GoNextLevel(), level tidak bisa lanjut.");
//    }

//    // Optional: reset trigger, bisa dipanggil saat reload level atau tutorial ulang
//    public void ResetTrigger()
//    {
//        triggered = false;
//        gameObject.SetActive(false);
//    }
//}


//using System.Collections;
//using UnityEngine;

//public class TriggerFinish : MonoBehaviour
//{
//    private bool triggered = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("TRIGGER TERPANGGIL oleh: " + other.name);

//        if (triggered) return;

//        if (other.CompareTag("Players"))
//        {
//            triggered = true;

//            if (LevelManager.Instance == null)
//            {
//                Debug.LogError("LevelManager.Instance NULL!");
//                return;
//            }

//            Debug.Log("LevelManager ADA, pindah level...");
//            StartCoroutine(GoNextLevel());
//        }
//    }

//    private IEnumerator GoNextLevel()
//    {
//        yield return new WaitForSeconds(0.2f);
//        LevelManager.Instance.NextLevel();
//    }
//}


//using UnityEngine;

//public class TriggerFinish : MonoBehaviour
//{
//    private bool triggered = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (triggered) return;

//        if (other.CompareTag("Player"))
//        {
//            triggered = true;
//            Debug.Log("Player masuk Finish -> pindah level");
//            StartCoroutine(GoNextLevel());
//        }

//        if (LevelManager.Instance == null)
//        {
//            Debug.LogError("LevelManager.Instance NULL!");
//        }
//        else
//        {
//            Debug.Log("LevelManager ditemukan");
//            LevelManager.Instance.NextLevel();
//        }

//    }

//    private System.Collections.IEnumerator GoNextLevel()
//    {
//        yield return new WaitForSeconds(0.2f);
//        LevelManager.Instance.NextLevel();
//    }
//}



//using UnityEngine;


//public class TriggerFinish : MonoBehaviour
//{

//    private void OnTriggerEnter(Collider other)
//    {
//        Debug.Log("Trigger disentuh: " + other.name);

//        if (other.CompareTag("Player"))
//        {
//            LevelManager.Instance.NextLevel();
//        }
//    }
//}
