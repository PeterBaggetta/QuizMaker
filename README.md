# QuizMaker

## About
QuizMaker is a simple console application that lets you create, save, and play multiple-choice quizzes. Questions can have one or more correct answers. Quizzes persist between runs using an XML file.

## How to play
1. From the menu choose:
   - `0` — Exit
   - `1` — Build questions
   - `2` — Play quiz
2. Building questions:
   - Enter the question text (press Enter on an empty line to stop).
   - Enter the number of choices (between 2 and 10).
   - Enter each choice text.
   - Enter the correct answer number(s) as comma-separated one-based indices (examples: `2` or `1,3`).
   - Confirm whether to add another question.
3. Playing the quiz:
   - Questions and answer choices are shuffled.
   - For multiple correct answers enter comma-separated indices (e.g. `1,3`).
   - Your score is shown at the end.

## Features
- Create questions with 2–10 answer choices.
- Support for multiple correct answers per question.
- Randomized order of questions and choices during play.
- Persistent storage in `QuizList.xml` via XML serialization.
- Simple console-based UI (no external dependencies).

## Data storage
Quizzes are saved to `QuizList.xml` in the application's working directory. The file path is defined by the `PATH` constant in `Contants.cs`. If the file is missing or invalid, the app starts with an empty question store.

## Project layout
- `Program.cs` — application entry and main menu loop
- `UI.cs` — console input/output and user interaction flows
- `Logic.cs` — quiz-building, parsing, shuffling, and scoring logic
- `Contants.cs` — application constants (menu options, limits, `PATH`)
- `XmlStorage.cs` — XML serialization/deserialization for persistence
- `QuizMaker.csproj` — project file (targets `net8.0`)
