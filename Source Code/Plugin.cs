using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.IO;
using System.Reflection;
using BepInEx.Configuration;

namespace SeventysKunaiMod

{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        public static ConfigEntry<bool> isModEnabled;
        void Start()
        {
            ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "SeventysKunai.cfg"), true);
            isModEnabled = config.Bind<bool>("Config", "Is Mod Active?", true, "Toggle mod here");
        }
        void OnGameInitialized(object sender, EventArgs e)
        {
            if (isModEnabled.Value)
            {
                Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("SeventysKunaiMod.Assets.seventysheirloomwraith");
                AssetBundle bundle = AssetBundle.LoadFromStream(str);
                if (bundle == null)
                {
                    Debug.Log("Failed to load AssetBundle!");
                    return;
                }
                var kunai = bundle.LoadAsset<GameObject>("kunai");

                GameObject lefthand = GameObject.Find("palm.01.R");


                Instantiate(kunai);
                GameObject kunaiInstance = GameObject.Find("kunai(Clone)");
                kunaiInstance.transform.SetParent(lefthand.transform, false);
                kunaiInstance.transform.localScale = new Vector3(4, 4, 4);
                kunaiInstance.transform.localPosition = new Vector3(0.01f, 0.03f, -0.07f);
                kunaiInstance.transform.localEulerAngles = new Vector3(1.1745f, 335.6311f, 272.9784f);
            }


        }

        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
