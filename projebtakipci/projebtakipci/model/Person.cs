using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;
using Firebase.Database.Query;

namespace projebtakipci.model
{
    class Person
    {
        public int kullanici_id { get; set; }
        public string kullanici_sifresi { get; set; }
        public string kullanici_email { get; set; }
        public DateTime kullanici_date { get; set; }

    }
}
