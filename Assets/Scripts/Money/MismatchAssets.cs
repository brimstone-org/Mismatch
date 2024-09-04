
//using Soomla.Store;
//using System.Collections.Generic;
//ublic class MismatchAssets : IStoreAssets{


//  public int GetVersion() {
//    return 4;
//  }

  // NOTE: Even if you have no use in one of these functions, you still need to
  // implement them all and just return an empty array.
/*

  public VirtualCurrency[] GetCurrencies() {
      return new VirtualCurrency[] { HINT_CURRENCY };
  }

  public VirtualGood[] GetGoods() {
      return new VirtualGood[] { NO_ADS_LTVG, LEVELPACK_WINTER_LTVG }; //NO_ADS_LTVG  SHIELD_GOOD 
  }

  public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] { MINI_HINT_PACK, NORMAL_HINT_PACK };
  }

  public VirtualCategory[] GetCategories() {
    return new VirtualCategory[]{GENERAL_CATEGORY};
  }
*/
  /** Virtual Currencies **/

/*
  public static VirtualCurrency HINT_CURRENCY = new VirtualCurrency(
    "10 Hints",                               // Name
    "10 Hints to help solve any level.",     // Description
    "hints_currency_ID"                      // Item ID
  );

  /** Virtual Currency Packs **/

/*
  public static VirtualCurrencyPack MINI_HINT_PACK = new VirtualCurrencyPack(
    "10 Hints",                           // Name
    "10 Hints to help solve any level.",  // Description
    "hints_10_ID",                        // Item ID
    10,                                   // Number of currencies in the pack
    "hints_currency_ID",                  // ID of the currency associated with this pack
    new PurchaseWithMarket(               // Purchase type (with real money $)
      // "android.test.purchased",
      "com.tedrasoft.mismatched.10hints",           // Product ID
      0.99                                   // Price (in real money $)
    )
  );

public static VirtualCurrencyPack NORMAL_HINT_PACK = new VirtualCurrencyPack(
	"30 Hints",                           // Name
	"30 Hints to help solve any level.",  // Description
	"hints_30_ID",                        // Item ID
	30,                                   // Number of currencies in the pack
	"hints_currency_ID",                  // ID of the currency associated with this pack
	new PurchaseWithMarket(               // Purchase type (with real money $)
		// "android.test.purchased",
		"com.tedrasoft.mismatched.30hints",           // Product ID
		2.49                                  // Price (in real money $)
	)
);*/



  /** Virtual Goods **/

  //public static VirtualGood SHIELD_GOOD = new SingleUseVG(
  //  "Shield",                             // Name
  //  "Protect yourself from enemies",      // Description
  //  "shield_ID",                          // Item ID
  //  new PurchaseWithVirtualItem(          // Purchase type (with virtual currency)
  //   "hints_currency_ID",                 // ID of the item used to pay with
  //    225                                 // Price (amount of coins)
  //  )
  //);

  // NOTE: Create non-consumable items using LifeTimeVG with PurchaseType of PurchaseWithMarket.

/*
  public static VirtualGood NO_ADS_LTVG = new LifetimeVG(
    "No Ads",                             // Name
    "No More Ads!",                       // Description
    "no_ads_ID",                          // Item ID
    new PurchaseWithMarket(               // Purchase type (with real money $)
      "com.tedrasoft.mismatched.noads",      // Product ID
      0.99                                   // Price (in real money $)
    )
  );

  public static VirtualGood LEVELPACK_WINTER_LTVG = new LifetimeVG(
    "Winter Level Pack",                  // Name
    "Winter themed hard level pack",      // Description
    "levelpack_winter_ID",                // Item ID
    new PurchaseWithMarket(               // Purchase type (with real money $)
      "com.tedrasoft.mismatched.winterpack",      // Product ID
      0.99                                   // Price (in real money $)
    )
  );
  */

  /** Virtual Categories **/

  //public static VirtualCategory GENERAL_CATEGORY = new VirtualCategory(
    //"General", new List<string>(new string[] { }) //SHIELD_GOOD.ItemId
 // );

//}
