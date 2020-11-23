/*
모든 커스텀 인스펙터 클래스는 다음과 같은 중요한 부분들을 포함하고 있다.



1. 모든 커스텀 인스펙터는 반드시 Editor 클래스를 상속받아야 한다.

2. Unity에게 LevelData 컴포넌트에 대해서 작업하고 있다고 알리기 위해, 

반드시[CustomEditor(typeof(LevelData))] 속성을 추가해야 한다.

3. private void OnEnable() 메소드는 초기화를 하는데 사용된다.

4. public override void OnIspectorGUI() 메소드는 인스펙터가 다시 그려질 때 호출된다.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(DialogTool))]
public class DialogToolEditor : Editor
{
    private ReorderableList list;
    private DialogTool dialogTool;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("Waves"),
                true, true, true, true);

        list.drawElementCallback =
    (Rect rect, int index, bool isActive, bool isFocused) => {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += EditorGUIUtility.standardVerticalSpacing;

        SerializedProperty count = element.FindPropertyRelative("Count");
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 200, EditorGUI.GetPropertyHeight(count, true)), count, new GUIContent("반복 횟수"));
                                                                                                           // true 로 바꿀시 이름 표기 GUIContent.none 일시 이름 표기 x
        SerializedProperty waitingTime = element.FindPropertyRelative("waitingTime");
        EditorGUI.PropertyField(new Rect(rect.x + 170, rect.y, rect.width - 200, EditorGUI.GetPropertyHeight(waitingTime, true)), waitingTime, new GUIContent("대기 시간"));

        SerializedProperty isMovePlayer = element.FindPropertyRelative("isMovePlayer");
        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 19, rect.width - 120, EditorGUI.GetPropertyHeight(isMovePlayer, true)), isMovePlayer, new GUIContent("플레이어 움직임"));

        SerializedProperty textEffect = element.FindPropertyRelative("textEffect");
        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 38, rect.width - 200, EditorGUI.GetPropertyHeight(textEffect, true)), textEffect, new GUIContent("글자 효과"));

        SerializedProperty textColor = element.FindPropertyRelative("textColor");
        EditorGUI.PropertyField(new Rect(rect.x + 170, rect.y + 38, rect.width - 200, EditorGUI.GetPropertyHeight(textColor, true)), textColor, new GUIContent("글자 색"));

        SerializedProperty text = element.FindPropertyRelative("Text");
        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 57, rect.width - 100, EditorGUI.GetPropertyHeight(textColor, true)), text, new GUIContent("대사"));
    };

        list.elementHeightCallback = (int index) =>
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            float height = EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty waitingTime = element.FindPropertyRelative("waitingTime");
            height += EditorGUI.GetPropertyHeight(waitingTime, true) + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty isMovePlayer = element.FindPropertyRelative("isMovePlayer");
            height += EditorGUI.GetPropertyHeight(isMovePlayer, true) + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty textColor = element.FindPropertyRelative("textColor");
            height += EditorGUI.GetPropertyHeight(textColor, true) + EditorGUIUtility.standardVerticalSpacing;

            SerializedProperty text = element.FindPropertyRelative("Text");
            height += EditorGUI.GetPropertyHeight(text, true) + EditorGUIUtility.standardVerticalSpacing * 3;

            return height + EditorGUIUtility.standardVerticalSpacing;
        };

        /*
                  list.drawElementCallback =
    (Rect rect, int index, bool isActive, bool isFocused) => {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 60, 20),
            element.FindPropertyRelative("Type"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("Text"), GUIContent.none);
        EditorGUI.PropertyField(
            new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("Count"), GUIContent.none);
        EditorGUI.PropertyField(
             new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight),
             element.FindPropertyRelative("isMovePlayer"), GUIContent.none);
    };
         */

        // 리스트 이름
        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "대화");
        };
        /*
        // 선택시 하이라이트 기능 추가
        list.onSelectCallback = (ReorderableList l) => {
            var prefab = l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Prefab").objectReferenceValue as GameObject;
            if (prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };
        */
        // 남은 List수 확인하기
        // List의 적어도 하나의 요소가 남아 있게 설정
        list.onCanRemoveCallback = (ReorderableList l) => {
            return l.count > 1;
        };

        // 경고문 추가
        list.onRemoveCallback = (ReorderableList l) => {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete the wave?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        };

        /*
        // 새로 만들어진 요소들을 초기화
        // + 버튼을 눌러 새로운 요소들을 추가 했을 때, 미리 설정된 값들이 정의됨
        list.onAddCallback = (ReorderableList l) => {
            var index = l.serializedProperty.arraySize;
            l.serializedProperty.arraySize++;
            l.index = index;
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Type").enumValueIndex = 0;
            element.FindPropertyRelative("Count").intValue = 20;
            element.FindPropertyRelative("Text").stringValue = "대사를 입력해 주세요";
            element.FindPropertyRelative("isMovePlayer").boolValue = false;
        };
        */
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("npcName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("targetLocation"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("height"));

        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

// 출쳐 https://unityindepth.tistory.com/56

// 텍스트 효과 https://github.com/mdechatech/CharTweener 