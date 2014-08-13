using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Syncbak.SyncboxSetup.Models;
using Syncbak.SyncboxSetup.Data;
using System.Data.SqlClient;
using Syncbak.SyncboxSetup.Data.Extensions;

namespace Syncbak.SyncboxSetup.Repositories
{
    public class StationRepository : BaseDataMapper
    {
        public StationRepository() : base("Syncbak") { }

        public IEnumerable<Station> GetStation(int? id, string callsign, bool? streamingOnly)
        {
            var result = new List<Station>();

            var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@stationID", id.HasValue ? (object)id.Value : DBNull.Value),
                    new SqlParameter("@callsign", !string.IsNullOrEmpty(callsign) ? (object)callsign : DBNull.Value),
                    new SqlParameter("@ReturnStreamingOnly", streamingOnly.HasValue ? (object)streamingOnly.Value : DBNull.Value)
                };

            ExecuteDataReader("syncbak.StationGet", parameters, reader =>
            {
                while (reader.Read())
                {
                    result.Add(ReadStationRecord(reader));
                }
            });

            result.Sort((x, y) => String.Compare(x.CallSign, y.CallSign));
            return result;
        }

        private Station ReadStationRecord(System.Data.IDataReader reader)
        {
            return new Station
            {
                ID = reader.ReadInt("StationID"),
                CallSign = reader.ReadString("CallSign"),
                StationName = reader.ReadString("Name"),
                DMAID = reader.ReadInt("DMAID"),
                DMAName = reader.ReadString("DMAName"),
                StationOwnerID = reader.ParseValue<int?>("StationOwnerID", false),
                StationOwner = reader.ReadString("StationOwner"),
                NetworkAffiliation = reader.ReadString("Affiliate"),
                ShortCallSign = reader.ReadString("ShortCallSign"),

                GMTOffset = reader.ReadInt("GMTOffset"),
                ObservesDLS = reader.ReadBool("ObservesDaylightSavings"),
                BroadcastDayOffsetMinutes = reader.ReadInt("BroadcastDayOffsetMinutes"),

                City = reader.ReadString("City"),
                State = reader.ReadString("State"),

                ImportStationID = reader.ParseValue<int?>("ImportStationID"),
                LanguageID = reader.ReadInt("LanguageID"),

                //MediaCaptureEnabled = reader.ReadBool("IsMediaCaptureEnabled", false, false),
                WowzaServerID = reader.ParseValue<int?>("WowzaServerID", false),
                StreamFormatTypeID = reader.ReadInt("StreamFormatTypeID"),
                StreamFormatType = reader.ReadString("FormatKey"),
                MediaCaptureDefaultRequiresReview = reader.ReadBool("DefaultRequiresReview", false, false),
                MediaCaptureDefaultExpirationMinutes = reader.ParseValue<int?>("DefaultMediaCaptureAgeMinutes", false),
                DefaultSourceStream = reader.ReadString("DefaultSourceStream"),
                DefaultAlternateStream = reader.ReadString("DefaultAlternateStream"),
                DefaultOffAirStream = reader.ReadString("DefaultOffAirStream"),
                DefaultTargetStream = reader.ReadString("DefaultTargetStream"),
                IsSyncbakEnabled = reader.ReadBool("IsSyncbakEnabled"),
                IsPrimary = reader.ReadBool("IsPrimary"),
                IsClosedCaptionEnabled = reader.ReadBool("IsClosedCaptionEnabled"),
                StreaduleAutoFill = reader.ReadBool("StreaduleAutoFill"),
                VirtualStreamSourceStationID = reader.ParseValue<int?>("VirtualStreamSourceStationID", reader.ReadInt("StationID"), false),
                MediaID = reader.ReadInt("MediaID")
            };
        }
    }
}