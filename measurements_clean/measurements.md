c# Programmieren (c) HTL-Leonding

# Sensoren im Feld

## Lehrziele

* Linq to Object
* Navigation Properties
* Wiederholung: Projektion, Join, GroupBy.

## Aufgabenstellung

In einem Iot Projekt werden Messdaten gesammelt und in einer Datenbank abgespeichert. Von dieser Datenbank sind Csv Exportdateien vorhanden:

* **SensorType.csv**  
  Kategorisierung der Sensoren.  
* **sensor.csv**  
  Liste aller im System vorhandenen Sensoren.  
  Über die Spalte `SensorTypeId` wird eine Beziehung zu den Sensor-Kategorien aufgebaut.  
* **MeasurementType.csv**  
  Liste aller Messwert-Kategorieen.  
  Der Name (z.B. "Temperature") ist in dieser Liste hinterlegt und wird beim Messwert nicht gespeichert.  
* **Measurement.csv**  
  Liste aller (historischen) Messwerte.  
  Messwerte werden über die Spalte `SensorId` einem Sensor, über `MeasurementTypeId` einer Messwertkategorie (=Name) zugeordnet.

**Task 1**

Erstellen Sie eine Solution mit Projekten für eine datenzentrierte Anwendung, wie wir es besprochen haben.
Vergessen Sie nicht auf das Unit Test Projekt!

**Task 2**

Erstellen Sie ein passendes objektorientiertes Modell, das die Daten aus den CSV-Files repräsentiert.
Legen Sie die Klassen in einen Ordner / Namespace `Entities` im `ApplicationDataContext` Projekt ab.

* Die Daten haben **Beziehungen** zueinander, diese sind zu modellieren.
* Beachten Sie die Richtung der Navigation und die Vielfachheiten!
* Führen Sie Basisklassen ein, wo es sinnvoll ist.
* Verwenden Sie das passende Interface (`IList` oder `ICollection`), nicht die implementierende Klasse. Im ctor wird die Instanz dann gebunden.
* Erstellen Sie ein PUML Diagramm.

Hinweis: Wer hier KI verwendet, hat schon verloren...das war der ganze Sinn dieser Übung. Der Rest ist tippen.

**Task 3**

Lesen Sie die Csv Dateien ein und erstellen Sie das Objektmodell der Daten. 

* Benutzen Sie kompakte Linq-Ausdrücke zum Verarbeiten der Daten.
* Überlegen Sie, in welcher Reihenfolge die Daten gelesen werden müssen (die Blätter zuerst...)
* Befüllen Sie die `ISet` Member im `ApplicationDataContext` mit den Klassen aus dem Objektmodell.
* Überprüfen Sie mit Unit Tests, dass die Beziehungen im Objektmodell korrekt aus Test-CSV Daten gebildet werden (Tipp: Erstellen Sie eine CSV-Einlese-Methode, die anstelle der Dateinamen schon `string[]` nimmt.)


**Task 4**  

Legen Sie eine Klasse `ApplicationDataContextQueries.cs` an, die eine `partial class` zum `ApplicationDataContext` ist.


Erstellen Sie Methoden für folgende Auswertungen und je einen zugehörigen Unit Test!

* Übersicht über alle Sensoren mit Anzahl der Messwerte.
* Übersicht über alle Messwertkategorien mit Anzahl der Messwerte.
* Übersicht über alle Messwerte aufgeteilt nach 'Sensoren' und 'Messwerttypen'.  
  Die Liste enthält die Anzahl der Messwerte sowie den minimum un den maximum Wert.  
  Sortieren Sie die Liste nach Sensoren und Messwerttypen.  

  **Unit Tests**

  Lassen Sie sich aus den gegebenen Dateien mittels KI verkürzte Versionen generieren, deren Ergebnisse dann in den Unit Tests abgefragt werden.


## Beispielhafte Ergebnisse (Bildschirmausgabe)

```
List of Sensors
Id Name              Count
==========================
 1 tasmota_PC         7533
 2 tasmota_WZ         7529
 3 tasmota_Surface    7479
 4 tasmota_Server     6701
 6 tasmota_SW1           0
 7 tasmota_SW2           0
 8 tasmota_SW3           0
 9 tasmota_Temp       1177

List of Measurement-Types
Id Name              Count
==========================
 1 Total              4626
 2 Today              4626
 3 Period             4626
 4 Power              4626
 5 ApparentPower      1528
 6 ReactivePower      1528
 7 Factor             4626
 8 Voltage            1528
 9 Current            1528
10 Temperature         178
11 Humidity            178
12 DewPoint            178
13 Pressure            178
14 eCO2                155
15 TVOC                155
16 aHumidity           155

List of Sensor by Measurement-Types
Sensor         Measurement        Count         Min        Max
==============================================================
tasmota_PC      ApparentPower       407           0        175
tasmota_PC      Current             407           0      0,749
tasmota_PC      Factor             1181           0          1
tasmota_PC      Period             1181           0          8
tasmota_PC      Power              1181           0        130
tasmota_PC      ReactivePower       407           0        117
tasmota_PC      Today              1181           0      0,568
tasmota_PC      Total              1181       0,131      0,826
tasmota_PC      Voltage             407         227        236
tasmota_Server  ApparentPower       314          93        180
tasmota_Server  Current             314       0,397      0,775
tasmota_Server  Factor             1089        0,39       0,85
tasmota_Server  Period             1089           0          7
tasmota_Server  Power              1089          48        119
tasmota_Server  ReactivePower       314          
...

```
