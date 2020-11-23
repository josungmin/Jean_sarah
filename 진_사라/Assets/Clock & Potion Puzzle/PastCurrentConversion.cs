using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PastCurrentConversion : MonoBehaviour
{
    public enum Stage
    {
        CURRENT,
        PAST
    }
    public Stage Current_Stage = Stage.CURRENT;

    public GameObject player;

    // DontDestroyOnLoad 중복 생성 확인
    public static PastCurrentConversion Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            if (SceneManager.GetActiveScene().name == "Clock&PotionPuzzleCurrent")
            {
                LoadingSceneManager.LoadScene("Clock&PotionPuzzlePast");
                player.gameObject.transform.position = new Vector3(player.transform.position.x, 13.0f, 9.41f);
            }
            else if (SceneManager.GetActiveScene().name == "Clock&PotionPuzzlePast")
            {
                LoadingSceneManager.LoadScene("Clock&PotionPuzzleCurrent");
                player.gameObject.transform.position = new Vector3(player.transform.position.x, 13.0f, 9.41f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        
    }
}
