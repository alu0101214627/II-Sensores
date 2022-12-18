using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class samuraiControler : MonoBehaviour
{
    float speed = 10.0f;
    public TMP_Text coordenadas;  
    // Start is called before the first frame update
    void Start()
    {
        coordenadas.color = Color.red; 
        Input.compass.enabled = true;
        Input.location.Start();
    }

    // Update is called once per frame
    void Update()
    {
        string cadena = "";
        computeRotation();
        computeMovement();
        if (Input.location.status == LocationServiceStatus.Running) {
            updateCanvas(cadena);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Input.location.Stop();
            Application.Quit();
        }
    }

    void computeRotation() {
        var yzmag = Mathf.Sqrt(Mathf.Pow(Input.acceleration.y, 2) + Mathf.Pow(Input.acceleration.z, 2));
        var zrot = Mathf.Atan2(Input.acceleration.x, yzmag);
        var zangle = -zrot * (180 / Mathf.PI);
        transform.eulerAngles = new Vector3(0, gameObject.transform.rotation.y - Input.compass.trueHeading, 0);
    }

    void computeMovement() {
        Vector3 dir = Vector3.zero;
        dir.x = -Input.acceleration.x;
        dir.z = Input.acceleration.y;
        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        dir *= Time.deltaTime;
        transform.Translate(dir * speed);
    }

    void updateCanvas(string cadena) {
        cadena = "Latitud: " + Input.location.lastData.latitude.ToString() + '\n';
        cadena += "Longitud: " + Input.location.lastData.longitude.ToString() + '\n';
        cadena += "Altura: " + Input.location.lastData.altitude.ToString();
        coordenadas.text = cadena;
    }

}
