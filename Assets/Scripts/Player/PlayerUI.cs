using UnityEngine;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string message)
    {
        text.text = message;
    }
}
