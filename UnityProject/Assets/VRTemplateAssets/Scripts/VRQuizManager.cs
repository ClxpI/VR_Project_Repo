using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VRQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;
    }

    public TMP_Text questionDisplayText;
    public TMP_Text[] answerDisplayTexts;
    public Button[] answerButtons;
    public TMP_Text progressText;

    private Question[] questions;
    private int currentQuestionIndex = 0;
    private int score = 0;

    void Start()
    {
        // Define the questions
        questions = new Question[]
        {
            new Question {
                questionText = "What does a function do in programming?",
                answers = new string[] { "Stores a list", "Groups code to perform a task", "Makes code run faster", "Hides bugs" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "What is the output of: print(2 + 3 * 4)?",
                answers = new string[] { "14", "20", "24", "9" },
                correctAnswerIndex = 0
            },
            new Question {
                questionText = "What does 'commit' mean in Git?",
                answers = new string[] { "Delete old code", "Send code to production", "Save changes locally", "Merge code from others" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "Which variable name is most suitable for storing a score?",
                answers = new string[] { "x", "data", "user123", "score" },
                correctAnswerIndex = 3
            },
            new Question {
                questionText = "Why do software engineers write comments in code?",
                answers = new string[] { "To make it run faster", "To explain the code", "To hide errors", "To reduce memory" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "Which symbol is used for a comment in Python?",
                answers = new string[] { "//", "#", "/* */", "<!-- -->" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "What is the main goal of testing code?",
                answers = new string[] { "Make it look nice", "Speed it up", "Find errors", "Rename variables" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "Which of these is a logic error?",
                answers = new string[] { "Spelling mistake in variable", "Crash on startup", "Code runs but gives wrong result", "Missing semicolon" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "Which loop repeats while a condition is true?",
                answers = new string[] { "for", "if", "while", "switch" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "Why do we use version control like Git?",
                answers = new string[] { "To make the app faster", "To save and track code changes", "To change colours", "To write HTML" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "What is an IDE used for?",
                answers = new string[] { "Designing logos", "Editing audio", "Writing and running code", "Making spreadsheets" },
                correctAnswerIndex = 2
                        },
            new Question {
                questionText = "A teammate asks you to review their code. What should you do?",
                answers = new string[] { "Ignore it", "Delete it", "Check for errors and improvements", "Copy it into your project" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "What is 'syntax' in programming?",
                answers = new string[] { "How code looks", "The rules of writing code", "Colour of code", "The result of a program" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "Why do we break code into functions?",
                answers = new string[] { "To make it run faster", "To confuse others", "To organise and reuse code", "To reduce memory" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "What type of error is missing a colon in Python?",
                answers = new string[] { "Runtime", "Logic", "Syntax", "Type" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "What happens if a list index is too high?",
                answers = new string[] { "It repeats", "Index out of range error", "Code speeds up", "It gives 0" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "How should you name variables?",
                answers = new string[] { "Short and unclear", "Random letters", "With clear and meaningful names", "In all caps always" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "What is 'pseudocode'?",
                answers = new string[] { "Fake code for fun", "Code that runs in Java", "Code written in English-like language", "Encrypted code" },
                correctAnswerIndex = 2
            },
            new Question {
               questionText = "What is the output of len(\"hello\")?",
                answers = new string[] { "4", "5", "6", "Error" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "What is the purpose of a loop?",
                answers = new string[] { "To stop code", "To repeat code", "To save memory", "To draw shapes" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "Which file extension is used for Python files?",
                answers = new string[] { ".py", ".exe", ".html", ".docx" },
                correctAnswerIndex = 0
            },
            new Question {
                questionText = "What should you do before deploying code?",
                answers = new string[] { "Nothing", "Guess it works", "Test it carefully", "Rename all files" },
                correctAnswerIndex = 2
            },
            new Question {
                questionText = "A user reports a bug. What is the FIRST thing to do?",
                answers = new string[] { "Blame them", "Try to reproduce the error", "Delete the app", "Ignore the message" },
               correctAnswerIndex = 1
            },
            new Question {
                questionText = "What is a 'Boolean' value?",
                answers = new string[] { "Any number", "True or False", "A string", "A list of text" },
                correctAnswerIndex = 1
            },
            new Question {
                questionText = "Which of these is best for storing a shopping list?",
                answers = new string[] { "int", "float", "list", "bool" },
                correctAnswerIndex = 2
            }

        };

        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            // End of quiz — show score in the Question text
            questionDisplayText.text = $"{score}/{questions.Length} answers were correct";

            // Hide all answer texts
            foreach (var text in answerDisplayTexts)
            {
                text.gameObject.SetActive(false);
            }

            // Hide all buttons
            foreach (var btn in answerButtons)
            {
                btn.gameObject.SetActive(false);
            }

            // Hide progress counter
            progressText.gameObject.SetActive(false);

            return;
        }

        // Normal question display
        Question q = questions[currentQuestionIndex];
        questionDisplayText.text = "Q: " + q.questionText;

        for (int i = 0; i < answerDisplayTexts.Length; i++)
        {
            answerDisplayTexts[i].text = q.answers[i];
            answerDisplayTexts[i].gameObject.SetActive(true);
            answerButtons[i].gameObject.SetActive(true);
        }

        progressText.text = $"{currentQuestionIndex + 1}/{questions.Length}";
        progressText.gameObject.SetActive(true);
    }


    public void OnAnswerSelected(int index)
    {
        if (index == questions[currentQuestionIndex].correctAnswerIndex)
        {
            score++;
        }

        currentQuestionIndex++;
        ShowQuestion();
    }

}
