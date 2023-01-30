using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MyIntEventAttribute))]
public class MyIntEventDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        int newValue = EditorGUI.IntSlider(position, label, property.intValue, ((MyIntEventAttribute)attribute).minValue, ((MyIntEventAttribute)attribute).maxValue);
        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = newValue;
            (property.serializedObject.targetObject as ExampleScript).MyInt1Event();
        }
    }
}

public class ExampleScript : MonoBehaviour
{
    [MyIntEvent(0, 100)]
    [SerializeField]
    private int m_MyInt1;

    public void MyInt1Event()
    {
        Debug.Log("MyInt1 changed to: " + m_MyInt1);
    }
}

public class MyIntEventAttribute : PropertyAttribute
{
    public int minValue;
    public int maxValue;

    public MyIntEventAttribute(int minValue, int maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}
