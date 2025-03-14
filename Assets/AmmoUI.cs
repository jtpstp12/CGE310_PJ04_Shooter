using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Gun gun;  // �ҡ Object �ͧ Gun �ҷ�� Inspector
    public Slider ammoSlider; // �ҡ UI Slider �����

    void Start()
    {
        if (gun != null && ammoSlider != null)
        {
            ammoSlider.maxValue = gun.maxAmmo; // ��˹���ҡ���ع�٧�ش
            ammoSlider.value = gun.maxAmmo; // ��駤���������
        }
    }

    void Update()
    {
        if (gun != null && ammoSlider != null)
        {
            ammoSlider.value = gun.GetCurrentAmmo(); // �ѻവ��� Slider
        }
    }
}
