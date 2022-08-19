using Arcturus.MapLoader;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCont : MonoBehaviour
{
    private IPlayerInteract interactable;
    private Gateway_ID targetGate;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactUI;

    [Header("Player")]
    public CharacterController controller;
    public float speed = 6f;

    private static PlayerCont I;
    public static PlayerCont Player => I;

    private void Start()
    {
        #region Static Referencing
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
        #endregion

        MapLoader.OnCellLoaded += OnCellLoaded;
    }

    private void OnDisable()
    {
        MapLoader.OnCellLoaded -= OnCellLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && interactable != null)
        {
            if (interactable.PlayerInteract(gameObject, out object data))
            {
                var dataArray = data as List<object>;
                if ((LoadNoise)dataArray[0] == LoadNoise.Loud)
                    targetGate = dataArray[1] as Gateway_ID;
                else
                    targetGate = null;

                ToggleInteractUI(false);
                Debug.Log("Interacted successfully");
            }
            else
                Debug.LogWarning("Unable to interact for some reason.");
        }

        // Movement stuff below
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interactable = other.GetComponent<IPlayerInteract>();
        if (interactable == null) return;

        interactable.PingCanInteract();
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactable != null)
        {
            interactable.PingEndInteract();
            interactable = null;
        }
        else
        {
            interactable = other.GetComponent<IPlayerInteract>();
            if (interactable == null) return;

            interactable.PingEndInteract();
            interactable = null;
        }
    }

    private void OnCellLoaded(string cell, CellCoordinator coordinator)
    {
        if (targetGate != null)
        {
            var pos = coordinator.GetGateway(targetGate);
            transform.position = pos.transform.position;
            targetGate = null;
        }
    }

    public void ToggleInteractUI(bool state)
    {
        interactUI.SetActive(state);
    }
}
