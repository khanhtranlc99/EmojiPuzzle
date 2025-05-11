using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UMPControlle : MonoBehaviour
{
    public Action callBack;
   public void Init( Action paramCallBack)
    {
        callBack = paramCallBack;
        // Create a ConsentRequestParameters object.
        var requestParameters = new ConsentRequestParameters
        {
            // False means users are not under age.
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = new ConsentDebugSettings
            {
                // For debugging consent settings by geography.
                DebugGeography = DebugGeography.EEA,
           
             
            }
        };

        // Check the current consent information status.
        ConsentInformation.Update(requestParameters, OnConsentInfoUpdated);
     
    }





    void OnConsentInfoUpdated(FormError consentError)
    {
     
        if (consentError != null)
        {
            // Handle the error.
            callBack?.Invoke();
            Debug.LogError("===1" + consentError);
            return;
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
       
        if (ConsentInformation.CanRequestAds())
        {
            callBack?.Invoke();
         
            return;
            //MobileAds.Initialize((InitializationStatus initstatus) =>
            //{
            //    // TODO: Request an ad.
            //});
        }

        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                Debug.LogError("Consent Form Load Failed: " + formError.Message);
                callBack?.Invoke(); // Gọi callback khi form bị lỗi
                return;
            }

            // Kiểm tra trạng thái đồng ý

            HandleConsentStatus();
        });

    }
    private void HandleConsentStatus()
    {
        var status = ConsentInformation.ConsentStatus;

        switch (status)
        {
            case ConsentStatus.Obtained:
               
             //   callBack?.Invoke();// Gọi callback khi người dùng đồng ý
                break;

            case ConsentStatus.Required:

                //callBack?.Invoke();
                break;

            case ConsentStatus.Unknown:
             
              /*  callBack?.Invoke(); */// Gọi callback khi trạng thái không xác định
                break;
        }

        callBack?.Invoke();
    }
}
