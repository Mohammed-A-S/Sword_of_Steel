using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class studio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(to_the_next());
    }

    IEnumerator to_the_next()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
