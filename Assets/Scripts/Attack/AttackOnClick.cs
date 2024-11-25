using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class SpawnCubeOnClick : MonoBehaviour
{

    public GameObject cubePrefab;
    public float spawnCooldown = 0.5f;

    private void Start()
    {
        //InputManager.Instance.RegisterOnMouseLeftClicked(Spawn, true);
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnFingerDown;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        TouchSimulation.Disable();
    }

    private void OnDestroy()
    {
        InputManager.Instance.RegisterOnMouseLeftClicked(Spawn, false);

    }

    private void Spawn(InputAction.CallbackContext callbackContext)
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        //{
        //    GameObject cube = Instantiate(cubePrefab, hit.point, Quaternion.identity);

        //    float cubeHeight = cube.GetComponent<Renderer>().bounds.size.y;
        //    cube.transform.position += new Vector3(0, cubeHeight / 2, 0);
        //}
        //StartCoroutine(SpawnCooldown());
    }

    private void OnFingerDown(Finger finger)
    {
        Vector3 fingerPosition = finger.screenPosition;

        /*RectTransform joystickRect = UIManager.Instance.joystick.transform as RectTransform;

        if (joystickRect.offsetMin.x <= fingerPosition.x && fingerPosition.x <= joystickRect.offsetMax.x &&
            joystickRect.offsetMin.y <= fingerPosition.y && fingerPosition.y <= joystickRect.offsetMax.y)
        {
            return;
        }*/

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(fingerPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, Mathf.Infinity, LayerMask.GetMask("UI")))
        {
            return;
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            GameObject cube = Instantiate(cubePrefab, hit.point, Quaternion.identity);

        }

        StartCoroutine(SpawnCooldown());
    }

    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);
    }
}
