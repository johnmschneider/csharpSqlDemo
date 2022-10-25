using ADODB;
using jobDemo1.DAC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jobDemo1.Models
{
    /**
     * Data Access Class. Retrieves data from SQL, then stores data in a model
     *  for use by the application.
     */
    public class Album : DACBase
    {
        public static readonly int NO_YEAR_SPECIFIED = -1;

        /**
         * Database Id (i.e. the primary key)
         */
        public int DbId { get; set; }
        public String Name { get; set; }
        public int ReleaseYear { get; set; } = -1;
        public ArrayList TrackIds { get; set; }

        public Album(Connection dbConnection) : base(dbConnection)
        {

        }

        public void ApplyFilterToQuery()
        {
            // whether or not there was a previous filter applied in the query
            bool previousFilterApplied = false;

            SqlQuery = "SELECT * FROM tabAlbums WHERE ";

            // DbId should be guaranteed to be unique
            if (DbId != null)
            {
                SqlQuery += "i32AlbumDbId = " + DbId;
            }
            // Else we may have to filter by each column
            else
            {
                if (Name != null)
                {
                    if (previousFilterApplied)
                    {
                        SqlQuery += "AND ";
                    }

                    SqlQuery += "(strAlbumName = " + Name + ") ";
                    previousFilterApplied = true;
                }

                if (ReleaseYear != NO_YEAR_SPECIFIED)
                {
                    if (previousFilterApplied)
                    {
                        SqlQuery += "AND ";
                    }

                    SqlQuery += "(i32ReleaseYear = " + ReleaseYear + ")";
                    previousFilterApplied = true;
                }

                if (TrackIds != null)
                {
                    if (previousFilterApplied)
                    {
                        SqlQuery += "AND ";
                    }

                    var trackIdsAsCsv = "";

                    foreach (var track in TrackIds)
                    {
                        trackIdsAsCsv += ((string) track) + ",";
                    }

                    SqlQuery += "(TrackIds = " + trackIdsAsCsv + ")";
                    previousFilterApplied = true;
                }
            }
        }

        public override void Retrieve()
        {
            if (mRecordSet.State == ((int) ObjectStateEnum.adStateClosed))
            {
                ApplyFilterToQuery();
                Open();
            }

            DbId = (int) mRecordSet.Fields[0].Value;
            Name = (string) mRecordSet.Fields[1].Value;
            ReleaseYear = (int) mRecordSet.Fields[1].Value;

            TrackIds.AddRange(((string) mRecordSet.Fields[2].Value).Split(","));
        }
    }
}
