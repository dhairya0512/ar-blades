using UnityEngine;
using UnityEngine.XR.Management;

using System.IO;
using System.Linq;

namespace UnityEditor.XR.ARKit
{
    /// <summary>
    /// Holds settings that are used to configure the ARKit XR Plugin.
    /// </summary>
    [System.Serializable]
    [XRConfigurationData("ARKit", "UnityEditor.XR.ARKit.ARKitSettings")]
    public class ARKitSettings : ScriptableObject
    {
        /// <summary>
        /// Enum which defines whether ARKit is optional or required.
        /// </summary>
        public enum Requirement
        {
            /// <summary>
            /// ARKit is required, which means the app cannot be installed on devices that do not support ARKit.
            /// </summary>
            Required,

            /// <summary>
            /// ARKit is optional, which means the the app can be installed on devices that do not support ARKit.
            /// </summary>
            Optional
        }

        [SerializeField, Tooltip("Toggles whether ARKit is required for this app. Will make app only downloadable by devices with ARKit support if set to 'Required'.")]
        Requirement m_Requirement;

        /// <summary>
        /// Determines whether ARKit is required for this app: will make app only downloadable by devices with ARKit support if set to <see cref="Requirement.Required"/>.
        /// </summary>
        public Requirement requirement
        {
            get { return m_Requirement; }
            set { m_Requirement = value; }
        }

        /// <summary>
        /// Gets the currently selected settings, or create a default one if no <see cref="ARKitSettings"/> has been set in Player Settings.
        /// </summary>
        /// <returns>The ARKit settings to use for the current Player build.</returns>
        public static ARKitSettings GetOrCreateSettings()
        {
            var settings = currentSettings;
            if (settings != null)
                return settings;

            return CreateInstance<ARKitSettings>();
        }

        /// <summary>
        /// Get or set the <see cref="ARKitSettings"/> that will be used for the player build.
        /// </summary>
        public static ARKitSettings currentSettings
        {
            get => EditorBuildSettings.TryGetConfigObject(k_SettingsKey, out ARKitSettings settings) ? settings : null;

            set
            {
                if (value == null)
                {
                    EditorBuildSettings.RemoveConfigObject(k_SettingsKey);
                }
                else
                {
                    EditorBuildSettings.AddConfigObject(k_SettingsKey, value, true);
                }
            }
        }

        internal static bool TrySelect()
        {
            var settings = currentSettings;
            if (settings == null)
                return false;

            Selection.activeObject = settings;
            return true;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

        const string k_SettingsKey = "UnityEditor.XR.ARKit.ARKitSettings";
        const string k_OldConfigObjectName = "com.unity.xr.arkit.PlayerSettings";

    }
}
