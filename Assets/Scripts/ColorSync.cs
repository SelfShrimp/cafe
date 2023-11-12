using Unity.Netcode;
using UnityEngine;

public class ColorSync : NetworkBehaviour
{
    private NetworkVariable<Color> playerColor = new NetworkVariable<Color>(Color.white,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        playerColor.OnValueChanged += OnPlayerColorChanged;
        gameObject.GetComponent<Renderer>().material.color = playerColor.Value;
    }

    public void ChangeColor()
    {
        playerColor.Value = UnityEngine.Random.ColorHSV();
    }

    public override void OnNetworkDespawn()
    {
        playerColor.OnValueChanged -= OnPlayerColorChanged;
    }

    private void OnPlayerColorChanged(Color previousValue, Color newValue)
    {
        gameObject.GetComponent<Renderer>().material.color = newValue;
    }
}