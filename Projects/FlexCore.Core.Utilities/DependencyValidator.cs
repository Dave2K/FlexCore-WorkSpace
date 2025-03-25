using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexCore.Core.Utilities
{
    /// <summary>
    /// Classe statica che fornisce metodi per la validazione e la risoluzione delle dipendenze tra progetti.
    /// </summary>
    public static class DependencyValidator
    {
        /// <summary>
        /// Valida le dipendenze tra i progetti per rilevare eventuali dipendenze circolari.
        /// </summary>
        /// <param name="projectDependencies">Un dizionario contenente i progetti come chiavi e le rispettive dipendenze come valori.</param>
        /// <exception cref="InvalidOperationException">Lanciato quando viene rilevata una dipendenza circolare.</exception>


        //public static void ValidateDependencies(Dictionary<string, List<string>> projectDependencies)
        //{
        //    foreach (var project in projectDependencies.Keys)
        //    {
        //        var dependencies = projectDependencies[project];
        //        // Controlla se un progetto dipende da se stesso (dipendenza circolare)
        //        if (dependencies.Contains(project))
        //        {
        //            throw new InvalidOperationException($"Dipendenza circolare rilevata nel progetto: {project}");
        //        }
        //    }
        //}
        public static void ValidateDependencies(Dictionary<string, List<string>> projectDependencies)
        {
            var visited = new HashSet<string>();
            var stack = new HashSet<string>();

            void Visit(string project)
            {
                if (stack.Contains(project))
                    throw new InvalidOperationException($"Dipendenza circolare rilevata: {project}");

                if (!visited.Contains(project))
                {
                    visited.Add(project);
                    stack.Add(project);

                    foreach (var dependency in projectDependencies.GetValueOrDefault(project, new List<string>()))
                    {
                        Visit(dependency);
                    }

                    stack.Remove(project);
                }
            }

            foreach (var project in projectDependencies.Keys)
            {
                Visit(project);
            }
        }


        /// <summary>
        /// Risolve le dipendenze circolari tra i progetti, se presenti.
        /// </summary>
        /// <param name="projectDependencies">Un dizionario contenente i progetti come chiavi e le rispettive dipendenze come valori.</param>
        public static void ResolveCircularDependencies(Dictionary<string, List<string>> projectDependencies)
        {
            var resolved = new HashSet<string>(); // Per tenere traccia dei progetti risolti
            foreach (var project in projectDependencies.Keys)
            {
                // Se il progetto non è ancora stato risolto, risolvi le sue dipendenze
                if (!resolved.Contains(project))
                {
                    ResolveDependencies(project, projectDependencies, resolved, new HashSet<string>());
                }
            }
        }

        /// <summary>
        /// Risolve ricorsivamente le dipendenze di un singolo progetto.
        /// </summary>
        /// <param name="project">Il progetto per il quale risolvere le dipendenze.</param>
        /// <param name="projectDependencies">Un dizionario contenente i progetti come chiavi e le rispettive dipendenze come valori.</param>
        /// <param name="resolved">Un insieme che tiene traccia dei progetti già risolti.</param>
        /// <param name="visiting">Un insieme che tiene traccia dei progetti attualmente in fase di visita per evitare dipendenze circolari.</param>
        /// <exception cref="InvalidOperationException">Lanciato quando viene rilevata una dipendenza circolare durante la risoluzione delle dipendenze.</exception>
        private static void ResolveDependencies(string project, Dictionary<string, List<string>> projectDependencies, HashSet<string> resolved, HashSet<string> visiting)
        {
            visiting.Add(project); // Aggiungi il progetto all'elenco dei progetti in visita
            foreach (var dependency in projectDependencies[project])
            {
                // Se una dipendenza non è ancora stata risolta
                if (!resolved.Contains(dependency))
                {
                    // Se la dipendenza è già in visita, significa che c'è una dipendenza circolare
                    if (visiting.Contains(dependency))
                    {
                        throw new InvalidOperationException($"Dipendenza circolare rilevata: {project} -> {dependency}");
                    }
                    // Risolvi ricorsivamente la dipendenza
                    ResolveDependencies(dependency, projectDependencies, resolved, visiting);
                }
            }
            resolved.Add(project); // Aggiungi il progetto ai risolti
            visiting.Remove(project); // Rimuovi il progetto dall'elenco dei progetti in visita
        }
    }
}
