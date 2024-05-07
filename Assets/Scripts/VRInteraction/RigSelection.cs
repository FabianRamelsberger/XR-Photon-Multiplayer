/* --------------------------------------------------------------------------------
# Created by: Fabian Ramelsberger
# Created Date: 2024
# --------------------------------------------------------------------------------*/

using Fusion;
using Fusion.XR.Shared.Rig;
using UnityEngine;
using UnityEngine.Events;

/**
 *
 * Script to display an overlay UI to select desktop or VR mode, and active the associated rig, alongside the connexion component
 *
 **/
public interface IRigSelection
{
    public UnityEvent OnSelectRig { get; }
    public bool IsRigSelected { get; }
    public bool IsVRRigSelected { get; }
}


public class RigSelection : MonoBehaviour, IRigSelection
{
    private UnityEvent onSelectRig;
    public UnityEvent OnSelectRig => onSelectRig;
    public bool IsRigSelected => rigSelected;
    public bool IsVRRigSelected => vrRig && vrRig.isActiveAndEnabled;

    private const string RIGMODE_VR = "VR";
    private const string RIGMODE_DESKTOP = "Desktop";
    private const string SETTING_RIGMODE = "RigMode";

    public GameObject connexionHandler;
    public HardwareRig vrRig;
    public HardwareRig desktopRig;

    public Camera RigSelectionCamera => _rigSelectionCamera;
    private Camera _rigSelectionCamera;

    public Camera RigCamera => _rigCamera;
    private Camera _rigCamera;
    public bool forceVROnAndroid = true;

    public bool rigSelected = false;

    public enum Mode
    {
        SelectedByUI,
        SelectedByUserPref,
        ForceVR,
        ForceDesktop
    }

    public Mode mode = Mode.SelectedByUI;
    [SerializeField] private Camera _desktopCamera;
    [SerializeField] private Camera _vrCamera;

    private void Awake()
    {
        _rigSelectionCamera = GetComponentInChildren<Camera>();
        if (connexionHandler)
        {
            connexionHandler.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError(
                "No connexion handler provided to RigSelection: risk of connection before choosing the appropriate hardware rig !");
        }

        vrRig.gameObject.SetActive(false);
        desktopRig.gameObject.SetActive(false);

#if !UNITY_EDITOR && UNITY_ANDROID
            if (forceVROnAndroid)
            {
                EnableVRRig();
                return;
            }
#endif
        if (mode == Mode.ForceVR)
        {
            EnableVRRig();
            return;
        }

        if (mode == Mode.ForceDesktop)
        {
            EnableDesktopRig();
            return;
        }

        // In release build, we replace SelectedByUI by SelectedByUserPref unless overriden
        DisableDebugSelectedByUI();

        if (mode == Mode.SelectedByUserPref)
        {
            var sessionPrefMode = PlayerPrefs.GetString(SETTING_RIGMODE);
            if (sessionPrefMode != "")
            {
                if (sessionPrefMode == RIGMODE_VR) EnableVRRig();
                if (sessionPrefMode == RIGMODE_DESKTOP) EnableDesktopRig();
            }
        }
    }

    protected virtual void DisableDebugSelectedByUI()
    {
#if !UNITY_EDITOR
            if (mode == Mode.SelectedByUI) mode = Mode.SelectedByUserPref;
#endif
    }

    protected virtual void OnGUI()
    {
        GUILayout.BeginArea(new Rect(5, 5, Screen.width - 10, Screen.height - 10));
        {
            GUILayout.BeginVertical(GUI.skin.window);
            {
                if (GUILayout.Button("VR"))
                {
                    EnableVRRig();
                }

                if (GUILayout.Button("Desktop"))
                {
                    EnableDesktopRig();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
    }

    void EnableVRRig()
    {
        gameObject.SetActive(false);
        vrRig.gameObject.SetActive(true);
        _rigCamera = _vrCamera;

        SetVRPreference();
        OnConnectRigSelected();
    }

    void EnableDesktopRig()
    {
        gameObject.SetActive(false);
        desktopRig.gameObject.SetActive(true);
        _rigCamera = _desktopCamera;

        SetDesktopPreference();
        OnConnectRigSelected();
    }

    void OnConnectRigSelected()
    {
        if (connexionHandler && connexionHandler.gameObject.activeSelf == false)
        {
            connexionHandler.gameObject.SetActive(true);
            var runner = connexionHandler.GetComponent<NetworkRunner>();
            if (runner)
            {
                // As the runner was disabled, the runner may not have auto registered its listeners
                foreach (var listener in runner.GetComponentsInChildren<INetworkRunnerCallbacks>())
                {
                    runner.AddCallbacks(listener);
                }
            }
        }

        if (OnSelectRig != null) OnSelectRig.Invoke();
        if (_rigSelectionCamera) _rigSelectionCamera.gameObject.SetActive(false);
        rigSelected = true;
    }

    [ContextMenu("Set preference to desktop")]
    public void SetDesktopPreference()
    {
        PlayerPrefs.SetString(SETTING_RIGMODE, RIGMODE_DESKTOP);
        PlayerPrefs.Save();
    }

    [ContextMenu("Set preference to VR")]
    public void SetVRPreference()
    {
        PlayerPrefs.SetString(SETTING_RIGMODE, RIGMODE_VR);
        PlayerPrefs.Save();
    }
}