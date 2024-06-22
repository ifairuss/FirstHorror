using TMPro;
using UnityEngine;

public class TestInteractable : Interactable
{
    [SerializeField] private GameObject flash;

    private void Start()
    {
        flash.SetActive(false);

    }
    public override void OnFocus()
    {
        print("LOOK AT ME" + gameObject.name);
    }

    public override void OnInteract()
    {
        print("CHECK INVENTORY" + gameObject.name);
        flash.SetActive(true);

        Destroy(gameObject);
    }

    public override void OnLoseFocus()
    {
        print("DONT LOOK AT ME" + gameObject.name);
    }
}
