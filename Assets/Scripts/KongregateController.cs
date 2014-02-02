using System;
using System.Collections.Generic;
using UnityEngine;

public class KongregateController : MonoBehaviour
{
    public static Dictionary<string, string> AspectToyMappings;
    public Sprite ConnectionEstablishedImage;
    public static int DialoguesCompleted = 0;

    const string KONGREGATE_STAT_SUBMIT = "kongregate.stats.submit";

    private static bool _kongregateConnected = false;
    public static bool KongregateConnected
    {
        get { return KongregateController._kongregateConnected; }
    }


    public KongregateController()
    {
        AspectToyMappings = new Dictionary<string, string>();
        AspectToyMappings.Add("domestic", "Monkey");
        AspectToyMappings.Add("divorce", "Train");
        AspectToyMappings.Add("lonely", "Wolf");
    }



    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);

        /*
         * Try to connect to Kongregate. The GameObject's name is supplied so that if
         * Kongregate replies back, it will use SendMessage on the following GameObject
         * towards a method name called 'OnKongregateAPILoaded'
        */

        if (!KongregateConnected)
            Application.ExternalEval("if(typeof(kongregateUnitySupport) != 'undefined'){kongregateUnitySupport.initAPI('" + gameObject.name + "','OnKongregateAPILoaded');};");
    }



    #region Kongregate Callbacks

    /// <summary>
    /// Called when the Kongregate API is loaded and responds to our initialization
    /// request.
    /// </summary>
    /// <param name="userInfoString">
    /// A pipe separated string containing the User Id, Username and AuthToken.
    /// </param>
    void OnKongregateAPILoaded(string userInfoString)
    {
        _kongregateConnected = true;

        if (ConnectionEstablishedImage)
            transform.GetComponent<SpriteRenderer>().sprite = ConnectionEstablishedImage;

        try
        {
            //string[] parameters = userInfoString.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);

            //int userId = Convert.ToInt32(parameters[0]);
            //string userName = parameters[1], gameAuthToken = parameters[2];
        }
        catch { }

        ReportInitialization();
    }

    /// <summary>
    /// Called when the Kongregate user signs in.
    /// </summary>
    /// <param name="userInfoString">
    /// A pipe separated string containing the User Id, Username and AuthToken.
    /// </param>
    void OnKongregateUserSignedIn(string userInfoString)
    {
        //string[] parameters = userInfoString.Split(new char[] { '|' }, System.StringSplitOptions.RemoveEmptyEntries);

        //int userId = Convert.ToInt32(parameters[0]);
        //string userName = parameters[1], gameAuthToken = parameters[2];
    }

    #endregion Kongregate Callbacks



    public static void ReportInitialization()
    {
        Application.ExternalCall(KONGREGATE_STAT_SUBMIT, "Initialized", 1);
    }

    public static void ReportDialogueCompleted(string aspect, bool ending)
    {
        string toyName = AspectToyMappings[aspect];

        if (!string.IsNullOrEmpty(toyName))
        {
            if (KongregateConnected)
            {
                Application.ExternalCall(KONGREGATE_STAT_SUBMIT, toyName + "ToyPositiveEnding", Convert.ToInt32(ending));
                Application.ExternalCall(KONGREGATE_STAT_SUBMIT, toyName + "ToyNegativeEnding", Convert.ToInt32(!ending));
            }

            DialoguesCompleted++;

            if (DialoguesCompleted == 3)
                ReportGameCompleted();
        }
    }

    public static void ReportGameCompleted()
    {
        if (KongregateConnected)
            Application.ExternalCall(KONGREGATE_STAT_SUBMIT, "GameCompleted", 1);
    }
}