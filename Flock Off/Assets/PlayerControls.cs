// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""57759c1d-9a7d-400c-a795-355f4a71a020"",
            ""actions"": [
                {
                    ""name"": ""Shout"",
                    ""type"": ""Button"",
                    ""id"": ""618c4646-7c1a-4e7c-9618-14685cc5b4ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""22b4ed98-190d-4b54-8780-c1c27907d822"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveN"",
                    ""type"": ""Button"",
                    ""id"": ""818e9385-3839-44b3-ad7f-626ca08d40c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveS"",
                    ""type"": ""Button"",
                    ""id"": ""d7c252e9-410e-4644-93d2-f866360a3c98"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveE"",
                    ""type"": ""Button"",
                    ""id"": ""e6f1cd3a-438c-4ee7-a96c-97049a72cf4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveW"",
                    ""type"": ""Button"",
                    ""id"": ""15ac70bc-309d-4d30-b47f-9bacde08c55f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8780747a-5e40-45b1-9b7b-7625e2c9de7d"",
                    ""path"": ""<HID::MY-POWER CO.,LTD. 2In1 USB Joystick>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shout"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d76644f-3c4a-4203-aa64-52bdd177c019"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shout"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6f60b889-65af-4b2d-aa0f-d6586725e080"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shout"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e4fcb78-052b-460d-9432-e8297fdf50be"",
                    ""path"": ""<Joystick>/stick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d56dc14c-3c11-49ff-988c-140a1f667b6e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83dbab38-7296-4369-b9ca-02211d5f3c3d"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveN"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7fa211b-7702-4591-b69a-82da73946176"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveS"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a295d9c-c4cc-4e8e-b6cd-7397d624501a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveE"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98267860-0930-4ba4-8000-17d5d0a3d662"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveW"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Shout = m_Gameplay.FindAction("Shout", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_MoveN = m_Gameplay.FindAction("MoveN", throwIfNotFound: true);
        m_Gameplay_MoveS = m_Gameplay.FindAction("MoveS", throwIfNotFound: true);
        m_Gameplay_MoveE = m_Gameplay.FindAction("MoveE", throwIfNotFound: true);
        m_Gameplay_MoveW = m_Gameplay.FindAction("MoveW", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Shout;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_MoveN;
    private readonly InputAction m_Gameplay_MoveS;
    private readonly InputAction m_Gameplay_MoveE;
    private readonly InputAction m_Gameplay_MoveW;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shout => m_Wrapper.m_Gameplay_Shout;
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @MoveN => m_Wrapper.m_Gameplay_MoveN;
        public InputAction @MoveS => m_Wrapper.m_Gameplay_MoveS;
        public InputAction @MoveE => m_Wrapper.m_Gameplay_MoveE;
        public InputAction @MoveW => m_Wrapper.m_Gameplay_MoveW;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Shout.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShout;
                @Shout.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShout;
                @Shout.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShout;
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @MoveN.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveN;
                @MoveN.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveN;
                @MoveN.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveN;
                @MoveS.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveS;
                @MoveS.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveS;
                @MoveS.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveS;
                @MoveE.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveE;
                @MoveE.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveE;
                @MoveE.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveE;
                @MoveW.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveW;
                @MoveW.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveW;
                @MoveW.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveW;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shout.started += instance.OnShout;
                @Shout.performed += instance.OnShout;
                @Shout.canceled += instance.OnShout;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MoveN.started += instance.OnMoveN;
                @MoveN.performed += instance.OnMoveN;
                @MoveN.canceled += instance.OnMoveN;
                @MoveS.started += instance.OnMoveS;
                @MoveS.performed += instance.OnMoveS;
                @MoveS.canceled += instance.OnMoveS;
                @MoveE.started += instance.OnMoveE;
                @MoveE.performed += instance.OnMoveE;
                @MoveE.canceled += instance.OnMoveE;
                @MoveW.started += instance.OnMoveW;
                @MoveW.performed += instance.OnMoveW;
                @MoveW.canceled += instance.OnMoveW;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnShout(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnMoveN(InputAction.CallbackContext context);
        void OnMoveS(InputAction.CallbackContext context);
        void OnMoveE(InputAction.CallbackContext context);
        void OnMoveW(InputAction.CallbackContext context);
    }
}
