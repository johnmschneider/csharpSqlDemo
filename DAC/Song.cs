using ADODB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jobDemo1.DAC
{
    public class Song : DACBase
    {
        public static readonly int NO_ALBUM_SPECIFIED = -1;
        public static readonly int NO_LENGTH_SPECIFIED = -1;

        /**
         * Database Id (i.e. the primary key)
         */
        public int DbId { get; set; }
        
        /**
         * The DbId for the album this song is found on
         */
        public int AlbumDbId { get; set; }

        public string Name { get; set; }
        public int LengthInSeconds { get; set; }

        public Song(Connection dbConnection) : base(dbConnection)
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
                if (AlbumDbId != NO_ALBUM_SPECIFIED)
                {
                    if (previousFilterApplied)
                    {
                        SqlQuery += "AND ";
                    }

                    SqlQuery += "(i32AlbumDbId = " + AlbumDbId + ")";
                    previousFilterApplied = true;
                }

                if (Name != null)
                {
                    if (previousFilterApplied)
                    {
                        SqlQuery += "AND ";
                    }

                    SqlQuery += "(strAlbumName = " + Name + ") ";
                    previousFilterApplied = true;
                }

                if (LengthInSeconds != NO_LENGTH_SPECIFIED)
                {
                    if (previousFilterApplied)
                    {
                        SqlQuery += "AND ";
                    }

                    SqlQuery += "(i32LengthInSeconds = " + LengthInSeconds + ")";
                    previousFilterApplied = true;
                }
            }
        }

        public override void Retrieve()
        {
            if (mRecordSet.State == ((int)ObjectStateEnum.adStateClosed))
            {
                ApplyFilterToQuery();
                Open();
            }

            DbId = (int) mRecordSet.Fields[0].Value;
            AlbumDbId = (int) mRecordSet.Fields[1].Value;
            Name = (string) mRecordSet.Fields[2].Value;
            LengthInSeconds = (int) mRecordSet.Fields[3].Value;
        }
    }
}
