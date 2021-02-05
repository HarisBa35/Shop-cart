using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace Korpa2019
{
    class ProizvodDal
    {
        public static List<Proizvod> VratiProizvode()
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnProdavnicaDb))
            {
                try
                {
                    IEnumerable<Proizvod> listaProizvoda = konekcija.Query<Proizvod>("SELECT * FROM Proizvod");
                    return listaProizvoda.ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            }


        }
    }
}
