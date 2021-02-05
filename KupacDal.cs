using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace Korpa2019
{
    static class KupacDal
    {
        public static List<Kupac> VratiKupce()
        {
            string upit = "SELECT * FROM Kupac";
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnProdavnicaDb))
            {
                try
                {
                    IEnumerable<Kupac> listaKupaca = konekcija.Query<Kupac>(upit);
                    return listaKupaca.ToList();
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }
    }
}
