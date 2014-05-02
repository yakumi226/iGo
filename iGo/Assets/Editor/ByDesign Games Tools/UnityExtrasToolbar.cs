//
// 	UnityExtrasToolbar.cs
//
// 	Copyright (c) 2011 ByDesign Games SARL
// 	Joe Schultz <support@bydesigngames.com>
//
//  Usage: 	Place in any Editor sub-folder, then click on the menu entry Window -> Unity Extras or press CMD+SHIFT+T
//  Hint: 	Fits great docked below your Inspector view! :)


using UnityEngine;
using UnityEditor;

class UnityExtrasToolbar : EditorWindow {

	[MenuItem ("Window/Unity Extras #%t")]
	static void Init () {
		UnityExtrasToolbar window = (UnityExtrasToolbar)EditorWindow.GetWindow (typeof (UnityExtrasToolbar), false, "Extras");
		window.Show();
	}

	void OnGUI () {

		GUIStyle someGUIStyle = GUI.skin.GetStyle("minibutton");
		// update style to fit minimum editor window width
		someGUIStyle.padding = new RectOffset(1,1,0,0);
		someGUIStyle.overflow = new RectOffset(0,0,2,4);
		// someGUIStyle.fixedWidth = 0;
		someGUIStyle.fixedHeight = 20f;
		someGUIStyle.imagePosition = ImagePosition.ImageAbove;

		// project settings
//		Texture2D someGuiContent;
		GUIContent someGuiContent = new GUIContent();

		someGuiContent.tooltip = someGuiContent.text = "Project Settings";
//		someGuiContent.image = EditorGUIUtility.Load("/d_unityeditor.projectwindow.png") as Texture2D;
		GUILayout.Label (someGuiContent, EditorStyles.boldLabel);
		GUILayout.Space(4f);

		GUILayout.BeginHorizontal ();

			someGuiContent.text = "";

			someGuiContent.tooltip = "Input";
			someGuiContent.image = EditorGUIUtility.Load("icons/d_movetool.png") as Texture2D;
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Input");
			}

			someGuiContent.tooltip = "Tags";
			someGuiContent.image = EditorGUIUtility.Load("icons/d_unityeditor.hierarchywindow.png") as Texture2D;
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags");
			}

			someGuiContent.tooltip = "NavMeshLayers";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/meshrenderer icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(MeshRenderer)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/NavMeshLayers");
			}

			someGuiContent.tooltip = "Audio";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_AudioClip Icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(AudioClip)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Audio");
			}

			someGuiContent.tooltip = "Time";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_animation icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(Animation)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Time");
			}

			someGuiContent.tooltip = "Player";
			someGuiContent.image = EditorGUIUtility.Load("icons/d_unityeditor.gameview.png") as Texture2D;
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
			}

			someGuiContent.tooltip = "Physics";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_PhysicMaterial Icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(PhysicMaterial)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Physics");
			}

			someGuiContent.tooltip = "Quality";
			someGuiContent.image = EditorGUIUtility.Load("icons/d_viewtoolorbit.png") as Texture2D;
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Quality");
			}

			someGuiContent.tooltip = "Network";
			someGuiContent.image = EditorGUIUtility.Load("icons/d_unityeditor.serverview.png") as Texture2D;
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Network");
			}

			someGuiContent.tooltip = "Editor";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_GUISkin Icon.png") as Texture2D;
			someGuiContent.text = "";
#else
			someGuiContent.text = "Editor";
			someGuiContent.image = null;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Editor");
			}		
		
			someGuiContent.tooltip = "Script Execution Order";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_script icon.png") as Texture2D;
			someGuiContent.text = "";
#else
			someGuiContent.text = "Order";
			someGuiContent.image = null;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Script Execution Order");
			}		
		
		EditorGUILayout.EndHorizontal ();

		// scene settings & creation
		someGuiContent.tooltip = someGuiContent.text = "Scene Settings & Creation";
//		someGuiContent.image = EditorGUIUtility.Load("icons/d_unityeditor.sceneview.png") as Texture2D;
		GUILayout.Label (someGuiContent, EditorStyles.boldLabel);
		GUILayout.Space(4);

		EditorGUILayout.BeginHorizontal ();
		
			someGuiContent.text = "";

			someGuiContent.tooltip = "Render Settings";
			someGuiContent.image = EditorGUIUtility.Load("icons/d_unityeditor.sceneview.png") as Texture2D;
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("Edit/Render Settings");
			}

			someGuiContent.tooltip = "Particle System";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_particlerenderer icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(ParticleSystem)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Particle System");
			}

			someGuiContent.tooltip = "Camera";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_camera icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(Camera)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Camera");
			}
		
			someGuiContent.tooltip = "GUI Text";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.ObjectContent(null, typeof(GUIText)).image as Texture2D;
			someGuiContent.text = "GUIText";
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(GUIText)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/GUI Text");
			}

			someGuiContent.tooltip = "GUI Texture";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.ObjectContent(null, typeof(GUITexture)).image as Texture2D;
			someGuiContent.text = "GUITexture";
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(GUITexture)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/GUI Texture");
			}

			someGuiContent.tooltip = "3D Text";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.ObjectContent(null, typeof(TextMesh)).image as Texture2D;
			someGuiContent.text = "3DText";
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(TextMesh)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/3D Text");
			}

			someGuiContent.tooltip = "Directional Light";
			someGuiContent.text = "Dir";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_light icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(Light)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Directional Light");
			}
		
			someGuiContent.tooltip = "Point Light";
			someGuiContent.text = "Point";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_light icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(Light)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Point Light");
			}
		
			someGuiContent.tooltip = "Spot Light";
			someGuiContent.text = "Spot";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_light icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(Light)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Spot Light");
			}
		
			someGuiContent.tooltip = "Area";
			someGuiContent.text = "Area";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_light icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(Light)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Area Light");
			}
		
		GUILayout.EndHorizontal ();
		
		GUILayout.Space (4);

		GUILayout.BeginHorizontal ();

			someGuiContent.text = "";

			someGuiContent.tooltip = "Cube";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_boxcollider icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(BoxCollider)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Cube");
			}

			someGuiContent.tooltip = "Sphere";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_spherecollider icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(SphereCollider)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Sphere");
			}

			someGuiContent.tooltip = "Capsule";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_capsulecollider icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(CapsuleCollider)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Capsule");
			}

			someGuiContent.tooltip = "Cylinder";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_prematcylinder.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(CapsuleCollider)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Cylinder");
			}

			someGuiContent.tooltip = "Plane";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/d_meshcollider icon.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(MeshCollider)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Plane");
			}

			someGuiContent.tooltip = "Cloth";
			someGuiContent.text = "Cloth";
#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			someGuiContent.image = EditorGUIUtility.Load("icons/clothinspector.viewvalue.png") as Texture2D;
#else
			someGuiContent.image = AssetPreview.GetMiniTypeThumbnail(typeof(Cloth)) as Texture2D;
#endif
			if ( GUILayout.Button( someGuiContent, someGUIStyle, GUILayout.MinWidth(24) ) ) {
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Cloth");
			}

		GUILayout.EndHorizontal ();

	}
}
