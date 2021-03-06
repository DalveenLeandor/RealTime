﻿// <copyright file="MethodPatcher.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace RealTime.Patching
{
    using System;
    using Harmony;
    using RealTime.Tools;

    /// <summary>
    /// A class that uses Harmony library for redirecting the game's methods.
    /// </summary>
    internal sealed class MethodPatcher
    {
        private const string HarmonyId = "com.cities_skylines.dymanoid.realtime";

        private readonly Patcher patcher;
        private readonly IPatch[] patches;

        /// <summary>Initializes a new instance of the <see cref="MethodPatcher"/> class.</summary>
        /// <param name="patches">The patches to process by this object.</param>
        /// <exception cref="ArgumentException">Thrown when no patches specified.</exception>
        public MethodPatcher(params IPatch[] patches)
        {
            if (patches == null || patches.Length == 0)
            {
                throw new ArgumentException("At least one patch is required");
            }

            this.patches = patches;
            var harmony = HarmonyInstance.Create(HarmonyId);
            patcher = new Patcher(harmony);
        }

        /// <summary>Applies all patches this object knows about.</summary>
        public void Apply()
        {
            try
            {
                Revert();
            }
            catch (Exception ex)
            {
                Log.Warning("The 'Real Time' mod failed to clean up methods before patching: " + ex);
            }

            foreach (IPatch patch in patches)
            {
                patch.ApplyPatch(patcher);
            }
        }

        /// <summary>Reverts all patches, if any applied.</summary>
        public void Revert()
        {
            foreach (IPatch patch in patches)
            {
                patch.RevertPatch(patcher);
            }
        }
    }
}
