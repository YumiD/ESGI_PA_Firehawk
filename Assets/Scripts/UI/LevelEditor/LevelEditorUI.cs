using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelEditorUI : MonoBehaviour
{
    [SerializeField]
    private GameObject gridGenerator;
    public void GenerateLevel(int dropdownValue) {
        Instantiate(gridGenerator, new Vector3(0, 0, 0), Quaternion.identity);
    }
    public void QuitLevelEditor() {
        SceneManager.LoadScene(0);
    }
}
