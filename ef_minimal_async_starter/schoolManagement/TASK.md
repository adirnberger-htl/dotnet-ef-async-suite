<!-- LTeX: language=de-AT -->
//TODO 1 READ THIS FIRST
# Arbeitsauftrag POSE4 - Laptops für Pupils

## Einleitung 
Das bestehende Modell zum Verwalten einer Schule soll erweitert werden, sodaß Lehrer und Laptops verwaltet werden können.

Das Modell wird dazu um folgende Typen erweitert:

| Klasse | Beziehung | Klasse |
| --- | --- | --- |
| Teacher | m:n | Subject (Id, Title) |
| SchoolClass | m:n | Subject |
| Pupil | 0..1:0..1 | SchoolLaptop (Id, SerialNr, MakeAndModel, InstalledSWImage) |
| Teacher | 1:n | SchoolLaptop |

Ein Lehrer (Teacher) kann mehrere Fächer (Subject) unterrichten, und gleiche Fächer können von mehreren Lehrern unterrichtet werden. Eine Klasse (SchoolClass) erhält Unterricht in mehreren Fächern, und gleiche Fächer können in mehreren Klassen unterrichtet werden. Ein Schüler (Pupil) erhält optional genau einen Laptop (SchoolLaptop). Ein Laptop kann einem Schüler zugeordnet sein, muss aber nicht. Jeder Laptop gehört immer zu genau einem Lehrer als Administrator; ein Lehrer kann mehrere Laptops administrieren.

`Subject.Title` sowie `SchoolLaptop.SerialNr`, `SchoolLaptop.MakeAndModel` und `SchoolLaptop.InstalledSWImage` sind verpflichtend als `string` anzugeben.

Folgende Attribute müssen eindeutig sein:

- Die Kombination aus Pupil.{FirstName, LastName, DateOfBirth}
- Alle Bezeichner (Subject.Title, SchoolClass.Name, etc)

Alle Namen dürfen höchstens 50 Zeichen lang sein, alle Bezeichner höchstens 100 Zeichen.

## Aufgabenstellung
Die Namen der Solution und der Projekte, sowie der Testklassen dürfen nicht verändert werden.

### Erweiterung des Modells
Erweitern Sie das Datenmodell, sodaß die beschriebenen Beziehungen modelliert werden. Erstellen Sie die entsprechenden Migrations und aktualisieren Sie die Datenbank.

### Test des Modells
Demonstrieren Sie die Fähigkeiten des Datenmodells, indem Sie die die vorgegebenen Intergrationstests implementieren (Beschreibung als Kommentar bei den Tests). Die Tests müssen nach folgendem Schema programmiert werden:
1. DbContext erzeugen
2. Transaktion erzeugen
3. ggf. Daten vorbereiten
4. Testaktion setzen (z.B. Objekt erstellen und zum DbContext hinzufügen)
5. **await dbContext.SaveChangesAsync();**
6. Entität, die getestet wird, vom DbContext erneut abholen.
7. Gewünschte Eigenschaften der Entität mit `Assert` überprüfen.
8. Wenn bei SaveChanges() eine Exception erwartet wird (meist `DbUpdateException`), dann ist dies mit dem Attribut `ExpectedException(typeof(...))` an der Testmethode zu prüfen.
