using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsInitialize : MonoBehaviour
{
    void Awake()
    {
        MobileAds.Initialize(initStatus => { });
    }
}
