﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventScript))] // 'EventScript'스크립트를 에디팅하는 애튜리뷰터
public class EventScriptEditor : Editor {

    // todo. 일단 직렬화에 대해 공부해야 함.
    public override void OnInspectorGUI ()
	{
		showScriptField ();

		직렬화된요소표시 ("텍스트필드"); /// EventScript클래스의 변수 '텍스트필드'를 표시
        직렬화된요소표시 ("isDrawable");

		EventScript es = (EventScript)serializedObject.targetObject;

		if (es.isDrawable == true) {
			직렬화된요소표시 ("isHighPriorityCard");

			if (es.isHighPriorityCard == false) {
				직렬화된요소표시 ("cardPropability");
			}
				직렬화된요소표시 ("maxDraws");

		}

		직렬화된요소표시 ("conditions");
		직렬화된요소표시 ("Results");
		직렬화된요소표시 ("changeValueOnCardDespawn");
		직렬화된요소표시 ("OnCardSpawn");
		직렬화된요소표시 ("OnCardDespawn");

		직렬화된요소표시 ("OnSwipeLeft");
		직렬화된요소표시 ("OnSwipeRight");

		GUILayout.Space (15);

		//base.OnInspectorGUI ();
	}

	void showSerializedSubElement(string class1, string class2){
		SerializedProperty c1 = serializedObject.FindProperty (class1);
		SerializedProperty c2 = c1.FindPropertyRelative (class2);
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(c2, true);
		if(EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();
	}

    /// <summary>
    ///  todo. 3번째
    /// </summary>
    /// <param name="class1"></param>
    void 직렬화된요소표시(string class1)
    {
        SerializedProperty 인스펙터에노출하고자하는변수 = serializedObject.FindProperty(class1); /// 스크립트로는 접근할 수 없는 속성인데 인스펙터 창엔 표시되는 것이 있습니다. 그런 속성은 FindProperty를 이용해서 접근하면 된다. 

        EditorGUI.BeginChangeCheck(); /// 편집창에 변화가 있는지 확인 시작

        /// 기존 유니티 클래스들을 상속받는 새로운 클래스를 만들 경우, 그 안의 변수들은 에디터 상에 표시되지 않는다. 
        /// 이를 표시해 주기 에디팅 기능을 만드는데, 
        /// 먼저 위 FindProperty()에서 인스펙터에 노출하고자 하는 변수를 찾아서
        /// PropertyField()을 이용해서 노출한다.
        EditorGUILayout.PropertyField(인스펙터에노출하고자하는변수, true); 

        if (EditorGUI.EndChangeCheck()) /// 편집창에 변화가 있다면
            serializedObject.ApplyModifiedProperties();
    }

	void showScriptField(){
		//show the script field
		serializedObject.Update();
		SerializedProperty prop = serializedObject.FindProperty("m_Script");
		GUI.enabled = false;
		EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
		GUI.enabled = true;
		serializedObject.ApplyModifiedProperties();

	}
}

// TextFieldDrawer
[CustomPropertyDrawer(typeof(EventScript.이벤트텍스트))]
public class TextFieldDrawer : PropertyDrawer
{
	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);

		// Draw label
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		float scndWidth = 50f;

		// Calculate rects
		var textRect = new Rect(position.x, position.y, (position.width-scndWidth) * 0.98f, position.height);
		var textFieldRect = new Rect(position.x + (position.width-scndWidth)  , position.y, scndWidth , position.height);

		//var textRect = new Rect(position.x, position.y, position.width * 0.88f, position.height);
		//var textFieldRect = new Rect(position.x + position.width*0.9f  , position.y, position.width * 0.1f , position.height);


		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(textRect, property.FindPropertyRelative("textContent"), GUIContent.none);
		EditorGUI.PropertyField(textFieldRect, property.FindPropertyRelative("textField"), GUIContent.none);


		// Set indent back to what it was
		EditorGUI.indentLevel = indent;

		EditorGUI.EndProperty();
	}
}

// Modifier Drawer
[CustomPropertyDrawer(typeof(EventScript.resultModifier))]
public class ModifierDrawer : PropertyDrawer
{
	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//don't alter
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var modRect = new Rect(position.x, position.y, position.width * 0.70f, position.height);
		var valRect = new Rect(position.x + position.width * 0.71f  , position.y, position.width * 0.29f , position.height);

		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField(modRect, property.FindPropertyRelative("modifier"), GUIContent.none);
		EditorGUI.PropertyField(valRect, property.FindPropertyRelative("valueAdd"), GUIContent.none);


		//don't alter
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}


// ConditionDrawer
[CustomPropertyDrawer(typeof(EventScript.콘디션))]
public class ConditionDrawer : PropertyDrawer
{
	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//don't alter
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		// Calculate rects
		var modRect = new Rect(position.x, position.y, position.width * 0.58f, position.height);
		var valminRect = new Rect(position.x + position.width * 0.59f  , position.y, position.width * 0.20f , position.height);
		var valmaxRect = new Rect(position.x + position.width * 0.8f  , position.y, position.width * 0.20f , position.height);

		// 그리기필드 - GUIContent.none을 각각 전달하여 레이블 없이 그립니다.
		EditorGUI.PropertyField(modRect, property.FindPropertyRelative("value"), GUIContent.none);
		EditorGUI.PropertyField(valminRect, property.FindPropertyRelative("valueMin"), GUIContent.none);
		EditorGUI.PropertyField(valmaxRect, property.FindPropertyRelative("valueMax"), GUIContent.none);

		//don't alter
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}


// Drawer for the Results-selection
[CustomPropertyDrawer(typeof(EventScript.result))]
public class ResultDrawer : PropertyDrawer
{

	float mySize = 0f;

	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//don't alter
		EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		float startY = position.y;

		//show the result type selection
		EditorGUI.PropertyField( new Rect(position.x, position.y , position.width, position.height) , property.FindPropertyRelative ("resultType"),GUIContent.none,true ); 

		position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("resultType"),GUIContent.none ,true);

		//dependent on selection
		EventScript.resultTypes res =  (EventScript.resultTypes) property.FindPropertyRelative ("resultType").enumValueIndex;
		if (res == EventScript.resultTypes.simple) {

			EditorGUI.PropertyField (new Rect (50, position.y, position.x + position.width - 50, position.height), property.FindPropertyRelative ("modifiers"), true); 
			position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("modifiers"),GUIContent.none ,true);

		} else if (res == EventScript.resultTypes.conditional || res == EventScript.resultTypes.randomConditions) {

			EditorGUI.PropertyField (new Rect (50, position.y, position.x + position.width - 50, position.height), property.FindPropertyRelative ("conditions"), true); 
			position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("conditions"),GUIContent.none ,true);

			EditorGUI.PropertyField (new Rect (50, position.y, position.x + position.width - 50, position.height), property.FindPropertyRelative ("modifiersTrue"), true); 
			position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("modifiersTrue"),GUIContent.none ,true);

			EditorGUI.PropertyField (new Rect (50, position.y, position.x + position.width - 50, position.height), property.FindPropertyRelative ("modifiersFalse"), true); 
			position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("modifiersFalse"),GUIContent.none ,true);

		} else if (res == EventScript.resultTypes.random) {
			EditorGUI.PropertyField (new Rect (50, position.y, position.x + position.width - 50, position.height), property.FindPropertyRelative ("randomModifiers"), true); 
			position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("randomModifiers"),GUIContent.none ,true);
		}

		//draw the events
		//EditorGUI.PropertyField (new Rect (50, position.y, position.x + position.width - 50, position.height), property.FindPropertyRelative ("OnSwipe"), true); 
		//position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("OnSwipe"),GUIContent.none ,true);

		mySize = position.y - startY;

		//don't alter
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}

	float getSize(SerializedProperty property){
		mySize = 0f;

		mySize += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("resultType"),GUIContent.none ,true);
		//dependent on selection
		EventScript.resultTypes res =  (EventScript.resultTypes) property.FindPropertyRelative ("resultType").enumValueIndex;
		if (res == EventScript.resultTypes.simple) {

			mySize += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("modifiers"),GUIContent.none ,true);

		} else if (res == EventScript.resultTypes.conditional || res == EventScript.resultTypes.randomConditions) {

			mySize += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("conditions"),GUIContent.none ,true);
			mySize += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("modifiersTrue"),GUIContent.none ,true);
			mySize += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("modifiersFalse"),GUIContent.none ,true);

		} else if (res == EventScript.resultTypes.random) {
			mySize += EditorGUI.GetPropertyHeight(property.FindPropertyRelative ("randomModifiers"),GUIContent.none ,true);
		}
		return mySize;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{

		return getSize (property);
		//return mySize;

	}


}
	