using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.IO;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Mirror.Examples.Common;
using System.Net.Mail;
using System.Net;
using Mirror.Authenticators;
using static Mirror.Authenticators.BasicAuthenticator;
using static InstPlayer;

public class MenuController : MonoBehaviour
{
    public NetworkManager manager;
    public GameObject menuPanel;
    public GameObject settingsPanel;
    public static bool active = true;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public InputField fpsInputField;
    public Toggle toggleWindowMode;
    public Toggle toggleVSync;
    public Toggle classicControllToggle;
    public Slider DynamicResolutionSlider;
    public Dropdown DynamicResolutionDropdown;
    public Toggle togglePostProcessing;
    public Toggle dynamicResolutionAutoToggle;
    public Dropdown antiAliasing;
    public Slider RenderDistanceSlider;
    public Slider ShadowSlider;
    public Slider CameraSensitiveSlider;
    public InputField LoginInputField;
    public InputField PasswordInputField;
    public GameObject loginPanel;
    public Light light;
    HDAdditionalLightData hDAdditionalLightData;
    public Volume volume;
    MoveControll moveControll;
    HDShadowSettings hDShadowSettings;
    float fpsTimer;
    int targetFrameRate = 60;
    public Text FPSText;
    bool settingsActive;
    int framesPerSecond = 0;
    int framesRendered = 0;
    DateTime _lastTime;
    float resolutionScale = 1;
    public static MenuController instance;
    public TMPro.TextMeshProUGUI PasswordErrorText;
    public WaterSurface waterSurface;
    private void OnEnable()
    {
        NetworkManager.OnServerStart += OnServerStart;
        BasicAuthenticator.OnIncorrectPassword += OnIncorrectPassword;
        BasicAuthenticator.OnAccountCreated += OnAccountCreated;
        InstPlayer.OnPlayerInstanceEvent += OnPlayerInstance;
    }
    private void OnDisable()
    {
        NetworkManager.OnServerStart -= OnServerStart;
        BasicAuthenticator.OnIncorrectPassword -= OnIncorrectPassword;
        BasicAuthenticator.OnAccountCreated -= OnAccountCreated;
        InstPlayer.OnPlayerInstanceEvent -= OnPlayerInstance;
    }
    void OnIncorrectPassword()
    {
        PasswordErrorText.text = "Введен неверный пароль";
    }
    void OnAccountCreated()
    {
        PasswordErrorText.text = "Аккаунт зарегистрирован";
    }
    private void Awake()
    {
        if (!instance)
            instance = this;
        InterfaceSettings.Load();
    }
    void OnServerStart()
    {
#if !UNITY_EDITOR
        enabled = false;
#endif
    }
    public void OnPlayerInstance()
    {
        SwitchMenuActive();
        loginPanel.SetActive(false);
    }
    float DynamicResolutionScaler()
    {
        if (dynamicResolutionAutoToggle.isOn)
        {
            resolutionScale = Mathf.Clamp((float)(framesPerSecond + 1) / targetFrameRate, 0.2f, 1f);
            return resolutionScale;
        }
        else
        {
            return DynamicResolutionSlider.value * 0.01f;
        }
    }
    void Start()
    {
        DynamicResolutionHandler.SetDynamicResScaler(DynamicResolutionScaler, DynamicResScalePolicyType.ReturnsMinMaxLerpFactor);

        hDAdditionalLightData = light.GetComponent<HDAdditionalLightData>();
        DynamicResolutionDropdown.onValueChanged.AddListener(delegate { ValueChangeDynamicResolutionDropdown(); });
        dynamicResolutionAutoToggle.onValueChanged.AddListener(delegate { OnDynamicResolutionAutoToggleValueChange(); });
        resolutionDropdown.value = InterfaceSettings.resolution;
        qualityDropdown.value = InterfaceSettings.textureLevel;
        toggleWindowMode.isOn = InterfaceSettings.window;
        toggleVSync.isOn = InterfaceSettings.vSync;
        DynamicResolutionDropdown.value = InterfaceSettings.dynamicResolution;
        DynamicResolutionSlider.value = InterfaceSettings.dynamicResolutionPercent;
        togglePostProcessing.isOn = InterfaceSettings.postprocessing;
        antiAliasing.value = InterfaceSettings.antialiasing;
        RenderDistanceSlider.value = InterfaceSettings.renderDistance;
        DistanceInterestManagement.visRange = (int)(RenderDistanceSlider.value / 2);
        ShadowSlider.value = InterfaceSettings.shadows;
        targetFrameRate = InterfaceSettings.targetFrameRate;
        fpsInputField.text = InterfaceSettings.targetFrameRate.ToString();
        dynamicResolutionAutoToggle.isOn = InterfaceSettings.dynamicResolutionAuto;
        classicControllToggle.isOn = InterfaceSettings.classicControll;
        MoveControll.classicControll = InterfaceSettings.classicControll;
        CameraSensitiveSlider.value = InterfaceSettings.cameraSensitive;
        LoginInputField.text = InterfaceSettings.login;
        PasswordInputField.text = InterfaceSettings.password;
        BasicAuthenticator.username = InterfaceSettings.login;
        BasicAuthenticator.password = InterfaceSettings.password;
        ApplySettings();

    }
    public void OnDynamicResolutionAutoToggleValueChange()
    {
        DynamicResolutionSlider.interactable = !dynamicResolutionAutoToggle.isOn;
    }
    private void LateUpdate()
    {
        framesRendered++;

        if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
        {
            framesPerSecond = framesRendered;
            framesRendered = 0;
            _lastTime = DateTime.Now;
            FPSText.text = framesPerSecond.ToString();
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !settingsActive && GOManager.player)
        {
            SwitchMenuActive();
        }
    }

    public static void SwitchMenuActive()
    {
        active = !active;
        instance.menuPanel.SetActive(active);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ActiveSettingsPanel()
    {
        settingsActive = true;
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }
    public void DeactiveSettingsPanel()
    {
        settingsActive = false;
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    public void ValueChangeDynamicResolutionDropdown()
    {
        if (DynamicResolutionDropdown.value != 0)
        {
            if (!dynamicResolutionAutoToggle.isOn)
                DynamicResolutionSlider.interactable = true;
            dynamicResolutionAutoToggle.interactable = true;
        }
        else
        {
            dynamicResolutionAutoToggle.interactable = false;
            DynamicResolutionSlider.interactable = false;
        }
    }
    public void ApplySettings()
    {
        if (resolutionDropdown.value == 0)
        {
            Screen.SetResolution(800, 600, !toggleWindowMode.isOn);
        }
        else
            if (resolutionDropdown.value == 1)
        {
            Screen.SetResolution(1280, 720, !toggleWindowMode.isOn);
        }
        else
            if (resolutionDropdown.value == 2)
        {
            Screen.SetResolution(1920, 1080, !toggleWindowMode.isOn);
        }
        else
            if (resolutionDropdown.value == 3)
        {
            Screen.SetResolution(2048, 1080, !toggleWindowMode.isOn);
        }
        else
            if (resolutionDropdown.value == 4)
        {
            Screen.SetResolution(4096, 2160, !toggleWindowMode.isOn);
        }


        QualitySettings.SetQualityLevel(5 - qualityDropdown.value);
        if (qualityDropdown.value == 0)
        {
            waterSurface.deformationRes = WaterSurface.WaterDeformationResolution.Resolution2048;
        }
        else
            if (qualityDropdown.value == 1)
        {
            waterSurface.deformationRes = WaterSurface.WaterDeformationResolution.Resolution1024;
        }
        else
            if (qualityDropdown.value == 2)
        {
            waterSurface.deformationRes = WaterSurface.WaterDeformationResolution.Resolution512;
        }
        else
            if (qualityDropdown.value >= 4)
        {
            waterSurface.deformationRes = WaterSurface.WaterDeformationResolution.Resolution256;
        }
        volume.enabled = togglePostProcessing.isOn;
        CameraController.instance.sensitive = CameraSensitiveSlider.value;
        CameraController.instance.cinemachineFreeLook.m_Lens.FarClipPlane = RenderDistanceSlider.value;
        DistanceInterestManagement.visRange = (int)(RenderDistanceSlider.value / 2);
        if (toggleVSync.isOn)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;


        if ((int)ShadowSlider.value == 0)
            hDAdditionalLightData.EnableShadows(false);
        else
        {
            hDAdditionalLightData.EnableShadows(true);
            hDAdditionalLightData.SetShadowResolution((int)ShadowSlider.value);
        }
        HDAdditionalCameraData hDAdditionalCameraData = Camera.main.GetComponent<HDAdditionalCameraData>();

        if (DynamicResolutionDropdown.value > 0)
            hDAdditionalCameraData.allowDynamicResolution = true;
        else
        {
            hDAdditionalCameraData.allowDynamicResolution = false;
        }
        if (DynamicResolutionDropdown.value == 1)
        {
            hDAdditionalCameraData.allowDeepLearningSuperSampling = true;
            //  hDAdditionalCameraData.allowFidelityFX2SuperResolution = false;
        }
        else
        if (DynamicResolutionDropdown.value == 2)
        {
            hDAdditionalCameraData.allowDeepLearningSuperSampling = false;
            // hDAdditionalCameraData.allowFidelityFX2SuperResolution = true;
        }



        MoveControll.classicControll = classicControllToggle.isOn;
        hDAdditionalCameraData.antialiasing = (HDAdditionalCameraData.AntialiasingMode)antiAliasing.value;
        targetFrameRate = int.Parse(fpsInputField.text);
        Application.targetFrameRate = targetFrameRate;
        InterfaceSettings.SaveGraphicSettings(LoginInputField.text, PasswordInputField.text, resolutionDropdown.value, toggleWindowMode.isOn, toggleVSync.isOn, qualityDropdown.value, togglePostProcessing.isOn, (int)ShadowSlider.value, (int)RenderDistanceSlider.value, antiAliasing.value, DynamicResolutionDropdown.value, (int)DynamicResolutionSlider.value, targetFrameRate, dynamicResolutionAutoToggle.isOn, classicControllToggle.isOn, CameraSensitiveSlider.value);

    }
    public void SetLogin()
    {
        NetworkManager.autoCreatePlayer = true;
        if (NetworkClient.active)
        {
            PasswordErrorText.text = "Вы уже подключены";
            return;
        }
        if (LoginInputField.text.Length == 0)
        {
            if (PasswordInputField.text.Length == 0)
            {
                PasswordErrorText.text = "Вы не ввели почту и пароль";
                return;
            }
            PasswordErrorText.text = "Вы не ввели почту";
            return;

        }
        else
        {
            if (PasswordInputField.text.Length == 0)
            {
                PasswordErrorText.text = "Вы не ввели пароль";
                return;
            }
        }
        InterfaceSettings.SaveGraphicSettings(LoginInputField.text, PasswordInputField.text, resolutionDropdown.value, toggleWindowMode.isOn, toggleVSync.isOn, qualityDropdown.value, togglePostProcessing.isOn, (int)ShadowSlider.value, (int)RenderDistanceSlider.value, antiAliasing.value, DynamicResolutionDropdown.value, (int)DynamicResolutionSlider.value, targetFrameRate, dynamicResolutionAutoToggle.isOn, classicControllToggle.isOn, CameraSensitiveSlider.value);
        BasicAuthenticator.username = LoginInputField.text;
        BasicAuthenticator.password = PasswordInputField.text;

        using (StreamReader sr = File.OpenText("Config.txt"))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                manager.StartClient(s);

                return;
            }
        }

    }

    public void PasswordRestore()
    {
        NetworkManager.autoCreatePlayer = false;
        if (LoginInputField.text.Length == 0)
        {
            if (PasswordInputField.text.Length == 0)
            {
                PasswordErrorText.text = "Вы не ввели почту и пароль";
                return;
            }
            PasswordErrorText.text = "Вы не ввели почту";
            return;

        }
        PasswordErrorText.text = "Письмо с паролем отправлено на вашу почту";
        BasicAuthenticator.username = "00";
        BasicAuthenticator.password = "00";
        using (StreamReader sr = File.OpenText("Config.txt"))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                manager.StartClient(s);
                StopCoroutine(asd());
                StartCoroutine(asd());
                return;
            }
        }


    }
    IEnumerator asd()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (NetworkClient.isConnected)
            {
                ChatOmegaLul.MailRequestMessage mailRequestMessage = new ChatOmegaLul.MailRequestMessage
                {
                    email = LoginInputField.text
                };

                NetworkClient.Send(mailRequestMessage);
                break;
            }
        }
    }
}
