using System.Collections.Generic;
using UnityEngine;

public class AmazonIAPEventListener : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnEnable()
	{
		AmazonIAPManager.itemDataRequestFailedEvent += itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent += itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent += purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent += purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent += purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent += onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent += onGetUserIdResponseEvent;
	}

	private void OnDisable()
	{
		AmazonIAPManager.itemDataRequestFailedEvent -= itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent -= itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent -= purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent -= purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent -= purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent -= onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent -= onGetUserIdResponseEvent;
	}

	private void itemDataRequestFailedEvent()
	{
		Debug.Log("itemDataRequestFailedEvent");
		iSniperAndroidIAP.m_bIsSupport = false;
	}

	private void itemDataRequestFinishedEvent(List<string> unavailableSkus, List<AmazonItem> availableItems)
	{
		Debug.Log("itemDataRequestFinishedEvent. unavailable skus: " + unavailableSkus.Count + ", avaiable items: " + availableItems.Count);
		iSniperAndroidIAP.m_bIsSupport = true;
	}

	private void purchaseFailedEvent(string reason)
	{
		Debug.Log("purchaseFailedEvent: " + reason);
		if (iSniperAndroidIAP.FailureFunc != null)
		{
			iSniperAndroidIAP.FailureFunc();
		}
	}

	private void purchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		Debug.Log("purchaseSuccessfulEvent: " + receipt);
		Debug.Log(receipt.sku);
		iSniperAndroidIAP.SuccessEvent(receipt.sku);
		if (iSniperAndroidIAP.CompleteFunc != null)
		{
			iSniperAndroidIAP.CompleteFunc();
		}
	}

	private void purchaseUpdatesRequestFailedEvent()
	{
		Debug.Log("purchaseUpdatesRequestFailedEvent");
	}

	private void purchaseUpdatesRequestSuccessfulEvent(List<string> revokedSkus, List<AmazonReceipt> receipts)
	{
		Debug.Log("purchaseUpdatesRequestSuccessfulEvent. revoked skus: " + revokedSkus.Count);
		foreach (AmazonReceipt receipt in receipts)
		{
			Debug.Log(receipt);
		}
	}

	private void onSdkAvailableEvent(bool isTestMode)
	{
		Debug.Log("onSdkAvailableEvent. isTestMode: " + isTestMode);
	}

	private void onGetUserIdResponseEvent(string userId)
	{
		Debug.Log("onGetUserIdResponseEvent: " + userId);
	}
}
