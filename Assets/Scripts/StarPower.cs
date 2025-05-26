using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StarPower : MonoBehaviour
{
    public float incrementValue;
    public bool isActive = false;
    public float currentValue = 0;
    public Image slider;
    public Key keyToPress;
    public void IncrementStarPower()
    {
        if(currentValue < incrementValue * 4) currentValue += incrementValue;
    }

    // Update is called once per frame
    void Update()
    {
        //slider.fillAmount = currentValue / (incrementValue * 4);
        if (isActive)
        {
            if (currentValue <= 0) { isActive = false; currentValue = 0; }
            currentValue -= Time.deltaTime;
        }
        else if (Keyboard.current[keyToPress].wasPressedThisFrame && currentValue > 0) isActive = true;
    }
}
