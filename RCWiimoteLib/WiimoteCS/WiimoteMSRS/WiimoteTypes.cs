//------------------------------------------------------------------------------
// WiimoteTypes.cs
//
//     This code was generated by the DssNewService tool.
//
//------------------------------------------------------------------------------
using Microsoft.Ccr.Core;
using Microsoft.Dss.Core.Attributes;
using Microsoft.Dss.ServiceModel.Dssp;
using System;
using System.Collections.Generic;
using W3C.Soap;
using wiimotelib = WiimoteLib;
using System.ComponentModel;

namespace WiimoteLib
{
    
    /// <summary>
    /// Wiimote Contract
    /// </summary>
    public sealed class Contract
    {
        /// The Unique Contract Identifier for the Wiimote service
        public const String Identifier = "http://schemas.tempuri.org/2007/06/wiimote.html";
    }

    /// <summary>
    /// Wiimote Operations
    /// </summary>
    public class WiimoteOperations : PortSet<DsspDefaultLookup, DsspDefaultDrop, Get, Subscribe, WiimoteChanged, SetLEDs, SetRumble>
    {
    }
    /// <summary>
    /// Get
    /// </summary>
    public class Get : Get<GetRequestType, PortSet<WiimoteState, Fault>>
    {
    }

    [DisplayName("Subscribe")]
    [Description("Subscribe to Wiimote service notifications.")]
    public class Subscribe : Subscribe<SubscribeRequestType, PortSet<SubscribeResponseType, Fault>>
    {
        /// <summary>
        /// Subscribe to Wiimote
        /// </summary>
        public Subscribe() { this.Body = new SubscribeRequestType(); }
    }

    public class WiimoteChanged : Update<WiimoteState, DsspResponsePort<DefaultUpdateResponseType>>
    {
        /// <summary>
        /// Indicates an update to the Wiimote sensors
        /// </summary>
        public WiimoteChanged()
        {
            if (this.Body == null)
                this.Body = new WiimoteState();
        }

        /// <summary>
        /// Indicates an update to the Wiimote sensors
        /// </summary>
        /// <param name="state"></param>
        public WiimoteChanged(WiimoteState state)
        {
            this.Body = state ?? new WiimoteState();
        }
    }

    [DisplayName("Set Wiimote Leds")]
    public class SetLEDs : Update<LEDState, DsspResponsePort<DefaultUpdateResponseType>>
    {
    }

    [DisplayName("Set Wiimote Rumble")]
    public class SetRumble : Update<RumbleRequest, DsspResponsePort<DefaultUpdateResponseType>>
    {
    }
}
