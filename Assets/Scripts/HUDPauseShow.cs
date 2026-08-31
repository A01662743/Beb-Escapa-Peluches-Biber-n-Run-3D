using UnityEngine;

public class HUDPauseShow : MonoBehaviour
{

    public void OnEnable()
    {
        HUDPauseButton.PausaPlay += ShowHUD;
    }

    public void OnDisable()
    {
        HUDPauseButton.PausaPlay -= ShowHUD;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowHUD(bool IsPaused)
    {
        if (IsPaused)
        {
            
        }
        else
        {
            
        }
    }
}
