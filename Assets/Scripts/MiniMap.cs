using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    public Camera minimap_camera;
    public UI_References ui_refs;

    bool wolf_map = false;
    bool revert = false;

    void Start()
    {
        homeMap();
    }

    void LateUpdate()
    {
        if (player != null)
        {
            if (player.position.x < 6.4f && player.position.x >= -17.3f && player.position.z < 23.5f && player.position.z >= -0.6f)
                homeMap();

            if (player.position.x < -17.3 && player.position.x > -41.5 && player.position.z < 23.5f && player.position.z >= -0.6f)
                mouseMap();

            if (player.position.x < -17.3 && player.position.x > -41.5 && player.position.z > -24.5f && player.position.z < -0.6f)
                frogMap();

            if (player.position.x < 6.4f && player.position.x >= -17.3f && player.position.z > -24.5f && player.position.z < -0.6f)
                rabbitMap();

            /*if (player.position.x < 30.0f && player.position.x >= 6.4f && player.position.z > -24.5f && player.position.z < -0.6f)
                birdMap();

            if (player.position.x < 54.1f && player.position.x >= 30.0f && player.position.z > -24.5f && player.position.z < -0.6f)
                birdMap2();

            if (player.position.x < 30.0f && player.position.x >= 6.4f && player.position.z < 23.5f && player.position.z >= -0.6f)
                skunkMap1();

            if (player.position.x < 30.0f && player.position.x >= 6.2f && player.position.z < 47.0f && player.position.z >= 23.5f)
                skunkMap2();

            if (player.position.x < 54.1f && player.position.x >= 30.0f && player.position.z < 47.0f && player.position.z >= 23.5f)
                skunkMap3();

            if (player.position.x < 54.1f && player.position.x >= 30.0f && player.position.z < 23.5f && player.position.z >= -0.6f)
                skunkMap4();
             * */

            if (player.position.x < 54.1f && player.position.x >= 6.4f && player.position.z > -24.5f && player.position.z < 47.0f)
                entireMapSkunkWolf();

            if (player.position.x < 6.4f && player.position.x >= -17.3f && player.position.z < 47.0f && player.position.z >= 23.5f)
                bearMap();

            if (player.position.x < -17.3 && player.position.x > -41.5 && player.position.z < 47.0f && player.position.z >= 23.5f)
                tigerMap();

        }
    }
    void homeMap()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(-5.68f, 15f, 11.42f);
        ui_refs.GetComponent<UI_References>().revertMinimapBorder();
        ui_refs.GetComponent<UI_References>().disableDetectionBar();
    }

    void mouseMap()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(-29.58f, 15f, 11.42f);
    }

    void frogMap()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(-29.58f, 15f, -12.58f);
    }

    void rabbitMap()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(-5.68f, 15f, -12.58f);
        ui_refs.GetComponent<UI_References>().revertMinimapBorder();
        ui_refs.GetComponent<UI_References>().disableDetectionBar();
    }

    void entireMapSkunkWolf()
    {
        minimap_camera.transform.position = new Vector3(42.5f, 15f, 11.42f);
        minimap_camera.orthographicSize = 36;
        ui_refs.GetComponent<UI_References>().changeMinimapBorder();
        ui_refs.GetComponent<UI_References>().enableDetectionBar();
    }

    void bearMap()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(-5.68f, 15f, 35.42f);
        ui_refs.GetComponent<UI_References>().revertMinimapBorder();
        ui_refs.GetComponent<UI_References>().disableDetectionBar();
    }

    void tigerMap()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(-29.58f, 15f, 35.42f);
    }

    void birdMap()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(18.92f, 15f, -12.58f);
    }

    void birdMap2()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(42.0f, 15f, -12.58f);
    }


    void skunkMap1()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(18.92f, 15f, 11.42f);
    }

    void skunkMap2()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(18.92f, 15f, 35.42f);
    }

    void skunkMap3()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(42.0f, 15f, 35.42f);
    }

    void skunkMap4()
    {
        minimap_camera.orthographicSize = 12;
        minimap_camera.transform.position = new Vector3(42.0f, 15f, 11.42f);
    }
}
