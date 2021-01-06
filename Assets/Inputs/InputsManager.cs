// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/InputsManager.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputsManager : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputsManager()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputsManager"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""3afc273a-d1f7-4cb4-85f1-b5cf85abe6ed"",
            ""actions"": [
                {
                    ""name"": ""MovementHorizontal"",
                    ""type"": ""Button"",
                    ""id"": ""df685b34-6b79-4fff-a716-214cceb39bd5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MovementVertical"",
                    ""type"": ""Button"",
                    ""id"": ""d56b9c71-9992-4804-901c-1477b6aacb49"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""4c228a4b-543a-4062-8f9b-14ec5d1ab60b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Levitate"",
                    ""type"": ""Button"",
                    ""id"": ""c8143545-ab28-4b68-838a-2cf6f90d2401"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""Value"",
                    ""id"": ""e0c0772e-11c2-42a4-bd50-8a72304e79fa"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollButton"",
                    ""type"": ""Button"",
                    ""id"": ""d4d59c92-cf62-4b17-b28d-41b00b8308bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""KEYBOARD"",
                    ""id"": ""a143696f-9ed9-40ad-afae-e0bfb137dfd3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementHorizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""f2164f6f-e1d8-43a2-9ce2-3cf8e37a506a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""MovementHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""908c6ee1-4e8c-4c14-8ea1-7d0a10910e71"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""MovementHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""GAMEPAD"",
                    ""id"": ""6f18b00c-e846-4535-9406-bd7388f9c61f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementHorizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""bd976f4f-8bb4-4440-b582-a9dcc602ac9c"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4 Controller;Xbox Controller"",
                    ""action"": ""MovementHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8d649867-8300-4828-882e-a6ce0b4bb588"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4 Controller;Xbox Controller"",
                    ""action"": ""MovementHorizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""KEYBOARD"",
                    ""id"": ""310e6497-2aac-4367-99bf-2aa12b865360"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementVertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9570fd85-93d7-4f7d-b8f0-b3a2b571a64c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""MovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d5522bcf-38ce-4d11-9410-955a77948d5f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""MovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""GAMEPAD"",
                    ""id"": ""21e489d2-5a89-42f6-9b93-003f0aadc9b3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementVertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""be4d332d-30f4-47aa-95bb-7aeeaf451b32"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4 Controller;Xbox Controller"",
                    ""action"": ""MovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ddd0a8fa-e872-47dd-981a-7615c233f4a9"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4 Controller;Xbox Controller"",
                    ""action"": ""MovementVertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""KEYBOARD"",
                    ""id"": ""fd908334-eadd-473f-80f4-edca5c08d263"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""0e6736c5-98a3-4c8c-840e-c70bdbdbe689"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""27f67dc8-ec04-481d-b117-9cf202c80528"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""GAMEPAD"",
                    ""id"": ""4f5ad6c7-111d-4f4a-a1ff-7d9ef6db6505"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b8967484-5b9a-4738-9834-21e703820219"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PS4 Controller;Xbox Controller"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e1a3e70f-81eb-4f3c-8229-eb3de7113ca1"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox Controller;PS4 Controller"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""KEYBOARD"",
                    ""id"": ""e169f79c-abe7-4ab8-af18-3fb3ccea4399"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Levitate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f331f5dd-4e43-4921-bd11-d5f91e185126"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Levitate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""076224d7-fea4-452b-a022-7b53cae62fc5"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Levitate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""GAMEPAD"",
                    ""id"": ""94156212-db6d-45e3-8619-db32dfb1e247"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Levitate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8430d144-7f74-48b0-8403-4eba126f0eec"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Levitate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""93325e5f-b43f-440b-81a3-0846472eb46f"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Levitate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e117a7cc-adcd-4581-ad57-3da417e996a4"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2364d681-1094-4c2c-9b13-aff093de52a7"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""ScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""423686ac-1d25-4bcb-afba-7e0dee2cfcad"",
            ""actions"": [
                {
                    ""name"": ""DeveloperConsole"",
                    ""type"": ""Button"",
                    ""id"": ""78258e1a-b59e-4fbe-9174-e5a2007dedb4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""0cf2300a-408b-4e08-a247-283153d59e62"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""1cef8fb1-5ba5-4448-9190-e602d55818d7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2827825c-7241-4fa4-87a7-475fc13b3336"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""DeveloperConsole"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""130fbb6d-4d52-4b8c-9d95-0626915b0a46"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9b2dd423-75c8-4f83-88a8-5d14d0c8051e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard And Mouse"",
            ""bindingGroup"": ""Keyboard And Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PS4 Controller"",
            ""bindingGroup"": ""PS4 Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Xbox Controller"",
            ""bindingGroup"": ""Xbox Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MovementHorizontal = m_Player.FindAction("MovementHorizontal", throwIfNotFound: true);
        m_Player_MovementVertical = m_Player.FindAction("MovementVertical", throwIfNotFound: true);
        m_Player_Rotate = m_Player.FindAction("Rotate", throwIfNotFound: true);
        m_Player_Levitate = m_Player.FindAction("Levitate", throwIfNotFound: true);
        m_Player_ScrollWheel = m_Player.FindAction("ScrollWheel", throwIfNotFound: true);
        m_Player_ScrollButton = m_Player.FindAction("ScrollButton", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_DeveloperConsole = m_UI.FindAction("DeveloperConsole", throwIfNotFound: true);
        m_UI_Enter = m_UI.FindAction("Enter", throwIfNotFound: true);
        m_UI_Select = m_UI.FindAction("Select", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MovementHorizontal;
    private readonly InputAction m_Player_MovementVertical;
    private readonly InputAction m_Player_Rotate;
    private readonly InputAction m_Player_Levitate;
    private readonly InputAction m_Player_ScrollWheel;
    private readonly InputAction m_Player_ScrollButton;
    public struct PlayerActions
    {
        private @InputsManager m_Wrapper;
        public PlayerActions(@InputsManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovementHorizontal => m_Wrapper.m_Player_MovementHorizontal;
        public InputAction @MovementVertical => m_Wrapper.m_Player_MovementVertical;
        public InputAction @Rotate => m_Wrapper.m_Player_Rotate;
        public InputAction @Levitate => m_Wrapper.m_Player_Levitate;
        public InputAction @ScrollWheel => m_Wrapper.m_Player_ScrollWheel;
        public InputAction @ScrollButton => m_Wrapper.m_Player_ScrollButton;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MovementHorizontal.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementHorizontal;
                @MovementHorizontal.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementHorizontal;
                @MovementHorizontal.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementHorizontal;
                @MovementVertical.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementVertical;
                @MovementVertical.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementVertical;
                @MovementVertical.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovementVertical;
                @Rotate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Levitate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLevitate;
                @Levitate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLevitate;
                @Levitate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLevitate;
                @ScrollWheel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollWheel;
                @ScrollButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollButton;
                @ScrollButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollButton;
                @ScrollButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnScrollButton;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MovementHorizontal.started += instance.OnMovementHorizontal;
                @MovementHorizontal.performed += instance.OnMovementHorizontal;
                @MovementHorizontal.canceled += instance.OnMovementHorizontal;
                @MovementVertical.started += instance.OnMovementVertical;
                @MovementVertical.performed += instance.OnMovementVertical;
                @MovementVertical.canceled += instance.OnMovementVertical;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Levitate.started += instance.OnLevitate;
                @Levitate.performed += instance.OnLevitate;
                @Levitate.canceled += instance.OnLevitate;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @ScrollButton.started += instance.OnScrollButton;
                @ScrollButton.performed += instance.OnScrollButton;
                @ScrollButton.canceled += instance.OnScrollButton;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_DeveloperConsole;
    private readonly InputAction m_UI_Enter;
    private readonly InputAction m_UI_Select;
    public struct UIActions
    {
        private @InputsManager m_Wrapper;
        public UIActions(@InputsManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @DeveloperConsole => m_Wrapper.m_UI_DeveloperConsole;
        public InputAction @Enter => m_Wrapper.m_UI_Enter;
        public InputAction @Select => m_Wrapper.m_UI_Select;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @DeveloperConsole.started -= m_Wrapper.m_UIActionsCallbackInterface.OnDeveloperConsole;
                @DeveloperConsole.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnDeveloperConsole;
                @DeveloperConsole.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnDeveloperConsole;
                @Enter.started -= m_Wrapper.m_UIActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnEnter;
                @Select.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @DeveloperConsole.started += instance.OnDeveloperConsole;
                @DeveloperConsole.performed += instance.OnDeveloperConsole;
                @DeveloperConsole.canceled += instance.OnDeveloperConsole;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard And Mouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    private int m_PS4ControllerSchemeIndex = -1;
    public InputControlScheme PS4ControllerScheme
    {
        get
        {
            if (m_PS4ControllerSchemeIndex == -1) m_PS4ControllerSchemeIndex = asset.FindControlSchemeIndex("PS4 Controller");
            return asset.controlSchemes[m_PS4ControllerSchemeIndex];
        }
    }
    private int m_XboxControllerSchemeIndex = -1;
    public InputControlScheme XboxControllerScheme
    {
        get
        {
            if (m_XboxControllerSchemeIndex == -1) m_XboxControllerSchemeIndex = asset.FindControlSchemeIndex("Xbox Controller");
            return asset.controlSchemes[m_XboxControllerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovementHorizontal(InputAction.CallbackContext context);
        void OnMovementVertical(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnLevitate(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnScrollButton(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnDeveloperConsole(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
