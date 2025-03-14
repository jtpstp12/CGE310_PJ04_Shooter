using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Gun gun;  // ลาก Object ของ Gun มาที่ Inspector
    public Slider ammoSlider; // ลาก UI Slider มาใส่

    void Start()
    {
        if (gun != null && ammoSlider != null)
        {
            ammoSlider.maxValue = gun.maxAmmo; // กำหนดค่ากระสุนสูงสุด
            ammoSlider.value = gun.maxAmmo; // ตั้งค่าเริ่มต้น
        }
    }

    void Update()
    {
        if (gun != null && ammoSlider != null)
        {
            ammoSlider.value = gun.GetCurrentAmmo(); // อัปเดตค่า Slider
        }
    }
}
