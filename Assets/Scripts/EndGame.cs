using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        Cursor.visible = true;
    }

    public void OnClick()
    {
        Application.Quit();
    }
}
