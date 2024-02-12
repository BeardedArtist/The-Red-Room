using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomTools : EditorWindow
{
    static bool Draw = false;

    static float Timer;

    static Vector2 MousePos;

    static bool SinglePressed;

    static List<Vector2> UpdatedPos = new List<Vector2>();

    static int UpdateBy;

    public static float progress = 4;

    static float min = 1;

    static float max = 8;

    static bool ShowSettings = false;

    [MenuItem("River/Enable Tools")]
    public static void Enable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        //       instance = this;
    }

    [MenuItem("River/Disable Tools")]
    public static void Disable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        //     instance = null;
    }

    [MenuItem("River/Toggle Settings")]
    public static void Settings()
    {
        ShowSettings = !ShowSettings;
    }

    private static void OnSceneGUI(SceneView view)
    {
        if (ShowSettings) DrawSettings();
        Texture2D LogoTex = (Texture2D) Resources.Load("Logo") as Texture2D; //don't put png
        Event e = Event.current;
        if (
            e.type == EventType.KeyDown &&
            e.keyCode == KeyCode.C &&
            !SinglePressed
        )
        {
            Draw = true;
            MousePos = Event.current.mousePosition;
            Config.Initialize();

            InitializeButtons();
            SinglePressed = true;
            // Debug.Log("Mouse is Down");
        }

        if (e.type == EventType.KeyUp && e.keyCode == KeyCode.C)
        {
            Draw = false;
            SinglePressed = false;
            Timer = 0f;
            UpdatedPos.Clear();
            Config.Clear();
        }

        Handles.BeginGUI();
        if (Draw) DrawCircle((int) progress, MousePos, 120);

        if (Draw)
        {
            GUILayout
                .BeginArea(new Rect(MousePos.x - 15, MousePos.y - 15, 30, 30),
                LogoTex);

            GUILayout.EndArea();
        }
        Handles.EndGUI();
    }

    public static void DrawCircle(int num, Vector2 point, float radius)
    {
        for (int i = 0; i < num; i++)
        {
            /* Distance around the circle */
            var radians = 2 * 3.17f / num * i;

            /* Get the vector direction */
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);

            var spawnDir = new Vector2(horizontal, vertical);

            /* Get the spawn position */
            var spawnPos = point + spawnDir * radius; // Radius is just the distance away from the point

            spawnPos.x -= 50;
            spawnPos.y -= 10;

            //UpdatedPos.Add(spawnPos);
            GUILayout
                .BeginArea(new Rect(UpdatedPos[i].x,
                    UpdatedPos[i].y,
                    100,
                    100));

            // Debug.Log(mousePosition);
            //  Debug.Log(Draw);
            if (Draw)
            {
                Timer += Time.deltaTime;

                Vector2 newUpdatedPos =
                    Vector2.Lerp(UpdatedPos[i], spawnPos, Time.deltaTime);

                UpdatedPos[i] = newUpdatedPos;

                // Debug.Log (Timer);
                if (
                    GUILayout
                        .Button(Config._ButtonConfig[i].description,
                        GUILayout.Width(100),
                        GUILayout.Height(20))
                )
                {
                    DemoFunctions.DemoFunction(i);
                }
            }

            GUILayout.EndArea();
        }
    }

    public static void InitializeButtons()
    {
        for (int i = 0; i < progress; i++)
        {
            UpdatedPos.Add (MousePos);
        }
    }

    public static void DrawSettings()
    {
        var rect = new Rect(10, 10, 200, 100);
        GUILayout.BeginArea (rect);

        GUILayout.BeginVertical("Box");

        GUILayout.BeginHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.Label("Settings");
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        progress =
            GUILayout
                .HorizontalSlider(progress, min, max, GUILayout.Width(200));

        GUILayout.BeginHorizontal();

        GUILayout.Label(min.ToString());
        GUILayout.FlexibleSpace();
        GUILayout.Label(((int) progress).ToString());
        GUILayout.FlexibleSpace();
        GUILayout.Label(max.ToString());

        GUILayout.EndHorizontal();

        GUILayout.Label("Number of Buttons");

        GUILayout.EndVertical();

        GUILayout.EndArea();
    }
}
