using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] �͎�v�f�t�H���g�f�B�X�v���C�ŁA��� ON�B
        // �ǉ��f�B�X�v���C���\�����m�F���A���ꂼ����A�N�e�B�x�[�g���܂��B
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();
    }

}
