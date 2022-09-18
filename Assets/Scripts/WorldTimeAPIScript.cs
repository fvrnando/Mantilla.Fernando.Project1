using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class WorldTimeAPIScript : MonoBehaviour
{
    public GameObject timeTextObject;
        // Chicago
       string url = "http://worldtimeapi.org/api/timezone/America/Chicago";
       // Los Angeles
       string url2 = "http://worldtimeapi.org/api/timezone/America/Los_Angeles";
   
    void Start()
    {

    // wait a couple seconds to start and then refresh every 60 seconds

       InvokeRepeating("GetDataFromWeb", 2f, 600f);
   }

   void GetDataFromWeb()
   {
        if (gameObject.name == "TimeTextLA") {
            StartCoroutine(GetRequest(url2));
        } else {
            StartCoroutine(GetRequest(url));
        }
   }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.result ==  UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                // print out the weather data to make sure it makes sense
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);

                // this code will NOT fail gracefully, so make sure you have
                // your API key before running or you will get an error

            	// grab the current temperature and simplify it if needed
            	int startTime = webRequest.downloadHandler.text.IndexOf("datetime",0);
            	int endTime = webRequest.downloadHandler.text.IndexOf(",",startTime);

                if (gameObject.name == "TimeTextLA") {
                    string timeF = webRequest.downloadHandler.text.Substring(startTime+22, 5);
                    timeTextObject.GetComponent<TextMeshPro>().text = "" + timeF;
                } else {
                    string hour = webRequest.downloadHandler.text.Substring(startTime+22, 2);
                    string min = webRequest.downloadHandler.text.Substring(startTime+25, 2);
                    int hourI = int.Parse(hour);
                    if (hourI > 12) {
                        hourI = hourI - 12;
                        string timeF = hourI.ToString() + ":" + min + " PM";
                        timeTextObject.GetComponent<TextMeshPro>().text = "" + timeF;
                    } else {
                        string timeF = hourI.ToString() + ":" + min + " AM";
                        timeTextObject.GetComponent<TextMeshPro>().text = "" + timeF;
                    }

                }
            }
        }
    }
}

