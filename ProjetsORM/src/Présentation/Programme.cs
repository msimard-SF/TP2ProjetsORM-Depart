using Microsoft.EntityFrameworkCore;
using ProjetsORM.Persistence;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetsORM.Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjetsORMContexte>();
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["bdProjetsORMConnectionString"].ConnectionString);
            ProjetsORMContexte contexte = new ProjetsORMContexte(optionsBuilder.Options);

            //Instanciation des repositories
            EFClientRepository clientRepo = new EFClientRepository(contexte);
            EFAffTravailRepository affTravailRepo = new EFAffTravailRepository(contexte);
            EFEmployeRepository employeRepo = new EFEmployeRepository(contexte);
            EFProjetRepository projetRepo = new EFProjetRepository(contexte);
            EFDepartementRepository departementRepo = new EFDepartementRepository(contexte);


            Console.WriteLine("Démarrage...");

            if (contexte.Employes.Count() == 0)
            {
                //Mettre en place le code nécessaire pour exécuter la procédure stockée PR_PREMIER_CHARGEMENT_PROJETS_ORM
            }

            Console.ReadKey();

        }
    }
}
