using UnityEngine;

public class SideClamp : MonoBehaviour
{
    // x軸方向の移動範囲の最小値
    [SerializeField] private float _minX = -3.5f;

    // x軸方向の移動範囲の最大値
    [SerializeField] private float _maxX = 1.5f;

    private void Update()
    {
        var pos = transform.position;

        // x軸方向の移動範囲制限
        pos.x = Mathf.Clamp(pos.x, _minX, _maxX);

        transform.position = pos;
    }
}