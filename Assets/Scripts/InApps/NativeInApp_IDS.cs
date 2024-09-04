using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace NativeInApps
{

    public static class NativeInApp_IDS
    {
        public const string HINTS_10_ID = "com.tedrasoft.mismatched.10hints";
        public const string HINTS_30_ID = "com.tedrasoft.mismatched.30hints";
        public const string NO_ADS_ID = "com.tedrasoft.mismatched.noads";
        public const string WINTER_PACK_ID ="com.tedrasoft.mismatched.winterpack";

        public static NativeInAppItem[] Items =
        {
            new NativeInAppItem(HINTS_10_ID, ProductType.Consumable),
            new NativeInAppItem(HINTS_30_ID, ProductType.Consumable),
            new NativeInAppItem(NO_ADS_ID, ProductType.NonConsumable),
            new NativeInAppItem(WINTER_PACK_ID, ProductType.NonConsumable)
        };

    }
}
