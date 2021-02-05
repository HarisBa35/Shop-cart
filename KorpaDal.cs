using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;

namespace Korpa2019
{
    static class KorpaDal
    {
        public static int SacuvajKorpu(Dictionary<int, int> korpa, int KupacId)
        {
            string upit1 = @"INSERT INTO Korpa(KupacId) VALUES(@KupacId);
            SELECT CAST(SCOPE_IDENTITY() AS int)";

            string upit2 = @"INSERT INTO StavkaKorpe VALUES (@KorpaId,  @ProizvodId, @Kolicina)";

            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnProdavnicaDb))
            {
                konekcija.Open();
                using (SqlTransaction transakcija = konekcija.BeginTransaction())
                {
                    int IdKorpe = -1;
                    try
                    {
                        IdKorpe = konekcija.QuerySingleOrDefault<int>(upit1, new {KupacId}, transakcija );
                    }
                    catch (Exception)
                    {

                        transakcija.Rollback();
                        return -1;
                    }
                    try
                    {
                        foreach (var stavka in korpa)
                        {
                            konekcija.Execute(upit2, new
                            {
                                KorpaId = IdKorpe,
                                ProizvodId = stavka.Key,
                                Kolicina = stavka.Value
                            }, transakcija);
                        }
                        transakcija.Commit();
                        return 0;
                    }
                    catch (Exception)
                    {
                        transakcija.Rollback();
                        return -1;
                    }
                }
            }
        }

       
    }
}
