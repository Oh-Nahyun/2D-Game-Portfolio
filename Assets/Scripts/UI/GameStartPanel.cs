using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartPanel : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Button start = GetComponentInChildren<Button>();
        start.onClick.AddListener(() => SceneManager.LoadScene(0));
    }
}
