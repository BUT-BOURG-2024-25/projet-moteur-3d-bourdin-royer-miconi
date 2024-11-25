using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            m_instance = this as T;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Reload();
    }

    /// <summary>
    /// M�thode abstraite que les sous-classes doivent impl�menter.
    /// Appel�e automatiquement lors d'un changement de sc�ne.
    /// </summary>
    public abstract void Reload();
}
