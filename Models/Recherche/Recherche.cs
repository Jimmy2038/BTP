using BTP.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BTP.Models.Recherche
{
    public class Recherche
    {
        readonly BtpDBContext _context;

        public Recherche(BtpDBContext context)
        {
            _context = context;
        }

        public IQueryable<Authentif> SearchSimple(string searchTerm)
        {
            return _context.auth.Where(p => p.id.Contains(searchTerm));
        }

        public IQueryable<Authentif> SearchMultiCriteria(string idSearchTerm, string idSalleSearchTerm)
        {
            return _context.auth
                           .Where(p => p.id.Contains(idSearchTerm) && p.id.Contains(idSalleSearchTerm));
        }

        public IQueryable<Authentif> SearchMultiWord(string searchTerms)
        {
            var words = searchTerms.Split(' ');
            return _context.auth
                           .Where(p => words.All(word => p.id.Contains(word)));
        }
        //mande fa commentena fotsiny
       /* public IQueryable<Authentif> SearchFullText(string searchTerm)
        {
            var words = searchTerm.Split(' ');

            // Initialiser une nouvelle instance de DbSet pour construire la requête
            var query = _context.auth.AsQueryable();

            // Parcourir chaque mot de recherche et ajouter des conditions à la requête
            var accumulatedResults = new List<Authentif>();
            foreach (var word in words)
            {
                // Convertir le mot de recherche en minuscules
                var lowerWord = word.ToLower();

                // Utiliser LOWER() pour convertir les colonnes en minuscules et comparer avec le mot de recherche
                var tempQuery = query.Where(p => EF.Functions.Like(p.id.ToLower(), $"%{lowerWord}%")
                                                 || EF.Functions.Like(p.id_salle.ToLower(), $"%{lowerWord}%")
                                                 || EF.Functions.Like(p.rangee.ToLower(), $"%{lowerWord}%")
                                                 || EF.Functions.Like(p.num_auth.ToString().ToLower(), $"%{lowerWord}%"));
                accumulatedResults.AddRange(tempQuery.ToList());

            }
            var finalQuery = accumulatedResults.AsQueryable();
            return finalQuery;
        }*/

        //tsy mande
       /* public IQueryable<Authentif> SearchFullText(string searchTerm)
        {
            var searchTerms = searchTerm.Split(' ');

            var Authentifs = _context.auth
                       .AsEnumerable() // Convertit IQueryable en IEnumerable pour évaluation côté client
                       .Where(auth => searchTerms.All(term => auth.id.Contains(term)
                        || auth.id_salle.Contains(term)
                        || auth.rangee.Contains(term)
                        || auth.num_auth.ToString().Contains(term)
                        ));
            return auths;
        }*/


    }
}
