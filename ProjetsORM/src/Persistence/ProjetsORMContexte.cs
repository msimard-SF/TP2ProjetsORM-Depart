using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjetsORM.Persistence
{
    public class ProjetsORMContexte : DbContext
    {
        public ProjetsORMContexte(DbContextOptions<ProjetsORMContexte> options)
              : base(options)
            {
                //Assurez-vous que la BD soit supprimée, puis recréée à chaque exécution...
            }

        //Définir les BDSet


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Définir les clés uniques, les clés primaires composées, etc.


            //****  Exemple pour la définition de certaines clés étrangères pouvant causer
            //****  une erreur de référence circulaire:

            //Projet
            //FK NoGestionnaire 
            builder.Entity<Projet>()
            .HasOne(e => e.Gestionnaire)
            .WithMany(e => e.ProjetsGestionnaire)
            .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
