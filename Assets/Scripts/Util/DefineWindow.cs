using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public class DefineWindow : EditorWindow
{
	public enum eBuildDefine
	{
		// SERVER TYPE
		DEV = 1,
		QA,
		REVIEW,
		DIST,
		// ...
		MAX
	}

	Vector2 vecScrollView;

	public static bool[] _isFlagOn = new bool[(int)eBuildDefine.MAX-1];

	public static string[] FLAG_STRING = null;
	public static string[] _FLAG_STRING
	{
		get
		{
			if (null == FLAG_STRING)
				FLAG_STRING = Enum.GetNames(typeof(eBuildDefine));
			return FLAG_STRING;
		}
	}

	public static BuildTargetGroup[] SETTING_TARGET = new BuildTargetGroup[]
	{
		BuildTargetGroup.Standalone,
		BuildTargetGroup.iOS,
		BuildTargetGroup.Android
	};

	public static string[] _defineList = new string[(int)eBuildDefine.MAX];[MenuItem("Example/Simple Recorder")]

	[MenuItem("Custom/DefineWindow")]
	static void Init()
	{
		DefineWindow window =
			(DefineWindow)EditorWindow.GetWindow(typeof(DefineWindow));
	}

	public static void GetFlagData()
	{
		for (int i = 0; i < _isFlagOn.Length; i++)
			_isFlagOn[i] = false;

		_defineList = Enum.GetNames(typeof(eBuildDefine));

		for (int i = 0; i < _defineList.Length; i++)
		{
			for (int j = 0; j < _FLAG_STRING.Length; j++)
			{
				if (_defineList[i].Equals(_FLAG_STRING[j]))
				{
					_isFlagOn[j] = true;
					break;
				}
			}
		}
	}

	public static void SetDefineFlag()
	{
		string defineStr = string.Empty;

		for (int i = 0; i < (int)eBuildDefine.MAX; i++)
		{
			if (_isFlagOn[i])
				defineStr = string.Concat(defineStr, _FLAG_STRING[i], ";");
		}

		for (int i = 0; i < SETTING_TARGET.Length; i++)
			PlayerSettings.SetScriptingDefineSymbolsForGroup(SETTING_TARGET[i], defineStr);
	}

	void OnGUI()
	{
		GetFlagData();

		float originValue = EditorGUIUtility.labelWidth;
		EditorGUIUtility.labelWidth = 300;

		vecScrollView = EditorGUILayout.BeginScrollView(vecScrollView);

		EditorGUILayout.LabelField("##### Common Defines #####");
		EditorGUILayout.Space();
		for (int i = 0; i < (int)eBuildDefine.MAX; i++)
		{
			_isFlagOn[i] = EditorGUILayout.Toggle(FLAG_STRING[i], _isFlagOn[i], GUILayout.Width(400));
		}

		EditorGUILayout.EndScrollView();
		EditorGUIUtility.labelWidth = originValue;

		if (GUILayout.Button("APPLY", new GUILayoutOption[] { GUILayout.Width(400), GUILayout.Height(50) }))
		{
			SetDefineFlag();
		}
	}
}