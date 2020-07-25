using UnityEngine;


public enum Buttons
{
    Light,
    Medium,
    Heavy
}

[CreateAssetMenu]
public class Combo :ScriptableObject
{
    public Buttons[] buttons;
    
    public AnimationClip specialMove;
    public float allowedTimeBetweenButtons = 0.3f; //tweak as needed
    private int currentIndex = 0; //moves along the array as buttons are pressed   
    private float timeLastButtonPressed;
    int player;

    public void Init(int player)
    {
        this.player = player;
    }
     
    //usage: call this once a frame. when the combo has been completed, it will return true
    public bool Check(int player)
    {
        if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;

        if (currentIndex < buttons.Length)
        {
            if (Input.GetButtonDown(buttons[currentIndex].ToString()))
            {                
                timeLastButtonPressed = Time.time;
                currentIndex++;
            }

            if (currentIndex >= buttons.Length)
            {
                currentIndex = 0;
                return true;
            }
            else return false;
        }

		return false;

    }
}
