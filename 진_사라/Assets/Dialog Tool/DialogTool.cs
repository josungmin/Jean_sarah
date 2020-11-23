using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogTool : MonoBehaviour
{
    public string npcName;                    // npc 이름
    public GameObject targetLocation;         // 말풍선이 띄워질 타겟(NPC)
    public float height;                      // 말풍선이 띄워질 높이 (targetLocation 기준)

    public List<DialogWave> Waves = new List<DialogWave>();

    public void ShowDialogue()
    {
        /*
        // 리스트 채우기 //
        for (int i = 0; i < Waves.; ++i)
        {
            listSentences.Add(dialogue.sentences[i]);
            listSprites.Add(dialogue.sprites[i]);
            listWindows.Add(dialogue.dialogueWindow[i]);
        }

        // 캐릭터 및 대화창이 나오도록 함
        aniSprite.SetBool("Appear", true);
        aniWindow.SetBool("Appear", true);

        // 다이어로그를 보여주는 코루틴 실행
        StartCoroutine(StartDialogueCoroutine());
        */
    }
}
