using ADODB;
using jobDemo1.DAC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jobDemo1.Models
{
    public class Artist : DACBase
    {
        public int DbId { get; set; }
        public String Name { get; set; }
        public ArrayList AlbumIds { get; set; }

        public Artist(Connection dbConnection) : base(dbConnection)
        {

        }

        public void ApplyFilterToQuery()
        {
            // whether or not there was a previous filter applied in the query
            bool previousFilterApplied = false;

            SqlQuery = "SELECT * FROM tabArtists WHERE ";

            // DbId should be guaranteed to be unique
            if (DbId != null)
            {
                SqlQuery += "i32ArtistDbId = " + DbId;
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

                    SqlQuery += "(strArtistName = " + Name + ") ";
                    previousFilterApplied = true;
                }
                if (AlbumIds != null)
                {
                    if (previousFilterApplied)
                    {
                        SqlQuery += "AND ";
                    }

                    var albumsAsCsv = "";

                    foreach (var album in AlbumIds)
                    {
                        albumsAsCsv += ((string) album) + ",";
                    }

                    SqlQuery += "(AlbumIds = " + albumsAsCsv + ")";
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

            DbId = (int)mRecordSet.Fields[0].Value;
            Name = (string) mRecordSet.Fields[1].Value;
            AlbumIds.AddRange(((string)mRecordSet.Fields[2].Value).Split(","));
        }
    }
}
