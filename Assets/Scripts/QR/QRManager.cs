using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRManager : MonoBehaviour
{
    public QRCode[] QRCodes;
    public Text qRText;
    public Text scanResult;
    Dictionary<string, QRCode> qRDictionary = new Dictionary<string, QRCode>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(QRCode qrc in QRCodes) {
            qRDictionary.Add(qrc.name, qrc);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScanResult(string qr) {
        qRText.text = qr;
        if(qRDictionary.ContainsKey(qr)) scanResult.text = qRDictionary[qr].description;
    }
}
