//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/Input/TouchControlls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @TouchControlls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TouchControlls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchControlls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c12fb245-4e05-4eed-9dcd-bb5c1393d81c"",
            ""actions"": [
                {
                    ""name"": ""Flip"",
                    ""type"": ""Button"",
                    ""id"": ""fd50d90d-1f97-4b26-aac3-8b11e3c68e80"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a3ac25af-fa9f-494d-86d2-7e12f7b3a057"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": ""MultiTap(tapDelay=0.2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Flip = m_Player.FindAction("Flip", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Flip;
    public struct PlayerActions
    {
        private @TouchControlls m_Wrapper;
        public PlayerActions(@TouchControlls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Flip => m_Wrapper.m_Player_Flip;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Flip.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFlip;
                @Flip.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFlip;
                @Flip.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFlip;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Flip.started += instance.OnFlip;
                @Flip.performed += instance.OnFlip;
                @Flip.canceled += instance.OnFlip;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnFlip(InputAction.CallbackContext context);
    }
}
