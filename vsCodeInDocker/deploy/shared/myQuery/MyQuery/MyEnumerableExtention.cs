using System;
using System.Collections.Generic;

namespace MyQuery
{
    public static class MyEnumerableExtensions
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            foreach (T item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            foreach (T item in source)
            {
                yield return selector(item);
            }
        }

        public static double Sum<T>(this IEnumerable<T> source, Func<T, double> transform)
        {
            // TODO: 1. Null-Checks (source und transform)
            
            // TODO: 2. Variable für die Summe anlegen (z.B. double sum = 0.0;)
            
            // TODO: 3. foreach-Schleife über 'source'
            
            // TODO: 4. In der Schleife: transform(item) aufrufen und das Ergebnis auf die Summe addieren
            
            // TODO: 5. Die Summe mit 'return' zurückgeben (KEIN yield return!)
            
            return 0; // Platzhalter, entfernen sobald du deinen Code einfügst
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            // TODO: Null-Checks (source und action)
            
            // TODO: Schleife über 'source'
            // In der Schleife einfach 'action(item);' aufrufen.
            // Hinweis: void heißt, es gibt KEIN return!
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            // TODO: Null-Checks
            
            // TODO: Variable für den Index anlegen (z.B. int index = 0;)
            
            // TODO: Schleife über 'source'. 
            // In der Schleife 'action(item, index);' aufrufen und danach den Index um 1 erhöhen.
        }

        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source)
        {
            // TODO: Null-Check (hier nur für 'source', da es keinen zweiten Parameter gibt)
            
            // TODO: Ein HashSet<T> anlegen (z.B. var seen = new HashSet<T>();)
            
            // TODO: foreach-Schleife über 'source'
            // Wenn das HashSet das Element noch nicht enthält -> ins HashSet einfügen UND per yield return zurückgeben!
            
            yield break; // Platzhalter, entfernen sobald du deinen Code einfügst
        }
    }
}