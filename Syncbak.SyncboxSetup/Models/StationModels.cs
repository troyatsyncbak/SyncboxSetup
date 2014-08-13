using System;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Syncbak.SyncboxSetup.Models
{
    public enum AdNetworkTypes
    {
        [Description("None - Ads Disabled")]
        None = 0,
        YuMe = 2
    }

    public class Station
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string CallSign { get; set; }

        public string StationName { get; set; }

        public string NetworkAffiliation { get; set; }

        [DisplayName("DMA #")]
        public int? DMAID { get; set; }

        [DisplayName("DMA Name")]
        public string DMAName { get; set; }

        public int? StationOwnerID { get; set; }

        [DisplayName("Station Owner")]
        public string StationOwner { get; set; }

        [DisplayName("Base GMT Offset")]
        public int GMTOffset { get; set; }

        [DisplayName("Observes Daylight Savings")]
        public bool ObservesDLS { get; set; }

        [DisplayName("Wowza Server")]
        public string ServerURL { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        [DisplayName("Offset of Start of Broadcast Day, in Minutes")]
        public int BroadcastDayOffsetMinutes { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        [DisplayName("Is Primary")]
        public bool IsPrimary { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public AdNetworkTypes? AdNetwork { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string AdNetworkKey { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public bool MediaCaptureDefaultRequiresReview { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public int? MediaCaptureDefaultExpirationMinutes { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public int? WowzaServerID { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public int StreamFormatTypeID { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string StreamFormatType { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string DefaultSourceStream { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string DefaultAlternateStream { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string DefaultOffAirStream { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string DefaultTargetStream { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public bool IsSyncbakEnabled { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public bool IsClosedCaptionEnabled { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string City { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string State { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public int? ImportStationID { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public int LanguageID { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string ShortCallSign { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public bool StreaduleAutoFill { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public int? MediaID { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public int? VirtualStreamSourceStationID { get; set; }
    }

}