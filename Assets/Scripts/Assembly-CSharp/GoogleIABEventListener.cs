using System.Collections.Generic;
using Prime31;
using UnityEngine;

public class GoogleIABEventListener : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	}

	private void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");
		GoogleIAB.queryInventory(iSniperAndroidIAP.m_GooglePlayId);
	}

	private void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
		iSniperAndroidIAP.m_bIsSupport = false;
	}

	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
		Prime31.Utils.logObject(purchases);
		Prime31.Utils.logObject(skus);
		iSniperAndroidIAP.m_bIsSupport = true;
	}

	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
		iSniperAndroidIAP.m_bIsSupport = false;
	}

	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		iSniperAndroidIAP.SuccessEvent(purchase.productId);
		if (iSniperAndroidIAP.CompleteFunc != null)
		{
			iSniperAndroidIAP.CompleteFunc();
		}
		GoogleIAB.consumeProduct(purchase.productId);
		iSniperAndroidIAP.curBuySku = string.Empty;
	}

	private void purchaseFailedEvent(string error)
	{
		Debug.Log("purchaseFailedEvent: " + error);
		GoogleIAB.consumeProducts(iSniperAndroidIAP.m_GooglePlayId);
		GoogleIAB.consumeProduct(iSniperAndroidIAP.curBuySku);
		iSniperAndroidIAP.curBuySku = string.Empty;
		if (iSniperAndroidIAP.FailureFunc != null)
		{
			iSniperAndroidIAP.FailureFunc();
		}
	}

	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
	}

	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("######### consumePurchaseFailedEvent: " + error);
	}
}
