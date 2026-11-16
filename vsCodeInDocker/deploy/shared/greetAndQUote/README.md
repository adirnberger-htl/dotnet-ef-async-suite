# Greet And Quote

This is a tool that creates a greeting and a quote for a user.

## Requirements

Consider the following requirements for a small command-line program.

* Purpose: output a greeting phrase and a quote, for example:

  ```text
  Hello, John! It's so nice to see you again.
  Guten Tag, Maria! Ich hoffe, Sie haben einen wunderbaren Tag.
  ```

* Input on the command line:

  * `-n` Name to use in greeting
  * `-i` / `-f` Greeting is **informal** or **formal**
  * `-l` Language code according to ISO 639

* Platform: .NET

* Quotes are stored in `quotes-<languageCode>.csv`, e.g. `quotes-it.csv` for Italian quotes.

### Details for different languages

As a proof of concept, the program shall support German (de), English (en) and Italian (it). The following table summarizes the language specific rules.

| Aspect             | English                       | German                          | Italian                                                                             |
| ------------------ | ----------------------------- | ------------------------------- | ----------------------------------------------------------------------------------- |
| Informal greeting  | `Hi John!`                    | `Hallo John!`                   | `Ciao John!`                                                                        |
| Formal greeting    | `Hello, Mr Smith.`            | `Guten Tag, Herr Smith.`        | `Buongiorno, Signor Smith.`                                                         |
| Morning            | `Good morning, John!`         | `Guten Morgen, John!`           | `Buongiorno, John!`                                                                 |
| Afternoon          | `Good afternoon, John!`       | `Guten Tag, John!`              | `Buon pomeriggio, John!`                                                            |
| Evening            | `Good evening, John!`         | `Guten Abend, John!`            | `Buonasera, John!`                                                                  |
| Typical time rules | morning → afternoon → evening | morning → `Guten Tag` → evening | `Buongiorno` often extends well beyond morning; `Buonasera` starts later in the day |
| Formal morning     | `Good morning, Mr Smith.`     | `Guten Morgen, Herr Smith.`     | `Buongiorno, Signor Smith.`                                                         |
| Formal evening     | `Good evening, Mr Smith.`     | `Guten Abend, Herr Smith.`      | `Buonasera, Signor Smith.`                                                          |
| Formal title       | Mr / Ms                       | Herr / Frau                     | Signor / Signora                                                                    |
| Formal pronoun     | `you`                         | `Sie`                           | `Lei`                                                                               |
| Informal pronoun   | `you`                         | `du`                            | `tu`                                                                                |


## Tasks to do

1. Perform an OO Analysis and Design. Note your design as PUML in `greetAndQuote.puml`.
2. Strive for an elegant OO design using the concepts you know.
3. Implement the solution and cover the core functionality of the classes with xUnit unit tests.
4. Use your brain, not AI.
