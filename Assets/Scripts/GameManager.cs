using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int scorep0 = 0;
    public int scorep1 = 0;
    public int scorep2 = 0;
    public int scorep3 = 0;
    public int scorep4 = 0;
    public int scorep5 = 0;
    public int scorep6 = 0;
    public int scorep7 = 0;
    public int scorep8 = 0;

    public void addScore(int id) {
        switch (id) {
            case 0:
                scorep0++;
                break;
            case 1:
                scorep1++;
                break;
            case 2:
                scorep2++;
                break;
            case 3:
                scorep3++;
                break;
            case 4:
                scorep4++;
                break;
            case 5:
                scorep5++;
                break;
            case 6:
                scorep6++;
                break;
            case 7:
                scorep7++;
                break;
            case 8:
                scorep8++;
                break;
        }
    }
}
